using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IJob
    {
        Task<TReturnData> Run<TInputData, TReturnData, TMapperKeyIn, TMapperValueIn, TMapperOutputKey, TMapperOutputValue>(TInputData input);

    }
}
