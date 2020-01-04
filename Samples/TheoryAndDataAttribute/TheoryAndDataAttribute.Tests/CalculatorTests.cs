using System;
using System.Collections.Generic;
using Xunit;

namespace TheoryAndDataAttribute.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, -2, -3)]
        public void InlineDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }

        public static IEnumerable<object[]> GetValues() =>
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(GetValues))]
        public void MemberDataTestByMethod(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }

        public static IEnumerable<object[]> ValuesProperty { get; } =
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(ValuesProperty))]
        public void MemberDataTestByProperty(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }

        public static readonly IEnumerable<object[]> ValuesField =
            new List<object[]>
            {
                new object[]{1, 2, 3},
                new object[]{-1, -2, -3},
            };

        [Theory]
        [MemberData(nameof(ValuesField))]
        public void MemberDataTestByField(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }

        [Theory]
        [ClassData(typeof(AddTestDataSets))]
        public void ClassDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Calculator.Add(x, y));
        }
    }
}
