namespace MetricMe.Server.Extensions
{
    public static class StringExtensions
    {
        public static string Formatted(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}