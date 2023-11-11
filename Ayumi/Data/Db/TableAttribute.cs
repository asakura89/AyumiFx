using System;

namespace Ayumi.Data.Db {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute {
        public String Name { get; set; }
        public String DisplayName { get; set; }

        public TableAttribute(String name, String displayName) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;
        }

        public TableAttribute(String name) : this(name, String.Empty) { }
    }
}