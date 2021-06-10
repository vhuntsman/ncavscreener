using Xunit;

namespace NcavScreener.Test
{
    [Collection("Sample source collection")]
    public class ParserTest
    {
        SampleFixture testFixture;
        Parser parser;

        public ParserTest(SampleFixture fixture)
        {
            // Arrange
            testFixture = fixture;
            parser = new Parser();
        }

        [Fact]
        public void ExtractCrumb()
        {
            var crumb = parser.ExtractCrumb(testFixture.ScreenerSource);
            Assert.Equal("IpknhJuL/hh", crumb);
        }

        [Fact]
        public void GetNcavInvalid()
        {
            var ncav = parser.GetNcav(testFixture.NullBalanceSheet, "MSN");
            Assert.Equal(double.NaN, ncav);
        }

        [Fact]
        public void GetNcav()
        {
            var ncav = parser.GetNcav(testFixture.BalanceSheet, "MSN");
            Assert.Equal(1.61d, ncav);
        }
    }
}