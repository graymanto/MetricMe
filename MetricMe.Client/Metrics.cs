using MetricMe.Client.Constants;
using MetricMe.Client.Messages;
using MetricMe.Client.Transport;

namespace MetricMe.Client
{
    public static class Metrics
    {
        private static IMetricTransport transport = new UdpMetricTransport(
            DefaultConfigurationValues.UdpSendDestination,
            DefaultConfigurationValues.UdpSendPort);

        public static MetricConfiguration Configure()
        {
            return new MetricConfiguration(SetTransport, GetTransport);
        }

        public static void Counter(string metricName, int count = 1)
        {
            var message = new CounterMessage(metricName, count);
            transport.Send(message.ToString());
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