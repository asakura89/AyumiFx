using System;

namespace Puru {
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginInfoAttribute : Attribute {
        public String Code { get; private set; }
        public String Name { get; private set; }
        public String Desc { get; private set; }

        public PluginInfoAttribute(String code, String name, String desc = "") {
            Code = code;
            Name = name;
            Desc = desc;
        }
    }
}
