using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
    public class WordCountBenchmarkSplitByCoreCapacity
    {
        private string _content;
        public WordCountBenchmarkSplitByCoreCapacity()
        {
            _content = ReadFile();
        }
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitByCoreCapacity));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitByCoreCapacity), 2);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

   

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombinerAutoNumberOfMappers()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitByCoreCapacity));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        
       
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitByCoreCapacity), 2);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(_content);
            return result;
        }

        [Benchmark]
        public async Task<Hashtable> WordCountWithoutUsingMapReduce()
        {
            var dataProcessor = new WordCountDataBatchProcessorSplitByCoreCapacity();
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



        private string ReadFile()
        {
            var assembly = typeof(WordCountBenchmark1LinePerInput).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("MapReduce.Net.Benchmark.GoogleMapApi.txt");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


    }
}
