using System;

namespace MapReduce.Net.Impl
{
    public class JobConfigurator : IJobConfigurator
    {
        public Type TypeOfMapper { get; }
        public Type TypeOfCombiner { get; }
        public Type TypeOfReducer { get; }
        public Type TypeOfDataBatchProcessor { get; }
        public int NumberOfMappersPerNode { get; }
        public IDependancyScope DependancyScope { get; }


        public JobConfigurator(Type typeOfMapper, Type typeOfCombiner, Type typeOfReducer, Type typeOfDataBatchProcessor, int numberOfMappersPerNode = 2, IDependancyScope dependancyScope = null)
        {
            TypeOfMapper = typeOfMapper;
            TypeOfCombiner = typeOfCombiner;
            TypeOfReducer = typeOfReducer;
            TypeOfDataBatchProcessor = typeOfDataBatchProcessor;
            NumberOfMappersPerNode = numberOfMappersPerNode;
            DependancyScope = dependancyScope;
        }
        
    }
}