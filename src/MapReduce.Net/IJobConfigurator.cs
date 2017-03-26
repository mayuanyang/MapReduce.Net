using System;

namespace MapReduce.Net
{
    public interface IJobConfigurator
    {
        Type TypeOfMapper { get; }
        Type TypeOfCombiner { get; }
        Type TypeOfReducer { get; }
        Type TypeOfDataBatchProcessor { get; }
        Type TypeOfContext { get; }
        IDependancyScope DependancyScope { get; }
    }
}
