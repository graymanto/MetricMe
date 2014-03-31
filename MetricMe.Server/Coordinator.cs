using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Timers;

using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    public class Coordinator
    {
        private readonly IEnumerable<IMetricListener> listeners;

        private readonly IEnumerable<IBackend> backEnds;

        private readonly IList<IDisposable> subscriptions = new List<IDisposable>();

        private readonly MetricGatherer gatherer = new MetricGatherer();

        private readonly Timer timer = new Timer(1000 * 60);

        public Coordinator(IEnumerable<IMetricListener> listeners, IEnumerable<IBackend> backEnds)
        {
            this.listeners = listeners;
            this.backEnds = backEnds;
        }

        public void Start()
        {
            listeners.ForEach(SubscribeListener);

            this.gatherer.Collect();

            timer.Elapsed += FlushMetrics;
            timer.Enabled = true;
        }

        private void SubscribeListener(IMetricListener listener)
        {
            var subscription = listener.Metrics.SubscribeOn(Scheduler.Default).Subscribe(m => this.gatherer.Queue(m));
            subscriptions.Add(subscription);
        }

        public void Stop()
        {
            subscriptions.ForEach(s => s.Dispose());

            var finalMetrics = this.gatherer.Stop();

            SendToBackEnd(finalMetrics);
        }

        private void FlushMetrics(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timer.Enabled = false;

            var metrics = this.gatherer.Flush();
            SendToBackEnd(metrics);

            timer.Enabled = true;
        }

        private void SendToBackEnd(MetricCollection metrics)
        {
            backEnds.AsParallel().ForAll(be => be.Flush(metrics));
        }
    }
}
