using SharedContext.Tests.Fixtures;
using Xunit;

namespace SharedContext.Tests
{
    public class UnitTest3 : IClassFixture<AsyncHeavyFixture>
    {
        private readonly AsyncHeavyFixture _asyncHeavyFixture;

        public UnitTest3(AsyncHeavyFixture asyncAsyncHeavyFixture)
        {
            _asyncHeavyFixture = asyncAsyncHeavyFixture;
        }

        [Fact]
        public void Test() => _asyncHeavyFixture.Use();
    }
}