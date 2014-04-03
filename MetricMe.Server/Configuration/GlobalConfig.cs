using MetricMe.Core.Configuration;
using MetricMe.Core.Extensions;
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
                var backends = ConfigurationFetcher.Get(ConfigurationKeys.BackEnds, string.Empty);
                if (backends.IsNullOrEmpty())
                    return new string[0];

                return backends.Split(';');
            }
        }

        /// <summary>
        /// Gets the required listeners.
        /// </summary>
        /// <value>
        /// The listeners.
        /// </value>
        public static string[] Listeners
        {
            get
            {
                var listeners = ConfigurationFetcher.Get(ConfigurationKeys.Listeners, string.Empty);
                if (listeners.IsNullOrEmpty())
                    return new string[0];

                return listeners.Split(';');
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
        /// Gets the UDP listening port.
        /// </summary>
        /// <value>
        /// The UDP listening port.
        /// </value>
        public static int UdpListeningPort
        {
            get
            {
                return ConfigurationFetcher.Get(
                    ConfigurationKeys.UdpListeningPort,
                    DefaultConfigurationValues.UdpListeningPort);
            }
        }

        /// <summary>
        /// Gets the TCP listening port.
        /// </summary>
        /// <value>
        /// The TCP listening port.
        /// </value>
        public static int TcpListeningPort
        {
            get
            {
                return ConfigurationFetcher.Get(
                    ConfigurationKeys.TcpListeningPort,
                    DefaultConfigurationValues.TcpListeningPort);
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