using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.lib.svc
{
    internal abstract class IGDirector
    {
        internal ConcurrentDictionary<string, IGraph> GraphSet { get; private set; } = [];
        abstract public void AddGraph();
        abstract public IGraph FindGraph(string gName);
        abstract public IGraph CombineGraphs(string g1, string g2);
    }

    internal class GraphDirector : IGDirector
    {
        public override void AddGraph()
        {
            throw new NotImplementedException();
        }

        public override IGraph CombineGraphs(string g1, string g2)
        {
            throw new NotImplementedException();
        }

        public override IGraph FindGraph(string gName)
        {
            throw new NotImplementedException();
        }
    }
}
