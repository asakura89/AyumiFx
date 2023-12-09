using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace Nino.ScItemConfiguration {
    public interface IConfigurationServiceFactory {
        IConfigurationService CreateByItem(Item configRoot);
        IConfigurationService CreateByPath(String configRootPath);
        IConfigurationService CreateById(ID configRootId);
    }
}