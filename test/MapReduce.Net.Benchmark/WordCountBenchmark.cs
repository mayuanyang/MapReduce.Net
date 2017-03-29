using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;

namespace MapReduce.Net.Benchmark
{
    public class WordCountBenchmark
    {
        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithoutCombiner()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 20);
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, int>>> WordCountWithCombiner()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), 20);
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
