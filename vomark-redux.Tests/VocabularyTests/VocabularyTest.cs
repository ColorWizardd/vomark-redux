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
                FakeTokenizer ft = new();
                Vocabulary voc = new(ft);

                voc.GetOrAddToken("test1");
                voc.GetOrAddToken("test2");
                List<string> vList = voc.GetList();
                Assert.Equal(2, vList.Count);
                Assert.Equal("test1", vList[0]);
                Assert.Equal(1, voc.GetOrAddToken("test2"));
            }
        }
    }
}
