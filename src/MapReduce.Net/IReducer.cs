using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IReducer<TKey, TValue, TContext>
    {
        Task Reduce(TKey input, TValue values, TContext context);
    }
}
