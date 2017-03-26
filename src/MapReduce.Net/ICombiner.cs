using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface ICombiner<TInputKey, TInputValue, TOutputKey, TOutputValue, TContext> : IReducer<TInputKey, TInputValue, TOutputKey, TOutputValue, TContext>
    {
        
    }
}
