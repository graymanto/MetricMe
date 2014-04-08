namespace MetricMe.Client.Messages
{
    public abstract class StatsDNumericMessage : StatsDMessage<int>
    {
        protected StatsDNumericMessage(string name, int messageValue, double? sampleRate = null)
            : base(name, messageValue, sampleRate)
        {
        }
    }
}