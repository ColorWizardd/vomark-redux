using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vomark_redux.lib.domain
{
    public abstract class IVocabulary
    {
        protected ConcurrentDictionary<string, int> _vocabList;
        protected ITokenizer _tokenizer;

        public IVocabulary(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _vocabList = [];
        }
        public IVocabulary(ITokenizer tokenizer, ConcurrentDictionary<string, int> vocabList)
        {
            _vocabList = vocabList;
            _tokenizer = tokenizer;
        }

        public List<string> GetList()
        {
            return [.. _vocabList.Keys];
        }

        abstract public int GetOrAddToken(string token);
    }

    public class Vocabulary : IVocabulary
    {

        public Vocabulary(ITokenizer tokenizer) : base(tokenizer) { }
        public Vocabulary(ITokenizer tokenizer, ConcurrentDictionary<string, int> vocabList) : base(tokenizer, vocabList) { }
        public override int GetOrAddToken(string token)
        {
            return _vocabList.GetOrAdd(token, x => _tokenizer.Tokenize(token));
        }
    }

    public interface ITokenizer
    {
        abstract int Tokenize(string inp);
    }

    public class CountTokenizer : ITokenizer
    {
        protected int currCount = 0;
        public int Tokenize(string inp)
        {
            return currCount++;
        }
    }


}
