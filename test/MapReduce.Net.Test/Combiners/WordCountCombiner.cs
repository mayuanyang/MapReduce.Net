using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.Combiners
{
    class WordCountCombiner : ICombiner<string, IEnumerable<KeyValuePair<string, int>>, string, int>
    {
        public WordCountCombiner()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }
        public IList<KeyValuePair<string, int>> KeyValuePairs { get; }
        public Task Combine(string key, IEnumerable<KeyValuePair<string, int>> values)
        {
            var dictionary = new Dictionary<string, int>();
            
            foreach (var keyValue in values)
            {
                if (dictionary.ContainsKey(keyValue.Key.ToUpper()))
                {
                    dictionary[keyValue.Key.ToUpper()] = dictionary[keyValue.Key.ToUpper()] + 1;
                }
                else
                {
                    dictionary.Add(keyValue.Key.ToUpper(), 1);
                }
            }

            foreach (var de in dictionary)
            {
                Console.WriteLine($"Combiner {GetHashCode()} Key: {de.Key} Value: {de.Value}");
                KeyValuePairs.Add(new KeyValuePair<string, int>(de.Key, de.Value));
            }

            return Task.FromResult(0);
        }
    }
}
