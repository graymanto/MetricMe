using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MetricMe.Client.Transport
{
    public class TcpMetricTransport : IMetricTransport
    {
        private readonly string host;

        private readonly int port;

        public TcpMetricTransport(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Send(string message)
        {
            var client = new TcpClient { ExclusiveAddressUse = false };
            client.Connect(this.host, this.port);

            var messageBytes = Encoding.Default.GetBytes(message);

            try
            {
                using (var messageStream = client.GetStream())
                using (var writer = new BinaryWriter(messageStream))
                {
                    writer.Write(messageBytes.Length);
                    writer.Write(messageBytes);
                }
            }
            finally
            {
                client.Close();
            }
        }
    }
}