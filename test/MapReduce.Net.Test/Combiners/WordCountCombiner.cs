using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.Combiners
{
    public class WordCountCombiner : ICombiner<string, List<KeyValuePair<string, int>>, string, int>
    {
        public WordCountCombiner()
        {
            _keyValuePairs = new List<KeyValuePair<string, int>>();
        }

        private List<KeyValuePair<string, int>> _keyValuePairs;
        
        public Task<List<KeyValuePair<string, int>>> Combine(string key, List<KeyValuePair<string, int>> values)
        {
            var dictionary = new Dictionary<string, int>();
            foreach (var kv in values)
            {
                if (dictionary.ContainsKey(kv.Key.ToUpper()))
                {
                    dictionary[kv.Key.ToUpper()] = dictionary[kv.Key.ToUpper()] + kv.Value;
                }
                else
                {
                    dictionary.Add(kv.Key.ToUpper(), kv.Value);
                }
            }

            foreach (var de in dictionary)
            {
#if DEBUG
                Console.WriteLine($"Combiner {GetHashCode()} Key: {de.Key} Value: {de.Value}");
#endif
                var kvForThisMapper = new KeyValuePair<string, int>(de.Key, de.Value);
                _keyValuePairs.Add(kvForThisMapper);
            }

            return Task.FromResult(_keyValuePairs);
        }
    }
}
