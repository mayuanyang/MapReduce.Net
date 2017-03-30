using System;
using System.Collections;
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
            var ht = new Hashtable();
            foreach (var word in words)
            {
                if (ht.ContainsKey(word))
                {
                    ht[word] = (int) ht[word] + 1;
                }
                else
                {
                    ht.Add(word, 1);
                }
                
            }
            foreach (var word in ht.Keys)
            {
                KeyValuePairs.Add(new KeyValuePair<string, int>(word.ToString(), (int)ht[word]));
                printContent += $"Key: {word}, Value: {1} \n";
            }
#if DEBUG
            Console.WriteLine($"Mapper: {key} ThreadId: {System.Threading.Thread.CurrentThread.ManagedThreadId}\nKeyValues:\n{printContent}");
#endif
            return Task.FromResult(0);
        }
        
    }
}
