using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MapReduce.Net.Test.Reducers
{
    public class WordCountReducer : IReducer<string, IEnumerable<KeyValuePair<string, int>>, List<KeyValuePair<string, int>>>
    {
        public List<KeyValuePair<string, int>> KeyValuePairs { get; }

        public WordCountReducer()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }

        public async Task<List<KeyValuePair<string, int>>> Reduce(string key, IEnumerable<KeyValuePair<string, int>> values)
        {
#if DEBUG
            Console.WriteLine();
#endif
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
                Console.WriteLine($"Reducer {GetHashCode()} Key: {de.Key} Value: {de.Value}");
#endif

                KeyValuePairs.Add(new KeyValuePair<string, int>(de.Key, de.Value));
            }

            return await Task.FromResult(KeyValuePairs);
        }
    }
}
