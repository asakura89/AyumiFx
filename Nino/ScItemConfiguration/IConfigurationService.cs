using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Nino.ScItemConfiguration {
    public interface IConfigurationService {
        IList<Item> GetAllConfigItems();
    }
}