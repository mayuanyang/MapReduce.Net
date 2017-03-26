using System.Threading.Tasks;

namespace MapReduce.Net
{
    public interface IJob
    {
        Task Run();
    }
}
