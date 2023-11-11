using System;

namespace WK.DBUtility
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WkFieldAttribute : Attribute
    {
        public string FieldName { get; private set; }
        public DbFieldType FieldType { get; private set; }

        public WkFieldAttribute(string vDbFieldName, DbFieldType vDbFieldType)
        {
            FieldName = vDbFieldName;
            FieldType = vDbFieldType;
        }

        public WkFieldAttribute(string vDbFiledName)
            : this(vDbFiledName, DbFieldType.NORMAL)
        {
        }

        public WkFieldAttribute(DbFieldType vDbFieldType)
            : this("", vDbFieldType)
        {
        }

        public WkFieldAttribute() : this("")
        {
        }
    }
}
