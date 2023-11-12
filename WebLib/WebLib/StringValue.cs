using System;

namespace EnumExperimentation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringValue : Attribute
    {
        public String Value { get; set; }
        public StringValue(String value) { Value = value; }
    }
}