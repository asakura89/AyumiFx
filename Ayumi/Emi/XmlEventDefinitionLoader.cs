using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Eksmaru;
using Reflx;

namespace Emi {
    static class XmlEventDefinitionLoader {
        static XmlEventDefinition MapConfigItemToEventDefinition(XmlNode eventConfig) {
            var xmlH = new XmlHelper();
            String nameValue = xmlH.GetAttributeValue(eventConfig, "name");
            String onlyOnceValue = xmlH.GetAttributeValue(eventConfig, "onlyOnce");
            String typeValue = xmlH.GetAttributeValue(eventConfig, "type");
            String methodValue = xmlH.GetAttributeValue(eventConfig, "method");
            TypeAndAssembly eventTypeNAsm = TypeAndAssemblyParser.Instance.Parse(typeValue);

            if (String.IsNullOrEmpty(methodValue))
                throw new EmitterException($"Wrong Method configuration. '{methodValue}'.");

            return new XmlEventDefinition(nameValue, onlyOnceValue, eventTypeNAsm.Type, eventTypeNAsm.Assembly, methodValue);
        }

        internal static IEnumerable<XmlEventDefinition> Load(String configPath) {
            var xmlH = new XmlHelper();
            XmlDocument config = xmlH.LoadFromPath(configPath);
            String eventsSelector = $"configuration/events";
            XmlNode eventsConfig = config.SelectSingleNode(eventsSelector);
            if (eventsConfig == null)
                throw new EmitterException($"{eventsSelector} wrong configuration.");

            IEnumerable<XmlEventDefinition> events = eventsConfig
                .SelectNodes("event")
                .Cast<XmlNode>()
                .Select(MapConfigItemToEventDefinition);

            return events;
        }
    }
}