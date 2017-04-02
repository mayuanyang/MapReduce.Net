using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IMapper
    {
        
    }
    public interface IMapper<TKeyIn, TValueIn, TKeyOut, TValueOut> : IMapper 
    {
        Task<List<KeyValuePair<TKeyOut, TValueOut>>> Map(TKeyIn key, TValueIn value);
    }
}
