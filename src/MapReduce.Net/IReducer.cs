using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IReducer
    {
        
    }
    public interface IReducer<TKeyIn, TValueIn, TKeyOut, TValueOut> : IReducer
    {
        Task<List<KeyValuePair<TKeyOut,TValueOut>>> Reduce(TKeyIn key, TValueIn values);
    }
}
