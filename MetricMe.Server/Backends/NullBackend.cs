namespace MetricMe.Server.Backends
{
    public class NullBackend : IBackend
    {
        public void Flush(MetricCollection metrics)
        {
        }
    }
}