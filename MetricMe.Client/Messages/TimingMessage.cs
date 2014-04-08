using MetricMe.Core.Constants;

namespace MetricMe.Client.Messages
{
    public class TimingMessage : StatsDNumericMessage
    {
        public TimingMessage(string name, int messageValue, double? sampleRate = null)
            : base(name, messageValue, sampleRate)
        {
        }

        protected override string MessageType
        {
            get
            {
                return MetricStringSections.Timer;
            }
        }
    }
}