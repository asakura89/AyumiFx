using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace WebApp.Config {
    public static class AppConfig {
        public static T Get<T>() where T : class, new() {
            T t = Activator.CreateInstance<T>();
            Type tType = typeof(T);
            PropertyInfo[] propList = tType.GetProperties();
            FieldInfo[] fieldList = tType.GetFields();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
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

        public static T GetBySection<T>() where T : class, new() {
            T t = Activator.CreateInstance<T>();
            Type tType = typeof(T);
            PropertyInfo[] propList = tType.GetProperties();
            FieldInfo[] fieldList = tType.GetFields();

            String sectionName = tType.Name;
            var configSection = ConfigurationManager.GetSection(sectionName) as NameValueCollection;
            if (configSection != null) {
                foreach (PropertyInfo prop in propList) {
                    String config = configSection[prop.Name];
                    if (!String.IsNullOrEmpty(config))
                        prop.SetValue(t, Convert.ChangeType(configSection[prop.Name], prop.PropertyType), null);
                }

                foreach (FieldInfo field in fieldList) {
                    String config = configSection[field.Name];
                    if (!String.IsNullOrEmpty(config))
                        field.SetValue(t, Convert.ChangeType(configSection[field.Name], field.FieldType));
                }
            }

            return t;
        }
    }
}