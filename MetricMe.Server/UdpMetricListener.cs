using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Core.Logging;

namespace MetricMe.Server
{
    public class UdpMetricListener : IMetricListener, IDisposable
    {
        private readonly UdpClient socket;

        private ILogger log;

        public UdpMetricListener(int udplistenerPort)
        {
            socket = new UdpClient(udplistenerPort);
        }

        public ILogger Log
        {
            get
            {
                return log ?? (log = new NullLogger());
            }
            set
            {
                log = value;
            }
        }

        public IObservable<string> Metrics
        {
            get
            {
                return
                    Observable.FromAsync(socket.ReceiveAsync)
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
            socket.Close();
        }
    }
}
