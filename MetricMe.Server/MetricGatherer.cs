using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    public class MetricGatherer
    {
        private readonly BlockingCollection<string> incomingMetrics = new BlockingCollection<string>();

        private readonly Aggregator aggregator = new Aggregator();

        private CancellationTokenSource cancellationToken;

        private Task processTask;

        public void Queue(string rawMetric)
        {
            this.incomingMetrics.TryAdd(rawMetric);
        }

        public void Collect()
        {
            this.cancellationToken = new CancellationTokenSource();

            this.processTask = Task.Factory.StartNew(
                () => this.incomingMetrics.GetConsumingEnumerable(this.cancellationToken.Token)
                          .SelectMany(
                              m =>
                              m.Split(new[] { "\n" }, StringSplitOptions.None))
                          .ForEach(this.aggregator.Add),
                this.cancellationToken.Token);

            this.processTask.ContinueWith(LogError, TaskContinuationOptions.OnlyOnFaulted);
        }

        public MetricCollection Stop()
        {
            this.incomingMetrics.CompleteAdding();

            this.processTask.Wait();

            var collection = this.aggregator.GetAggregatedCollection();
            this.aggregator.Clear();

            return collection;
        }

        public MetricCollection Flush()
        {
            this.cancellationToken.Cancel();

            this.processTask.Wait();

            var collection = this.aggregator.GetAggregatedCollection();
            this.aggregator.Clear();

            Collect();

            return collection;
        }

        private void LogError(Task t)
        {
        }
    }
}