using System;

namespace WebLib.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class Table : Attribute
    {
        public String DisplayName { get; set; }
        public String TableName { get; set; }

        public Table(String tableName, String displayName)
        {
            TableName = tableName;
            DisplayName = displayName;
        }

        public Table(String tableName) : this(tableName, String.Empty) { }
    }
}