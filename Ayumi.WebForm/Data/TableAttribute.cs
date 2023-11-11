using System;

namespace WebLib.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public String DisplayName { get; set; }
        public String TableName { get; set; }

        public TableAttribute(String tableName, String displayName)
        {
            TableName = tableName;
            DisplayName = displayName;
        }

        public TableAttribute(String tableName) : this(tableName, String.Empty) { }
    }
}