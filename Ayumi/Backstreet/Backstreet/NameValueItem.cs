using System;

namespace Nvy {
    [Serializable]
    public class NameValueItem : Item<String, String> {
        public const String NameProperty = "Name";
        public const String ValueProperty = "Value";
        public const Char ListDelimiter = '·'; // ALT+250
        public const Char ItemDelimiter = '•'; // ALT+7

        public String Name {
            get {
                return InternalName;
            }

            private set {
                InternalName = value;
            }
        }

        public String Value {
            get {
                return InternalValue;
            }

            private set {
                InternalValue = value;
            }
        }

        public static NameValueItem Empty => new NameValueItem();

        public NameValueItem(String name, String value) {
            InternalName = name;
            InternalValue = value;
        }

        public NameValueItem() : this(String.Empty, String.Empty) { }
    }
}