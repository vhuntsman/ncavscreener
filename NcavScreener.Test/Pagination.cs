using Xunit;

namespace NcavScreener.Test
{
    [Collection("Sample source collection")]
    public class Pagination
    {
        SampleFixture testFixture;
        Parser parser;

        public Pagination(SampleFixture fixture)
        {
            // Arrange
            testFixture = fixture;
            parser = new Parser(testFixture.Source);
        }

        [Fact]
        public void TestExtractPageLinks()
        {
            Assert.True(parser.ExtractPageLinks().Count == 10);
        }
    }
}