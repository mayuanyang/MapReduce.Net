using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IMapper
    {
        
    }
    public interface IMapper<TInputKey, TInputValue, TMapKey, TMapValue> : IMapper
    {
        List<KeyValuePair<TMapKey, TMapValue>> KeyValuePairs { get; } 
        Task Map(TInputKey key, TInputValue value);
        void Combine();
    }
}
