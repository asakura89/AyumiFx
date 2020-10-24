using System;
using System.Collections.Generic;
using System.Linq;

namespace Ria {
    public class XmlPipelinesDefinition {
        public String Name { get; }
        public String ContextType { get; }
        public String ContextAssembly { get; }
        public IList<XmlPipelineActionDefinition> Actions { get; }

        public XmlPipelinesDefinition(String name, String contextType, String contextAssembly, IList<XmlPipelineActionDefinition> actions) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(contextType))
                throw new ArgumentNullException(nameof(contextType));

            if (String.IsNullOrEmpty(contextAssembly))
                throw new ArgumentNullException(nameof(contextAssembly));

            if (actions == null || !actions.Any())
                throw new ArgumentNullException(nameof(actions));

            Name = name;
            ContextType = contextType;
            ContextAssembly = contextAssembly;
            Actions = actions;
        }
    }
}