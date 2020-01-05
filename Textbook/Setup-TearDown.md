# 初期化処理と終了処理

同一テストクラスの共通の初期化処理はコンストラクタに実装し、終了処理はIDisposableを実装します。xUnit.netではテストクラスは、一つのテストメソッドを実行する都度、テストクラスのインスタンスが生成されます。

実際に試してみましょう。

ここではファイルに対するユーティリティクラスを作成し、テスト対象となるファイルを初期化時に作成し、終了時にファイルを削除する処理を実装してみます。

ユーティリティクラスではファイルを指定して、存在した場合にだけ削除するようなメソッドを実装します。実行した結果、ファイルが存在していたらtrueを返します。

それではWorkSpaceフォルダ下にあるSetupTearDownソリューションを開いてください。

SetupTearDownソリューションには以下の二つのプロジェクトが存在します。

1. SetupTearDown
2. SetupTearDown.Tests

まずSetupTearDownプロジェクトのFileクラスを開き、テスト対象のメソッドを定義します（まだ中身は実装しません）。しばらくはテストファーストな開発スタイルで進めていきます。


```cs
using System.IO;

namespace HelloXUnit
{
    public class Files
    {
        public static bool DeleteIfExist(string file)
        {
            throw new NotImplementedException();
        }
    }
}
```

続いてSetupTearDown.Testsプロジェクトを開いてください。事前にIDisposableの空実装が済まされているのが見て取れます。

```cs
using System;
using System.IO;
using Xunit;

namespace HelloXUnit.Test
{
    public class FilesTests : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
```

まずは初期処理として、コンストラクタで削除対象のファイルを作成します。

```cs
        private const string ExistFileName = "test.txt";
        private const string TextFileContent = "Hello, xUnit.net!";
        private const string NotExistFileName = "NotExistFile";

        public FilesTests()
        {
            if (File.Exists(ExistFileName))
                File.Delete(ExistFileName);

            File.WriteAllText(ExistFileName, TextFileContent);
        }
```

そしてDisposeメソッドに終了処理を記述します。

```cs
        public void Dispose()
        {
            if (File.Exists(ExistFileName))
                File.Delete(ExistFileName);
        }
```

これで準備が整いました。

ではユーティリティクラスのメソッドを実装する前に、テストコードを書きましょう。ファイルが存在したときと、しなかったときの二つのメソッドを実装します。

```cs
        [Fact]
        public void DeleteIfExistWhenExistFile()
        {
            Assert.True(Files.DeleteIfExist(ExistFileName));
            Assert.False(File.Exists(ExistFileName));
        }

        [Fact]
        public void DeleteIfExistWhenNotExistFile()
        {
            Assert.False(Files.DeleteIfExist("NotExistFile"));
        }
```

ではテストを実行しましょう。ユーティリティクラスはまだ実装されておらず、常に例外がスローされるため、テストはすべてエラーになります。

ではユーティリティクラスを実装しましょう。微妙なタイミングで他から先に削除された場合に正しく動作しませんが、今回は見逃してあげてください。

```cs
        public static bool DeleteIfExist(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            File.Delete(file);
            return true;
        }
```

では再度テストを実行します。全て成功することが確認できるはずです。

[戻る](../README.md)
