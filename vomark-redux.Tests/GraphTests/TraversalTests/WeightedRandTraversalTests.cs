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
    public class WeightedRandTraversalTests
    {
        public class WeightedRandBehaviorTest
        {
            /**
             * Okay, this is INHERENTLY messy but no other good way to test random.
             * This should ALMOST (99.99999...) always output correctly.
             * If it doesn't screenshot it and I'll buy you a coffee or something.
             */
            [Fact]
            public void TestWeightedRandOutput()
            {
                WeightedRandTraversal wrt = new();
                Random rand = new();
                ConcurrentDictionary<string, int> data = FakeTravData.BaseTravData();
                List<string> res = [];
                for(int i = 0; i < 100; ++i)
                {
                    string? next = wrt.Next(data, rand);
                    if(next != null)
                    {
                        res.Add(next);
                    }
                }

                Assert.Equal(100, res.Count);
                Assert.Contains("test1", res);
                Assert.Contains("test2", res);

                List<string> t1 = [.. res.Where(x => (x == "test1"))];
                List<string> t2 = [.. res.Where(x => (x == "test2"))];
                Assert.True(t2.Count >= t1.Count);
            }
        }

        [Fact]
        public void TestEmptyList()
        {
            WeightedRandTraversal wrt = new();
            Random rand = new Random();
            ConcurrentDictionary<string, int> data = [];

            Assert.Null(wrt.Next(data, rand));
        }
    }
}
