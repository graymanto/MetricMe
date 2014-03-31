using System.Linq;

using FluentAssertions;

using MetricMe.Server;
using MetricMe.Server.Extensions;

using NUnit.Framework;

namespace MetricMe.UnitTests.Server
{
    [TestFixture]
    public class AggregatorTests
    {
        [Test]
        public void Add_CounterMetric_CountsCorrectAmount()
        {
            var aggregator = new Aggregator();

            const string CounterKey = "mytest.metric";
            const int AccumulationAmount = 1;
            var counterMetric = "{0}:{1}|c".Formatted(CounterKey, AccumulationAmount);

            aggregator.Add(counterMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var counters = metrics.Counters.ToList();

            counters.Should().NotBeEmpty("a metric was added");
            counters.Should().HaveCount(1, "1 metric was added");

            var metricItem = counters.First();
            metricItem.Name.Should().Be(CounterKey);
            metricItem.Value.Should().Be(AccumulationAmount);
        }

        [Test]
        public void Add_CounterMetricGreaterThan1_CountsCorrectAmount()
        {
            var aggregator = new Aggregator();

            const string CounterKey = "mytest.metric";
            const int AccumulationAmount = 8;
            var counterMetric = "{0}:{1}|c".Formatted(CounterKey, AccumulationAmount);

            aggregator.Add(counterMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var counters = metrics.Counters.ToList();

            counters.Should().NotBeEmpty("a metric was added");
            counters.Should().HaveCount(1, "1 metric was added");

            var metricItem = counters.First();
            metricItem.Name.Should().Be(CounterKey);
            metricItem.Value.Should().Be(AccumulationAmount);
        }

        [Test]
        public void Add_MultipleCounters_CountsCorrectAmount()
        {
            var aggregator = new Aggregator();

            const string CounterKey = "mytest.metric";

            Enumerable.Range(1, 4).ForEach(
                i =>
                    {
                        var counterMetric = "{0}:{1}|c".Formatted(CounterKey, i);
                        aggregator.Add(counterMetric);
                    });

            var metrics = aggregator.GetAggregatedCollection();

            var counters = metrics.Counters.ToList();

            counters.Should().NotBeEmpty("a metric was added");
            counters.Should().HaveCount(1, "1 metric key only was added");

            var metricItem = counters.First();
            metricItem.Name.Should().Be(CounterKey);
            metricItem.Value.Should().Be(1 + 2 + 3 + 4);
        }

        [Test]
        public void Add_Gauge_GaugeIsSetCorrectly()
        {
            var aggregator = new Aggregator();

            const string GaugeKey = "mytest.metric";
            const int GaugeAmount = 8;
            var gaugeMetric = "{0}:{1}|g".Formatted(GaugeKey, GaugeAmount);

            aggregator.Add(gaugeMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var gauges = metrics.Gauges.ToList();

            gauges.Should().NotBeEmpty("a gauge metric was added");
            gauges.Should().HaveCount(1, "1 gauge metric was added");

            var metricItem = gauges.First();
            metricItem.Name.Should().Be(GaugeKey);
            metricItem.Value.Should().Be(GaugeAmount);
        }

        [Test]
        public void Add_GaugeAccumulatePositive_GaugeIsSetCorrectly()
        {
            var aggregator = new Aggregator();

            const string GaugeKey = "mytest.metric";
            const int GaugeAmount = 8;
            const int GaugeAccumulationAmount = 15;

            var gaugeMetric = "{0}:{1}|g".Formatted(GaugeKey, GaugeAmount);
            var gaugeAccumulationMetric = "{0}:+{1}|g".Formatted(GaugeKey, GaugeAccumulationAmount);

            aggregator.Add(gaugeMetric);
            aggregator.Add(gaugeAccumulationMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var gauges = metrics.Gauges.ToList();

            gauges.Should().NotBeEmpty("a gauge metric was added");
            gauges.Should().HaveCount(1, "1 gauge metric was added");

            var metricItem = gauges.First();
            metricItem.Name.Should().Be(GaugeKey);
            metricItem.Value.Should().Be(GaugeAmount + GaugeAccumulationAmount);
        }

        [Test]
        public void Add_GaugeAccumulateNegative_GaugeIsSetCorrectly()
        {
            var aggregator = new Aggregator();

            const string GaugeKey = "mytest.metric";
            const int GaugeAmount = 100;
            const int GaugeAccumulationAmount = 34;

            var gaugeMetric = "{0}:{1}|g".Formatted(GaugeKey, GaugeAmount);
            var gaugeAccumulationMetric = "{0}:-{1}|g".Formatted(GaugeKey, GaugeAccumulationAmount);

            aggregator.Add(gaugeMetric);
            aggregator.Add(gaugeAccumulationMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var gauges = metrics.Gauges.ToList();

            gauges.Should().NotBeEmpty("a gauge metric was added");
            gauges.Should().HaveCount(1, "1 gauge metric was added");

            var metricItem = gauges.First();
            metricItem.Name.Should().Be(GaugeKey);
            metricItem.Value.Should().Be(GaugeAmount - GaugeAccumulationAmount);
        }

        [Test]
        public void ClearAggregatedValues_WithNonEmptySet_ResultsInEmptyCollection()
        {
            var aggregator = new Aggregator();

            aggregator.Add("any.test.value:339|ms");
            aggregator.Add("general.test:1|c");

            aggregator.ClearAggregatedValues();
            var collection = aggregator.GetAggregatedCollection();

            collection.Counters.Should().BeEmpty();
            collection.Sets.Should().BeEmpty();
            collection.Timers.Should().BeEmpty();
        }

        [Test]
        public void GetCollection_NoMetrics_ExpectEmptyCollection()
        {
            var aggregator = new Aggregator();
            var collection = aggregator.GetAggregatedCollection();

            collection.Counters.Should().BeEmpty();
            collection.Gauges.Should().BeEmpty();
            collection.Sets.Should().BeEmpty();
            collection.Timers.Should().BeEmpty();
        }

        [Test]
        public void Add_SetWithSingleValue_ExpectOneCountOfOne()
        {
            var aggregator = new Aggregator();

            const string SetKey = "mytest.metric";
            const int SetAmount = 4375;
            var setMetric = "{0}:{1}|s".Formatted(SetKey, SetAmount);

            aggregator.Add(setMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var sets = metrics.Sets.ToList();

            sets.Should().NotBeEmpty("a set metric was added");
            sets.Should().HaveCount(1, "1 set metric was added");

            var metricItem = sets.First();
            metricItem.Name.Should().Be(SetKey);
            metricItem.Value.Should().Be(1);
        }

        [Test]
        public void Add_SetWithMetricSentTwice_ExpectOneCountOfOne()
        {
            var aggregator = new Aggregator();

            const string SetKey = "mytest.metric";
            const int SetAmount = 4375;
            var setMetric = "{0}:{1}|s".Formatted(SetKey, SetAmount);

            aggregator.Add(setMetric);
            aggregator.Add(setMetric);

            var metrics = aggregator.GetAggregatedCollection();

            var sets = metrics.Sets.ToList();

            sets.Should().NotBeEmpty("a set metric was added");
            sets.Should().HaveCount(1, "1 set metric was added");

            var metricItem = sets.First();
            metricItem.Name.Should().Be(SetKey);
            metricItem.Value.Should().Be(1);
        }

        [Test]
        public void Add_SetWithTwoValues_ExpectOneCountOfTwo()
        {
            var aggregator = new Aggregator();

            const string SetKey = "mytest.metric";
            const int SetAmount = 4375;
            const int SetAmount2 = 3482;
            var setMetric = "{0}:{1}|s".Formatted(SetKey, SetAmount);
            var setMetric2 = "{0}:{1}|s".Formatted(SetKey, SetAmount2);

            aggregator.Add(setMetric);
            aggregator.Add(setMetric2);

            var metrics = aggregator.GetAggregatedCollection();

            var sets = metrics.Sets.ToList();

            sets.Should().NotBeEmpty("a set metric was added");
            sets.Should().HaveCount(1, "1 set metric was added");

            var metricItem = sets.First();
            metricItem.Name.Should().Be(SetKey);
            metricItem.Value.Should().Be(2);
        }

        [Test]
        public void Add_SetWithTwoKeys_ExpectTwoCountsOfOne()
        {
            var aggregator = new Aggregator();

            const string SetKey = "mytest.metric";
            const string SetKey2 = "mytest.metric2";
            const int SetAmount = 4375;
            var setMetric = "{0}:{1}|s".Formatted(SetKey, SetAmount);
            var setMetric2 = "{0}:{1}|s".Formatted(SetKey2, SetAmount);

            aggregator.Add(setMetric);
            aggregator.Add(setMetric2);

            var metrics = aggregator.GetAggregatedCollection();

            var sets = metrics.Sets.ToList();

            sets.Should().NotBeEmpty("a set metric was added");
            sets.Should().HaveCount(2, "2 set metrics were added");

            var metricItem = sets.First();
            var metricItem2 = sets.Skip(1).First();
            metricItem.Name.Should().Be(SetKey);
            metricItem.Value.Should().Be(1);
            metricItem2.Name.Should().Be(SetKey2);
            metricItem2.Value.Should().Be(1);
        }
    }
}