using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace NcavScreener.Test
{
    [ExcludeFromCodeCoverage]
    [Collection("Sample source collection")]
    public class ParserTest
    {
        private SampleFixture _testFixture;
        private Parser _parser;

        public ParserTest(SampleFixture fixture)
        {
            // Arrange
            _testFixture = fixture;
            _parser = new Parser();
        }

        [Fact]
        public void ExtractCrumb()
        {
            var crumb = _parser.ExtractCrumb(_testFixture.ScreenerSource);
            Assert.Equal("IpknhJuL/hh", crumb);
        }

        [Fact]
        public void GetNcavInvalid()
        {
            var ncav = _parser.GetNcav(_testFixture.NullBalanceSheet, "MSN");
            Assert.Equal(double.NaN, ncav);

            // No shares outstanding
            ncav = _parser.GetNcav(_testFixture.BalanceSheetNoOutstanding, "MSN");
            Assert.Equal(double.NaN, ncav);
        }

        [Fact]
        public void GetNcav()
        {
            var ncav = _parser.GetNcav(_testFixture.BalanceSheet, "MSN");
            Assert.Equal(1.61d, ncav);
        }
    }
}