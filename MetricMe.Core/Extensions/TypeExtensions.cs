using System;

namespace MetricMe.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the default value of a type.
        /// </summary>
        /// <param name="input">The type to get the value of.</param>
        /// <returns>The default value.</returns>
        public static dynamic DefaultValue(this Type input)
        {
            if (input.IsValueType)
            {
                return Activator.CreateInstance(input);
            }

            return null;
        }

        /// <summary>
        /// Creates a new instance of the type using the default constructor.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static dynamic CreateNew(this Type input)
        {
            return Activator.CreateInstance(input);
        }
    }
}