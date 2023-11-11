using System;

namespace Ayumi.Data
{
    [Serializable]
    public class NameValueItem
    {
        public const String NameProperty = "Name";
        public const String ValueProperty = "Value";
        public const Char ListDelimiter = '·';
        public const Char ItemDelimiter = '•';

        public String Name { get; set; }
        public String Value { get; set; }

        public static NameValueItem Empty
        {
            get { return new NameValueItem(String.Empty, String.Empty); }
        }

        public static NameValueItem None
        {
            get { return new NameValueItem("None", String.Empty); }
        }

        public NameValueItem(String name, String value)
        {
            Name = name;
            Value = value;
        }

        public NameValueItem() : this(String.Empty, String.Empty) { }
    }
}
