using System;
using System.Collections.Generic;

namespace MetricMe.Server.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<TInput>(this IEnumerable<TInput> input, Action<TInput> action)
        {
            foreach (var inputItem in input)
            {
                action(inputItem);
            }
        }
    }
}