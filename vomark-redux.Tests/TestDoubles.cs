/**
 * Shared collection of test data.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;

namespace vomark_redux.Tests
{
    internal class TestDoubles
    {
        internal class FakeTokenizer : ITokenizer
        {
            int currCount = 0;
            public int Tokenize(string inp)
            {
                return currCount++;
            }
        }
    }
}
