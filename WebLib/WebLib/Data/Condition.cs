using System;

namespace WebLib.Data
{
    public class Condition
    {
        public String Connector { get; set; }
        public String ColumnName { get; set; }
        public String LogicOperator { get; set; }
        public String[] ColumnValue { get; set; }

        public Condition(String connector, String columnName, String logicOperator, params String[] columnValue)
        {
            Connector = connector;
            ColumnName = columnName;
            LogicOperator = logicOperator;
            ColumnValue = columnValue;
        }

        public Condition(String connector, String columnName, String logicOperator, String columnValue)
            : this(connector, columnName, logicOperator, new [] { columnValue }) { }

        public Condition()
            : this(String.Empty, String.Empty, String.Empty, String.Empty) { }
    }
}
