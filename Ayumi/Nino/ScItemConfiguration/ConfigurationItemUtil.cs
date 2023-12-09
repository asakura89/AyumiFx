using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Configuration;

namespace Nino.ScItemConfiguration {
    public static class ConfigurationItemUtil {
        static String ConfigurationItemTemplateId => ConfigurationManager.AppSettings["Sc.ConfigItem.TemplateId"] ?? String.Empty;

        public static Boolean IsConfigurationItem(Item item) => item.TemplateID == ID.Parse(ConfigurationItemTemplateId);
    }
}