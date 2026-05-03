using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.lib.repo
{
    public abstract class IGBuilder<IGraph>
    {
        protected IGraph Graph { get; private set; }

        public IGBuilder()
        {
            Graph = Reset();
        }
        public abstract IGBuilder<IGraph> AddTokenizer(ITokenizer t);
        public abstract IGBuilder<IGraph> AddVocabulary(IVocabulary v);
        public abstract IGBuilder<IGraph> AddTraversal(ITraversal t);
        public abstract IGBuilder<IGraph> AddName(string name);
        public abstract IGraph Reset();
        public IGraph Build() { return Graph; }

    }

    public class GraphBuilder : IGBuilder<Graph>
    {
        public override IGBuilder<Graph> AddName(string name)
        {
            Graph.SetName(name);
            return this;
        }

        public override IGBuilder<Graph> AddTokenizer(ITokenizer t)
        {
            Graph.SetTokenizer(t);
            return this;
        }

        public override IGBuilder<Graph> AddTraversal(ITraversal t)
        {
            Graph.SetTraversal(t);
            return this;
        }

        public override IGBuilder<Graph> AddVocabulary(IVocabulary v)
        {
            Graph.SetVocabulary(v);
            return this;
        }

        public override Graph Reset()
        {
            return new Graph();
        }
    }
}
