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
    public class WordCountBenchmark
    {
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner15MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 15);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner30MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 30);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner50MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 50);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner100MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 100);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner150MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 150);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner200MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 200);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombinerAutoNumberOfMappers()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner5MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 5);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner15MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 15);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner30MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 30);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner150MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 150);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<Hashtable> WordCountWithoutUsingMapReduce()
        {
            var dataProcessor = new WordCountDataBatchProcessor();
            var lines = await dataProcessor.Run(ReadFile());

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
            var assembly = typeof(WordCountBenchmark).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("MapReduce.Net.Benchmark.GoogleMapApi.txt");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


    }
}
