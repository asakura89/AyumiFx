using System;
using System.Collections.Generic;
using System.Reflection;

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

    public static class DataTypeExt {
        public static IList<DataType> AsDataTypes<T>(this T tObj) where T : class {
            IList<DataType> result = new List<DataType>();
            Type tType = typeof (T);
            PropertyInfo[] tProperties = tType.GetProperties();
            if (tProperties.Length != 0)
                foreach (PropertyInfo property in tProperties)
                    result.Add(new DataType(property.Name, property.GetValue(tObj, null), property.PropertyType));

            return result;
        }
    }
}
