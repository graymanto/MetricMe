using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;

namespace MetricMe.Server.Listeners
{
    public class UdpMetricListener : IMetricListener, IDisposable
    {
        private readonly UdpClient socket;

        public UdpMetricListener(int udplistenerPort)
        {
            this.socket = new UdpClient(udplistenerPort);
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
            Debug.WriteLine("Udp listener subscription finished.");
        }

        public void Dispose()
        {
            this.socket.Close();
        }
    }
}