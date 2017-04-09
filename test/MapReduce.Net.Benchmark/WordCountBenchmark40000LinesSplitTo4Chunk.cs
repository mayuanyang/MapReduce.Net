using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Order;
using MapReduce.Net.Impl;
using MapReduce.Net.Test;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;

namespace MapReduce.Net.Benchmark
{
    [RankColumn]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class WordCountBenchmark40000LinesSplitTo4Chunk
    {
        private readonly string _content;
        public WordCountBenchmark40000LinesSplitTo4Chunk()
        {
            _content = FileUtil.ReadFile("MapReduce.Net.Benchmark.40000Lines.txt");
        }
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job(configurator);
            var result = await job.Run<string, List<KeyValuePair<string, int>>, string, int, string, int>(_content);
            return result;
        }

        
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 2);
            var job = new Job(configurator);
            var result = await job.Run<string, List<KeyValuePair<string, int>>, string, int, string, int>(_content);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombinerAutoNumberOfMappers()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job(configurator);
            var result = await job.Run<string, List<KeyValuePair<string, int>>, string, int, string, int>(_content);
            return result;
        }
       
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 2);
            var job = new Job(configurator);
            var result = await job.Run<string, List<KeyValuePair<string, int>>, string, int, string, int>(_content);
            return result;
        }

  
        [Benchmark]
        public async Task<Hashtable> WordCountWithoutUsingMapReduce()
        {
            var dataProcessor = new WordCountDataBatchProcessor();
            var lines = await dataProcessor.Run(_content);

            var wordCount = new Hashtable();
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                foreach (var word in words)
                {
                    if (wordCount.ContainsKey(word.ToUpper()))
                    {
                        wordCount[word.ToUpper()] = (int) wordCount[word.ToUpper()] + 1;
                    }
                    else
                    {
                        wordCount.Add(word.ToUpper(), 1);
                    }
                }
            }
            return wordCount;
        }
        
    }
}
