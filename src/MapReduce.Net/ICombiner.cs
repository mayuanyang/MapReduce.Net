using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface ICombiner<TKey, TValue, TContext> : IReducer<TKey, TValue, TContext>
    {
        
    }
}
