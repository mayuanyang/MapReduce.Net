using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapReduce.Net.Test.Context;

namespace MapReduce.Net.Test.Mappers
{
    class WordCountMapper : IMapper<string, string, string, int, WordCountContext>
    {
        public List<KeyValuePair<string, int>> KeyValuePairs { get; }

        public WordCountMapper()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }
        public Task Map(string key, string value, WordCountContext context)
        {
            var words = value.Split(' ');
            string printContent = "";
            foreach (var word in words)
            {
                KeyValuePairs.Add(new KeyValuePair<string, int>(word, 1));
                printContent += $"Key: {word}, Value: {1} \n";
            }
            
            Console.WriteLine($"Mapper: {key} ThreadId: {System.Threading.Thread.CurrentThread.ManagedThreadId}\nKeyValues:\n{printContent}");

            return Task.FromResult(0);
        }

    }
}
