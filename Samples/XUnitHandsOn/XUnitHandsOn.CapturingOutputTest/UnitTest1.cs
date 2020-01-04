using Xunit;
using Xunit.Abstractions;

namespace HelloXUnit.CapturingOutputTest
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("This is output from {0}", "Constructor");
        }

        [Fact]
        public void Test1()
        {
            _output.WriteLine("This is output from {0}", "Test1");
        }
    }
}
