using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.Mappers
{
    public class WordCountMapper : IMapper<string, string, string, int>
    {
        public WordCountMapper()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }

        public List<KeyValuePair<string, int>> KeyValuePairs { get; }

        public Task Map(string key, string value)
        {
            var words = value.Split(' ');
            string printContent = "";
            foreach (var word in words)
            {
                KeyValuePairs.Add(new KeyValuePair<string, int>(word, 1));
                printContent += $"Key: {word}, Value: {1} \n";
            }
#if DEBUG
            Console.WriteLine($"Mapper: {key} ThreadId: {System.Threading.Thread.CurrentThread.ManagedThreadId}\nKeyValues:\n{printContent}");
#endif
            return Task.FromResult(0);
        }
        
    }
}
