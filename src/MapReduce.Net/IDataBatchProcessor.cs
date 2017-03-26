using System.Collections;
using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IDataBatchProcessor
    {
        
    }
    public interface IDataBatchProcessor<TInputData, TOutputData> : IDataBatchProcessor
        where TOutputData : IEnumerable
    {
        Task<TOutputData> Run(TInputData inputData);
    }
}
