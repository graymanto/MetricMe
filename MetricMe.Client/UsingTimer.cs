using System;
using System.Diagnostics;

namespace MetricMe.Client
{
    public class UsingTimer : IDisposable
    {
        private readonly string metricName;

        private readonly Stopwatch timer;

        private bool disposed;

        public UsingTimer(string metricName)
        {
            this.metricName = metricName;
            this.timer = new Stopwatch();
            this.timer.Start();
        }

        public void Dispose()
        {
            if (disposed) return;

            this.timer.Stop();
            Metrics.Timer(this.metricName, (int)this.timer.ElapsedMilliseconds);
            disposed = true;
        }
    }
}