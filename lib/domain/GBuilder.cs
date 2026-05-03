using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vomark_redux.lib.domain
{
    public abstract class IGBuilder<G> where G : IGraph
    {
        internal IVocabulary? Vocabulary { get; set; }
        internal ITokenizer? Tokenizer { get; set; }
        internal ITraversal? Traversal { get; set; }
        internal Random? Random { get; set; }
        internal string? Name { get; set; }

        public IGBuilder() { }
        public abstract IGBuilder<G> AddTokenizer(ITokenizer t);
        public abstract IGBuilder<G> AddVocabulary(IVocabulary v);
        public abstract IGBuilder<G> AddTraversal(ITraversal t);
        public abstract IGBuilder<G> AddName(string name);
        public abstract IGBuilder<G> AddRandom(Random rand);
        public abstract G Build();

    }

    public class GraphBuilder : IGBuilder<Graph>
    {
        public override IGBuilder<Graph> AddName(string name)
        {
            this.Name = name;
            return this;
        }

        public override IGBuilder<Graph> AddRandom(Random rand)
        {
            this.Random = rand;
            return this;
        }

        public override IGBuilder<Graph> AddTokenizer(ITokenizer t)
        {
            this.Tokenizer = t;
            return this;
        }

        public override IGBuilder<Graph> AddTraversal(ITraversal t)
        {
            this.Traversal = t;
            return this;
        }

        public override IGBuilder<Graph> AddVocabulary(IVocabulary v)
        {
            this.Vocabulary = v;
            return this;
        }

        public override Graph Build()
        {
            if (Random == null)
            {
                throw new InvalidOperationException("Random is needed for graph traversal");
            }
            if (Traversal == null)
            {
                throw new InvalidOperationException("Traversal type is needed for graph traversal");
            }
            if (Vocabulary == null)
            {
                throw new InvalidOperationException("Vocabulary is needed for graph building/traversal");
            }

            return new Graph(this);
        }
    }
}
