using System;

namespace MetricMe.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a datetime to a unix timestamp.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static double ToUnixTimestamp(this DateTime input)
        {
            return (input - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        /// <summary>
        /// Converts a datetime to a java style unix timestamp.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static double ToJavaUnixTimestamp(this DateTime input)
        {
            return (input - new DateTime(1970, 1, 1).ToLocalTime()).TotalMilliseconds;
        }
    }
}