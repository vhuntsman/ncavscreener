using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace NcavScreener.Test
{
    public class SampleFixture : IDisposable
    {
        public string Source { get; }

        public SampleFixture()
        {
            // ... initialize data in the test database ...
            Source = File.ReadAllText("SampleSource.html");
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
