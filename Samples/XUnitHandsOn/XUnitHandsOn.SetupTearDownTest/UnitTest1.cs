using System;
using System.IO;
using Xunit;

namespace XUnitHandsOn.SetupTearDownTest
{
    public class UnitTest1 : IDisposable
    {
        public static bool DeleteIfExist(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            File.Delete(file);
            return true;
        }

        private const string ExistFileName = "test.txt";
        private const string TextFileContent = "Hello, xUnit.net!";
        private const string NotExistFileName = "NotExistFile";

        public UnitTest1()
        {
            if (File.Exists(ExistFileName))
                File.Delete(ExistFileName);

            File.WriteAllText(ExistFileName, TextFileContent);
        }

        public void Dispose()
        {
            if (File.Exists(ExistFileName))
                File.Delete(ExistFileName);
        }

        [Fact]
        public void DeleteIfExistWhenExistFile()
        {
            Assert.True(DeleteIfExist(ExistFileName));
            Assert.False(File.Exists(ExistFileName));
        }

        [Fact]
        public void DeleteIfExistWhenNotExistFile()
        {
            Assert.False(DeleteIfExist("NotExistFile"));
        }
    }
}
