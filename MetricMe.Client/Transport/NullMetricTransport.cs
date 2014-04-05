namespace MetricMe.Client.Transport
{
    public class NullMetricTransport : IMetricTransport
    {
        public void Send(string message)
        {
        }
    }
}