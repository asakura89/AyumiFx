using System;

namespace Tipe {
    [Serializable]
    public class DataType {
        public String Name { get; private set; }
        public Object Value { get; private set; }
        public Type Type { get; private set; }

        public DataType(String name, Object value, Type type) {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
