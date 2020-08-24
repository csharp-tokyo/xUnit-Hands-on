# 並列テストの実行

この項は公式ドキュメント「[Running Tests in Parallel](https://xunit.net/docs/running-tests-in-parallel)」をベースとしています。

xUnit.net（の2.x以降）では標準でテストの並列化が有効になっています。xUnit.netのテストの並列化を理解するためには「テストコレクション」の概念を理解する必要があります。

実際に手を動かしながら理解していきましょう。

Parallelソリューションを開いてください。このソリューションにはParallel.Testsのみが存在します。テスト対象を用意する必要がない、もしくはわざわざテスト対象を用意すると理解を余計に阻害する場合、以降はテストコードプロジェクトだけ利用します。

## テストコレクション

xUnit.netでは、同一のテストコレクション内は並列化されず逐次実行されます

Parallel.Testsプロジェクト内のUnitTest1.csを開き次のように実装してみてください。

```cs
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }

        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
```

実行すると完了までおおよそ8秒程度かかり、並列化されていないことが見て取れます。

** なお、テストの実行環境によって実行時間の表示仕様が異なります。実行時間の表示が異なると感じた場合は実測してみてください。

つづいて、次のようにテストクラスを分離してみてください。

```cs
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    public class UnitTest2
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
```

実行すると5秒程度で完了し、並列化されていることが分かります。

とくに指定しなかった場合、1つのクラスが1つのテストコレクションとなります。xUnit.netでは単一のテストコレクションは逐次実行され、ことなるテストコレクションは平行実行されます。

このためテストをクラスを分割すると平行実行され、全体の実行時間が短縮されることになりました。

## カスタム テストコレクション

テストクラス間で並列実行されては困る場合、異なるテストクラスを同一のテストコレクションに含めることが可能です。

任意のテストコレクションに含めるためにはColllection属性を利用します。先ほどのテストを次のように修正してみましょう。

```cs
    [Collection("Our Test Collection #1")]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    [Collection("Our Test Collection #1")]
    public class UnitTest2
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
```

実行すると、ふたたび8秒程度かかるようになったことが確認できます。

逆に同一クラス内の別々のテストメソッドを並列実行することはできません。その場合同一のcsファイルに別クラスとしてテストクラスを作成するなどの対応が必要でしょう。

## その他の並列実行の詳細

並列実行をカスタムする機能が提供されていますが、ここでの解説は省略します。公式ドキュメントの「[Running Tests in Parallel](https://xunit.net/docs/running-tests-in-parallel)」に詳細な記述があります。

---

[次へ: 共有コンテキスト](./Shared-Context.md) | [README に戻る](../README.md)