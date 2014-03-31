using MetricMe.Server.Constants;

namespace MetricMe.Server
{
    public class MetricParser
    {
        public static MetricParseInformation Parse(string metric)
        {
            var info = new MetricParseInformation();

            var metricSections = metric.Split(':');
            if (metricSections.Length < 2)
            {
                return info;
            }

            var key = metricSections[0];
            var packet = metricSections[1];

            var packetSections = packet.Split('|');
            if (packetSections.Length < 2)
            {
                return info;
            }

            int metricValue;
            if (!int.TryParse(packetSections[0], out metricValue))
            {
                return info;
            }

            var metricType = ParseForMetricType(packetSections[1]);
            if (metricType == MetricType.Unknown)
            {
                return info;
            }

            double? sampleRate = null;
            if (packetSections.Length > 2)
            {
                double parseSampleRate;
                var sampleRateSection = packetSections[2];
                if (sampleRateSection[0] != '@' || !double.TryParse(sampleRateSection.Substring(1), out parseSampleRate))
                {
                    return info;
                }

                sampleRate = parseSampleRate;
            }

            info.IsValid = true;
            info.Name = key;
            info.Value = metricValue;
            info.Type = metricType;
            info.SampleRate = sampleRate;

            return info;
        }

        private static MetricType ParseForMetricType(string metricType)
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