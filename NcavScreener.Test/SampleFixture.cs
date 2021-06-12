using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace NcavScreener.Test
{
    [ExcludeFromCodeCoverage]
    public class SampleFixture : IDisposable
    {
        public string ScreenerSource { get; }
        public string BalanceSheet { get; }
        public string NullBalanceSheet { get; }
        public string BalanceSheetNoOutstanding { get; }

        public SampleFixture()
        {
            // ... initialize data in the test database ...
            ScreenerSource = File.ReadAllText("ScreenerSource.html");
            BalanceSheet = File.ReadAllText("BalanceSheet.html");
            NullBalanceSheet = File.ReadAllText("NullBalanceSheet.html");
            BalanceSheetNoOutstanding = File.ReadAllText("BalanceSheetNoOutstanding.html");
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        [CollectionDefinition("Sample source collection")]
        public class DatabaseCollection : ICollectionFixture<SampleFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }
    }
}
