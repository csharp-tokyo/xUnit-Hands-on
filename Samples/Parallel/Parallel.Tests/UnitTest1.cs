using System;
using System.Threading;
using Xunit;

namespace Parallel.Tests
{
    //public class UnitTest1
    //{
    //    [Fact]
    //    public void Test1()
    //    {
    //        Thread.Sleep(3000);
    //    }

    //    [Fact]
    //    public void Test2()
    //    {
    //        Thread.Sleep(5000);
    //    }
    //}

    //public class UnitTest1
    //{
    //    [Fact]
    //    public void Test1()
    //    {
    //        Thread.Sleep(3000);
    //    }
    //}

    //public class UnitTest2
    //{
    //    [Fact]
    //    public void Test2()
    //    {
    //        Thread.Sleep(5000);
    //    }
    //}

    [Collection("Our Test Collection #1")]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Thread.Sleep(3000);
        }
    }

    [Collection("Our Test Collection #1")]
    public class UnitTest2
    {
        [Fact]
        public void Test2()
        {
            Thread.Sleep(5000);
        }
    }
}
