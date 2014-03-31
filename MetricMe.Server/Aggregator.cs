using System.Collections.Generic;
using System.Linq;

using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    public class Aggregator
    {
        private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

        private readonly Dictionary<string, int> gauges = new Dictionary<string, int>();

        private readonly Dictionary<string, HashSet<string>> sets = new Dictionary<string, HashSet<string>>();

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

        public void ClearAggregatedValues()
        {
            this.counters.Clear();
            this.sets.Clear();
        }

        public MetricCollection GetAggregatedCollection()
        {
            return new MetricCollection
                       {
                           Counters = BuildCounters(),
                           Gauges =
                               gauges.Select(c => new MetricItem<int> { Name = c.Key, Value = c.Value }),
                           Sets = BuildSets()
                       };
        }

        private IEnumerable<MetricItem<int>> BuildCounters()
        {
            return counters.Select(c => new MetricItem<int> { Name = c.Key, Value = c.Value });
        }

        private IEnumerable<MetricItem<int>> BuildSets()
        {
            return sets.Select(s => new MetricItem<int> { Name = s.Key, Value = s.Value.Count });
        }

        private void ProcessCounter(MetricParseInformation metricInfo)
        {
            this.counters.IncrementCountForKey(metricInfo.Name, metricInfo.Value);
        }

        private void ProcessGauge(MetricParseInformation metricInfo)
        {
            if (metricInfo.GaugeDirection == GaugeDirection.NotSpecified)
            {
                gauges[metricInfo.Name] = metricInfo.Value;
                return;
            }

            if (metricInfo.GaugeDirection == GaugeDirection.Plus)
            {
                gauges[metricInfo.Name] = gauges.GetOrDefault(metricInfo.Name) + metricInfo.Value;
            }
            else
            {
                gauges[metricInfo.Name] = gauges.GetOrDefault(metricInfo.Name) - metricInfo.Value;
            }
        }

        private void ProcessSet(MetricParseInformation metricInfo)
        {
            HashSet<string> valuesSoFar;
            if (!sets.TryGetValue(metricInfo.Name, out valuesSoFar))
            {
                valuesSoFar = new HashSet<string>();
                sets[metricInfo.Name] = valuesSoFar;
            }
            valuesSoFar.Add(metricInfo.ValueString);
        }

        private void ProcessTiming(MetricParseInformation metricInfo)
        {
        }
    }
}