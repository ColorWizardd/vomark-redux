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
        protected ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _edgeList;
        protected ITraversal? _traversal;
        protected ITokenizer? _tokenizer;
        protected Random? _rand;
        protected string? _name;

        public IGraph() { _edgeList = []; }
        public IGraph(string name) { _name = name; _edgeList = []; }
        
        public void SetVocabulary(IVocabulary voc) { _vocabulary = voc; }
        public void SetTraversal(ITraversal tra) { _traversal = tra; }
        public void SetName(string name) { _name = name; }
        public void SetTokenizer(ITokenizer tok) { _tokenizer = tok; }
        public void SetRandom(Random rand) { _rand = rand; }

        abstract public void AddOrStrengthenEdge(string from, string to, int weight = 1);
        abstract public int GetWeight(string from, string to);
        abstract public string? GetName();
        abstract public string? GetNextNode(string curr);
        abstract public ConcurrentDictionary<string, int>? FindAllNext(string curr);
        abstract public ConcurrentDictionary<string, ConcurrentDictionary<string, int>> GetEdgeList();
    }

    public class Graph : IGraph
    {
        public Graph() : base() { }

        public Graph(GraphBuilder gb) : base()
        {
            _vocabulary = gb.Vocabulary;
            _tokenizer = gb.Tokenizer;
            _traversal = gb.Traversal;
            _rand = gb.Random;
            _name = gb.Name;

            _edgeList = [];
        }
        public Graph(string name) : base(name) { }

        public override void AddOrStrengthenEdge(string from, string to, int weight = 1)
        {
            var nextList = _edgeList.GetOrAdd(from, x => new ConcurrentDictionary<string, int>());
            nextList.AddOrUpdate(to, weight, (x, oldWeight) => weight + oldWeight);
        }

        public override ConcurrentDictionary<string, int>? FindAllNext(string curr)
        {
            if (_edgeList.TryGetValue(curr, out var nextList)){
                return nextList;
            }
            return null;
        }

        public override ConcurrentDictionary<string, ConcurrentDictionary<string, int>> GetEdgeList()
        {
            return _edgeList;
        }

        public override string? GetName()
        {
            return _name;
        }

        public override string? GetNextNode(string curr)
        {
            if(_traversal == null || _rand == null) { return null; }
            var nextList = FindAllNext(curr);
            if (nextList != null)
            {
            return _traversal.Next(nextList, _rand);
            }
            return null;
        }

        public override int GetWeight(string from, string to)
        {
            if(_edgeList.TryGetValue(from, out var nextList))
            {
                nextList.TryGetValue(to, out int res);
                return res;
            }
            return -1;
        }
    }

    public interface ITraversal
    {
        abstract string? Next(ConcurrentDictionary<string, int> nextList, Random rand);
    }

    public class WeightedRandTraversal : ITraversal
    {
        public string? Next(ConcurrentDictionary<string, int> nextList, Random rand)
        {
            int weightSum = nextList.Values.Sum();
            int currThresh = 0;
            int thresh = rand.Next(0, weightSum);
            foreach(string key in nextList.Keys)
            {
                currThresh += nextList[key];
                if(currThresh >= thresh)
                {
                    return key;
                }
            }
            return null;
        }
    }

    public class GreedyTraversal : ITraversal
    {
        public string? Next(ConcurrentDictionary<string, int> nextList, Random rand)
        {
            return nextList.MaxBy(x => x.Value).Key;
        }
    }

}
