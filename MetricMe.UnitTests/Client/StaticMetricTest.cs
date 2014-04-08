using System.Threading;

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

        [Test]
        public void Gauge_SendGaugeCount_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();
            var count = this.fixture.Create<int>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            Metrics.Gauge(testMetricName, count);

            var result = GetBitsFromMetricString(sentMetric);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Gauge);
            result.Number.Should().Be(count);
        }

        [Test]
        public void Set_SendSetString_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();
            var setKey = this.fixture.Create<string>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            Metrics.Set(testMetricName, setKey);

            var result = GetBitsFromMetricString(sentMetric, false);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Set);
            result.MetricValue.Should().Be(setKey);
        }

        [Test]
        public void Timer_TimedOperation_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();
            var time = this.fixture.Create<int>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            Metrics.Timer(testMetricName, time);

            var result = GetBitsFromMetricString(sentMetric);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Timing);
            result.Number.Should().Be(time);
        }

        [Test]
        public void Timer_UsingSection_CorrectMessageSent()
        {
            var testMetricName = this.fixture.Create<string>();

            string sentMetric = null;
            this.mockTransport.Setup(t => t.Send(It.IsAny<string>())).Callback<string>(s => sentMetric = s);

            using (Metrics.TimedSection(testMetricName))
            {
                Thread.Sleep(1);
            }

            mockTransport.Verify(m => m.Send(It.IsAny<string>()));

            var result = GetBitsFromMetricString(sentMetric);
            result.Name.Should().Be(testMetricName);
            result.MetricType.Should().Be(MetricType.Timing);
            result.Number.Should().BeGreaterThan(0);
        }

        private MetricBits GetBitsFromMetricString(string metricString, bool isNumeric = true)
        {
            metricString.Should().NotBeNull("a metric should have been sent");

            var bits = metricString.Split(':');
            var secondBits = bits[1].Split('|');

            secondBits.Length.Should().BeGreaterThan(1);
            int number = -1;
            if (isNumeric)
                int.TryParse(secondBits[0], out number).Should().BeTrue("metric string section should be numeric");

            return new MetricBits
                       {
                           Name = bits[0],
                           MetricType = MetricTypeParser.CreateFromString(secondBits[1]),
                           Number = isNumeric ? number : -1,
                           MetricValue = secondBits[0]
                       };
        }

        private class MetricBits
        {
            public string Name { get; set; }

            public MetricType MetricType { get; set; }

            public int Number { get; set; }

            public string MetricValue { get; set; }
        }
    }
}