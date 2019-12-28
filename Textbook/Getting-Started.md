# xUnitをはじめよう！

この項は公式ドキュメント「[Getting Started with xUnit.net](https://xunit.net/docs/getting-started/netcore/cmdline)」をベースとしています。

## プロジェクトを用意する

WorkSpaceフォルダの以下を実行して、テスト用のプロジェクトを作成しましょう。

- Windows ： Getting-Started.cmd

WorksSpaceフォルダに作成されたGetting-Startedフォルダ内のGetting-Started.slnを開きましょう。

ソリューション内にはつぎの二つのプロジェクトが含まれています。

1. GettingStarted
2. GettingStarted.Test

GettingStartedがテスト対象のプロジェクトで、GettingStarted.Testがテストする側のプロジェクトです。それでは早速始めましょう。

## 初めてのxUnit

まずはテスト対象のクラスを作成します。

GettingStartedプロジェクトにCalculatorクラスを作成し、以下のように記述してください。

```cs
namespace GettingStarted
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

続いてGettingStarted.TestプロジェクトにCalculatorクラスをテストするCalculatorFixture

テストクラスを作成する

```cs
using Xunit;

namespace GettingStarted.Test
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
