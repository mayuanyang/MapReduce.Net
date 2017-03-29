using BenchmarkDotNet.Running;
using Xunit;

namespace MapReduce.Net.Benchmark
{
    public class Harness
    {
        [Fact]
        public void RunWordcountBenchmark()
        {
            var report = BenchmarkRunner.Run<WordCountBenchmark>();
            
        }
    }
}
