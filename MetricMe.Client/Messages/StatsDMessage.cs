using MetricMe.Core.Extensions;

namespace MetricMe.Client.Messages
{
    public abstract class StatsDMessage<TMessage>
    {
        private readonly string name;

        private readonly TMessage messageValue;

        private readonly double? sampleRate;

        protected abstract string MessageType { get; }

        protected StatsDMessage(string name, TMessage messageValue, double? sampleRate = null)
        {
            this.name = name;
            this.messageValue = messageValue;
            this.sampleRate = sampleRate;
        }

        public override string ToString()
        {
            var metric = "{0}:{1}|{2}".Formatted(this.name, this.messageValue, MessageType);

            return this.sampleRate.HasValue ? metric + "|@@" + this.sampleRate : metric;
        }
    }
}