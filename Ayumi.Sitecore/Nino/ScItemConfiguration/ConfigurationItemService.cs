using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using Sitecore.SecurityModel;
using System;
using ScContext = Sitecore.Context;

namespace Nino.ScItemConfiguration {
    public class ConfigurationItemService : IConfigurationItemService {
        const String KeyFieldName = "Key";
        const String ValueFieldName = "Value";

        readonly Item configItem;

        public ConfigurationItemService(Item configItem) {
            this.configItem = configItem ?? throw new ArgumentNullException(nameof(configItem));
            if (!ConfigurationItemUtil.IsConfigurationItem(this.configItem))
                throw new InvalidItemException($"{this.configItem.Paths.FullPath} is not a Configuration Item.");
        }

        public ConfigurationItemService(String configItemPath) : this(ScContext.Database.GetItem(configItemPath)) { }

        public ConfigurationItemService(ID configItemId) : this(ScContext.Database.GetItem(configItemId)) { }

        public String GetValue() {
            Field valueField = configItem.Fields[ValueFieldName];
            return valueField == null ? String.Empty : valueField.ToString();
        }

        public void SetValue(String value) {
            using (new SecurityDisabler()) {
                configItem.Editing.BeginEdit();
                configItem[ValueFieldName] = value;
                configItem.Editing.EndEdit();
            }
        }

        public String GetDescription() {
            Field descField = configItem.Fields[KeyFieldName];
            return descField == null ? String.Empty : descField.ToString();
        }

        public void SetDescription(String desc) {
            using (new SecurityDisabler()) {
                configItem.Editing.BeginEdit();
                configItem[KeyFieldName] = desc;
                configItem.Editing.EndEdit();
            }
        }
    }
}