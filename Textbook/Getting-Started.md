# xUnitをはじめよう！

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

## 引数を利用したテスト

足し算、引き算ともに引数を利用して複数のテストを実施する。
FactをTheoryに変更し、実行する引数をInlineDataで指定する。

```cs
[Theory]
[InlineData(2, 2, 4)]
[InlineData(3, 3, 6)]
[InlineData(2, 2, 5)]
public void AddTest(int x, int y, int sum)
{
    Assert.Equal(sum, Calculator.Add(x, y));
}
```

実行すると3番目がエラーになる。

> [InlineData(2, 2, 5)]

を

> [InlineData(2, 3, 5)]

に修正して再実行する。全て正常になるのが見て取れる。
