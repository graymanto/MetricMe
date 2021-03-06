﻿using System;
using System.Collections.Generic;
using System.Linq;

using MetricMe.Core;
using MetricMe.Server.Configuration;
using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    /// <summary>
    /// Aggregates metrics
    /// </summary>
    public class Aggregator
    {
        private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

        private readonly Dictionary<string, int> gauges = new Dictionary<string, int>();

        private readonly Dictionary<string, HashSet<string>> sets = new Dictionary<string, HashSet<string>>();

        private readonly List<Tuple<string, int>> timers = new List<Tuple<string, int>>();

        /// <summary>
        /// Adds the specified metric into the aggregated collections.
        /// </summary>
        /// <param name="metric">The metric.</param>
        public void Add(string metric)
        {
            var parsedInformation = MetricParser.Parse(metric);
            if (!parsedInformation.IsValid)
            {
                return;
            }

            if (parsedInformation.Type == MetricType.Counter)
            {
                ProcessCounter(parsedInformation);
            }

            if (parsedInformation.Type == MetricType.Gauge)
            {
                ProcessGauge(parsedInformation);
            }

            if (parsedInformation.Type == MetricType.Set)
            {
                ProcessSet(parsedInformation);
            }

            if (parsedInformation.Type == MetricType.Timing)
            {
                ProcessTiming(parsedInformation);
            }
        }

        /// <summary>
        /// Clears the aggregated values.
        /// </summary>
        public void ClearAggregatedValues()
        {
            this.counters.Clear();
            this.sets.Clear();
            this.timers.Clear();
        }

        /// <summary>
        /// Builds an aggregated collection of metrics from all the metrics collected since the last reset.
        /// </summary>
        /// <returns>The aggregated metrics.</returns>
        public MetricCollection GetAggregatedCollection()
        {
            var aggregatedCounters = BuildCounters().ToList();

            return new MetricCollection
                       {
                           Counters = aggregatedCounters,
                           CounterRates =
                               aggregatedCounters.Select(
                                   c =>
                                   new MetricItem<double>
                                       {
                                           Name = c.Name,
                                           Value = (double)c.Value / ((double)GlobalConfig.FlushInterval / 1000)
                                       }),
                           Gauges =
                               this.gauges.Select(
                                   c => new MetricItem<int> { Name = c.Key, Value = c.Value }),
                           Sets = BuildSets(),
                           Timers = BuildTimers(),
                           TimerData = TimerCalculation.CalculateTimerData(this.timers)
                       };
        }

        private IEnumerable<MetricItem<int>> BuildCounters()
        {
            return this.counters.Select(c => new MetricItem<int> { Name = c.Key, Value = c.Value });
        }

        private IEnumerable<MetricItem<int>> BuildSets()
        {
            return this.sets.Select(s => new MetricItem<int> { Name = s.Key, Value = s.Value.Count });
        }

        private IEnumerable<MetricItem<int>> BuildTimers()
        {
            return this.timers.Select(t => new MetricItem<int> { Name = t.Item1, Value = t.Item2 });
        }

        private void ProcessCounter(MetricParseInformation metricInfo)
        {
            this.counters.IncrementCountForKey(metricInfo.Name, metricInfo.Value);
        }

        private void ProcessGauge(MetricParseInformation metricInfo)
        {
            if (metricInfo.GaugeDirection == GaugeDirection.NotSpecified)
            {
                this.gauges[metricInfo.Name] = metricInfo.Value;
                return;
            }

            if (metricInfo.GaugeDirection == GaugeDirection.Plus)
            {
                this.gauges[metricInfo.Name] = this.gauges.GetOrDefault(metricInfo.Name) + metricInfo.Value;
            }
            else
            {
                this.gauges[metricInfo.Name] = this.gauges.GetOrDefault(metricInfo.Name) - metricInfo.Value;
            }
        }

        private void ProcessSet(MetricParseInformation metricInfo)
        {
            HashSet<string> valuesSoFar;
            if (!this.sets.TryGetValue(metricInfo.Name, out valuesSoFar))
            {
                valuesSoFar = new HashSet<string>();
                this.sets[metricInfo.Name] = valuesSoFar;
            }
            valuesSoFar.Add(metricInfo.ValueString);
        }

        private void ProcessTiming(MetricParseInformation metricInfo)
        {
            this.timers.Add(new Tuple<string, int>(metricInfo.Name, metricInfo.Value));
        }
    }
}