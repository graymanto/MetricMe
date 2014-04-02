using MetricMe.Core.Configuration;
using MetricMe.Server.Constants;

namespace MetricMe.Server.Configuration
{
    /// <summary>
    /// Global configuration.
    /// </summary>
    public static class GlobalConfig
    {
        private static readonly ConfigurationFetcher ConfigurationFetcher =
            new ConfigurationFetcher(new ConfigurationManagerConfigurationProvider());

        private static GraphiteSettings graphite;

        /// <summary>
        /// Gets the graphite settings.
        /// </summary>
        /// <value>
        /// The graphite.
        /// </value>
        public static GraphiteSettings Graphite
        {
            get
            {
                return graphite ?? (graphite = CreateDefaultGraphiteSettings());
            }
        }

        /// <summary>
        /// Gets the flush interval in milliseconds.
        /// </summary>
        /// <value>
        /// The flush interval.
        /// </value>
        public static int FlushInterval
        {
            get
            {
                return ConfigurationFetcher.Get(ConfigurationKeys.FlushInterval, DefaultConfigurationValues.FlushInterval);
            }
        }

        /// <summary>
        /// Gets the required backends.
        /// </summary>
        /// <value>
        /// The backends.
        /// </value>
        public static string[] Backends
        {
            get
            {
                return ConfigurationFetcher.Get(ConfigurationKeys.BackEnds, "Console").Split(';');
            }
        }

        /// <summary>
        /// Gets the stats prefix.
        /// </summary>
        /// <value>
        /// The stats prefix.
        /// </value>
        public static string StatsPrefix
        {
            get
            {
                return ConfigurationFetcher.Get(ConfigurationKeys.StatsPrefix, DefaultConfigurationValues.StatsPrefix);
            }
        }

        /// <summary>
        /// Creates the default graphite settings.
        /// </summary>
        /// <returns></returns>
        private static GraphiteSettings CreateDefaultGraphiteSettings()
        {
            var settings = new GraphiteSettings
                               {
                                   CounterPrefix = DefaultConfigurationValues.GraphiteCounterPrefix, 
                                   GlobalPrefix = DefaultConfigurationValues.GraphiteGlobalPrefix,
                                   TimerPrefix = DefaultConfigurationValues.GraphiteTimerPrefix,
                                   GaugePrefix = DefaultConfigurationValues.GraphiteGaugePrefix,
                                   SetPrefix = DefaultConfigurationValues.GraphiteSetPrefix,
                                   Host = DefaultConfigurationValues.GraphiteHost,
                                   Port = DefaultConfigurationValues.GraphitePort, 
                                   FlushCounts = true
                               };

            ConfigurationFetcher.Populate(settings);
            return settings;
        }
    }
}