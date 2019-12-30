using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace Nino.ScItemConfiguration {
    public class ConfigurationItemServiceFactory : IConfigurationItemServiceFactory {
        public IConfigurationItemService CreateByItem(Item configItem) => new ConfigurationItemService(configItem);
        public IConfigurationItemService CreateByPath(String configItemPath) => new ConfigurationItemService(configItemPath);
        public IConfigurationItemService CreateById(ID configItemId) => new ConfigurationItemService(configItemId);
    }
}