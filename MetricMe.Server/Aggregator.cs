using System.Collections.Generic;
using System.Linq;

using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    public class Aggregator
    {
        private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

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

        public void Clear()
        {
            this.counters.Clear();
        }

        public MetricCollection GetAggregatedCollection()
        {
            return new MetricCollection { Counters = BuildCounters() };
        }

        private IEnumerable<MetricItem> BuildCounters()
        {
            return counters.Select(c => new MetricItem { Name = c.Key, Value = c.Value });
        }

        private void ProcessCounter(MetricParseInformation metricInfo)
        {
            this.counters.IncrementCountForKey(metricInfo.Name, metricInfo.Value);
        }

        private void ProcessGauge(MetricParseInformation metricInfo)
        {
        }

        private void ProcessSet(MetricParseInformation metricInfo)
        {
        }

        private void ProcessTiming(MetricParseInformation metricInfo)
        {
        }
    }
}