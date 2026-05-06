using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using vomark_redux.lib.svc;
using static vomark_redux.Tests.TestDoubles;

namespace vomark_redux.Tests.VomBrainTests
{
    public class VomBrainTests
    {
        public class VomBrainBehaviorTests
        {
            [Fact]
            public void TestAddDataSingleTarg()
            {
                VomBrain<FakeGraph> fg = new(new GraphDirector<FakeGraph>(new FakeParser()));
                FakeGraphBulder gb = new();
                FakeGraph fake = gb.AddTraversal(new FakeTraversal())
                    .AddVocabulary(new FakeVocabulary(new FakeTokenizer()))
                    .AddRandom(new Random())
                    .AddName("fake1")
                    .Build();

                fg.AddExistingGraph(fake);

                fg.AddData(
                    "This is a new sentence.",
                    ["fake1"],
                    new PuncDataParser()
                    );

                Assert.Equal("this is a new sentence.", fg.GenSentence(
                    ["fake1"],
                    new FakePuncBehavior(),
                    new FakeCapBehavior()
                    ));

            }

            [Fact]
            public void TestMultipleTargs()
            {
                VomBrain<FakeGraph> fg = new(new GraphDirector<FakeGraph>(new FakeParser()));
                FakeGraphBulder gb1 = new();
                FakeGraph fake1 = gb1.AddTraversal(new WeightedRandTraversal())
                    .AddVocabulary(new FakeVocabulary(new FakeTokenizer()))
                    .AddRandom(new Random())
                    .AddName("fake1")
                    .Build();

                FakeGraphBulder gb2 = new();
                FakeGraph fake2 = gb2.AddTraversal(new WeightedRandTraversal())
                    .AddVocabulary(new FakeVocabulary(new FakeTokenizer()))
                    .AddRandom(new Random())
                    .AddName("fake2")
                    .Build();

                fg.AddExistingGraph(fake1);
                fg.AddExistingGraph(fake2);

                fg.AddData("This is one sentence.", ["fake1"], new PuncDataParser());
                fg.AddData("This is another sentence!", ["fake2"], new PuncDataParser());

                List<string> resData = [];
                for(int i = 0; i < 10; ++i)
                {
                    resData.Add(fg.GenSentence(["fake1", "fake2"],
                        new FakePuncBehavior(),
                        new FakeCapBehavior()
                        ));
                }
                Assert.Contains("this is one sentence.", resData);
                Assert.Contains("this is another sentence.", resData);
            }

            [Fact]
            public void TestBadTargs()
            {
                VomBrain<FakeGraph> fg = new(new GraphDirector<FakeGraph>(new FakeParser()));
                var err = Assert.Throws<ArgumentException>(() => fg.GenSentence(
                    ["fake1"], new FakePuncBehavior(), new FakeCapBehavior()
                    ));
                Assert.Equal("No graph found with the given name", err.Message);
            }
        }
    }
}
