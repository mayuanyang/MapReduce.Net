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
        public int NumberOfChunks { get; }
        public IDependancyScope DependancyScope { get; }


        public JobConfigurator(Type typeOfMapper, Type typeOfCombiner, Type typeOfReducer, Type typeOfDataBatchProcessor, int numberOfMappersPerNode = 0, int numberOfChunks = 4, IDependancyScope dependancyScope = null)
        {
            if (numberOfChunks == 0)
            {
                numberOfChunks = 4;
            }
            TypeOfMapper = typeOfMapper;
            TypeOfCombiner = typeOfCombiner;
            TypeOfReducer = typeOfReducer;
            TypeOfDataBatchProcessor = typeOfDataBatchProcessor;
            NumberOfMappersPerNode = numberOfMappersPerNode;
            NumberOfChunks = numberOfChunks;
            DependancyScope = dependancyScope;
        }
        
    }
}