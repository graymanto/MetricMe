namespace MetricMe.Core.Configuration
{
    public interface IConfigurationProvider
    {
        string Get(string key);
    }
}