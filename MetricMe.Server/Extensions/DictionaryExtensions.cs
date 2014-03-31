using System.Collections.Generic;

namespace MetricMe.Server.Extensions
{
    public static class DictionaryExtensions
    {
        public static void IncrementCountForKey(this Dictionary<string, int> input, string key, int count)
        {
            input[key] = input.GetOrDefault(key) + count;
        }

        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key)
        {
            TValue output;
            input.TryGetValue(key, out output);

            return output;
        }
    }
}