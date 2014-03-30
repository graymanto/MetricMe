namespace MetricMe.Server
{
    public interface IBackend
    {
        void Flush(MetricCollection metrics);
    }
}