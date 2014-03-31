namespace MetricMe.Server
{
    public class MetricParseInformation
    {
        public bool IsValid { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public GaugeDirection GaugeDirection { get; set; } 

        public string ValueString { get; set; }

        public double? SampleRate { get; set; }

        public MetricType Type { get; set; }
    }
}