using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetricMe.Server
{
    public class MetricAccumulator
    {
        private readonly BlockingCollection<string> incomingMetrics = new BlockingCollection<string>();

        private CancellationTokenSource cancellationToken;

        private Task processTask;

        public void Queue(string rawMetric)
        {
            incomingMetrics.TryAdd(rawMetric);
        }

        public void Accumulate()
        {
            cancellationToken = new CancellationTokenSource();

            processTask = Task.Factory.StartNew(
                () =>
                {
                    foreach (var metric in incomingMetrics.GetConsumingEnumerable(cancellationToken.Token))
                    {

                    }
                },
                cancellationToken.Token);

            processTask.ContinueWith(LogError, TaskContinuationOptions.OnlyOnFaulted);
        }

        public MetricCollection Stop()
        {
            incomingMetrics.CompleteAdding();

            processTask.Wait();

            //TODO: final accumulation

            return new MetricCollection();
        }

        public MetricCollection Flush()
        {
            cancellationToken.Cancel();

            processTask.Wait();

            // TODO: the actual work 

            var collection = new MetricCollection();

            Accumulate();

            return collection;
        }

        private void LogError(Task t)
        {

        }
    }
}
