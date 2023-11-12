using System;

namespace Buruku {
    [AttributeUsage(AttributeTargets.Property)]
    public class BulkColumnAttribute : Attribute {
        public String BulkColumnName { get; private set; }

        public BulkColumnAttribute(String name) {
            BulkColumnName = name;
        }
    }
}