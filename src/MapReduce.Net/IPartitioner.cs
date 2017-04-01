using System.Threading.Tasks;
using MapReduce.Net.Impl;

namespace MapReduce.Net
{
    public interface IPartitioner
    {
        Task Shuffle();

    }
}
