using System;
using System.Diagnostics;
using System.Linq;
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
        /// Determines whether the specified string is empty or populated only by spaces.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsBlank(this string input)
        {
            return input.IsNullOrEmpty() || input.IsSpaces();
        }

        /// <summary>
        /// Determines whether the specified string is populated only by spaces.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsSpaces(this string input)
        {
            return input.All(c => c == ' ');
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
        /// Joins two strings together using a dot if the join string contains data or without dot otherwise.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="stringToJoin">The string to join.</param>
        /// <returns>The dot joined string.</returns>
        [DebuggerStepThrough]
        public static string JoinWithDotIfPopulated(this string input, string stringToJoin)
        {
            return stringToJoin.IsSpaces() ? input + stringToJoin : input.JoinWith(stringToJoin, ".");
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

        /// <summary>
        /// Splits the specified string on the given split string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="splitField">The split field.</param>
        /// <returns>A split string.</returns>
        [DebuggerStepThrough]
        public static string[] Split(this string input, string splitField)
        {
            return input.Split(new[] { splitField }, StringSplitOptions.None);
        }

        /// <summary>
        /// Splits the specified string on the given split string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="splitField">The split field.</param>
        /// <returns>A split string.</returns>
        [DebuggerStepThrough]
        public static string[] Split(this string input, string splitField, StringSplitOptions options)
        {
            return input.Split(new[] { splitField }, options);
        }
    }
}