using System;

using MetricMe.Client.Transport;

namespace MetricMe.Client
{
    public class MetricConfiguration
    {
        protected readonly Action<IMetricTransport> TransportSetter;
        protected readonly Func<IMetricTransport> TransportGetter;

        public MetricConfiguration(Action<IMetricTransport> transportSetter, Func<IMetricTransport> transportGetter)
        {
            this.TransportSetter = transportSetter;
            this.TransportGetter = transportGetter;
        }

        public MetricConfiguration WithTransport(IMetricTransport transport)
        {
            this.TransportSetter(transport);
            return this;
        }

        public MetricConfiguration WithUdpTransport(string host = "localhost", int port = 8989)
        {
            TransportSetter(new UdpMetricTransport(host, port));
            return this;
        }
    }
}