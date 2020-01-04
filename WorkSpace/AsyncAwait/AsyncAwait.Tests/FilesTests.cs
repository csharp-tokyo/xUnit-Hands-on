using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AsyncAwait.Tests
{
    public class FilesTests
    {
        private const string ExistFileName = "test.txt";
        private const string TextFileContent = "Hello, xUnit.net!";
        private const string NotExistFileName = "NotExistFile";

        public FilesTests()
        {
            if (File.Exists(ExistFileName))
                File.Delete(ExistFileName);

            File.WriteAllText(ExistFileName, TextFileContent);
        }
    }
}
