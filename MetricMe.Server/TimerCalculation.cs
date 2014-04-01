using System;
using System.Collections.Generic;
using System.Linq;

using MetricMe.Server.Extensions;

namespace MetricMe.Server
{
    public class TimerCalculation
    {
        public static IEnumerable<TimerData> CalculateTimerData(List<Tuple<string, int>> timerItems)
        {
            var byKey = timerItems.GroupBy(t => t.Item1);

            return byKey.Select(BuildTimerDataForTimerItemGroup);
        }

        private static TimerData BuildTimerDataForTimerItemGroup(
            IGrouping<string, Tuple<string, int>> timerByKeyGrouping)
        {
            var itemValues = timerByKeyGrouping.Select(t => t.Item2).ToList();

            // TODO: Calculate by looping through once rather than rely on linq
            return new TimerData
                       {
                           Key = timerByKeyGrouping.Key,
                           Lower = itemValues.Min(),
                           Upper = itemValues.Max(),
                           Sum = itemValues.Sum(),
                           Mean = itemValues.Average(),
                           Median = itemValues.Median(),
                           Count = itemValues.Count,
                           Std = itemValues.StandardDeviation(),
                           CountPs = itemValues.Count / (60000 / 1000)
                           // TODO: this should be the flush interval
                       };
        }
    }
}