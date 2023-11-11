using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace WK.DBUtility
{
    public class DataReaderUtility 
    {
        public static T ReadSingleDataFromDr<T>(IDataReader dataReader) where T : new()
        {
            if (dataReader.Read())
                return GetOneRow<T>(dataReader);

            return default(T);
        }

        public static IEnumerable<T> ReadMultipleDataFromDr<T>(IDataReader dataReader) where T : new()
        {
            var result = new List<T>();

            while (dataReader.Read())
            {
                var temp = GetOneRow<T>(dataReader);
                result.Add(temp);
            }

            return result.AsEnumerable();
        }

        private static T GetOneRow<T>(IDataRecord dataRecord) where T : new()
        {
            var fields = ReflectionUtility.GetAllDbFields<T>();
            var temp = new T();

            foreach (var field in fields)
                SetFieldValue(dataRecord, temp, field);

            return temp;
        }

        private static void SetFieldValue<T>(IDataRecord dataRecord, T result, PropertyInfo field)
        {
            string dbFieldName = ReflectionUtility.GetDbFieldName(field);
            object value = dataRecord[dbFieldName];

            if (value is DBNull) 
                return;

            field.SetValue(result, value, null);
        }
    }
}
