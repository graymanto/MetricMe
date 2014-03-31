using System;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;

using Castle.Core.Logging;

namespace MetricMe.Server
{
    public class UdpMetricListener : IMetricListener, IDisposable
    {
        private readonly UdpClient socket;

        private ILogger log;

        public UdpMetricListener(int udplistenerPort)
        {
            this.socket = new UdpClient(udplistenerPort);
        }

        public ILogger Log
        {
            get
            {
                return this.log ?? (this.log = new NullLogger());
            }
            set
            {
                this.log = value;
            }
        }

        public IObservable<string> Metrics
        {
            get
            {
                return
                    Observable.FromAsync(this.socket.ReceiveAsync)
                        .Retry()
                        .Repeat()
                        .Finally(OnSubscriptionFinished)
                        .Select(Convert);
            }
        }

        private string Convert(UdpReceiveResult result)
        {
            return Encoding.ASCII.GetString(result.Buffer);
        }

        private void OnSubscriptionFinished()
        {
            Log.Debug("Udp listener subscription finished.");
        }

        public void Dispose()
        {
            this.socket.Close();
        }
    }
}