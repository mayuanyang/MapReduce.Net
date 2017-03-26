using System;

namespace MapReduce.Net.Impl
{
    public class JobConfigurator : IJobConfigurator
    {
        public Type TypeOfMapper { get; }
        public Type TypeOfCombiner { get; }
        public Type TypeOfReducer { get; }
        public Type TypeOfDataBatchProcessor { get; }
        public Type TypeOfContext { get; }
        public IDependancyScope DependancyScope { get; }

        public JobConfigurator(Type typeOfMapper, Type typeOfCombiner, Type typeOfReducer, Type typeOfDataBatchProcessor, Type typeOfContext, IDependancyScope dependancyScope = null)
        {
            TypeOfMapper = typeOfMapper;
            TypeOfCombiner = typeOfCombiner;
            TypeOfReducer = typeOfReducer;
            TypeOfDataBatchProcessor = typeOfDataBatchProcessor;
            TypeOfContext = typeOfContext;
            DependancyScope = dependancyScope;
        }
        
    }
}