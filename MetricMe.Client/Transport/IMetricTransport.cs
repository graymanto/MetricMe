namespace MetricMe.Client.Transport
{
    public interface IMetricTransport
    {
        void Send(string message);
    }
}