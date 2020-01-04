# 非同期処理のテスト

非同期処理をテストする場合、テストケース側も同様に通常の非同期処理のように記述できます。

テキストファイルを非同期に読み取り文字列を返すメソッドを実装してみましょう。

まずFilesクラスに次のようなメソッドを定義します。

```cs
        public static async Task<string> ReadAllTextAsync(string file)
        {
            throw new NotImplementedException();
        }
```

続いてFilesTestsにテストケースを記述します。

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

テストメソッド自体もasync/awaitを利用して記述することができます。

ただし一点注意があります。xUnit.netの提供するAssertionには一部、同期用と非同期用のメソッドが異なるものがあります。上の例ではThrowsAsync&lt;T>を利用していますが、同期メソッドの場合はThrows&lt;T>を利用します。非同期用を利用しなかった場合、Assert.Throwsが完了する前にテストメソッドの実行が終わってしまい、テストが正常に終了してしまう可能性があります。

Assertionの詳細は[Assertionチートシート](Assertion-CheatSheet.md)を御覧ください。

さてテストが二つとも正しくエラーになることを確認したら、テスト対象を実装しましょう。

```cs
        public static async Task<string> ReadAllTextAsync(string file)
        {
            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                return await reader.ReadToEndAsync();
            }
        }
```

[戻る](../README.md)
