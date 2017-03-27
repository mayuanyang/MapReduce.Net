using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IReducer
    {
        
    }
    public interface IReducer<TInputKey, TInputValue, TOutPutKey, TOutputValue> : IReducer
    {
        List<KeyValuePair<TOutPutKey, TOutputValue>> KeyValuePairs { get; }
        Task Reduce(TInputKey key, TInputValue values);
    }
}
