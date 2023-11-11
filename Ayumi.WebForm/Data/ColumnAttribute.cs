using System;

namespace WebLib.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        public String DisplayName { get; set; }
        public String ColumnName { get; set; }
        public Int32 ColumnLength { get; set; }
        public Int32 ColumnIndex { get; set; }
        public Boolean IsPrimaryKey { get; set; }
        public Boolean IsOutputParam { get; set; }

        public ColumnAttribute(String columnName, String displayName)
        {
            ColumnName = columnName;
            DisplayName = displayName;
            ColumnIndex = -1;
            ColumnLength = -1;
            IsPrimaryKey = false;
            IsOutputParam = false;
        }

        public ColumnAttribute(String columnName) : this(columnName, String.Empty) { }
    }
}
