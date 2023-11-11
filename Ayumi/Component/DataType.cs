using System;

namespace Ayumi.Component {
    [Serializable]
    public class DataType {
        public String Name { get; }
        public Object Value { get; }
        public Type Type { get; }

        public DataType(String name, Object value, Type type) {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
