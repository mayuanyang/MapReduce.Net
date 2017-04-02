using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MapReduce.Net.Test.Reducers
{
    public class WordCountReducer : IReducer<string, List<KeyValuePair<string, int>>, string, int>
    {
        public List<KeyValuePair<string, int>> KeyValuePairs { get; }

        public WordCountReducer()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }

        public Task<List<KeyValuePair<string, int>>> Reduce(string key, List<KeyValuePair<string, int>> values)
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

            return Task.FromResult(KeyValuePairs);
        }
    }
}
