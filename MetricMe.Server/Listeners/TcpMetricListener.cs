using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;

using MetricMe.Server.Constants;

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
                        .SelectMany(GetMessageFromClient);
            }
        }

        private IEnumerable<string> GetMessageFromClient(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                InternalMetricQueue.AddCount(MetricMeInternalMetrics.ConnectionsOpened);

                do
                {
                    byte[] bytes;
                    try
                    {
                        var reader = new BinaryReader(stream);
                        var length = reader.ReadInt32();

                        bytes = reader.ReadBytes(length);

                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine(e);
                        yield break;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        yield break;
                    }
                    yield return Encoding.Default.GetString(bytes);
                }
                while (stream.DataAvailable);
            }
        }

        private void OnSubscriptionFinished()
        {
            Debug.WriteLine("Tcp listener subscription finished.");
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

            listener.Stop();
        }
    }
}