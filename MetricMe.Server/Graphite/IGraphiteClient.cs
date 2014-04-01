using System;

namespace MetricMe.Server.Graphite
{
    public interface IGraphiteClient
    {
        void Send(string metricName, int metricValue, DateTime timestamp);

        void Send(string metricString);
    }
}