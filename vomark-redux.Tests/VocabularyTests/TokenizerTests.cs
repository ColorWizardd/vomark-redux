using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.Tests.VocabularyTests
{
    public class TokenizerTests
    {
        public class CountTokenizerTests
        {
            [Fact]
            public void TestCTIncrement()
                // Checking increment behavior. Starts at 0, increments AFTER returning value.
            {
                CountTokenizer ct = new();
                ct.Tokenize("str1");
                ct.Tokenize("str2");
                Assert.Equal(2, ct.Tokenize("str1"));
            }
        }
    }
}
