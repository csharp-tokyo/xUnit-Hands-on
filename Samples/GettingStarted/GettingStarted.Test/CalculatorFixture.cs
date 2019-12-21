using Xunit;

namespace GettingStarted.Test
{
    public class CalculatorFixture
    {
        [Fact]
        public void Add()
        {
            Assert.Equal(5, Calculator.Add(2, 3));
        }

        [Fact]
        public void Subtract()
        {
            Assert.Equal(1, Calculator.Subtract(3, 2));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public void IsOddWhenTrue(int value)
        {
            Assert.True(Calculator.IsOdd(value));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void IsOddWhenFalse(int value)
        {
            Assert.False(Calculator.IsOdd(value));
        }
    }
}