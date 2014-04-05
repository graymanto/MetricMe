using MetricMe.Core.Extensions;

namespace MetricMe.Client.Messages
{
    public abstract class StatsDMessage
    {
        private readonly string name;

        private readonly int number;

        private readonly double? sampleRate;

        protected abstract string MessageType { get; }

        protected StatsDMessage(string name, int number, double? sampleRate = null)
        {
            this.name = name;
            this.number = number;
            this.sampleRate = sampleRate;
        }

        public override string ToString()
        {
            var metric = "{0}:{1}|{2}".Formatted(this.name, this.number, this.MessageType);

            return this.sampleRate.HasValue ? metric + "|@@" + this.sampleRate : metric;
        }
    }
}