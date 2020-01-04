using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace HelloXUnit.DataAttributeTest
{
    public class UnitTest1
    {
        public static int Add(int x, int y) => x + y;

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, -2, -3)]
        public void InlineDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Add(x, y));
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
            Assert.Equal(result, Add(x, y));
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
            Assert.Equal(result, Add(x, y));
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
            Assert.Equal(result, Add(x, y));
        }


        class AddTestDataSets : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    new object[]{1, 2, 3},
                    new object[]{-1, -2, -3},
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Theory]
        [ClassData(typeof(AddTestDataSets))]
        public void ClassDataTest(int x, int y, int result)
        {
            Assert.Equal(result, Add(x, y));
        }
    }
}
