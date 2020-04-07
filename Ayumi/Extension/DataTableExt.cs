using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Ayumi.Extension {
    public static class DataTableExt {
        public static IEnumerable<TResult> MapToIEnumerableProperty<TResult>(this DataTable dt) {
            foreach (DataRow dataRow in dt.Rows) {
                var t = Activator.CreateInstance<TResult>();
                Type tType = typeof(TResult);
                PropertyInfo[] tProperties = tType.GetProperties();

                foreach (PropertyInfo property in tProperties) {
                    Type propertyType = property.PropertyType;

                    if (dataRow[property.Name] != DBNull.Value)
                        property.SetValue(t, Convert.ChangeType(dataRow[property.Name], propertyType), null);
                }

                yield return t;
            }
        }

        public static IEnumerable<TResult> MapToIEnumerableField<TResult>(this DataTable dt) {
            foreach (DataRow dataRow in dt.Rows) {
                var t = Activator.CreateInstance<TResult>();
                Type tType = typeof(TResult);
                FieldInfo[] tFields = tType.GetFields();

                foreach (FieldInfo field in tFields) {
                    Type fieldType = field.FieldType;

                    if (dataRow[field.Name] != DBNull.Value)
                        field.SetValue(t, Convert.ChangeType(dataRow[field.Name], fieldType));
                }

                yield return t;
            }
        }
    }
}