# デバッグ出力

この項は公式ドキュメント「[Capturing Output](https://xunit.net/docs/capturing-output)」をベースとしています。

xUnit.netではテスト実行にデバッグ支援などのために利用できる出力のキャプチャー機構が用意されています。xUnit.netでは二つの仕組みが用意されています。

1. Unitテストでの出力のキャプチャー
2. xUnit.net機能拡張クラスでの主力のキャプチャー

ここでは前者のみ扱います。後者は[公式ドキュメント](https://xunit.net/docs/capturing-output)を参照してください。

## Unitテストでの出力のキャプチャー

それではHelloXUnit.CapturingOutputTestプロジェクトのUnitTest1.csクラスを開いて、次のように実装してください。

```cs
using Xunit;
using Xunit.Abstractions;

namespace HelloXUnit.CapturingOutputTest
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("This is output from {0}", "Constructor");
        }

        [Fact]
        public void Test1()
        {
            _output.WriteLine("This is output from {0}", "Test1");
        }
    }
}
```

ITestOutputHelperをコンストラクタでインジェクションしてもらい利用します。ITestOutputHelperはConsoleクラスと同様の出力フォーマットをサポートしています。
