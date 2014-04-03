using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;

namespace MetricMe.Server.Listeners
{
    public class TcpMetricListener : IMetricListener, IDisposable
    {
        private readonly TcpListener listener;

        public TcpMetricListener(int tcpListenerPort)
        {
            this.listener = new TcpListener(IPAddress.Any, tcpListenerPort) { ExclusiveAddressUse = false };
            this.listener.Start();
        }

        public IObservable<string> Metrics
        {
            get
            {
                return
                    Observable.FromAsync(this.listener.AcceptTcpClientAsync)
                        .Where(o => o != null && o.Connected)
                        .Retry()
                        .Repeat()
                        .Finally(OnSubscriptionFinished)
                        .Select(GetMessageFromClient);
            }
        }

        private string GetMessageFromClient(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var reader = new BinaryReader(stream);
                var length = reader.ReadInt32();

                var bytes = reader.ReadBytes(length);
                return Encoding.Default.GetString(bytes);
            }
        }

        private void OnSubscriptionFinished()
        {
            Debug.WriteLine("Udp listener subscription finished.");
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
            }
        }
    }
}