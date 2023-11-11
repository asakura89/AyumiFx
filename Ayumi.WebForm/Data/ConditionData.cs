namespace WebLib.Data
{
    public class ConditionData
    {
        private string _Connector;
        private string _ColumnName;
        private string _Operator;
        private string[] _Value;

        public string Connector
        {
            get
            {
                return _Connector;
            }
            set
            {
                _Connector = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
            }
        }
        public string Operator
        {
            get
            {
                return _Operator;
            }
            set
            {
                _Operator = value;
            }
        }

        public string[] Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public ConditionData(string pConnector, string pColumnName, string pOperator, string[] pValue)
        {
            this.Connector = pConnector;
            this.ColumnName = pColumnName;
            this.Operator = pOperator;
            this.Value = pValue;
        }

        public ConditionData(string pConnector, string pColumnName, string pOperator, string pValue)
        {
            this.Connector = pConnector;
            this.ColumnName = pColumnName;
            this.Operator = pOperator;
            string[] parVal = { pValue };
            this.Value = parVal;
        }

        public ConditionData()
            : this(string.Empty, string.Empty, string.Empty, string.Empty) { }

    }
}
