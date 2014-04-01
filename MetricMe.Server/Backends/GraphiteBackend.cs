using System;
using System.Collections.Generic;
using System.Linq;

using MetricMe.Core.Extensions;
using MetricMe.Server.Configuration;
using MetricMe.Server.Graphite;

namespace MetricMe.Server.Backends
{
    /// <summary>
    /// A graphite backend.
    /// </summary>
    public class GraphiteBackend : IBackend
    {
        private readonly IGraphiteClient client;

        private readonly string globalPrefix = GlobalConfig.Graphite.GlobalPrefix;

        private readonly string counterPrefix = GlobalConfig.Graphite.CounterPrefix;

        private readonly string timerPrefix = GlobalConfig.Graphite.TimerPrefix;

        private readonly string gaugePrefix = GlobalConfig.Graphite.GaugePrefix;

        private readonly string setPrefix = GlobalConfig.Graphite.SetPrefix;

        private readonly string globalSuffix = GlobalConfig.Graphite.GlobalSuffix.IsNullOrEmpty()
                                                   ? " "
                                                   : GlobalConfig.Graphite.GlobalSuffix + " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphiteBackend"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public GraphiteBackend(IGraphiteClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Flushes the specified metrics to Graphite.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public void Flush(MetricCollection metrics)
        {
            var statStrings = new List<string>();
            var timestampSuffix = " " + DateTime.UtcNow.ToJavaUnixTimestamp() + "\n";

            var counters = ProcessCounters(metrics.Counters, timestampSuffix);
            var timers = ProcessTimers(metrics.Timers, timestampSuffix);
            var gauges = ProcessGauges(metrics.Gauges, timestampSuffix);
            var sets = ProcessSets(metrics.Sets, timestampSuffix);

            statStrings.AddRange(counters);
            statStrings.AddRange(timers);
            statStrings.AddRange(gauges);
            statStrings.AddRange(sets);

            // TODO: some standard extra stats bits

            var completeStatsString = statStrings.Combine();
            this.client.Send(completeStatsString);
        }

        private IEnumerable<string> ProcessSets(IEnumerable<MetricItem<int>> sets, string timestampSuffix)
        {
            yield break;
        }

        private IEnumerable<string> ProcessTimers(IEnumerable<MetricItem<int>> timers, string timestampSuffix)
        {
            yield break;
        }

        private IEnumerable<string> ProcessGauges(IEnumerable<MetricItem<int>> gauges, string timeStampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.gaugePrefix);

            return from gauge in gauges
                   let nameSpace = prefix.JoinWithDot(gauge.Name)
                   select
                       nameSpace.JoinWithDot(this.globalSuffix).JoinTo(gauge.Value.ToString()).JoinTo(timeStampSuffix);
        }

        private IEnumerable<string> ProcessCounters(IEnumerable<MetricItem<int>> counters, string timeStampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.counterPrefix);
            foreach (var counter in counters)
            {
                var nameSpace = prefix.JoinWithDot(counter.Name);
                // TODO: need rates here
                var counterRate = counter.Value;

                yield return
                    nameSpace.JoinWithDot("rate")
                        .JoinWithDot(this.globalSuffix)
                        .JoinTo(counterRate.ToString())
                        .JoinTo(timeStampSuffix);
                ;

                if (GlobalConfig.Graphite.FlushCounts)
                {
                    yield return
                        nameSpace.JoinWithDot("count")
                            .JoinWithDot(this.globalSuffix)
                            .JoinTo(counter.Value.ToString())
                            .JoinTo(timeStampSuffix);
                }
            }
        }
    }
}