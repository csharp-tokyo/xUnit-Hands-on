# xUnitをはじめよう！

この項は公式ドキュメント「[Getting Started with xUnit.net](https://xunit.net/docs/getting-started/netcore/cmdline)」をベースとしています。

## ハンズオンワークスペースを作成する

まずは本ハンズオンを実行するためのワークスペース、ワークソリューションを作成しましょう。ここではコンソールで作業しますが、各種IDEを利用していただいても問題ありません。

まず本リポジトリのrootフォルダ内にあるWorkSpaceフォルダをコンソールで開き、ソリューションを作成します。

```cmd
dotnet new sln -o HelloXUnit
```

ソリューションを作成したら、作成されたフォルダに移動します。

```cmd
cd HelloXUnit
```

ソリューションには二つのプロジェクトを作成します。

1. テスト対象が含まれるプロジェクト
2. テストコードが含まれるプロジェクト

まずはテスト対象から作成します。

```cmd
dotnet new classlib -o HelloXUnit
```

作成したら、プロジェクトをソリューションに追加します。

```cmd
dotnet sln add HelloXUnit/HelloXUnit.csproj
```

同様に、テストコード側のプロジェクトも作成し、ソリューションへ追加します。

```cmd
dotnet new xunit -o HelloXUnit.Tests
dotnet sln HelloXUnit.sln add HelloXUnit.Tests/HelloXUnit.Tests.csproj
```

テストコードはテスト対象を参照する必要がありますので、テストコードプロジェクト側からの参照を追加しましょう。

```cmd
dotnet add HelloXUnit.Tests/HelloXUnit.Tests.csproj reference HelloXUnit/HelloXUnit.csproj
```


HelloXUnit.Testにはデフォルトで一つのテストメソッドが実装されています。まずはそのテストを実行し、ワークスペースが正しく作られたことを確認してみましょう。

以下のコマンドでコンソールから実行します。

```cmd
dotnet test
```

これで準備は完了です。それでは早速始めましょう。以降は各自の環境に合わせた開発環境を利用ください。

## 初めてのxUnit

まずはテスト対象のクラスを作成します。

HelloXUnitプロジェクトにCalculatorクラスを作成し、以下のように記述してください。

```cs
using System;

namespace HelloXUnit
{
    public static class Calculator
    {
        public static int Add(int x, int y) => throw new NotImplementedException();
        public static int Subtract(int x, int y) => throw new NotImplementedException();
        public static bool IsOdd(int value) => throw new NotImplementedException();
    }
}
```

足し算・引き算そして奇数判定を行うメソッドが定義されていますが、まだ実装はされていません。

続いてHelloXUnit.TestプロジェクトにCalculatorクラスをテストするCalculatorTestsクラスを作成します。

```cs
using Xunit;

namespace HelloXUnit.Tests
{
    public class CalculatorTests
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

xUnitではテストメソッドに「Fact」もしくは「Theory」属性を宣言してテストを実装します。

上記コードは足し算と引き算のテストコードが実装されています。それぞれ実行結果を、AssertクラスのAssertionメソッドであるEqualメソッドを利用して評価しています。

xUnitでは様々なAssertionメソッドで標準で用意されています。[Assertionチートシート](Assertion-CheatSheet.md)を用意してありますので、そちらを御覧ください。

ではテストを実行してみましょう。

TODO:各種IDEでもテストの実行方法の追記

もちろんメソッドが実装されていないため、実行結果はエラーになります。

ではCalculatorクラスに足し算と引き算を実装しましょう。

```cs
namespace HelloXUnit
{
    public static class Calculator
    {
        public static int Add(int x, int y) => x + y;
        public static int Subtract(int x, int y) => x - y;
        public static bool IsOdd(int value) => throw new NotImplementedException();
    }
}
```

実装できたら再度テストを実行します。テストが正常に実行されたことが確認できましたか？

これが最もシンプルなテストケースの実装になります。

## Theoryを利用したテスト

xUnit.netではテストメソッドに宣言する属性として「Fact」と「Teory」の二つがあります。それぞれ日本語に訳すと、事実と理論という意味になるでしょうが、そこをあまり深く掘り下る意味はないように思います。

分かりやすいのは、TheoryはFactとは異なり、複数のデータセットを定義することで一つのテストメソッドで複数のテストを実行できる点にあります。


実際にIsOddメソッド（奇数判定メソッド）で試してみよう。CalculatorTestsに次のようなテストメソッドを実装します。

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

実装したらエラーとなることを確認したのち、CalculatorのIsOddメソッドを実装しましょう。

```cs
namespace HelloXUnit
{
    public static class Calculator
    {
        public static int Add(int x, int y) => x + y;
        public static int Subtract(int x, int y) => x - y;
        public static bool IsOdd(int value) => value % 2 == 1;
    }
}

```


実行するとIsOddWhenTrueの2番目がエラーになります。

> [InlineData(4)]

を

> [InlineData(5)]

に修正して再実行します。全て正常になるのが見て取れます。

[戻る](../README.md)
