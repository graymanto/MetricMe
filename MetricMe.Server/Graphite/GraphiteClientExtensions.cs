namespace MetricMe.Server.Graphite
{
    public static class GraphiteClientExtensions
    {
        public static void Send(this IGraphiteClient client, string metricName, int metricValue)
        {
            client.Send(metricName, metricValue);
        }
    }
}