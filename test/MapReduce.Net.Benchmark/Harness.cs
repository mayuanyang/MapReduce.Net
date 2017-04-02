using BenchmarkDotNet.Running;
using Xunit;

namespace MapReduce.Net.Benchmark
{
    public class Harness
    {
        [Fact]
        public void RunWordcountBenchmark10000Lines()
        {
            BenchmarkRunner.Run<WordCountBenchmark10000LinesSplitTo4Chunk>();
            BenchmarkRunner.Run<WordCountBenchmark10000LinesSplitToVariceChunk>();
        }

        [Fact]
        public void RunWordcountBenchmark40000Lines()
        {
            BenchmarkRunner.Run<WordCountBenchmark40000LinesSplitTo4Chunk>();
            BenchmarkRunner.Run<WordCountBenchmark40000LinesSplitTo8Chunk>();
        }

        [Fact]
        public void RunWaveDataBenchmark5065Records()
        {
            BenchmarkRunner.Run<WaveDataAvgBenchmark5065RecordsSplitTo4Chunk>();
        }

        [Fact]
        public void RunWaveDataBenchmark8Chunks5065Records()
        {
            BenchmarkRunner.Run<WaveDataAvgBenchmark5065RecordsSplitTo8Chunk>();
        }

        [Fact]
        public void RunWaveDataBenchmark128Chunks5065Records()
        {
            BenchmarkRunner.Run<WaveDataAvgBenchmark5065RecordsSplitTo128Chunk>();
        }

        [Fact]
        public void RunWaveDataBenchmark64Chunks5065Records()
        {
            BenchmarkRunner.Run<WaveDataAvgBenchmark5065RecordsSplitTo64Chunk>();
        }
    }
}
