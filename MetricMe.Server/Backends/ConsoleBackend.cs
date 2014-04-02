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
            metrics.TimerData.ForEach(WriteOutTimer);
        }

        private void WriteOutTimer(TimerData timer)
        {
            const string BasicTimerFormat = "Timer {0}: Std {1} | Upper {2} | Lower {3} | Count {4}| CountPs {5}| Sum {6} | Mean {7} | Median {8}";
            var formattedTimer = BasicTimerFormat.Formatted(
                timer.Key,
                timer.Std,
                timer.Upper,
                timer.Lower,
                timer.Count,
                timer.CountPs,
                timer.Sum,
                timer.Mean,
                timer.Median);

            Console.WriteLine(formattedTimer);
        }
    }
}