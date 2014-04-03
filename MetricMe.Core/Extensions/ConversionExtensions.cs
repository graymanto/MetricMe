using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MetricMe.Core.Extensions
{
    /// <summary>
    /// Extensions for converting values.
    /// </summary>
    public static class ConversionExtensions
    {
        /// <summary>
        /// Converts a string to a given type or returns a default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T ConvertTo<T>(this string key, T defaultValue = default(T))
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {
                return converter.CanConvertFrom(typeof(string)) ? (T)converter.ConvertFromString(key) : defaultValue;
            }
            catch (NotSupportedException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts a string to a given type or returns a default value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static dynamic ConvertTo(this string key, Type conversionType)
        {
            var converter = TypeDescriptor.GetConverter(conversionType);

            try
            {
                return converter.CanConvertFrom(typeof(string))
                           ? converter.ConvertFromString(key)
                           : conversionType.DefaultValue();
            }
            catch (NotSupportedException)
            {
                return conversionType.DefaultValue();
            }
        }
    }
}