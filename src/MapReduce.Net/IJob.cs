using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IJob<TInputData, TReturnData>
    {
        Task<TReturnData> Run<TMapperKey, TMapperValue>(TInputData input);

    }
}
