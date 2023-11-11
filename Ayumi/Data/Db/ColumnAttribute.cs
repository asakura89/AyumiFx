using System;

namespace Ayumi.Data.Db {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute {
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public Boolean PrimaryKey { get; set; }
        public Int32 Length { get; set; }
        public Int32 Index { get; set; } = -1;
        public Boolean OutputParameter { get; set; }

        public ColumnAttribute(String name, String displayName) {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;
        }

        public ColumnAttribute(String name) : this(name, String.Empty) { }
    }
}