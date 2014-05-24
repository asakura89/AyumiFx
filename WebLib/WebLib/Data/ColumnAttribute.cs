using System;

namespace WebLib.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        private string _displayName;
        private bool _isPrimaryKey;
        private int _columnLengthValue;
        private int _columnIndex;

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }

        public int ColumnLengthValue
        {
            get
            {
                return _columnLengthValue;
            }
            set
            {
                _columnLengthValue = value;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
            set
            {
                _columnIndex = value;
            }

        }

        public bool IsPrimaryKey
        {
            get
            {
                return _isPrimaryKey;
            }
            set
            {
                _isPrimaryKey = value;
            }
        }

        public ColumnAttribute(string pDisplayName, int pColumnIndex, int pColumnLength,bool pIsPrimaryKey)
        {
            this.DisplayName = pDisplayName;
            this.ColumnIndex = pColumnIndex;
            this.ColumnLengthValue = pColumnLength;
            this.IsPrimaryKey = pIsPrimaryKey;
        }

        public ColumnAttribute(string pDisplayName, int pColumnIndex,bool pIsPrimaryKey)
            : this(pDisplayName, pColumnIndex, 0, pIsPrimaryKey) { }

        public ColumnAttribute(string pDisplayName, int pColumnIndex)
            : this(pDisplayName,pColumnIndex,0, false) { }
        public ColumnAttribute(string pDisplayName)
            : this(pDisplayName, 0) { }
    }

  
}
