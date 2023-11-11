using System;

namespace WebLib.Configuration
{
    [Serializable]
    public class BadConfigurationException : Exception
    {
        public BadConfigurationException(String configurationName) : base(String.Format("Configuration \"{0}\" is not configured correctly.", configurationName)) { }

        public BadConfigurationException(String configurationName, Exception innerException) : base(String.Format("Configuration \"{0}\" is not configured correctly.", configurationName), innerException) { }
    }
}