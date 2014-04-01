namespace MetricMe.Server
{
    public class TimerData
    {
        public string Key { get; set; }

        public double Std { get; set; }

        public double Upper { get; set; }

        public double Lower { get; set; }

        public int Count { get; set; }

        public int CountPs { get; set; }

        public int Sum { get; set; }

        public double Mean { get; set; }

        public int Median { get; set; }
    }
}