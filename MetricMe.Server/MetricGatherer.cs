using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MetricMe.Core.Extensions;

namespace MetricMe.Server
{
    public class MetricGatherer
    {
        private readonly BlockingCollection<string> incomingMetrics = new BlockingCollection<string>();

        private readonly Aggregator aggregator = new Aggregator();

        private CancellationTokenSource cancellationToken;

        private Task processTask;

        /// <summary>
        /// An approximate count for debugging purposes. 
        /// </summary>
        private int count;

        public void Queue(string rawMetric)
        {
            this.count++;
            this.incomingMetrics.Add(rawMetric);
        }

        public void Collect()
        {
            this.cancellationToken = new CancellationTokenSource();

            this.processTask =
                Task.Factory.StartNew(
                    () =>
                    this.incomingMetrics.GetConsumingEnumerable(this.cancellationToken.Token)
                        .SelectMany(m => m.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                        .ForEach(this.aggregator.Add));

            this.processTask.ContinueWith(LogError, TaskContinuationOptions.OnlyOnFaulted);
        }

        public MetricCollection Stop()
        {
            this.incomingMetrics.CompleteAdding();
            if (this.cancellationToken != null) this.cancellationToken.Cancel();

            try
            {
                this.processTask.Wait();
            }
            catch (AggregateException e)
            {
                if (!(e.InnerException is OperationCanceledException))
                {
                    throw;
                }
            }

            var collection = this.aggregator.GetAggregatedCollection();
            this.aggregator.ClearAggregatedValues();

            return collection;
        }

        public MetricCollection Flush()
        {
            this.cancellationToken.Cancel();

            Console.WriteLine("Received {0}", this.count);
            this.count = 0;
            try
            {
                this.processTask.Wait();
            }
            catch (AggregateException e)
            {
                if (!(e.InnerException is OperationCanceledException))
                {
                    throw;
                }
            }

            var collection = this.aggregator.GetAggregatedCollection();
            this.aggregator.ClearAggregatedValues();

            Collect();

            return collection;
        }

        private void LogError(Task t)
        {
            if (t.Exception != null && t.Exception.InnerException is OperationCanceledException)
            {
                return;
            }

            Console.WriteLine("There was a task error, {0}", t.Exception);
        }
    }
}