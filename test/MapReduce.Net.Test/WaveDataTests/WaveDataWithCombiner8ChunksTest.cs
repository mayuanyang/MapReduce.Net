using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;
using MapReduce.Net.Test.Utils;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace MapReduce.Net.Test.WaveDataTests
{
    public class WaveDataWithCombiner8ChunksTest
    {
        private List<WaveData> _waveDatas;

        private Job _job;
        private List<KeyValuePair<string, WaveDataAverage>> _result;
        public void GivenTheWaveData()
        {
            var resourceStream = typeof(FileUtil).GetTypeInfo().Assembly.GetManifestResourceStream("MapReduce.Net.Test.wave-7dayopdata.csv");

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

        public void AndGivenTheJobIsConfigured()
        {
            var configurator =
                new JobConfigurator(typeof(WaveDataMapper), typeof(WaveDataCombiner), typeof(WaveDataReducer), typeof(WaveDataBatchProcessor), 2, 8);
            _job = new Job(configurator);
        }

        public async Task WhenTheJobIsExecuted()
        {
            _result = await _job.Run<List<WaveData>, List<KeyValuePair<string, WaveDataAverage>>, string, List<WaveData>, string, List<WaveData>>(_waveDatas);
        }

        public void ThenWeShouldGetTheWordCountResult()
        {
            _result.Count.ShouldBe(14);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
