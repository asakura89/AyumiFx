using System;
using System.Collections.Generic;
using System.Linq;
using Ayumi.Extension;
using Microsoft.Extensions.Configuration;

namespace Ayumi.Configuration {

    public class AppConfig {

        public static TConfig Get<TConfig>() where TConfig : class, new() =>
            ConfigurationManager
                .AppSettings
                .AsT<TConfig>();

        public static TSection GetBySection<TSection>() where TSection : class, new() {
            IEnumerable<IConfigurationSection> appSettingsSection = ConfigurationManager
                .AppSettings
                .Where(item => item.Key.Equals(typeof(TSection).Name, StringComparison.InvariantCultureIgnoreCase));

            if (appSettingsSection != null)
                return appSettingsSection.AsT<TSection>();

            return new TSection();
        }
    }
}