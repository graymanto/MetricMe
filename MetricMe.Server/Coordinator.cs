using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Timers;

using MetricMe.Core.Extensions;
using MetricMe.Server.Configuration;
using MetricMe.Server.Listeners;

namespace MetricMe.Server
{
    public class Coordinator
    {
        private readonly IEnumerable<IMetricListener> listeners;

        private readonly IEnumerable<IBackend> backEnds;

        private readonly IList<IDisposable> subscriptions = new List<IDisposable>();

        private readonly MetricGatherer gatherer = new MetricGatherer();

        private readonly Timer timer = new Timer(GlobalConfig.FlushInterval);

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinator"/> class.
        /// </summary>
        /// <param name="listeners">The listeners.</param>
        /// <param name="backEnds">The back ends.</param>
        public Coordinator(IEnumerable<IMetricListener> listeners, IEnumerable<IBackend> backEnds)
        {
            this.listeners = listeners;
            this.backEnds = backEnds;
        }

        /// <summary>
        /// Starts the coordinator.
        /// </summary>
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
