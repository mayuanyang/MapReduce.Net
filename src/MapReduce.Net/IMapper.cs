using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IMapper
    {

    }
    public interface IMapper<TInputKey, TInputValue, TOutputKey, TOutputValue> : IMapper
    {
        List<KeyValuePair<TOutputKey, TOutputValue>> KeyValuePairs { get; }
        Task Map(TInputKey key, TInputValue value);
    }
}
