using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MapReduce.Net.Impl;
using MapReduce.Net.Test;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;
using Xunit;

namespace MapReduce.Net.Benchmark
{
    public class WordCountBenchmark
    {
        [Benchmark]
        public async Task WordCountWithoutCombiner()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
        }

        [Benchmark]
        public async Task WordCountWithCombiner()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            var job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
            var result = await job.Run<string, int>(ReadFile());
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
