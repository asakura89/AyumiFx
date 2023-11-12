using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace Nino.ScItemConfiguration {
    public interface IConfigurationItemServiceFactory {
        IConfigurationItemService CreateByItem(Item configItem);
        IConfigurationItemService CreateByPath(String configItemPath);
        IConfigurationItemService CreateById(ID configItemId);
    }
}