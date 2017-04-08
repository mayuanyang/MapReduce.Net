using System.Collections.Generic;
using System.Threading.Tasks;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Combiners;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Test.Reducers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace MapReduce.Net.Test.WordCountTests
{
    public class WordCountWithCombinerTest
    {
        private string _content = "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported\n" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported\n" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported";

        private Job _job;
        private List<KeyValuePair<string, int>> _result;
        public void GivenAString()
        {
            
        }

        public void AndGivenTheJobIsConfigured()
        {
            var configurator = new JobConfigurator();

            configurator.UseMapper(typeof(WordCountMapper))
                .UseCombiner(typeof(WordCountCombiner))
                .UseReducer(typeof(WordCountReducer))
                .UseDataBatchProcessor(typeof(WordCountDataBatchProcessor))
                .WithNumberOfChunk(4);
                
            _job = new Job(configurator);
        }

        public async Task WhenTheJobIsExecuted()
        {
            _result = await _job.Run<string, List<KeyValuePair<string, int>>, string , int>(_content);
        }

        public void ThenWeShouldGetTheWordCountResult()
        {
            int asserted = 0;
            foreach (var keyValuePair in _result)
            {
                if (keyValuePair.Key.ToUpper() == "DECOUPLE")
                {
                    keyValuePair.Value.ShouldBe(3);
                    asserted++;
                }
                if (keyValuePair.Key.ToUpper() == "FOR")
                {
                    keyValuePair.Value.ShouldBe(6);
                    asserted++;
                }
                if (keyValuePair.Key.ToUpper() == "COMMAND,")
                {
                    keyValuePair.Value.ShouldBe(3);
                    asserted++;
                }
            }
            asserted.ShouldBe(3);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
