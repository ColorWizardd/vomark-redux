using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vomark_redux.lib.domain;
using vomark_redux.lib.svc;
using Xunit.Abstractions;
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

        public class VomBrainFullRun(ITestOutputHelper helper)
        {
            // These are not treated as tests per se, but runtime checks to make sure we're not slow.
            // Also, we're 100% testing concrete implementation for ALL components here.
            private readonly ITestOutputHelper _helper = helper;
            private readonly string TEST_PATH_1 = "../../../VomBrainTests/VBTestFile1.txt";
            private readonly string TEST_PATH_2 = "../../../VomBrainTests/VBTestFile2.txt";
            private readonly int TEST_GEN_COUNT = 20;

            [Fact]
            public void TestFullRun()
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var g1Lines = File.ReadAllLines(TEST_PATH_1);
                var g2Lines = File.ReadAllLines(TEST_PATH_2);

                BasicPuncParser parser = new();
                PuncDataParser dParser = new();

                VomBrain<Graph> vb = new(new GraphDirector<Graph>(parser));
                GraphBuilder gb1 = new();
                Graph g1 = gb1.AddTraversal(new WeightedRandTraversal())
                    .AddVocabulary(new Vocabulary(new CountTokenizer()))
                    .AddRandom(new Random())
                    .AddName("test1")
                    .Build();

                GraphBuilder gb2 = new();
                Graph g2 = gb2.AddTraversal(new WeightedRandTraversal())
                    .AddVocabulary(new Vocabulary(new CountTokenizer()))
                    .AddRandom(new Random())
                    .AddName("test2")
                    .Build();

                vb.AddExistingGraph(g1);
                vb.AddExistingGraph(g2);

                foreach(string line in g1Lines)
                {
                    vb.AddData(line, ["test1"], dParser);
                }

                foreach(string line in g2Lines)
                {
                    vb.AddData(line, ["test2"], dParser);
                }

                watch.Stop();

                _helper.WriteLine($"TIME TO READ/ADD DATA: {watch.ElapsedMilliseconds}ms");

                watch.Restart();

                for(int i = 0; i < TEST_GEN_COUNT; ++i)
                {
                    _helper.WriteLine("\n" + vb.GenSentence(["test1", "test2"], new EndPeriod(), new FirstCap()));
                }

                watch.Stop();

                _helper.WriteLine($"TIME TO WRITE {TEST_GEN_COUNT} SENTENCES: {watch.ElapsedMilliseconds}ms");
            }
        }
    }
}
