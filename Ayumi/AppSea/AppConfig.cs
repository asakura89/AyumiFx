using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace AppSea {
    public class AppConfig {
        public static TConfig Get<TConfig>() where TConfig : class, new() {
            return ConfigurationManager
                .AppSettings
                .AsT<TConfig>();
        }

        public static TSection GetBySection<TSection>() where TSection : class, new() {
            var appSettingsSection = ConfigurationManager.GetSection(typeof(TSection).Name) as NameValueCollection;
            if (appSettingsSection != null)
                return appSettingsSection.AsT<TSection>();

            return new TSection();
        }
    }

    internal static class Ext {
        internal static TConfig AsT<TConfig>(this NameValueCollection appSettings) {
            var t = Activator.CreateInstance<TConfig>();
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

    [Serializable]
    public class BadConfigurationException : Exception {
        const String ExcMessage = "Configuration \"{0}\" is not configured correctly.";

        public BadConfigurationException(String configurationName) :
            base(String.Format(ExcMessage, configurationName)) { }

        public BadConfigurationException(String configurationName, Exception innerException) : base(
            String.Format(ExcMessage, configurationName), innerException) { }
    }
}