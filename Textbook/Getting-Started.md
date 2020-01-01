# xUnitをはじめよう！

この項は公式ドキュメント「[Getting Started with xUnit.net](https://xunit.net/docs/getting-started/netcore/cmdline)」をベースとしています。

## ハンズオンワークスペースを作成する

まずは本ハンズオンを実行するためのワークスペース、ワークソリューションを作成しましょう。

rootフォルダの以下のコマンドを実行してください。

- Windows ： CreateWorkSpace.cmd

WorkSpaceフォルダが作成され、その中にHelloXUnitソリューションが作成されます。ソリューション内にはつぎの二つのプロジェクトが含まれています。

1. HelloXUnit
2. HelloXUnit.Test

HelloXUnitがテスト対象のプロジェクトで、HelloXUnit.Testがテストする側のプロジェクトです。

HelloXUnit.Testにはデフォルトで一つのテストメソッドが実装されています。まずはそのテストを実行し、ワークスペースが正しく作られたことを確認してみましょう。

以下のコマンドでコンソールから実行します。

```cmd
dotnet test
```

TODO：個々のIDEからの実行方法（VSとVS for Mac）

これで準備は完了です。それでは早速始めましょう。

## 初めてのxUnit

まずはテスト対象のクラスを作成します。

HelloXUnitプロジェクトにCalculatorクラスを作成し、以下のように記述してください。

```cs
namespace HelloXUnit
{
    public class Calculator
    {
        public static int Add(int x, int y) => x + y;
        public static int Subtract(int x, int y) => x - y;
        public static bool IsOdd(int value) => value % 2 == 1;
    }
}
```

足し算・引き算そして奇数判定を行うメソッドが実装されています。

続いてHelloXUnit.TestプロジェクトにCalculatorクラスをテストするCalculatorFixture

テストクラスを作成する

```cs
using Xunit;

namespace HelloXUnit.Test
{
    public class CalculatorFixture
    {
        [Fact]
        public void AddTest()
        {
            Assert.Equal(4, Calculator.Add(2, 2));
        }

        [Fact]
        public void SubtractTest()
        {
            Assert.Equal(1, Calculator.Subtract(3, 2));
        }
    }
}
```

xUnitでは様々なAssertionメソッドで標準で用意されています。[Assertionチートシート](Assertion-CheatSheet.md)を用意してありますので、そちらを御覧ください。

## Theoryを利用したテスト

Factは常に正となるテストを実施するが、特定のデータセットでのみ当てはまるテストはTheoryを使う。対象のデータセットの指定は複数あるが、まずはInlineDataを利用する。

実際にIsOddメソッド（奇数判定メソッド）で試してみよう。

```cs
[Theory]
[InlineData(3)]
[InlineData(4)]
public void IsOddWhenTrue(int value)
{
    Assert.True(Calculator.IsOdd(value));
}

[Theory]
[InlineData(2)]
[InlineData(4)]
public void IsOddWhenFalse(int value)
{
    Assert.False(Calculator.IsOdd(value));
}
```

実行するとIsOddWhenTrueの2番目がエラーになる。

> [InlineData(4)]

を

> [InlineData(5)]

に修正して再実行する。全て正常になるのが見て取れる。

[戻る](../README.md)
