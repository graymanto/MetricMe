namespace MetricMe.Server.Extensions
{
    public static class StringExtensions
    {
        public static string Formatted(this string input, params object[] args)
        {
            return string.Format(input, args);
        }
    }
}