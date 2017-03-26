using System.Collections.Generic;

namespace MapReduce.Net.Test.Context
{
    class WordCountContext : IMapReduceContext<string, int>
    {
        public WordCountContext()
        {
            KeyValuePairs = new Dictionary<string, int>();
        }

        public Dictionary<string, int> KeyValuePairs { get; }
        public void Save(string key, int value)
        {
            KeyValuePairs.Add(key, value);
        }
    }
}
