using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using static vomark_redux.Tests.TestDoubles;

namespace vomark_redux.Tests.GraphTests.TraversalTests
{
    public class GreedyTraversalTests
    {
        public class TestGreedyTraversalBehavior
        {
            [Fact]
            public void TestBasicTraversal()
            {
                GreedyTraversal gt = new();
                ConcurrentDictionary<string, int> data = FakeTravData.BaseTravData();
                Assert.Equal("test2", gt.Next(data, new Random()));
            }

            [Fact]
            public void TestEmptyNext()
            {
                GreedyTraversal gt = new();
                ConcurrentDictionary<string, int> data = [];
                Assert.Null(gt.Next(data, new Random()));
            }
        }
    }
}
