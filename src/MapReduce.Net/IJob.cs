using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IJob<TData>
    {
        Task Run(TData input);

    }
}
