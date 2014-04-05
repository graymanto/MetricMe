using MetricMe.Core;
using MetricMe.Core.Extensions;

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

            var metricType = MetricTypeParser.CreateFromString(packetSections[1]);
            if (metricType == MetricType.Unknown)
            {
                return info;
            }

            var metricValue = -1;
            string valueString;
            string sign = null;
            if (metricType == MetricType.Gauge
                && (packetSections[0].StartsWith("+") || packetSections[0].StartsWith("-")))
            {
                sign = packetSections[0].Substring(0, 1);
                valueString = packetSections[0].Substring(1);
            }
            else
            {
                valueString = packetSections[0];
            }

            if (metricType != MetricType.Set && !int.TryParse(valueString, out metricValue))
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
            info.GaugeDirection = GetGaugeDirection(sign);
            info.ValueString = valueString;

            return info;
        }

        private static GaugeDirection GetGaugeDirection(string sign)
        {
            if (sign.IsNullOrEmpty())
            {
                return GaugeDirection.NotSpecified;
            }
            return sign == "+" ? GaugeDirection.Plus : GaugeDirection.Minus;
        }
    }
}