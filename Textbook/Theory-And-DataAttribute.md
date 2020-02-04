# TheoryとDataAttribute

「[xUnit.netをはじめよう！](Getting-Started.md)」でも触れたように、xUnit.netではTheory属性を宣言することで複数のデータセットによるテストを宣言的に記述することができます。データセットは[DataAttributeクラス](https://csharp-tokyo.github.io/xUnit-Hands-on/class_xunit_1_1_sdk_1_1_data_attribute.html)のサブクラスとして実装されており、3種類の属性が標準で用意されています。

1. InlineData
2. MemberData
3. ClassData

[MemberDataAttributeBaseクラス](https://csharp-tokyo.github.io/xUnit-Hands-on/class_xunit_1_1_member_data_attribute_base.html) を継承することで独自の属性を作成することもできますが、本項では割愛します。

## InlineData

最もシンプルなデータセット定義です。InlineDataで無理なく記述できるのであればInlineDataを利用するのが可読性なども高く好ましいでしょう。

TheoryAndDataAttributeソリューションを開き、TheoryAndDataAttributeプロジェクトのCalculator.csクラスを開いてください。

```cs
using System;

namespace TheoryAndDataAttribute
{
    public static class Calculator
    {
        public static int Add(int x, int y) => x + y;
    }
}
```

今回は事前に実装を用意しておきました。これに対して、2種類のデータセットを用いてテストを実施するコードを作成しましょう。

TheoryAndDataAttribute.TestsのCalculatorTests.csを開き、以下のようなテストを記述・実行してください。

```cs
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, -2, -3)]
        public void InlineDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
```

宣言されたInlineData属性の数だけテストが実行されます。

InlineData属性には対象のメソッドの引数に渡すための値を設定します。この時、対象メソッドの引数順序とInlineData属性に設定する順序がマッチしている必要があります。

## MemberData

InlineDataは非常にシンプルに記述することができますが、データセットの生成に何らかのロジックが必要な場合などは記述が困難になる、もしくは不可能なことがあります。その解決にはMemberDataとClassの二つがありますが、まずはMemberDataを試してみましょう。

MemberDataは3種類のメンバーを利用することができます。

1. メソッド
2. プロパティ
3. フィールド

いずれもIEnumerable<object[]>な値を返す、publicでstaticなメンバーである必要があります。


使い分ける明確なルールがあるわけではありませんが、例えば都度データを生成する場合はメソッドを、一度生成したものを別のテストケースでも使いまわすのであればメソッド以外を利用するのが明確かもしれません。使い分けが必要かどうかはプロダクトごとに検討すればよいでしょう。

### メソッドを利用したMemberData

ではInlineDataと等価なメソッドを利用したMemberDataによるテストケースを記述してみましょう。

```cs
        public static IEnumerable<object[]> GetValues() =>
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(GetValues))]
        public void MemberDataTestByMethod(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
```

MemberData属性の引数にメソッド名を指定します。

### プロパティを利用したMemberData

```cs
        public static IEnumerable<object[]> ValuesProperty { get; } =
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(ValuesProperty))]
        public void MemberDataTestByProperty(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
```

### フィールドを利用したMemberData

```cs
        public static readonly IEnumerable<object[]> ValuesField =
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(ValuesField))]
        public void MemberDataTestByField(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
```

## ClassData

MemberDataで記載した場合に、テストケースクラスにテストデータを生成するためのコードが大量に混在してしまう場合、ClassDataを利用すると課題が解決するかもしれません。

まずはテストデータを生成するクラスを作ります。AddTestDataSets.csを開き、次のように実装してください。

```cs
        class AddTestDataSets : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    new object[]{1, 2, 3},
                    new object[]{-1, -2, -3},
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
```

データセットとなるクラスはIEnumerable&lt;object[]>を実装します。続いてテストコードをCalculatorTests.csに追加しましょう。

```cs
        [Theory]
        [ClassData(typeof(AddTestDataSets))]
        public void ClassDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
```

ClassData属性でデータセットのTypeを指定しています。文字列ではない点に注意してください。


このようにデータセットをクラスに分離することで、テストデータの生成に複雑なロジックが必要であったり、例えばデータベースへの接続を必要とするテストケースで、テストデータの生成時に必要となるロジックの共有化などが可能となります。

---

[次へ: 並列テストの実行](./Running-Tests-in-Parallel.md) | [README へ戻る](../README.md)