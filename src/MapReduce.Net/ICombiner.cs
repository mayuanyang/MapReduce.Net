using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net
{

    public interface ICombiner
    {
        
    }

    /// <summary>
    /// Combines outputs from mappers before the reducer and partitioner, in hadoop combiner only combines output from mappers within the same node
    /// </summary>
    /// <typeparam name="TInputKey"></typeparam>
    /// <typeparam name="TInputValues"></typeparam>
    /// <typeparam name="TOutputKey"></typeparam>
    /// <typeparam name="TOutputValue"></typeparam>
    public interface ICombiner<TInputKey, TInputValues, TOutputKey, TOutputValue> : ICombiner
    {
        Task<List<KeyValuePair<TOutputKey, TOutputValue>>> Combine(TInputKey key, TInputValues values);
    }
}
