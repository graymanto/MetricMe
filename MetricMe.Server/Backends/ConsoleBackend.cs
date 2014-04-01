using System;
using System.Linq;

using MetricMe.Core.Extensions;
using MetricMe.Server.Extensions;

namespace MetricMe.Server.Backends
{
    public class ConsoleBackend : IBackend
    {
        public void Flush(MetricCollection metrics)
        {
            Console.WriteLine("Flushing stats at {0}".Formatted(DateTime.Now));

            metrics.Counters.Select(c => "Counter: {0} {1}".Formatted(c.Name, c.Value)).ForEach(Console.WriteLine);
            metrics.Gauges.Select(c => "Gauge: {0} {1}".Formatted(c.Name, c.Value)).ForEach(Console.WriteLine);
            metrics.Sets.Select(c => "Sets: {0} {1}".Formatted(c.Name, c.Value)).ForEach(Console.WriteLine);
            metrics.Timers.Select(c => "Timers: {0} {1} ms".Formatted(c.Name, c.Value)).ForEach(Console.WriteLine);
        }
    }
}