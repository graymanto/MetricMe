using System.Net.Sockets;
using System.Text;

namespace MetricMe.Client.Transport
{
    public class UdpMetricTransport : IMetricTransport
    {
        private readonly string host;

        private readonly int port;

        public UdpMetricTransport(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Send(string message)
        {
            using (var client = new UdpClient(this.host, this.port))
            {
                var messageBytes = Encoding.ASCII.GetBytes(message);
                client.Send(messageBytes, messageBytes.Length);
            }
        }
    }
}