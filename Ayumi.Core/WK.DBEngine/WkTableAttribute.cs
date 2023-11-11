using System;

namespace WK.DBUtility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WkTableAttribute : Attribute
    {
        public string TableName { get; private set; }

        public WkTableAttribute(string vTableName)
        {
            TableName = vTableName;
        }
    }
}
