using System;

namespace MetricMe.Server.Listeners
{
    public interface IMetricListener
    {
        IObservable<string> Metrics { get; }
    }
}