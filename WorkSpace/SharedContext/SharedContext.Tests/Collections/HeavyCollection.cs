using SharedContext.Tests.Fixtures;
using Xunit;

namespace SharedContext.Tests.Collections
{
    [CollectionDefinition("Heavy collection")]
    public class HeavyCollection : ICollectionFixture<HeavyFixture>
    {
        // CollectionDefinitionを付与したクラスのみ作成すればよい
        // 特別な実装は不要
    }
}