using System.Diagnostics;
using System.Text;

namespace MetricMe.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The formatted string.</returns>
        [DebuggerStepThrough]
        public static string Formatted(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        /// <summary>
        /// Determines whether given string is null or empty.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A value indicating if the given string is null or empty.</returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Joins two strings together using the given join character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="stringToJoin">The string to join.</param>
        /// <param name="joinChar">The join character.</param>
        /// <returns>The joined string.</returns>
        [DebuggerStepThrough]
        public static string JoinWith(this string input, string stringToJoin, string joinChar)
        {
            if (stringToJoin.IsNullOrEmpty()) return input;
            if (input.IsNullOrEmpty()) return stringToJoin;

            return input.EndsWith(joinChar) ? input + stringToJoin : input + joinChar + stringToJoin;
        }

        /// <summary>
        /// Joins two strings together using a dot.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="stringToJoin">The string to join.</param>
        /// <returns>The dot joined string.</returns>
        [DebuggerStepThrough]
        public static string JoinWithDot(this string input, string stringToJoin)
        {
            return input.JoinWith(stringToJoin, ".");
        }

        /// <summary>
        /// Concats the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="stringToConcat">The string to concat.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string JoinTo(this string input, string stringToConcat)
        {
            return input + stringToConcat;
        }

        /// <summary>
        /// Returns the given string as a byte array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>A byte array representation of the string.</returns>
        [DebuggerStepThrough]
        public static byte[] AsByteArray(this string input, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(input);
        }
    }
}