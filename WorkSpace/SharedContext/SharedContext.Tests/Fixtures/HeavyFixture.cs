using System;
using System.Threading;

namespace SharedContext.Tests.Fixtures
{
    public class HeavyFixture : IDisposable
    {
        public HeavyFixture() => Thread.Sleep(TimeSpan.FromSeconds(2));

        public void Use()
        {
        }

        public void Dispose() => Thread.Sleep(TimeSpan.FromSeconds(2));
    }
}
