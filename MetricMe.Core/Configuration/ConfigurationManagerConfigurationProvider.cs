using System.Configuration;

namespace MetricMe.Core.Configuration
{
    public class ConfigurationManagerConfigurationProvider : IConfigurationProvider
    {
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}