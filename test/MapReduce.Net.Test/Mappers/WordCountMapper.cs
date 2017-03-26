using System.Threading.Tasks;
using MapReduce.Net.Test.Context;

namespace MapReduce.Net.Test.Mappers
{
    class WordCountMapper : IMapper<string, string, WordCountContext>
    {
        public Task Map(string key, string value, WordCountContext context)
        {
            var words = value.Split(' ');
            foreach (var word in words)
            {
                context.Save(word, 1);
            }

            return Task.FromResult(0);
        }

    }
}
