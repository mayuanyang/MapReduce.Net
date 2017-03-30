using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.Combiners
{
    public class WordCountCombiner : ICombiner<string, IEnumerable<KeyValuePair<string, int>>, string, int>
    {
        public WordCountCombiner()
        {
            _keyValuePairs = new List<KeyValuePair<string, int>>();
        }

        private List<KeyValuePair<string, int>> _keyValuePairs;
        public Task<List<KeyValuePair<string, int>>> Combine(string key, IEnumerable<KeyValuePair<string, int>> values)
        {
            var dictionary = new Dictionary<string, int>();
            
            foreach (var keyValue in values)
            {
                if (dictionary.ContainsKey(keyValue.Key.ToUpper()))
                {
                    dictionary[keyValue.Key.ToUpper()] = dictionary[keyValue.Key.ToUpper()] + keyValue.Value;
                }
                else
                {
                    dictionary.Add(keyValue.Key.ToUpper(), keyValue.Value);
                }
            }

            foreach (var de in dictionary)
            {
#if DEBUG

                Console.WriteLine($"Combiner {GetHashCode()} Key: {de.Key} Value: {de.Value}");
#endif
                _keyValuePairs.Add(new KeyValuePair<string, int>(de.Key, de.Value));
            }

            return Task.FromResult(_keyValuePairs);
        }
    }
}
