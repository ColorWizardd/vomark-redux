using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vomark_redux.lib.domain
{

    public abstract class IGraph
    {
        protected IVocabulary? _vocabulary;
        protected ConcurrentDictionary<(string, string), int> _edgeList;
        protected ITraversal? _traversal;
        protected ITokenizer? _tokenizer;
        protected string _name;

        public IGraph(string name) { _name = name; _edgeList = []; }
        public void SetVocabulary(IVocabulary voc) { _vocabulary = voc; }
        public void SetTraversal(ITraversal tra) { _traversal = tra; }
        public void SetName(string name) { _name = name; }
        public void SetTokenizer(ITokenizer tok) { _tokenizer = tok; }

        abstract public void AddOrStrengthenEdge(string from, string to, int weight = 1);
        abstract public int GetWeight(string from, string to);
        abstract public string GetNextNode(string curr);
    }

    public class Graph : IGraph
    {
        public Graph(string name) : base(name) { }

        public override void AddOrStrengthenEdge(string from, string to, int weight = 1)
        {
            _edgeList.AddOrUpdate((from, to), weight, (key, oldWeight) => oldWeight + weight);
        }

        public override string GetNextNode(string curr)
        {
            throw new NotImplementedException();
        }

        public override int GetWeight(string from, string to)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ITraversal
    {

    }

    public class WeightedRandTraversal : ITraversal
    {

    }

    public class GreedyTraversal : ITraversal
    {

    }

}
