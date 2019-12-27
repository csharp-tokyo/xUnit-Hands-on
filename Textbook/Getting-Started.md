# xUnitをはじめよう！

この項は公式ドキュメント「[Getting Started with xUnit.net](https://xunit.net/docs/getting-started/netcore/cmdline)」をベースとしています。

## プロジェクトを用意する

ソリューション保管用のディレクトリを作成する

```cmd
mkdir GettingStarted
cd GettingStarted
```

空のソリューションを作成する

```cmd
dotnet new sln -n GettingStarted
```

テスト対象のプロジェクトを作成する

```cmd
dotnet new classlib -o GettingStarted
```

テストプロジェクトを作成する

```cmd
dotnet new xunit -o GettingStarted.Test
```

プロジェクトをソリューションに追加する

```cmd
dotnet sln GettingStarted.sln add GettingStarted/GettingStarted.csproj
dotnet sln GettingStarted.sln add GettingStarted.Test/GettingStarted.Test.csproj
```

テストプロジェクトからテスト対象のプロジェクトへ参照を追加する

## 初めてのxUnit

テスト対象のクラスを作成する

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
