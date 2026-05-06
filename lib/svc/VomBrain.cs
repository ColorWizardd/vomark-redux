using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.lib.svc
{
    public abstract class IVomBrain<G> where G : IGraph
    {
        const UInt16 MAX_LEN = 64;
        protected IGDirector<G> Director { get; private set; }
        public IVomBrain(IGDirector<G> director){ Director = director;}
        public abstract void AddExistingGraph(G g);
        public abstract void AddData(string data, List<string> targs, IDataParser parser);
        public abstract string GenSentence(List<string> targs, IPuncBehavior ip, ICapBehavior ic, int maxLen = MAX_LEN);
    }

    public class VomBrain<G> : IVomBrain<G> where G : IGraph
    {
        public VomBrain(IGDirector<G> director) : base(director) { }

        public override void AddData(string data, List<string> targs, IDataParser parser)
        {
            List<string> subData = parser.Parse(data);
            foreach(string s in subData)
            {
                foreach(string t in targs)
                {
                    Director.SendSentence(s, Director.FindGraph(t));
                }
            }
        }

        public override void AddExistingGraph(G g)
        {
            Director.AddGraph(g);
        }

        public override string GenSentence(List<string> targs, IPuncBehavior ip, ICapBehavior ic, int maxLen = 64)
        {
            try
            {
                return Director.GenSentence(
                    Director.CombineGraphs(targs),
                    ip, ic, maxLen
                    );
            }
            catch (Exception e)
            {
                //PLACEHOLDER - WILL MAYBE GET LOGGING SET UP? IDK TOO MUCH FOR A LIB I GUESS.
                Console.WriteLine(e.Message);
                throw;
            }
            return "";
        }
    }

    public interface IDataParser
    {
        /**
         * Compared to ISentenceParser, meant to SEPARATE data into lists instead of token parsing.
         */
        public abstract List<string> Parse(string inp);
    }

    public partial class PuncDataParser : IDataParser
    {
        public List<string> Parse(string inp)
        {
            inp = inp.Trim().ToLower();
            return [.. RegPattern().Split(inp)];
        }

        [GeneratedRegex(@"[.!?\n]+")]
        public static partial Regex RegPattern();
    }
}
