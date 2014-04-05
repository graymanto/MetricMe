using MetricMe.Core.Constants;

namespace MetricMe.Core
{
    public static class MetricTypeParser
    {
        public static MetricType CreateFromString(string metricType)
        {
            if (metricType == MetricStringSections.Counter)
            {
                return MetricType.Counter;
            }
            if (metricType == MetricStringSections.Gauge)
            {
                return MetricType.Gauge;
            }
            if (metricType == MetricStringSections.Set)
            {
                return MetricType.Set;
            }
            if (metricType == MetricStringSections.Timer)
            {
                return MetricType.Timing;
            }

            return MetricType.Unknown;
        }
    }
}