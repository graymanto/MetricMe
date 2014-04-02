using System;
using System.Collections.Generic;

using FluentAssertions;

using MetricMe.Core;
using MetricMe.Core.Extensions;
using MetricMe.Server;
using MetricMe.Server.Backends;
using MetricMe.Server.Configuration;
using MetricMe.Server.Graphite;

using Moq;

using NUnit.Framework;

using Ploeh.AutoFixture;

namespace MetricMe.UnitTests.Server.Backends
{
    [TestFixture]
    public class GraphiteBackendTests
    {
        private Mock<IGraphiteClient> graphiteClient;

        private GraphiteBackend backend;

        private readonly Fixture fixture = new Fixture();

        private const int StandardMessageCount = 2;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SystemTime.FreezeTime();
        }

        [SetUp]
        public void Setup()
        {
            this.graphiteClient = new Mock<IGraphiteClient>();

            this.backend = new GraphiteBackend(this.graphiteClient.Object);
        }

        [Test]
        public void FlushCounter_SingleCount_ExpectValidGraphiteEntry()
        {
            var testMetricName = this.fixture.Create<string>();
            var testMetricValue = this.fixture.Create<int>();
            var testRate = this.fixture.Create<double>();

            string resultingMessage = null;
            this.graphiteClient.Setup(c => c.Send(It.IsAny<string>()))
                .Callback((string message) => resultingMessage = message);

            var collection = new MetricCollection();
            ((IList<MetricItem<int>>)collection.Counters).Add(
                new MetricItem<int> { Name = testMetricName, Value = testMetricValue });
            ((IList<MetricItem<double>>)collection.CounterRates).Add(
                new MetricItem<double> { Name = testMetricName, Value = testRate });

            this.backend.Flush(collection);

            resultingMessage.Should().NotBe(null);

            Console.WriteLine(resultingMessage);

            var messages = resultingMessage.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            messages.Length.Should().Be(2 + StandardMessageCount, "two messages should have been created");
            var expectedTimeStamp = Math.Truncate(SystemTime.UtcNow.ToJavaUnixTimestamp()).ToString();

            var rateMessage = messages[0];
            var rateMessageParts = rateMessage.Split(' ');

            rateMessageParts.Length.Should().Be(3);
            rateMessageParts[0].Should()
                .Be(
                    DefaultConfigurationValues.GraphiteGlobalPrefix.JoinWithDot(
                        DefaultConfigurationValues.GraphiteCounterPrefix).JoinWithDot(testMetricName) + ".rate");

            double parsedRate;
            double.TryParse(rateMessageParts[1], out parsedRate).Should().BeTrue();
            parsedRate.Should().Be(testRate);

            rateMessageParts[2].Should().Be(expectedTimeStamp);

            var countMessage = messages[1];
            var countMessageParts = countMessage.Split(' ');

            countMessageParts.Length.Should().Be(3);
            countMessageParts[0].Should()
                .Be(
                    DefaultConfigurationValues.GraphiteGlobalPrefix.JoinWithDot(
                        DefaultConfigurationValues.GraphiteCounterPrefix).JoinWithDot(testMetricName) + ".count");
            countMessageParts[1].Should().Be(testMetricValue.ToString());
            countMessageParts[2].Should().Be(expectedTimeStamp);
        }
    }
}