using System.Collections.Specialized;
using System.Configuration;

namespace AppSea {

    public class AppConfig {

        public static TConfig Get<TConfig>() where TConfig : class, new() =>
            ConfigurationManager
                .AppSettings
                .AsT<TConfig>();

        public static TSection GetBySection<TSection>() where TSection : class, new() {
            var appSettingsSection = ConfigurationManager.GetSection(typeof(TSection).Name) as NameValueCollection;

            if (appSettingsSection != null)
                return appSettingsSection.AsT<TSection>();

            return new TSection();
        }
    }
}