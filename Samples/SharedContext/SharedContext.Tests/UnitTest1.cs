using System;
using SharedContext.Tests.Fixtures;
using Xunit;

namespace SharedContext.Tests
{
    [Collection("Heavy collection")]
    public class UnitTest1 : IDisposable
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest1(HeavyFixture heavyFixture)
        {
            _heavyFixture = heavyFixture;
        }

        [Fact]
        public void Test1() => _heavyFixture.Use();

        [Fact]
        public void Test2() => _heavyFixture.Use();

        public void Dispose()
        {
            //_heavyFixture.Dispose();
        }
    }


}