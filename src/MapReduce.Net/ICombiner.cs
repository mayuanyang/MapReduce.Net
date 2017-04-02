using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{

    public interface ICombiner<TKeyIn, TValueIn, TKeyOut, TValueOut> : IReducer
    {
        Task<List<KeyValuePair<TKeyOut, TValueOut>>> Combine(TKeyIn key, TValueIn values);
    }
}
