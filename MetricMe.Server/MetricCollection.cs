using System.Collections.Generic;

namespace MetricMe.Server
{
    public class MetricCollection
    {
        public MetricCollection()
        {
            Gauges = new List<MetricItem<int>>();
            Counters = new List<MetricItem<int>>();
            Timers = new List<MetricItem<int>>();
            Sets = new List<MetricItem<int>>();
        }

        public IEnumerable<MetricItem<int>> Gauges { get; set; }

        public IEnumerable<MetricItem<int>> Counters { get; set; }

        public IEnumerable<MetricItem<int>> Timers { get; set; }

        public IEnumerable<MetricItem<int>> Sets { get; set; }

        public IEnumerable<TimerData> TimerData { get; set; } 
    }
}