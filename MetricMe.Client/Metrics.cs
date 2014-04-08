using MetricMe.Client.Constants;
using MetricMe.Client.Messages;
using MetricMe.Client.Transport;

namespace MetricMe.Client
{
    /// <summary>
    /// Static implementation for sending metrics
    /// </summary>
    public static class Metrics
    {
        private static IMetricTransport transport = new UdpMetricTransport(
            DefaultConfigurationValues.UdpSendDestination,
            DefaultConfigurationValues.UdpSendPort);

        /// <summary>
        /// Configures the metric sender.
        /// </summary>
        /// <returns></returns>
        public static MetricConfiguration Configure()
        {
            return new MetricConfiguration(SetTransport, GetTransport);
        }

        /// <summary>
        /// Generates a counter metrics message.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="count">The count.</param>
        public static void Counter(string metricName, int count = 1)
        {
            var message = new CounterMessage(metricName, count);
            transport.Send(message.ToString());
        }

        /// <summary>
        /// Generates a timer metric message.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="time">The time.</param>
        public static void Timer(string metricName, int time)
        {
            var message = new TimingMessage(metricName, time);
            transport.Send(message.ToString());
        }

        /// <summary>
        /// Generates a gauge metric message.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="metricValue">The metric value.</param>
        public static void Gauge(string metricName, int metricValue)
        {
            var message = new GaugeMessage(metricName, metricValue);
            transport.Send(message.ToString());
        }

        /// <summary>
        /// Generates a set metric message.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="metricValue">The metric value.</param>
        public static void Set(string metricName, string metricValue)
        {
            var message = new SetMessage(metricName, metricValue);
            transport.Send(message.ToString());
        }

        /// <summary>
        /// To be used in a using block. Times a section and sends a timer metric.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <returns></returns>
        public static UsingTimer TimedSection(string metricName)
        {
            return new UsingTimer(metricName);
        }

        private static void SetTransport(IMetricTransport newTransport)
        {
            transport = newTransport;
        }

        private static IMetricTransport GetTransport()
        {
            return transport;
        }
    }
}