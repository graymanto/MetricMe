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
        public void Clear_WithNonEmptySet_ResultsInEmptyCollection()
        {
            var aggregator = new Aggregator();

            aggregator.Add("any.test.value:339|ms");
            aggregator.Add("general.test:1|c");
            aggregator.Add("a.gauge:444|g");

            aggregator.Clear();
            var collection = aggregator.GetAggregatedCollection();

            collection.Counters.Should().BeEmpty();
            collection.Gauges.Should().BeEmpty();
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
    }
}