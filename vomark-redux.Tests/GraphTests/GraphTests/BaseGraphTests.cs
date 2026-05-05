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
    public class BaseGraphTests
    {
        public class TestGraphBehavior
        {
            List<string> words = [
                Vocabulary.NODE_ROOT,
                "its",
                "a",
                "sentence",
                Vocabulary.NODE_TERM
                ];

            [Fact]
            public void TestLinearGraphSuccess()
            {
                FakeTokenizer tokenizer = new();
                FakeTraversal traversal = new();
                FakeVocabulary vocab = new(tokenizer);

                Graph graph = new();

                graph.SetRandom(new Random());
                graph.SetTraversal(traversal);
                graph.SetVocabulary(vocab);
                graph.SetTokenizer(tokenizer);
                graph.SetName("Fake");

                graph.AddOrStrengthenEdge(words[0], words[1]);
                graph.AddOrStrengthenEdge(words[1], words[2]);

                Assert.Single(graph.FindAllNext(words[0]));

                Assert.Equal(words[1], graph.GetNextNode(words[0]));
                Assert.Equal(words[2], graph.GetNextNode(words[1]));
            }

            [Fact]
            public void TestNullFetch()
            {
                // Ensure the graph returns null without failing if no next node is found.
                FakeTokenizer tokenizer = new();
                FakeTraversal traversal = new();
                FakeVocabulary vocab = new(tokenizer);

                Graph graph = new("Fake");

                graph.SetRandom(new Random());
                graph.SetTraversal(traversal);
                graph.SetVocabulary(vocab);
                graph.SetTokenizer(tokenizer);

                graph.AddOrStrengthenEdge(words[0], words[1]);
                graph.AddOrStrengthenEdge(words[1], words[2]);

                Assert.Null(graph.GetNextNode(words[2]));
            }

            [Fact]
            public void TestMissingRandom()
            {
                // FindAllNext should be null if no random is implemented.
                FakeTokenizer tokenizer = new();
                FakeTraversal traversal = new();
                FakeVocabulary vocab = new(tokenizer);

                Graph graph = new("Fake");

                graph.SetTraversal(traversal);
                graph.SetVocabulary(vocab);
                graph.SetTokenizer(tokenizer);

                graph.AddOrStrengthenEdge(words[0], words[1]);
                graph.AddOrStrengthenEdge(words[1], words[2]);

                Assert.Null(graph.GetNextNode(words[0]));
            }

            [Fact]
            public void TestMissingTraversal()
            {
                // FindAllNext should be null if no traversal is implemented.
                FakeTokenizer tokenizer = new();
                FakeTraversal traversal = new();
                FakeVocabulary vocab = new(tokenizer);

                Graph graph = new("Fake");

                graph.SetVocabulary(vocab);
                graph.SetTokenizer(tokenizer);
                graph.SetRandom(new Random());

                graph.AddOrStrengthenEdge(words[0], words[1]);
                graph.AddOrStrengthenEdge(words[1], words[2]);

                Assert.Null(graph.GetNextNode(words[0]));
            }

            [Fact]
            public void TestMissingVocabulary()
            {
                // AddOrStrengthenEdge should return w/ no-op if no vocab is implemented.
                // Therefore, null vocab shouldn't result in edgeList additions.
                FakeTokenizer tokenizer = new();
                FakeTraversal traversal = new();

                Graph graph = new();

                graph.SetRandom(new Random());
                graph.SetTraversal(traversal);
                graph.SetTokenizer(tokenizer);

                graph.AddOrStrengthenEdge(words[0], words[1]);
                Assert.Empty(graph.GetEdgeList());
            }
        }
    }
}
