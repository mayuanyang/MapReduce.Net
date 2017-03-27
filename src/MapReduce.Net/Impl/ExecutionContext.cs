using System.Collections.Generic;

namespace MapReduce.Net.Impl
{
    public sealed class ExecutionContext
    {
        internal IList<IMapper> Mappers { get; }
        internal IList<IReducer> Reducers { get; }
        public ExecutionContext(IList<IMapper> mappers, IList<IReducer> reducers)
        {
            Mappers = mappers;
            Reducers = reducers;
        }
    }
}
