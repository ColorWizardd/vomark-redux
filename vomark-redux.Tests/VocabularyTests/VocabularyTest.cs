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
                Assert.Equal(1, voc.GetOrAddToken("test2"));
                Assert.Equal(0, voc.GetOrAddToken("test1"));
                Assert.Equal(2, voc.GetOrAddToken("test3"));
            }
        }
    }
}
