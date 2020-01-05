# 共有コンテキスト

この項は公式ドキュメント「[Shared Context between Tests](https://xunit.net/docs/shared-context)」をベースとしています。

ユニットテスト間では、セットアップとクリーンアップを様々なレベルで共有するのが一般的です。複雑なロジックの再利用や、重たい初期化処理の共有化などが目的でしょう。

xUnit.netでは、つぎの三つレベルの共有化手法が提供されています。

|共有コンテキスト|スコープ|ロジックの共有|インスタンスの共有|
|--|--|--|--|
|コンストラクタとDispose ※1|単一クラス|〇|×|
|クラスフィクスチャー|単一クラス|〇|〇|
|コレクションフィクスチャー|カスタム テストコレクション ※2|〇|〇|

- ※1 「[初期化処理と終了処理](Textbook/Setup-TearDown.md)」で説明しました
- ※2 「[並列テストの実行](Textbook/Running-Tests-in-Parallel.md)」でも触れています

「スコープ」「ロジックの共有」「インスタンスの共有」がそれぞれどのような意味を持つのか、実際に見ていきましょう。

# 共有クラスの作成

まずロジック共有するためのクラスを作ります。

SharedContextソリューションを開き、SharedContext.Testsプロジェクト内のFixturesフォルダの下のHeavyFixture.csファイルを開いてください。

ここに共有するロジックを以下のように記述します。

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

例えばデータベース操作をするテスト群があったときに、Dockerをpullして起動するような場合などを想定しています。

これを用いてそれぞれの共有レベルを見ていきましょう。

## コンストラクタとDispose

UnitTest1.cs、UnitTest2.csファイルをそれぞれ以下のように記述します。

```cs
    public class UnitTest1 : IDisposable
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest1()
        {
            _heavyFixture = new HeavyFixture();
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
```

```cs
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


## クラス フィクスチャー

単一のテストクラス内でロジックとインスタンスを共有したい場合、クラス フィクスチャーを利用します。単一のクラス内で一度だけ実行すれば十分で、テスト間で共有できる場合に最適です。

クラス フィクスチャーは、つぎのように利用します。

1. 共有化する処理をフィクスチャークラスとして作成し、コンストラクタを実装する
2. フィクスチャークラスで終了処理が必要であればIDisposableを実装する
3. テストクラスにIClassFixture&lt;T>を宣言する
4. テストクラスのコンストラクタ引数にフィクスチャーを追加する

すでにHeavyFixtureとして1.2.は実装されているため、ここでは3.から開始します。

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

これでHeavyContextクラスの生成・破棄は4秒×2回の8CPU秒程度になり、UnitTest1とUnitTest2は平行実行されるため、経過時間も4秒程度に短縮されます。

なお複数のフィクスチャーを利用したい場合、IClassFixture&lt;T>を複数宣言し同じ数だけコンストラクタで受け取ります。

```cs
    public class UnitTest1 : IDisposable, IClassFixture<HeavyFixture>, IClassFixture<FooFixture>
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest1(HeavyFixture heavyFixture, FooFixture fooFixture)
        {
            ...
```

この場合、二つ注意が必要です。

1. フィクスチャーの生成順序は制御できない
2. コンストラクタで受け取るか否かにかかわらず、IClassFixtureを宣言するとxUnit.netはインスタンスを生成する

## コレクション フィクスチャー

異なるクラス間でコンテキストを共有したい場合、コレクションフィクスチャを利用します。

例えばデータベース操作をするようなテストケースで、同一の状態のデータベースを利用するテストケース群があったとします。データベースの利用に先立ち、起動処理や複雑な初期化処理が必要だったとした場合、コレクション フィクスチャーを利用することで問題を解決することができます。

1. 共有化する処理をフィクスチャークラスとして作成し、コンストラクタを実装する
2. フィクスチャークラスで終了処理が必要であればIDisposableを実装する
3. コレクション定義クラスを作成する
4. コレクション定義クラスと同一名を利用してテストケースにCollection属性を付与する
5. テストクラスのコンストラクタ引数にフィクスチャーを追加する

1.2.はすでに実装済みのHeavyFixtureを利用します。

Collectionsフォルダ内のHeavyCollection.csファイルを開き、次のように実装します。

```cs
    [CollectionDefinition("Heavy collection")]
    public class HeavyCollection : ICollectionFixture<HeavyFixture>
    {
        // CollectionDefinitionを付与したクラスのみ作成すればよい
        // 特別な実装は不要
    }
```

CollectionDefinitionでコレクション名を定義し、ICollectionFixture&lt;HeavyFixture>で利用するフィクスチャーを指定します。

フィクスチャーはアセンブリ間でも共有できますが、その場合、共有したい各アセンブリで同名のコレクション定義を作成する必要がある点に注意してください。

続いてテストケースを修正しましょう。UnitTest1からIClassFixtureが削除されている点に注意してください。

```cs
    [Collection("Heavy collection")]
    public class UnitTest1 : IDisposable
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

```cs
    [Collection("Heavy collection")]
    public class UnitTest2 : IDisposable
    {
        private readonly HeavyFixture _heavyFixture;

        public UnitTest2(HeavyFixture heavyFixture)
        {
            _heavyFixture = heavyFixture;
        }

        [Fact]
        public void Test() => _heavyFixture.Use();

        public void Dispose()
        {
            //_heavyFixture.Dispose();
        }
    }
```

テストクラスにCollection属性を定義し、先のコレクション定義の名称を設定します。その上でコンストラクタで共有対象のフィクスチャーをインジェクションしてもらいます。

これでHeavyContextクラスの生成・破棄は4秒×1回の4CPU秒まで削減されます。なお実行時間についてはクラスフィクスチャーの際と同様で4秒程度です。しかし平行処理数が増えれば利用するCPU時間が減るため、テスト全体の軽量化につながります。

コレクション フィクスチャーを利用した場合、同一コレクション内の全てのメソッドは逐次実行されます。これは同一のリソースを利用することを想定しているからでしょう。詳細は「[並列テストの実行](Textbook/Running-Tests-in-Parallel.md)」を御覧ください。

## 非同期ライフタイム

共有コンテキストには、非同期に初期化・終了処理を行うための、[IAsyncLifetime](https://nuitsjp.github.io/xUnit-and-Moq-Hands-on/interface_xunit_1_1_i_async_lifetime.html)が用意されています。

実際に試しながらみてみましょう。

Fixturesフォルダの下に、非同期に重たい処理を実行するAsyncHeavyFixtureクラスを作成しましょう。

```cs
    public class AsyncHeavyFixture : IAsyncLifetime
    {
        public Task InitializeAsync() => Task.Delay(TimeSpan.FromSeconds(2));

        public void Use()
        {
        }

        public Task DisposeAsync() => Task.Delay(TimeSpan.FromSeconds(2));
    }
```

そしてAsyncHeavyFixtureを利用するUnitTest3クラスを作成します。

```cs
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
```

AsyncHeavyFixtureはここまでに解説した共有コンテキストそれぞれで利用可能です。ここではIClassFixture&lt;T>を利用しているのが見て取れます。

[戻る](../README.md)
