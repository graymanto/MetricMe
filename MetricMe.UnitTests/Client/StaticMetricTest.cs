using FluentAssertions;

using MetricMe.Client;
using MetricMe.Client.Transport;
using MetricMe.Core;

using Moq;

using NUnit.Framework;

using Ploeh.AutoFixture;

namespace MetricMe.UnitTests.Client
{
    [TestFixture]
    public class StaticMetricTest
    {
        private readonly Fixture fixture = new Fixture();

        private Mock<IMetricTransport> mockTransport;

        [SetUp]
        public void Setup()
        {
            this.mockTransport = new Mock<IMetricTransport>();
            Metrics.Configure().WithTransport(this.mockTransport.Object);
        }

        [Test]
        public void Counter_IncrementCount_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            Metrics.Counter(testMetricName);

            var result = GetBitsFromMetricString(sentMetric);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Counter);
            result.Number.Should().Be(1);
        }

        [Test]
        public void Counter_IncrementCountAnyNumber_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();
            var count = this.fixture.Create<int>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            Metrics.Counter(testMetricName, count);

            var result = GetBitsFromMetricString(sentMetric);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Counter);
            result.Number.Should().Be(count);
        }

        private MetricBits GetBitsFromMetricString(string metricString)
        {
            metricString.Should().NotBeNull("a metric should have been sent");

            var bits = metricString.Split(':');
            var secondBits = bits[1].Split('|');

            secondBits.Length.Should().BeGreaterThan(1);
            int number;
            int.TryParse(secondBits[0], out number).Should().BeTrue("metric string section should be numeric");

            return new MetricBits
                       {
                           Name = bits[0],
                           MetricType = MetricTypeParser.CreateFromString(secondBits[1]),
                           Number = number
                       };
        }

        private class MetricBits
        {
            public string Name { get; set; }

            public MetricType MetricType { get; set; }

            public int Number { get; set; }
        }
    }
}