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
    }
}
