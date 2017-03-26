using System;

namespace MapReduce.Net
{
    public interface IMapReduceConfigurator
    {
        Type TypeOfMapper { get; }
        Type TypeOfCombiner { get; }
        Type TypeOfReducer { get; }
        Type TypeOfDataBatchProcessor { get; }
        IDependancyScope DependancyScope { get; }
    }
}
