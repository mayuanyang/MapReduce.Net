using System.Threading.Tasks;
using MapReduce.Net.Test.Mappers;
using MapReduce.Net.Impl;
using MapReduce.Net.Test.DataBatchProcessors;
using MapReduce.Net.Test.Reducers;
using Xunit;

namespace MapReduce.Net.Test
{
    public class WordCountTest
    {
        private string _content = "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported" +
                                  "Decouple does matter, A simple mediator for .Net for sending command, publishing event and request response with pipelines supported";

        private Job _job;
        public void GivenAString()
        {
            
        }

        public void AndGivenTheJobIsConfigured()
        {
            var configurator =
                new MapReduceConfigurator(typeof(WordCountMapper), null, typeof(WordCountReducer), typeof(WordCountDataBatchProcessor));
            _job = new Job(configurator);
        }

        public Task WhenTheJobIsExecuted()
        {
            return Task.FromResult(0);
        }

        public void ThenWeShouldGetTheWordCountResult()
        {
            
        }

        [Fact]
        public void Test1()
        {
        }
    }
}
