using System.Collections.Generic;

using FluentAssertions;

using MetricMe.Server.Extensions;

using NUnit.Framework;

namespace MetricMe.UnitTests.Server.Extensions
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void GetOrDefault_EmptyDic_ReturnsDefaultInt()
        {
            var dic = new Dictionary<string, int>();
            var result = dic.GetOrDefault("AnyOldKey");

            result.Should().Be(default(int));
        }

        [Test]
        public void IncrementCountForKey_NoPreviousEntry_ExpectIncrements()
        {
            const int IncrementAmount = 20;
            const string TestKey = "AnyOldKey";

            var dic = new Dictionary<string, int>();
            dic.IncrementCountForKey(TestKey, IncrementAmount);

            dic.Should().ContainKey(TestKey);
            dic[TestKey].Should().Be(IncrementAmount);
        }

        [Test]
        public void IncrementCountForKey_WithPreviousEntry_ExpectSummedIncrements()
        {
            const int IncrementAmount = 20;
            const int StartingValue = 7;
            const string TestKey = "AnyOldKey";

            var dic = new Dictionary<string, int> { { TestKey, StartingValue } };
            dic.IncrementCountForKey(TestKey, IncrementAmount);

            dic.Should().ContainKey(TestKey);
            dic[TestKey].Should().Be(IncrementAmount + StartingValue);
        }
    }
}