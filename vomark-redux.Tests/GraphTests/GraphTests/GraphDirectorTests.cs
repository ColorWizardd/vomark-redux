using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static vomark_redux.Tests.TestDoubles;
using vomark_redux.lib.domain;
using vomark_redux.lib.svc;

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
                Assert.Equal(2, fCombined.FindAllNext("fake").Count);
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
        }
    }
}
