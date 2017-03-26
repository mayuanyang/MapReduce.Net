using System;
using System.Threading.Tasks;

namespace MapReduce.Net.Impl
{
    public class Job : IJob
    {
        private readonly IMapReduceConfigurator _configurator;

        public Job(IMapReduceConfigurator configurator)
        {
            _configurator = configurator;
        }
        public Task Run()
        {
            if (_configurator.TypeOfMapper == null)
            {
                throw new ArgumentException("Type of mapper is not provided");
            }
            if (_configurator.TypeOfReducer == null)
            {
                throw new ArgumentException("Type of reducer is not provided");
            }
            if (_configurator.TypeOfDataBatchProcessor == null)
            {
                throw new ArgumentException("Type of IDataBatchProcessor is not provided");
            }

            if (_configurator.DependancyScope == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                var dataProcessor = _configurator.DependancyScope.Resolve<IDataBatchProcessor>();
                
            }

            throw new System.NotImplementedException();
        }
    }
}