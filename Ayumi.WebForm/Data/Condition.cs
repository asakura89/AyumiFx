using System;

namespace WebLib.Data
{
    public class Condition
    {
        public Connector Connector { get; set; }
        public String ColumnName { get; set; }
        public Operator Operator { get; set; }
        public String[] ColumnValue { get; set; }

        public Condition(Connector connector, String columnName, Operator op, params String[] columnValue)
        {
            Connector = connector;
            ColumnName = columnName;
            Operator = op;
            ColumnValue = columnValue;
        }

        public Condition(Connector connector, String columnName, Operator op, String columnValue)
            : this(connector, columnName, op, new [] { columnValue }) { }

        public Condition()
            : this(Connector.And, String.Empty, Operator.Equal, String.Empty) { }
    }
}
