using FluentAssertions;

using MetricMe.Server;
using MetricMe.Server.Extensions;

using NUnit.Framework;

namespace MetricMe.UnitTests.Server
{
    [TestFixture]
    public class MetricParserTests
    {
        [Test]
        public void Parse_WithCounterMetric_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int AccumulationAmount = 4;
            var counterMetric = "{0}:{1}|c".Formatted(CounterKey, AccumulationAmount);

            var parseResults = MetricParser.Parse(counterMetric);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(AccumulationAmount, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Counter, "counting metric was passed");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithCounterMetricWithSampling_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int AccumulationAmount = 4;
            const double SampleRate = 0.1;
            var counterMetric = "{0}:{1}|c|@{2}".Formatted(CounterKey, AccumulationAmount, SampleRate);

            var parseResults = MetricParser.Parse(counterMetric);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(AccumulationAmount, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Counter, "counting metric was passed");
            parseResults.SampleRate.HasValue.Should().BeTrue("sample rate was passed");
            parseResults.SampleRate.Should().Be(SampleRate, "the given sample rate was set");
        }

        [Test]
        public void Parse_WithTimingMetric_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int TimingAmount = 300;
            var metricString = "{0}:{1}|ms".Formatted(CounterKey, TimingAmount);

            var parseResults = MetricParser.Parse(metricString);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(TimingAmount, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Timing, "timing metric was passed");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithGaugeMetric_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int GaugeValue = 300;
            var metricString = "{0}:{1}|g".Formatted(CounterKey, GaugeValue);

            var parseResults = MetricParser.Parse(metricString);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(GaugeValue, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Gauge, "timing metric was passed");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithGaugeMetricWithDirectionNegative_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int GaugeValue = 300;
            var metricString = "{0}:-{1}|g".Formatted(CounterKey, GaugeValue);

            var parseResults = MetricParser.Parse(metricString);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(GaugeValue, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Gauge, "timing metric was passed");
            parseResults.GaugeDirection.Should().Be(GaugeDirection.Minus, "gauge direction was specified");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithGaugeMetricWithDirectionPositive_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const int GaugeValue = 300;
            var metricString = "{0}:+{1}|g".Formatted(CounterKey, GaugeValue);

            var parseResults = MetricParser.Parse(metricString);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.Value.Should().Be(GaugeValue, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Gauge, "timing metric was passed");
            parseResults.GaugeDirection.Should().Be(GaugeDirection.Plus, "gauge direction was specified");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithSetMetric_IsValidAndCorrect()
        {
            const string CounterKey = "mytest.metric";
            const string SetValue = "300";
            var metricString = "{0}:{1}|s".Formatted(CounterKey, SetValue);

            var parseResults = MetricParser.Parse(metricString);
            parseResults.IsValid.Should().Be(true, "given metric should be valid");
            parseResults.Name.Should().Be(CounterKey, "metric name should have been parsed correctly");
            parseResults.ValueString.Should().Be(SetValue, "metric value should have been parsed correctly");
            parseResults.Type.Should().Be(MetricType.Set, "timing metric was passed");
            parseResults.SampleRate.HasValue.Should().BeFalse("no sample rate was passed");
        }

        [Test]
        public void Parse_WithRandomString_IsNotValid()
        {
            var parseResults = MetricParser.Parse("sjkdl;kjs fjlsdfshfjh");

            parseResults.IsValid.Should().BeFalse("an invalid metric string was passed");
        }
    }
}
