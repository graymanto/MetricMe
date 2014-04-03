using System;

using FluentAssertions;

using MetricMe.Core.Extensions;

using NUnit.Framework;

using Ploeh.AutoFixture;

namespace MetricMe.UnitTests.Core.Extensions
{
    [TestFixture]
    public class ConversionExtensionsTests
    {
        private readonly Fixture fixture = new Fixture();

        [Test]
        public void ConvertTo_StringToInt_ExpectCorrectConversion()
        {
            var testInt = this.fixture.Create<int>();
            testInt.ToString().ConvertTo<int>().Should().Be(testInt);
        }

        [Test]
        public void ConvertTo_MultiTypeTest_ExpectCorrectConversion()
        {
            TestForType<bool>();
            TestForType<double>();
            TestForType<decimal>();
        }

        [Test]
        public void ConvertToWithType_StringToInt_ExpectCorrectConversion()
        {
            var test = this.fixture.Create<int>();
            var converted = test.ToString().ConvertTo(typeof(int));
            Assert.AreEqual(converted, test);
        }

        private void TestForType<T>()
        {
            var test = this.fixture.Create<T>();
            test.ToString().ConvertTo<T>().Should().Be(test, "type {0} should be convertible", typeof(T).Name);
        }
    }
}