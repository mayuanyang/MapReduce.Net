using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using Shouldly;
using Xunit;

namespace MapReduce.Net.Benchmark
{
    public class Harness
    {
        [Fact]
        public void RunWordcountBenchmark()
        {
            //BenchmarkRunner.Run<WordCountBenchmark1LinePerInput>();
            BenchmarkRunner.Run<WordCountBenchmarkSplitByCoreCapacity>();
            
        }

        [Fact]
        public async Task JustRunWithoutBenchmark()
        {
            var runner = new WordCountBenchmarkSplitByCoreCapacity();
            
            var result1 = await runner.WordCountWithoutCombinerAutoNumOfMappersPerNode();
            var result2 = await runner.WordCountWithCombinerAutoNumberOfMappers();
            var result3 = await runner.WordCountWithoutUsingMapReduce();
            result1.Count.ShouldBe(result2.Count);
            result2.Count.ShouldBe(result3.Count);
        }
        }
}
