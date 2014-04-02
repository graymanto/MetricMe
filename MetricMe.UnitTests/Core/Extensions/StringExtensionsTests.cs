using FluentAssertions;

using MetricMe.Core.Extensions;

using NUnit.Framework;

using Ploeh.AutoFixture;

namespace MetricMe.UnitTests.Core.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        private readonly Fixture fixture = new Fixture();

        [Test]
        public void IsBlank_EmptyString_ExpectTrue()
        {
            string.Empty.IsBlank().Should().BeTrue("an empty string was passed");
        }

        [Test]
        public void IsBlank_NullString_ExpectTrue()
        {
            const string NullString = null;
            NullString.IsBlank().Should().BeTrue("an null string was passed");
        }

        [Test]
        public void IsBlank_SingleSpaceString_ExpectTrue()
        {
            const string SpaceString = " ";
            SpaceString.IsBlank().Should().BeTrue("a single space only string was passed");
        }

        [Test]
        public void IsBlank_MultiSpaceString_ExpectTrue()
        {
            const string SpaceString = "  ";
            SpaceString.IsBlank().Should().BeTrue("a string containing only spaces was passed");
        }

        [Test]
        public void IsSpaces_SingleSpaceString_ExpectTrue()
        {
            const string SpaceString = " ";
            SpaceString.IsSpaces().Should().BeTrue("a single space only string was passed");
        }

        [Test]
        public void IsSpaces_MultiSpaceString_ExpectTrue()
        {
            const string SpaceString = "  ";
            SpaceString.IsSpaces().Should().BeTrue("a string containing only spaces was passed");
        }

        [Test]
        public void IsSpaces_NonSpaceString_ExpectFalse()
        {
            var testString = this.fixture.Create<string>();
            testString.IsSpaces().Should().BeFalse("a non space string was passed");
        }

        [Test]
        public void JoinWithDotIfPopulated_JoinStringSpaces_JoinsWithoutDot()
        {
            const string SpaceString = "  ";
            var joinOnto = this.fixture.Create<string>();

            joinOnto.JoinWithDotIfPopulated(SpaceString).Should().Be(joinOnto + SpaceString);
        }

        [Test]
        public void JoinWithDotIfPopulated_JoinStringNonBlank_JoinsWithDot()
        {
            var joinOnto = this.fixture.Create<string>();
            var joined = this.fixture.Create<string>();

            joinOnto.JoinWithDotIfPopulated(joined).Should().Be(joinOnto + "." + joined);
        }
    }
}