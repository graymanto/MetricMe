using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MetricMe.Core;
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
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var statStrings = new List<string>();
            var timestampSuffix = " " + Math.Truncate(SystemTime.UtcNow.ToJavaUnixTimestamp()) + "\n";

            var counters = ProcessCounters(metrics.Counters, metrics.CounterRates, timestampSuffix);
            var timers = ProcessTimerData(metrics.TimerData, timestampSuffix);
            var gauges = ProcessGauges(metrics.Gauges, timestampSuffix);
            var sets = ProcessSets(metrics.Sets, timestampSuffix);

            statStrings.AddRange(counters);
            statStrings.AddRange(timers);
            statStrings.AddRange(gauges);
            statStrings.AddRange(sets);

            stopWatch.Stop();

            var numberOfStats = statStrings.Count;
            var statsPrefix = globalPrefix.JoinWithDot(GlobalConfig.StatsPrefix);
            var numStatsMetric = CreateStatString(
                statsPrefix.JoinWithDot("numStats"),
                numberOfStats.ToString(),
                timestampSuffix);
            var calculationTimeMetric = CreateStatString(
                statsPrefix.JoinWithDot("graphiteStats.calculationtime"),
                stopWatch.ElapsedMilliseconds.ToString(),
                timestampSuffix);

            statStrings.Add(numStatsMetric);
            statStrings.Add(calculationTimeMetric);

            var completeStatsString = statStrings.Combine();
            this.client.Send(completeStatsString);
        }

        private IEnumerable<string> ProcessSets(IEnumerable<MetricItem<int>> sets, string timestampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.setPrefix);

            return from set in sets
                   let keyedPrefix = prefix.JoinWithDot(set.Name).JoinWithDot("count")
                   select CreateStatString(keyedPrefix, set.Value.ToString(), timestampSuffix);
        }

        private IEnumerable<string> ProcessTimerData(IEnumerable<TimerData> timers, string timestampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.timerPrefix);

            foreach (var timerData in timers)
            {
                var keyedPrefix = prefix.JoinWithDot(timerData.Key);

                yield return CreateStatString(keyedPrefix.JoinWithDot("count"), timerData.Count.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("countps"), timerData.CountPs.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("lower"), timerData.Lower.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("upper"), timerData.Upper.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("mean"), timerData.Mean.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("median"), timerData.Median.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("std"), timerData.Std.ToString(), timestampSuffix);
                yield return CreateStatString(keyedPrefix.JoinWithDot("sum"), timerData.Sum.ToString(), timestampSuffix);
            }
        }

        private IEnumerable<string> ProcessGauges(IEnumerable<MetricItem<int>> gauges, string timeStampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.gaugePrefix);

            return from gauge in gauges
                   let nameSpace = prefix.JoinWithDot(gauge.Name)
                   select CreateStatString(nameSpace, gauge.Value.ToString(), timeStampSuffix);
        }

        /// <summary>
        /// Generates counter graphite messages from the counter collections.
        /// </summary>
        /// <param name="counters">The counters.</param>
        /// <param name="counterRates">The counter rates.</param>
        /// <param name="timeStampSuffix">The time stamp suffix.</param>
        /// <returns>Counter graphite messages.</returns>
        private IEnumerable<string> ProcessCounters(
            IEnumerable<MetricItem<int>> counters,
            IEnumerable<MetricItem<double>> counterRates,
            string timeStampSuffix)
        {
            var prefix = this.globalPrefix.JoinWithDot(this.counterPrefix);
            int index = 0;
            var rateList = counterRates.ToList();

            foreach (var counter in counters)
            {
                var nameSpace = prefix.JoinWithDot(counter.Name);
                var counterRate = rateList[index].Value;

                yield return CreateStatString(nameSpace.JoinWithDot("rate"), counterRate.ToString(), timeStampSuffix);

                if (GlobalConfig.Graphite.FlushCounts)
                {
                    yield return CreateStatString(nameSpace.JoinWithDot("count"), counter.Value.ToString(), timeStampSuffix);
                }

                index++;
            }
        }

        /// <summary>
        /// Creates the graphite stat string from a set of inputs.
        /// </summary>
        /// <param name="keyedPrefix">The keyed prefix.</param>
        /// <param name="metricValue">The metric value.</param>
        /// <param name="timestampSuffix">The timestamp suffix.</param>
        /// <returns></returns>
        private string CreateStatString(string keyedPrefix, string metricValue, string timestampSuffix)
        {
            return keyedPrefix
                    .JoinWithDotIfPopulated(this.globalSuffix)
                    .JoinTo(metricValue)
                    .JoinTo(timestampSuffix);
        }
    }
}