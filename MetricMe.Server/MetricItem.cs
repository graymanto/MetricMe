namespace MetricMe.Server
{
    public class MetricItem<TValueType>
    {
        public string Name { get; set; }

        public TValueType Value { get; set; }
    }
}