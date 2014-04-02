using System;

namespace MetricMe.Core
{
    public static class SystemTime
    {
        private static Func<DateTime> now;

        private static Func<DateTime> utcNow;

        static SystemTime()
        {
            Reset();
        }

        public static void Reset()
        {
            now = () => DateTime.Now;
            utcNow = () => DateTime.UtcNow;
        }

        public static void FreezeTime()
        {
            var frozenTime = DateTime.Now;

            SetNowProvider(() => frozenTime);
            SetUtcNowProvider(frozenTime.ToUniversalTime);
        }

        public static DateTime Now
        {
            get
            {
                return now();
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                return utcNow();
            }
        }

        public static void SetNowProvider(Func<DateTime> nowProvider)
        {
            now = nowProvider;
        }

        public static void SetUtcNowProvider(Func<DateTime> utcNowProvider)
        {
            utcNow = utcNowProvider;
        }
    }
}