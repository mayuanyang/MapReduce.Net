using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IJob
    {
        Task<TReturnData> Run<TInputData, TReturnData, TMapperOutputKey, TMapperOutputValue>(TInputData input);

    }
}
