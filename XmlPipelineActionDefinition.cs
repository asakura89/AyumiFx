using System;

namespace Ria {
    public class XmlPipelineActionDefinition {
        public String Type { get; }
        public String Assembly { get; }
        public String Method { get; }
        public Boolean Enabled { get; set; }

        public XmlPipelineActionDefinition(String type, String assembly, String method, Boolean enabled = true) {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            if (String.IsNullOrEmpty(assembly))
                throw new ArgumentNullException(nameof(assembly));

            if (String.IsNullOrEmpty(method))
                throw new ArgumentNullException(nameof(method));

            Type = type;
            Assembly = assembly;
            Method = method;
            Enabled = enabled;
        }
    }
}