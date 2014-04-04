namespace MetricMe.Server.Configuration
{
    public static class DefaultConfigurationValues
    {
        public const int FlushInterval = 30 * 1000;

        public const string GraphiteCounterPrefix = "counters";

        public const string GraphiteGlobalPrefix = "stats";

        public const string GraphiteTimerPrefix = "timers";

        public const string GraphiteGaugePrefix = "gauges";

        public const string GraphiteSetPrefix = "sets";

        public const string GraphiteHost = "localhost";

        public const int GraphitePort = 8989;

        public const int UdpListeningPort = 8990;

        public const int TcpListeningPort = 8991;

        public const int HttpListeningPort = 88;

        public const string StatsPrefix = "statsd";
    }
}