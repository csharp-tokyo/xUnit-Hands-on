using System;
using System.Threading.Tasks;
using Xunit;

namespace SharedContext.Tests.Fixtures
{
    public class AsyncHeavyFixture : IAsyncLifetime
    {
        public Task InitializeAsync() => Task.Delay(TimeSpan.FromSeconds(2));

        public void Use()
        {
        }

        public Task DisposeAsync() => Task.Delay(TimeSpan.FromSeconds(2));
    }
}