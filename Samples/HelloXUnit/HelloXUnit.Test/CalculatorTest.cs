
using System;
using Xunit;

namespace HelloXUnit.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void AddTest()
        {
            Assert.Equal(4, Calculator.Add(2, 2));
        }

        [Fact]
        public void SubtractTest()
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
