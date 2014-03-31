using System.Collections.Generic;

namespace MetricMe.Server
{
    public class MetricCollection
    {
        public MetricCollection()
        {
            Gauges = new List<MetricItem>();
            Counters = new List<MetricItem>();
            Timers = new List<MetricItem>();
            Sets = new List<MetricItem>();
        }

        public IEnumerable<MetricItem> Gauges { get; set; }

        public IEnumerable<MetricItem> Counters { get; set; }

        public IEnumerable<MetricItem> Timers { get; set; }

        public IEnumerable<MetricItem> Sets { get; set; }
    }
}