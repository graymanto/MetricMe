using MetricMe.Core.Constants;

namespace MetricMe.Client.Messages
{
    public class GaugeMessage : StatsDMessage
    {
        public GaugeMessage(string name, int number, double? sampleRate = null)
            : base(name, number, sampleRate)
        {
        }

        protected override string MessageType
        {
            get
            {
                return MetricStringSections.Counter;
            }
        }
    }
}