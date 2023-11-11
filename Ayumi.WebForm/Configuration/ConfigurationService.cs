using System;
using System.Configuration;
using WebLib.Constant;

namespace WebLib.Configuration
{
    public class ConfigurationService
    {
        public DefaultConfiguration GetDefaultConfiguration()
        {
            ValidateDefaultConfiguration();

            DefaultConfiguration config = new DefaultConfiguration();
            config.AppName = ConfigurationManager.AppSettings[AppConfig.AppName];
            config.AppKey = ConfigurationManager.AppSettings[AppConfig.AppKey];
            config.Domain = ConfigurationManager.AppSettings[AppConfig.Domain];
            config.ADPath = ConfigurationManager.AppSettings[AppConfig.ADPath];
            config.FilePath = ConfigurationManager.AppSettings[AppConfig.FilePath];
            config.IsDebugMode = Convert.ToBoolean(ConfigurationManager.AppSettings[AppConfig.IsDebugMode]);

            return config;
        }

        private void ValidateDefaultConfiguration()
        {
            String appName = ConfigurationManager.AppSettings[AppConfig.AppName];
            String appKey = ConfigurationManager.AppSettings[AppConfig.AppKey];
            String domain = ConfigurationManager.AppSettings[AppConfig.Domain];
            String adPath = ConfigurationManager.AppSettings[AppConfig.ADPath];
            String filePath = ConfigurationManager.AppSettings[AppConfig.FilePath];
            String isDebugMode = ConfigurationManager.AppSettings[AppConfig.IsDebugMode];

            if (String.IsNullOrEmpty(appName))
                throw new BadConfigurationException("appName");
            if (String.IsNullOrEmpty(appKey))
                throw new BadConfigurationException("appKey");
            if (String.IsNullOrEmpty(domain))
                throw new BadConfigurationException("domain");
            if (String.IsNullOrEmpty(adPath))
                throw new BadConfigurationException("adPath");
            if (String.IsNullOrEmpty(filePath))
                throw new BadConfigurationException("filePath");
            if (String.IsNullOrEmpty(isDebugMode))
                throw new BadConfigurationException("isDebugMode");
        }
    }
}