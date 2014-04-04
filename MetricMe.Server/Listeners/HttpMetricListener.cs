using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;

using MetricMe.Core.Extensions;

namespace MetricMe.Server.Listeners
{
    public class HttpMetricListener : IMetricListener, IDisposable
    {
        private readonly HttpListener listener;

        public HttpMetricListener(int httpListenerPort)
        {
            this.listener = new HttpListener();
            this.listener.Prefixes.Add("http://*:{0}/MetricMe/".Formatted(httpListenerPort));
            this.listener.Start();
        }

        public IObservable<string> Metrics
        {
            get
            {
                return
                    Observable.FromAsync(this.listener.GetContextAsync)
                        .Retry()
                        .Repeat()
                        .Select(l => ReadMetricFromStream(l.Request.InputStream));
            }
        }

        private string ReadMetricFromStream(Stream inputStream)
        {
            return new StreamReader(inputStream).ReadToEnd();
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

            this.listener.Stop();
        }
    }
}