using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Emi {
    public class XmlEventDefinition {
        public String Name { get; set; }
        public Boolean OnlyOnce { get; set; }
        public String Type { get; set; }
        public String Method { get; set; }
    }

    public class XmlConfigEmitterLoader : IEmitterLoader {
        readonly String configPath;
        readonly Emitter globalEmitter;

        public XmlConfigEmitterLoader() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\emitter.config.xml") { }

        public XmlConfigEmitterLoader(String configPath) {
            this.configPath = configPath ?? throw new ArgumentNullException(nameof(configPath));
            globalEmitter = new Emitter();
        }

        public Emitter Load() {
            XmlDocument config = XmlExt.LoadFromPath(configPath);
            String eventsSelector = $"configuration/events";
            XmlNode eventsConfig = config.SelectSingleNode(eventsSelector);
            if (eventsConfig != null) {
                IEnumerable<XmlEventDefinition> events = eventsConfig
                    .SelectNodes("event")
                    .Cast<XmlNode>()
                    .Select(eventNode => new XmlEventDefinition {
                        Name = eventNode.GetAttributeValue("name"),
                        OnlyOnce = new Func<String, Boolean>(attrValue => {
                                if (String.IsNullOrEmpty(attrValue))
                                    return false;

                                return Convert.ToBoolean(attrValue);
                            })(eventNode.GetAttributeValue("onlyOnce")),
                        Type = eventNode.GetAttributeValue("type"),
                        Method = eventNode.GetAttributeValue("method")
                    });

                var typeNameRgx = new Regex("^(?<TypeN>.+)(?:,\\s{1,}?)(?<AsmN>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (XmlEventDefinition definition in events) {
                    String typeName = typeNameRgx.Match(definition.Type).Groups["TypeN"].Value;
                    String asmName = typeNameRgx.Match(definition.Type).Groups["AsmN"].Value;
                    if (String.IsNullOrEmpty(typeName) || String.IsNullOrEmpty(asmName))
                        throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                    Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(appDAsm => appDAsm.GetName().Name == asmName);
                    if (asm == null)
                        throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                    Type type = asm.GetTypes().FirstOrDefault(asmType => asmType.FullName.Replace("+", ".") == typeName);
                    if (type == null)
                        throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Type}");

                    Object instance = Activator.CreateInstance(type);
                    MethodInfo methodInfo = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(method => method.Name == definition.Method);
                    if (methodInfo == null)
                        throw new InvalidOperationException($"{eventsSelector} wrong configuration. {definition.Method}");

                    var eventDelegate = (Action<EmitterEventArgs>) Delegate.CreateDelegate(typeof(Action<EmitterEventArgs>), instance, methodInfo);
                    if (definition.OnlyOnce)
                        globalEmitter.Once(definition.Name, eventDelegate);
                    else
                        globalEmitter.On(definition.Name, eventDelegate);
                }
            }

            return globalEmitter;
        }
    }
}
