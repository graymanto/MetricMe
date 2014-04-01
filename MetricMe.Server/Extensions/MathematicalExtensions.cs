using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricMe.Server.Extensions
{
    public static class MathematicalExtensions
    {
        public static int Median(this IEnumerable<int> input)
        {
            var items = input.OrderBy(i => i).ToList();

            if (!items.Any())
            {
                return 0;
            }

            var middle = (items.Count - 1) / 2;
            var possibleMedian = items.ElementAt(middle);

            return items.Count % 2 == 0 ? (possibleMedian + items.ElementAt(middle + 1)) / 2 : possibleMedian;
        }

        public static double StandardDeviation(this IEnumerable<int> input)
        {
            double m = 0.0;
            double s = 0.0;
            int k = 1;
            foreach (var value in input)
            {
                double tmpM = m;
                m += (value - tmpM) / k;
                s += (value - tmpM) * (value - m);
                k++;
            }
            return Math.Sqrt(s / (k - 1));
        }
    }
}