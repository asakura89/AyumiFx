using System;
using System.Collections.Specialized;
using System.Reflection;

namespace AppSea {

    internal static class AppConfigExt {
        internal static TConfig AsT<TConfig>(this NameValueCollection appSettings) {
            TConfig t = Activator.CreateInstance<TConfig>();
            Type tType = typeof(TConfig);
            PropertyInfo[] propList = tType.GetProperties();
            FieldInfo[] fieldList = tType.GetFields();

            foreach (PropertyInfo prop in propList) {
                String value = appSettings[prop.Name];
                if (!String.IsNullOrEmpty(value))
                    prop.SetValue(t, Convert.ChangeType(value, prop.PropertyType), null);
            }

            foreach (FieldInfo field in fieldList) {
                String value = appSettings[field.Name];
                if (!String.IsNullOrEmpty(value))
                    field.SetValue(t, Convert.ChangeType(value, field.FieldType));
            }

            return t;
        }
    }
}
