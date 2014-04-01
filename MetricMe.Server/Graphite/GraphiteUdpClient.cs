using System;
using System.Net.Sockets;

using MetricMe.Core.Extensions;

namespace MetricMe.Server.Graphite
{
    public class GraphiteUdpClient : IGraphiteClient, IDisposable
    {
        private readonly UdpClient client;

        public GraphiteUdpClient(string host, int port)
        {
            this.client = new UdpClient(host, port);
        }

        public void Send(string metricName, int metricValue, DateTime timestamp)
        {
            var message = "{0} {1} {2}\n".Formatted(metricName, metricValue, timestamp);
            var messageBytes = message.AsByteArray();

            this.client.Send(messageBytes, messageBytes.Length);
        }

        public void Send(string metricString)
        {
            var messageBytes = metricString.AsByteArray();
            this.client.Send(messageBytes, messageBytes.Length);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.client != null)
            {
                this.client.Close();
            }
        }
    }
}