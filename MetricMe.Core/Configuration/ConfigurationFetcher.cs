using System.Reflection;

using MetricMe.Core.Extensions;

namespace MetricMe.Core.Configuration
{
    /// <summary>
    /// Gets configuration values.
    /// </summary>
    public class ConfigurationFetcher
    {
        private readonly IConfigurationProvider configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFetcher"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public ConfigurationFetcher(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        /// <summary>
        /// Gets the configuration value specified by the given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue = default(T))
        {
            var setting = this.configurationProvider.Get(key);
            return setting == null ? defaultValue : setting.ConvertTo(defaultValue);
        }

        /// <summary>
        /// Populates an instance of class type T with values from configuration.
        /// </summary>
        /// <typeparam name="T">The type of class to populate.</typeparam>
        /// <returns>The populated configuration class.</returns>
        public T Populate<T>() where T : new()
        {
            var configuration = new T();
            Populate(configuration);
            return configuration;
        }

        /// <summary>
        /// Populates the given instance with values from configuration.
        /// </summary>
        /// <typeparam name="T">The type of class to populate.</typeparam>
        /// <param name="input">The class to populate.</param>
        public void Populate<T>(T input)
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var typeName = typeof(T).Name;

            foreach (var prop in props)
            {
                var configKey = typeName + "." + prop.Name;

                var setting = Get<string>(configKey);
                if (setting.IsNullOrEmpty())
                {
                    continue;
                }

                var propValue = setting.ConvertTo(prop.PropertyType);
                prop.SetValue(input, propValue, null);
            }
        }
    }
}