using System.Collections.Generic;

namespace MetricMe.Server
{
    public class MetricCollection
    {
        public MetricCollection()
        {
            Gauges = new List<MetricItem<int>>();
            Counters = new List<MetricItem<int>>();
            CounterRates = new List<MetricItem<double>>();
            Timers = new List<MetricItem<int>>();
            TimerData = new List<TimerData>();
            Sets = new List<MetricItem<int>>();
        }

        public IEnumerable<MetricItem<int>> Gauges { get; set; }

        public IEnumerable<MetricItem<int>> Counters { get; set; }

        public IEnumerable<MetricItem<double>> CounterRates { get; set; }

        public IEnumerable<MetricItem<int>> Timers { get; set; }

        public IEnumerable<MetricItem<int>> Sets { get; set; }

        public IEnumerable<TimerData> TimerData { get; set; } 
    }
}