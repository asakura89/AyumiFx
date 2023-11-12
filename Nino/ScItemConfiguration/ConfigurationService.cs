using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using ScContext = Sitecore.Context;

namespace Nino.ScItemConfiguration {
    public class ConfigurationService : IConfigurationService {
        readonly Item configRoot;

        public ConfigurationService(Item configRoot) {
            this.configRoot = configRoot ?? throw new ArgumentNullException(nameof(configRoot));
        }

        public ConfigurationService(String configRootPath) : this(ScContext.Database.GetItem(configRootPath)) { }

        public ConfigurationService(ID configRootId) : this(ScContext.Database.GetItem(configRootId)) { }

        public IList<Item> GetAllConfigItems() {
            var items = new List<Item>();
            var children = new List<Item> {configRoot};
            while (children.Any()) {
                Item current = children.First();
                children.RemoveAt(0);

                if (current.HasChildren) {
                    IList<Item> currentChildren = current
                        .Children
                        .Where(child => ConfigurationItemUtil.IsConfigurationItem(child))
                        .ToList();

                    if (currentChildren.Any()) {
                        children.AddRange(currentChildren);
                        items.AddRange(currentChildren);
                    }
                }
            }

            return items;
        }
    }
}