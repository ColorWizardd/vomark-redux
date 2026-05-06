using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static vomark_redux.Tests.TestDoubles;
using vomark_redux.lib.domain;
using vomark_redux.lib.svc;
using System.Collections.Concurrent;

namespace vomark_redux.Tests.GraphTests.GraphTests
{
    public class GraphDirectorTests
    {
        public class GraphDirectorBehaviorTests
        {
            [Fact]
            public void TestAddFindGraph()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                gd.AddGraph(new FakeGraph("test1"));
                Assert.Single(gd.GetGraphSet());
                Assert.NotNull(gd.FindGraph("test1"));
            }

            [Fact]
            public void TestAddGraphNoName()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                var err = Assert.Throws<ArgumentException>(() => gd.AddGraph(new FakeGraph("")));
                Assert.Equal("Graph does not contain name, cannot be added to set", err.Message);
            }

            [Fact]
            public void TestFindGraphFail()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                gd.AddGraph(new FakeGraph("test1"));
                Assert.Single(gd.GetGraphSet());
                var err = Assert.Throws<ArgumentException>(() => gd.FindGraph("test3"));
                Assert.Equal("No graph found with the given name", err.Message);
            }

            [Fact]
            public void TestCombineGraph()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                FakeGraph f1 = new("fake1");
                FakeGraph f2 = new("fake2");
                gd.AddGraph(f1);
                gd.AddGraph(f2);
                gd.SendSentence("fake 1", f1);
                gd.SendSentence("fake 2", f2);

                FakeGraph fCombined = gd.CombineGraphs("fake1", "fake2");
                Assert.Contains("fake", fCombined.GetEdgeList());
                Assert.Equal(4, fCombined.GetEdgeList().Count);
                ConcurrentDictionary<string, int>? allNext = fCombined.FindAllNext("fake");
                Assert.NotNull(allNext);
                Assert.Equal(2, allNext.Count);
            }

            [Fact]
            public void TestListCombine()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                List<string> names = ["fake1", "fake2"];
                FakeGraph f1 = new(names[0]);
                FakeGraph f2 = new(names[1]);
                gd.AddGraph(f1);
                gd.AddGraph(f2);
                gd.SendSentence("fake 1", f1);
                gd.SendSentence("fake 2", f2);

                FakeGraph fCombined = gd.CombineGraphs(names);
                Assert.Contains("fake", fCombined.GetEdgeList());
                Assert.Equal(4, fCombined.GetEdgeList().Count);
                ConcurrentDictionary<string, int>? allNext = fCombined.FindAllNext("fake");
                Assert.NotNull(allNext);
                Assert.Equal(2, allNext.Count);
            }

            [Fact]
            public void TestBadListCombine()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                List<string> names = ["fake1", "fake2"];
                List<string> badNames = ["fake2", "fake3", "fake4"];
                FakeGraph f1 = new(names[0]);
                FakeGraph f2 = new(names[1]);
                gd.AddGraph(f1);
                gd.AddGraph(f2);
                gd.SendSentence("fake 1", f1);
                gd.SendSentence("fake 2", f2);

                var err = Assert.Throws<ArgumentException>(() => gd.CombineGraphs(badNames));
                Assert.Equal("One or more specfied graphs do not exist in the set", err.Message);
            }

            [Fact]
            public void TestBadCombine()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                FakeGraph f1 = new("fake1");
                FakeGraph f2 = new("fake2");
                gd.AddGraph(f1);
                gd.AddGraph(f2);
                var err = Assert.Throws<ArgumentException>(() => gd.CombineGraphs("fake1", "notreal"));
                Assert.Equal("One or more specfied graphs do not exist in the set", err.Message);
            }

            [Fact]
            public void TestGenSentence()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                string gName = "fake1";
                string inp = "this is a sentence";
                gd.AddGraph(new(gName));
                gd.SendSentence(inp, gd.FindGraph(gName));
                string res = gd.GenSentence(
                    gd.FindGraph(gName),
                    new FakePuncBehavior(),
                    new FakeCapBehavior(),
                    128
                    );
                Assert.Equal("this is a sentence.", res);
            }

            [Fact]
            public void TestEmptyGenSentence()
            {
                GraphDirector<FakeGraph> gd = new(new FakeParser());
                string gName = "fake1";
                gd.AddGraph(new(gName));
                gd.SendSentence("", gd.FindGraph(gName));
                Assert.Empty(
                    gd.GenSentence(gd.FindGraph(gName),
                        new FakePuncBehavior(),
                        new FakeCapBehavior(),
                        128
                        )
                    );
            }
        }
    }

    public class CapBehaviorTests
    {
        [Fact]
        public void TestFirstCap()
        {
            string inp = "test sentence.";
            Assert.Equal("Test sentence.", new FirstCap().ApplyBehavior(inp));
        }

        [Fact]
        public void TestAllCap()
        {
            string inp = "test sentence.";
            Assert.Equal("TEST SENTENCE.", new AllCap().ApplyBehavior(inp));
        }

        [Fact]
        public void TestLowerCap()
        {
            string inp = "TEST SENTENCE.";
            Assert.Equal("test sentence.", new LowerCap().ApplyBehavior(inp));
        }
    }

    public class PuncBehaviorTests
    {
        [Fact]
        public void TestEndPeriod()
        {
            string inp = "test sentence";
            Assert.Equal("test sentence.", new EndPeriod().ApplyBehavior(inp));
        }

        [Fact]
        public void TestEndExc()
        {
            string inp = "test sentence";
            Assert.Equal("test sentence!", new EndExc().ApplyBehavior(inp));
        }

        [Fact]
        public void TestEndQ()
        {
            string inp = "test sentence";
            Assert.Equal("test sentence?", new EndQ().ApplyBehavior(inp));
        }

    }
}
