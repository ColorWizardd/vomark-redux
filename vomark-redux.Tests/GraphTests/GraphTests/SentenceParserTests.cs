using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using vomark_redux.lib.svc;

namespace vomark_redux.Tests.GraphTests.GraphTests
{
    public class SentenceParserTests
    {
        [Fact]
        public void TestBasicPuncParser()
        {
            BasicPuncParser bp = new();
            List<string> expected = ["it's", "a", "sentence", "!"];
            var resList = bp.Parse("it's a sentence!");
            Assert.Equal(expected.Count, resList.Count);
            for(int i = 0; i < resList.Count; ++i)
            {
                Assert.Equal(expected[i], resList[i]);
            }
        }
    }
}
