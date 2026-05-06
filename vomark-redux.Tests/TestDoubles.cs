/**
 * Shared collection of test data.
 * 
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using vomark_redux.lib.svc;

namespace vomark_redux.Tests
{
    internal class TestDoubles
    {
        /**
         * Using CountTokenizer as a test base for consistency. 
         */
        internal class FakeTokenizer : ITokenizer
        {
            int currCount = 0;
            public int Tokenize(string inp)
            {
                return currCount++;
            }
        }

        /**
         * For now, we'll just replicate GreedyTraversal for consistency's sake.
         */
        internal class FakeTraversal : ITraversal
        {
            public string? Next(ConcurrentDictionary<string, int> nextList, Random rand)
            {
                return nextList.MaxBy(x => x.Value).Key;
            }

        }

        internal class FakeVocabulary : IVocabulary
        {
            public FakeVocabulary(ITokenizer tokenizer) : base(tokenizer)
            {
                _tokenizer = tokenizer;
                _vocabList = [];
                this.GetOrAddToken(NODE_ROOT);
                this.GetOrAddToken(NODE_TERM);
            }

            public override int GetOrAddToken(string token)
            {
                return _vocabList.GetOrAdd(token, x => _tokenizer.Tokenize(token));
            }
        }

        /**
         * Can't think of a good way to trim down behavior, so we'll just replicate the concrete
         * Graph for now.
         * Only difference is a quick construction.
         */
        public class FakeGraph : Graph
        {
            public FakeGraph(string name) : base(name)
            {
                _rand = new Random();
                _tokenizer = new FakeTokenizer();
                _vocabulary = new FakeVocabulary(_tokenizer);
                _traversal = new FakeTraversal();
            }

            public FakeGraph(FakeGraphBulder gb) : base()
            {
                _vocabulary = gb.Vocabulary;
                _tokenizer = gb.Tokenizer;
                _traversal = gb.Traversal;
                _rand = gb.Random;
                _name = gb.Name;
            }

            public override void AddOrStrengthenEdge(string from, string to, int weight = 1)
            {
                base.AddOrStrengthenEdge(from, to);
            }

            public override ConcurrentDictionary<string, int>? FindAllNext(string curr)
            {
                return base.FindAllNext(curr);
            }

            public override ConcurrentDictionary<string, ConcurrentDictionary<string, int>> GetEdgeList()
            {
                return base.GetEdgeList();
            }

            public override string? GetName()
            {
                return this._name;
            }

            public override string? GetNextNode(string curr)
            {
                return base.GetNextNode(curr);
            }

            public override int GetWeight(string from, string to)
            {
                return base.GetWeight(from, to);
            }
        }

        // FAKE JUST SPLITS ON SPACES!!!
        internal class FakeParser : ISentenceParser
        {
            public List<string> Parse(string inp)
            {
                return [.. inp.Split(" ")];
            }
        }

        public class FakeGraphBulder : IGBuilder<FakeGraph>
        {
            public override IGBuilder<FakeGraph> AddName(string name)
            {
                this.Name = name;
                return this;
            }

            public override IGBuilder<FakeGraph> AddRandom(Random rand)
            {
                this.Random = rand;
                return this;
            }

            public override IGBuilder<FakeGraph> AddTokenizer(ITokenizer t)
            {
                this.Tokenizer = t;
                return this;
            }

            public override IGBuilder<FakeGraph> AddTraversal(ITraversal t)
            {
                this.Traversal = t;
                return this;
            }

            public override IGBuilder<FakeGraph> AddVocabulary(IVocabulary v)
            {
                this.Vocabulary = v;
                return this;
            }

            public override FakeGraph Build()
            {
                return new FakeGraph(this);
            }
        }

        internal class FakeTravData
        {
            public static ConcurrentDictionary<string, int> BaseTravData()
            {
                var res = new ConcurrentDictionary<string, int>();
                res.TryAdd("test1", 1);
                res.TryAdd("test2", 5);
                return res;
            }
        }

        internal class FakePuncBehavior : IPuncBehavior
        {
            public string ApplyBehavior(string inp)
            {
                return string.Concat(inp, ".");
            }
        }

        internal class FakeCapBehavior : ICapBehavior
        {
            public string ApplyBehavior(string inp)
            {
                return inp;
            }
        }
    }
}
