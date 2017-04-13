using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    interface INode
    {
        IList<IMapper> Mappers { get; }
        IList<Task> MapperTasks { get; }
        Task<List<KeyValuePair<TMapperOutputKey, TMapperOutputValue>>> RunTasks<TMapperOutputKey, TMapperOutputValue>();
        IReducer Combiner { get; }
    }
}
