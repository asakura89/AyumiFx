using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Eksmaru;
using Reflx;

namespace Ria {
    public class XmlConfigPipelineLoader : IPipelineLoader {
        readonly String configPath;

        public XmlConfigPipelineLoader() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\pipeline.config.xml") { }

        public XmlConfigPipelineLoader(String configPath) {
            this.configPath = configPath ?? throw new ArgumentNullException(nameof(configPath));
        }

        Boolean ParseEnabledValue(String enabledValue) {
            if (String.IsNullOrEmpty(enabledValue))
                return true;

            if (enabledValue.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (enabledValue.Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase))
                return false;

            return false;
        }

        XmlPipelineActionDefinition MapConfigToActionDefinition(XmlNode actionConfig, Boolean pipelineEnabled) {
            String typeValue = actionConfig.GetAttributeValue("type");
            String methodValue = actionConfig.GetAttributeValue("method");
            TypeAndAssembly actionTypeNAsm = TypeAndAssemblyParser.Instance.Parse(typeValue);

            if (String.IsNullOrEmpty(methodValue))
                throw new PipelineException($"Wrong Method configuration. '{methodValue}'.");

            Boolean enabled = ParseEnabledValue(actionConfig.GetAttributeValue("enabled"));
            if (!pipelineEnabled)
                enabled = false;

            return new XmlPipelineActionDefinition(actionTypeNAsm.Type, actionTypeNAsm.Assembly, methodValue, enabled);
        }

        XmlPipelinesDefinition MapConfigToPipelineDefinition(XmlNode pipelineConfig) {
            Boolean enabled = ParseEnabledValue(pipelineConfig.GetAttributeValue("enabled"));
            IList<XmlPipelineActionDefinition> actions =
                pipelineConfig
                    .SelectNodes("action")
                    .Cast<XmlNode>()
                    .Select(node => MapConfigToActionDefinition(node, enabled))
                    .ToList();

            String nameValue = pipelineConfig.GetAttributeValue("name");
            String ctxTypeValue = pipelineConfig.GetAttributeValue("contextType");
            if (String.IsNullOrEmpty(nameValue))
                throw new PipelineException($"Pipeline Name was not defined.");

            if (String.IsNullOrEmpty(ctxTypeValue))
                throw new PipelineException($"Pipeline ContextType was not defined.");

            TypeAndAssembly ctxTypeNAsm = TypeAndAssemblyParser.Instance.Parse(ctxTypeValue);

            return new XmlPipelinesDefinition(nameValue, ctxTypeNAsm.Type, ctxTypeNAsm.Assembly, actions, enabled);
        }

        public IPipelineExecutor Load() {
            XmlDocument config = XmlExt.LoadFromPath(configPath);
            String pipelinesSelector = $"configuration/pipelines";
            IEnumerable<XmlNode> pipelinesConfig = config.SelectNodes(pipelinesSelector).Cast<XmlNode>();
            if (pipelinesConfig == null || !pipelinesConfig.Any())
                throw new PipelineException($"{pipelinesSelector} wrong configuration.");

            IList<XmlPipelinesDefinition> pipelines = pipelinesConfig
                .Select(MapConfigToPipelineDefinition)
                .ToList();

            return new PipelineExecutor(pipelines);
        }
    }
}
