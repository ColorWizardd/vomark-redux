using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using Xunit;

using static vomark_redux.Tests.TestDoubles;

namespace vomark_redux.Tests.GraphTests.GraphTests
{
    public class GraphBuilderTests
    {
        public class GraphBuilderBehaviorTests
        {
            [Fact]
            public void TestBuildGraph()
            {
                GraphBuilder gb = new();
                FakeTokenizer ft = new();
                Graph testGraph = (Graph)gb.AddName("test")
                    .AddTokenizer(ft)
                    .AddTraversal(new FakeTraversal())
                    .AddVocabulary(new FakeVocabulary(ft))
                    .AddRandom(new Random())
                    .Build();
                testGraph.AddOrStrengthenEdge(Vocabulary.NODE_ROOT, "test");
                Assert.NotNull(testGraph.GetNextNode(Vocabulary.NODE_ROOT));
                Assert.Equal("test", testGraph.GetName());
            }

            [Fact]
            public void TestMissingRand()
            {
                var err = Assert.Throws<System.InvalidOperationException>(() =>
                {
                    GraphBuilder gb = new();
                    FakeTokenizer ft = new();
                    Graph testGraph = (Graph)gb.AddName("test")
                        .AddTokenizer(ft)
                        .AddTraversal(new FakeTraversal())
                        .AddVocabulary(new FakeVocabulary(ft))
                        .Build();
                });

                Assert.Equal("Random is needed for graph traversal", err.Message);
            }

            [Fact]
            public void TestMissingTraversal()
            {
                var err = Assert.Throws<System.InvalidOperationException>(() =>
                {
                    GraphBuilder gb = new();
                    FakeTokenizer ft = new();
                    Graph testGraph = (Graph)gb.AddName("test")
                        .AddTokenizer(ft)
                        .AddRandom(new Random())
                        .AddVocabulary(new FakeVocabulary(ft))
                        .Build();
                });

                Assert.Equal("Traversal type is needed for graph traversal", err.Message);
            }

            [Fact]
            public void TestMissingVocabulary()
            {
                var err = Assert.Throws<System.InvalidOperationException>(() =>
                {
                    GraphBuilder gb = new();
                    FakeTokenizer ft = new();
                    Graph testGraph = (Graph)gb.AddName("test")
                        .AddTokenizer(ft)
                        .AddRandom(new Random())
                        .AddTraversal(new FakeTraversal())
                        .Build();
                });

                Assert.Equal("Vocabulary is needed for graph building/traversal", err.Message);
            }
        }
    }
}
