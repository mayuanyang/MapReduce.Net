using System.Threading.Tasks;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.Context;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Reducers;
using TestStack.BDDfy;
using Xunit;

namespace MapReduce.Net.Test
{
    public class WordCountTest
    {
        private string _content = "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported\n" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported\n" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported";

        private Job _job;
        public void GivenAString()
        {
            
        }

        public void AndGivenTheJobIsConfigured()
        {
            var configurator =
                new JobConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor), typeof(WordCountContext));
            _job = new Job(configurator);
        }

        public async Task WhenTheJobIsExecuted()
        {
            await _job.Run(_content);
        }

        public void ThenWeShouldGetTheWordCountResult()
        {
            
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
