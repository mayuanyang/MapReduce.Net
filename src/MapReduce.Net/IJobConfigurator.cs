using System;

namespace MapReduce.Net
{
    public interface IJobConfigurator
    {
        Type TypeOfMapper { get; }
        Type TypeOfCombiner { get; }
        Type TypeOfReducer { get; }
        Type TypeOfDataBatchProcessor { get; }
        int NumberOfMappersPerNode { get; }
        int NumberOfChunks { get; }
        IDependancyScope DependancyScope { get; }
        void ValidateConfiguration();

    }
}
