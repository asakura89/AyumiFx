using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace Nino.ScItemConfiguration {
    public class ConfigurationServiceFactory : IConfigurationServiceFactory {
        public IConfigurationService CreateByItem(Item configRoot) => new ConfigurationService(configRoot);

        public IConfigurationService CreateByPath(String configRootPath) => new ConfigurationService(configRootPath);

        public IConfigurationService CreateById(ID configRootId) => new ConfigurationService(configRootId);
    }
}