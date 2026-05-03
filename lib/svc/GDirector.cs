using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.lib.svc
{
    internal abstract class IGDirector
    {
        internal const string ERR_NAME_MISSING = "ERR_NAME_MISSING";
        internal ConcurrentDictionary<string, IGraph> GraphSet { get; private set; } = [];
        internal ISentenceParser Parser { get; private set; }

        internal IGDirector(ISentenceParser p)
        {
            Parser = p;
        }

        abstract public void AddGraph(IGraph g);
        abstract public IGraph FindGraph(string gName);
        abstract public IGraph CombineGraphs(string g1, string g2);
        abstract public void SendSentence(string inp, IGraph g);
    }

    internal class GraphDirector : IGDirector
    {
        GraphDirector(ISentenceParser p) : base(p) { }
        public override void AddGraph(IGraph g)
        {
            string n = g.GetName() ?? ERR_NAME_MISSING;
            if(n == ERR_NAME_MISSING)
            {
                throw new ArgumentException("Graph does not contain name, cannot be added to set");
            }
            if(!GraphSet.TryAdd(n, g))
            {
                throw new InvalidOperationException("Cannot add graph " + g.GetName() + " to set");
            }
        }

        public override IGraph CombineGraphs(string g1, string g2)
        {
            GraphSet.TryGetValue(g1, out IGraph? graph1);
            GraphSet.TryGetValue(g2, out IGraph? graph2);

            if(graph1 == null || graph2 == null)
            {
                throw new ArgumentException("One or more specfied graphs do not exist in the set");
            }
            IGraph res = graph1;
            foreach(var edge in graph2.GetEdgeList())
            {
                foreach(var next in edge.Value)
                {
                    res.AddOrStrengthenEdge(edge.Key, next.Key, next.Value);
                }
            }
            return res;
        }

        public override IGraph FindGraph(string gName)
        {
            throw new NotImplementedException();
        }

        public override void SendSentence(string inp, IGraph g)
        {
            List<string> wordList = Parser.Parse(inp);
            g.AddOrStrengthenEdge(Vocabulary.NODE_ROOT, wordList[0]);
            if(wordList.Count > 1)
            {
                for(int i = 1; i < wordList.Count; ++i)
                {
                    g.AddOrStrengthenEdge(wordList[i - 1], wordList[i]);
                }
            }
            g.AddOrStrengthenEdge(wordList[^1], Vocabulary.NODE_TERM);
        }
    }

    public interface ISentenceParser
    {
        /**
         * Meant to parse ONE SENTENCE AT A TIME
         */
        abstract List<string> Parse(string inp);
    }

    /** 
     * Parses all punctuation as separate tokens.
     * Meaning "it'd" -> ["it", "'", "d"]
     */

    public partial class BasicPuncParser : ISentenceParser
    {
        [GeneratedRegex("\\w+|[^\\w\\s]", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex RegPattern { get; }

        public List<string> Parse(string inp)
        {
            MatchCollection matches = RegPattern.Matches(inp);
            List<string> res = new(matches.Count);

            foreach (Match match in matches){
                res.Add(match.Value);
            }
            return res;
        }

    }
}
