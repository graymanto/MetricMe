using System.Collections.Generic;

namespace MetricMe.Core.Extensions
{
    /// <summary>
    /// Enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concats all the strings in the enumerable to one joined value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The combined string.</returns>
        public static string Combine(this IEnumerable<string> input)
        {
            return string.Join(string.Empty, input);
        }
    }
}