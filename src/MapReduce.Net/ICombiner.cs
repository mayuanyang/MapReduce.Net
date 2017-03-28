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
    /// <typeparam name="TInputValue"></typeparam>
    /// <typeparam name="TOutputKey"></typeparam>
    /// <typeparam name="TOutputValue"></typeparam>
    public interface ICombiner<TInputKey, TInputValues, TOutputKey, TOutputValue> : ICombiner
    {
        IList<KeyValuePair<TOutputKey, TOutputValue>> KeyValuePairs { get; }
        Task Combine(TInputKey key, TInputValues values);
    }
}
