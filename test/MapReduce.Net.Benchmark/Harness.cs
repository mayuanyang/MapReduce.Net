using System.Threading.Tasks;
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

        [Fact]
        public async Task JustRunWithoutBenchmark()
        {
            var runner = new WordCountBenchmark();
            var result1 = await runner.WordCountWithoutCombinerAutoNumOfMappersPerNode();
            var result2 = await runner.WordCountWithCombinerAutoNumberOfMappers();
            var result3 = await runner.WordCountWithoutUsingMapReduce();
        }
    }
}
