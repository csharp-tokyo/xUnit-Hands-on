using System.Collections;
using System.Collections.Generic;

namespace TheoryAndDataAttribute.Tests
{
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
}