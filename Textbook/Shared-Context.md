# 共有コンテキスト

この項は公式ドキュメント「[Shared Context between Tests](https://xunit.net/docs/shared-context)」をベースとしています。

ユニットテスト間では、セットアップとクリーンアップを様々なレベルで共有するのが一般的です。複雑なロジックの再利用や、重たい初期化処理の共有化などが目的でしょう。

xUnit.netでは、つぎの三つレベルの共有化手法が提供されています。

|No.|共有コンテキスト|スコープ|ロジックの共有|インスタンスの共有|
|--|--|--|--|--|
|1|コンストラクタとDispose　※1|単一クラス|〇|×|
|2|クラスフィクスチャー|単一クラス|〇|〇|
|3|コレクションフィクスチャー|カスタム テストコレクション　※2|〇|〇|

- ※1 「[初期化処理と終了処理](Textbook/Setup-TearDown.md)」で説明しました
- ※2 「[並列テストの実行](Textbook/Running-Tests-in-Parallel.md)」でも触れています

では実際にそれぞれがどう異なるのか見ていきましょう。

# 共有クラスの作成

まず処理を再利用するための共有クラスを作ります。

今回は処理の見通しをよくするため、HelloXUnit.SharedContextTestプロジェクトのUnitTest1.csファイルに全て記述していきます。

まずはUnitTest1.csの中に、つぎのようなクラスを作成してください。

```cs
    public class HeavyFixture : IDisposable
    {
        public HeavyFixture() => Thread.Sleep(TimeSpan.FromSeconds(2));

        public void Use()
        {
        }

        public void Dispose() => Thread.Sleep(TimeSpan.FromSeconds(2));
    }
```

コンストラクタとDisposeでそれぞれ2秒かかる重たいクラスです。

例えばデータベース操作をするテスト群があったときに、Dockerをpullして起動するような場合などを想定しています（この場合、終了処理は特別重いわけではありませんが）。

これを用いてそれぞれの共有レベルを見ていきましょう。

## コンストラクタとDispose

UnitTest1.csファイルに二つのテストクラスを作成してください。

```cs
    public class UnitTest1 : IDisposable, IClassFixture<HeavyFixture>
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
            _heavyFixture.Dispose();
        } 
    }

    public class UnitTest2 : IDisposable
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest2()
        {
            _heavyFixture = new HeavyFixture();
        }

        [Fact]
        public void Test() => _heavyFixture.Use();

        public void Dispose()
        {
            _heavyFixture.Dispose();
        }
    }
```

UnitTest1に二つ、UniteTest2に一つのテストメソッドがあります。

xUnit.netではテストの実施のつどテストクラスのインスタンスが生成・破棄されます。したがってこのケースでは合計3回、HeavyContextクラスが生成・破棄されるため4秒×3回の12CPU秒かかることになります。UnitTest1とUnitTest2は平行実行されるため、経過時間としては実際には8秒程度になります。

コンストラクタとDisposeではHeavyContextに記述されたロジックは共有されますが、インスタンスは共有されません。このため複雑なロジックの共有には適切ですが、重たい処理の共有には本来向きません。


## クラスフィクスチャー

単一のテストクラス内でロジックとインスタンスを共有したい場合、クラスフィクスチャーを利用します。単一のクラス内で一度だけ実行すれば十分で、テスト間で共有できる場合に最適です。

クラスフィクスチャーは、つぎのように利用します。

1. 共有化する処理をフィクスチャークラスとして作成し、コンストラクタを実装する
2. フィクスチャークラスで終了処理が必要であればIDisposableを実装する
3. テストクラスにIClassFixture<T>を宣言する
4. テストクラスのコンストラクタ引数にフィクスチャーを追加する

先ほどのクラスを修正して実際に利用してみましょう。

UnitTest1クラスをつぎのように修正します。

```cs
    public class UnitTest1 : IDisposable, IClassFixture<HeavyFixture>
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
```

HeavyFixtureのDisposeはフレームワーク側が実施してくれるため、コメントアウトしている点に注意してください。

これでHeavyContextクラスの生成・破棄は4秒×2回の8CPU秒程度になり、UnitTest1とUnitTest2は平行実行されるため、経過時間としては実際には4秒程度に短縮されます。

なお複数のフィクスチャーを利用したい場合、IClassFixture<T>を複数宣言し同じ数だけコンストラクタで受け取ります。

```cs
    public class UnitTest1 : IDisposable, IClassFixture<HeavyFixture>, IClassFixture<FooFixture>
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest1(HeavyFixture heavyFixture, FooFixture fooFixture)
        {
            ...
```

二つ注意が必要です。

1. フィクスチャーの生成順序は制御できない
2. コンストラクタで受け取るか否かにかかわらず、IClassFixtureを宣言するとxUnit.netはインスタンスを生成する

