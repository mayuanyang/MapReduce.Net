using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IMapper<TInputKey, TInputValue, TMapKey, TMapValue, TContext>
    {
        List<KeyValuePair<TMapKey, TMapValue>> KeyValuePairs { get; } 
        Task Map(TInputKey key, TInputValue value, TContext context);
    }
}
