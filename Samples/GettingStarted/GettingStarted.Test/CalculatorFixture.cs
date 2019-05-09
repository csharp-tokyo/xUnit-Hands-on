using Xunit;

namespace GettingStarted.Test
{
    public class CalculatorFixture
    {
        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 6)]
        [InlineData(2, 3, 5)]
        public void AddTest(int x, int y, int sum)
        {
            Assert.Equal(sum, Calculator.Add(x, y));
        }

        [Fact]
        public void SubtractTest()
        {
            Assert.Equal(1, Calculator.Subtract(3, 2));
        }
    }
}