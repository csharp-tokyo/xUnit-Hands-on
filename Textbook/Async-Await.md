# 非同期処理のテスト

xUnit.netでは非同期処理をテストする場合、テストケース側も通常の非同期処理と同様に記述できます。ここではテキストファイルを非同期に読み取り文字列を返すメソッドを実装・テストしてみましょう。

それではWorkSpaceフォルダー下にあるAsyncAwaitソリューションを開いてください。

まずAsyncAwaitプロジェクトのFilesクラスに次のようなメソッドを定義します。

```cs
        public static async Task<string> ReadAllTextAsync(string file)
        {
            throw new NotImplementedException();
        }
```

続いてAsyncAwait.TestsプロジェクトのFilesTestsにテストケースを記述します。FilesTestsには事前に[テスト用のファイルを初期化するコード](../WorkSpace/AsyncAwait/AsyncAwait.Tests/FilesTests.cs)が記述されています。

```cs
        [Fact]
        public async Task ReadAllTextAsyncWhenExistFile()
        {
            Assert.Equal(TextFileContent, await Files.ReadAllTextAsync(ExistFileName));
        }

        [Fact]
        public async Task ReadAllTextAsyncWhenNotExistFile()
        {
            await Assert.ThrowsAsync<FileNotFoundException>(
                () => Files.ReadAllTextAsync(NotExistFileName));
        }
```

テストメソッド自体もasync/awaitを利用して記述できます。

ただし一点注意があります。xUnit.netの提供するAssertionには一部、同期用と非同期用のメソッドが異なります。上の例ではThrowsAsync&lt;T>を利用していますが、同期メソッドの場合はThrows&lt;T>を利用します。非同期用を利用しなかった場合、Assert.Throwsが完了する前にテストメソッドの実行が終わってしまい、テストが正常に終了してしまう可能性があります。

非同期用のAssertionが提供されている場合、原則的にはそちらを利用してください。

Assertionの詳細は[Assertionチートシート](Assertion-CheatSheet.md)を御覧ください。

さてテストが2つとも正しくエラーになることを確認したら、テスト対象を実装しましょう。

```cs
        public static async Task<string> ReadAllTextAsync(string file)
        {
            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                return await reader.ReadToEndAsync();
            }
        }
```

---


[次へ: 並列テストの実行](./Running-Tests-in-Parallel.md) | [README に戻る](../README.md)