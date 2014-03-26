using System;

namespace MetricMe.Server
{
    public interface IMetricListener
    {
        IObservable<string> Metrics { get; }
    }
}