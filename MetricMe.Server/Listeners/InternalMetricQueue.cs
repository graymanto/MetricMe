using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;

namespace MetricMe.Server.Listeners
{
    public class InternalMetricQueue : IMetricListener, IDisposable
    {
        private static readonly BlockingCollection<string> MetricsQueue = new BlockingCollection<string>();

        public static void AddMetric(string metric)
        {
            MetricsQueue.TryAdd(metric);
        }

        public static void AddCount(string name, int amount = 1)
        {
            var formattedMetric = String.Format("{0}:{1}|c", name, amount);
            AddMetric(formattedMetric);
        }

        public IObservable<string> Metrics
        {
            get
            {
                return MetricsQueue.GetConsumingEnumerable().ToObservable();
            }
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

            MetricsQueue.CompleteAdding();
        }
    }
}