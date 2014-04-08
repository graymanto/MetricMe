using MetricMe.Core.Constants;

namespace MetricMe.Client.Messages
{
    public class SetMessage : StatsDMessage<string>
    {
        public SetMessage(string name, string messageValue, double? sampleRate = null)
            : base(name, messageValue, sampleRate)
        {
        }

        protected override string MessageType
        {
            get
            {
                return MetricStringSections.Set;
            }
        }
    }
}