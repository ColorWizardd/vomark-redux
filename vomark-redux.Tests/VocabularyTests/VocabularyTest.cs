using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using Xunit;
using static vomark_redux.Tests.TestDoubles;

namespace vomark_redux.Tests.VocabularyTests
{
    public class VocabularyTest
    {
        public class VocabBaseTest
        {
            [Fact]
            public void TestAddToken()
            {
                // Vocabs initialize with ROOT/TERM nodes already
                FakeTokenizer ft = new();
                Vocabulary voc = new(ft);

                List<string> vList = voc.GetList();
                Assert.Equal(2, vList.Count);

                voc.GetOrAddToken("test1");
                voc.GetOrAddToken("test2");
                vList = voc.GetList();
                Assert.Equal(4, vList.Count);
                Assert.Equal("test1", vList[1]);
                Assert.Equal(3, voc.GetOrAddToken("test2"));
            }

            [Fact]
            public void TestExistingList()
            {
                FakeTokenizer ft = new();
                Vocabulary voc = new(ft, ["test1", "test2"]);
                Assert.Equal(0, voc.GetOrAddToken("test1"));
                Assert.Equal(1, voc.GetOrAddToken("test2"));
            }
        }
    }
}
