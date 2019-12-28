# 並列テストの実行

この項は公式ドキュメント「[Running Tests in Parallel](https://xunit.net/docs/running-tests-in-parallel)」をベースとしています。

xUnit.net（の2.x以降）では標準でテストの並列化が有効になっています。

xUnit.netのテストの並列化を理解するためには「テストコレクション」の概念を理解する必要があります。

## テストコレクション

xUnit.netでは、同一のテストコレクション内は並列化されず逐次実行されます

HelloXUnit.ParallelTestプロジェクト内のUnitTest1.csを開き次のように実装してみてください。

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

並列実行をカスタムする機能が提供されていますが、ここでの解説は省略します。公式ドキュメントの「[Running Tests in Parallel](https://xunit.net/docs/running-tests-in-parallel)」に詳細な記述がありますので、そちらを御覧ください。

[戻る](README.md)