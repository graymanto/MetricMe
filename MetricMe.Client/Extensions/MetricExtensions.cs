using System;
using System.Collections.Generic;

namespace MetricMe.Client.Extensions
{
    public static class MetricExtensions
    {
        public static IEnumerable<TInput> IncrementMetricCounter<TInput>(this IEnumerable<TInput> input, string metricName)
        {
            foreach (var inputItem in input)
            {
                Metrics.Counter(metricName);

                yield return inputItem;
            }
        }

        public static IEnumerable<TInput> TimeEach<TInput>(
            this IEnumerable<TInput> input,
            Action<TInput> action,
            string metricName)
        {
            foreach (var inputItem in input)
            {
                using (Metrics.TimedSection(metricName))
                {
                    action(inputItem);
                    yield return inputItem;
                }
            }
        }
    }
}