using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Order;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;

namespace MapReduce.Net.Benchmark
{
    [RankColumn]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class WordCountBenchmark10000LinesSplitTo8Chunk
    {
        private readonly string _content;
        public WordCountBenchmark10000LinesSplitTo8Chunk()
        {
            _content = FileUtil.ReadFile(typeof(WordCountBenchmark10000LinesSplitTo8Chunk).GetTypeInfo().Assembly, "MapReduce.Net.Benchmark.10000Lines.txt");
        }
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks), 2);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner4MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks), 4);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombinerAutoNumberOfMappers()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }
        
       
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks), 2);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

    
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner4MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo8Chunks), 4);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        [Benchmark]
        public async Task<Hashtable> WordCountWithoutUsingMapReduce()
        {
            var dataProcessor = new WordCountDataBatchProcessorSplitTo8Chunks();
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
