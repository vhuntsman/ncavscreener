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
            parser = new Parser(testFixture.Source);
        }

        [Fact]
        public void ExtractPageLinks()
        {
            // Act
            var pageLinks = parser.ExtractPageLinks();
            // Assert
            Assert.True(pageLinks.Count == 9);
        }

        [Fact]
        public void ExtractStockLinks()
        {
            // Act
            var stockLinks = parser.ExtractStockLinks();
            // Assert
            Assert.True(stockLinks.Count == 10);
        }
    }
}