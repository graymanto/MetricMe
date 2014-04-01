using MetricMe.Core.Configuration;

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
        /// Creates the default graphite settings.
        /// </summary>
        /// <returns></returns>
        private static GraphiteSettings CreateDefaultGraphiteSettings()
        {
            var settings = new GraphiteSettings
                               {
                                   CounterPrefix = "counters",
                                   GlobalPrefix = "stats",
                                   TimerPrefix = "timers",
                                   GaugePrefix = "gauges",
                                   SetPrefix = "sets",
                                   Host = "localhost",
                                   Port = 8989, 
                                   FlushCounts = true
                               };

            ConfigurationFetcher.Populate(settings);
            return settings;
        }
    }
}