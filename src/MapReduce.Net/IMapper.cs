using System;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IMapper<TKey, TValue, TContext>
    {
        Task Map(TKey key, TValue value, TContext context);
    }
}
