using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Reducers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace MapReduce.Net.Test
{
    public class WordCountWithCombiner10000Text
    {
        private string _content = "";

        private Job<string, List<KeyValuePair<string, int>>> _job;
        private List<KeyValuePair<string, int>> _result;
        public void GivenAString()
        {
            _content = FileUtil.ReadFile(typeof(WordCountWithCombiner10000Text).GetTypeInfo().Assembly, "MapReduce.Net.Test.10000Lines.txt");
        }

        public void AndGivenTheJobIsConfigured()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), typeof(WordCountCombiner), typeof(WordCountReducer), typeof(WordCountDataBatchProcessorSplitTo4Chunks));
            _job = new Job<string, List<KeyValuePair<string, int>>>(configurator);
        }

        public async Task WhenTheJobIsExecuted()
        {
            _result = await _job.Run<string, int>(_content);
        }

        public void ThenWeShouldGetTheWordCountResult()
        {
            int asserted = 0;
            foreach (var keyValuePair in _result)
            {
                if (keyValuePair.Key.ToUpper() == "EASIEST")
                {
                    keyValuePair.Value.ShouldBe(70);
                    asserted++;
                }
                
            }
            asserted.ShouldBe(1);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
