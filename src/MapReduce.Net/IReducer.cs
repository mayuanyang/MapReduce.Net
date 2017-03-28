using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IReducer
    {
        
    }
    public interface IReducer<TInputKey, TInputValue, TReturn> : IReducer
    {
        Task<TReturn> Reduce(TInputKey key, TInputValue values);
    }
}
