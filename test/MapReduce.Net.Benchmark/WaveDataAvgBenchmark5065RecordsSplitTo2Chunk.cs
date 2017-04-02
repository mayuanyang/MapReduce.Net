using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Order;
using CsvHelper;
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
    public class WaveDataAvgBenchmark5065RecordsSplitTo2Chunk
    {
        private List<WaveData> _waveDatas;
        public WaveDataAvgBenchmark5065RecordsSplitTo2Chunk()
        {
            var resourceStream = typeof(WaveDataAvgBenchmark5065RecordsSplitTo4Chunk).GetTypeInfo().Assembly.GetManifestResourceStream("MapReduce.Net.Benchmark.wave-7dayopdata.csv");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                var line = reader.ReadLine(); // Skip the first line
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.IgnoreHeaderWhiteSpace = true;
                    csv.Configuration.IsHeaderCaseSensitive = false;
                    _waveDatas = csv.GetRecords<WaveData>().ToList();
                }

            }
        }
        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithoutCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), null, typeof(WaveDataReducer), typeof(WaveDataBatchProcessor));
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }
       
        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithoutCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), null, typeof(WaveDataReducer), typeof(WaveDataBatchProcessor), 2);
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithoutCombiner4MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), null, typeof(WaveDataReducer), typeof(WaveDataBatchProcessor), 4);
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithCombinerAutoNumOfMappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), typeof(WaveDataCombiner), typeof(WaveDataReducer), typeof(WaveDataBatchProcessor));
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }

        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithCombiner2MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), typeof(WaveDataCombiner), typeof(WaveDataReducer), typeof(WaveDataBatchProcessor), 2);
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }


        [Benchmark]
        public async Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithCombiner4MappersPerNode()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), typeof(WaveDataCombiner), typeof(WaveDataReducer), typeof(WaveDataBatchProcessor), 4);
            var job = new Job(configurator);
            var result = await job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>>(_waveDatas);
            return result;
        }

        [Benchmark]
        public Task<List<KeyValuePair<string, WaveDataAverage>>> WaveDataWithoutUsingMapReduce()
        {

            var result = new List<KeyValuePair<string, WaveDataAverage>>();
            var siteGroups = _waveDatas.GroupBy(x => x.Site, y => y);
            foreach (var siteGroup in siteGroups)
            {
                //var totalSeconds = siteGroup.Sum(x => x.Seconds);
                var totalHsig = siteGroup.Sum(x => x.Hsig);
                var totalHmax = siteGroup.Sum(x => x.Hmax);
                var totalTp = siteGroup.Sum(x => x.Tp);
                var totalTz = siteGroup.Sum(x => x.Tz);
                var totalSst = siteGroup.Sum(x => x.Sst);
                var totalDirection = siteGroup.Sum(x => x.Direction);
                var count = siteGroup.Count();
                var avg = new WaveDataAverage
                {
                    Site = siteGroup.Key,
                    //Seconds = totalSeconds / count,
                    Hisg = totalHsig / count,
                    Hmax = totalHmax / count,
                    Tp = totalTp / count,
                    Tz = totalTz / count,
                    Sst = totalSst / count,
                    Direction = totalDirection / count
                };
                result.Add(new KeyValuePair<string, WaveDataAverage>(siteGroup.Key, avg));
            }
            return Task.FromResult(result);
        }

    }
}
