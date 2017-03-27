using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MapReduce.Net.Test.Reducers
{
    class WordCountReducer : IReducer<string, IEnumerable<int>, string, int>
    {
        public List<KeyValuePair<string, int>> KeyValuePairs { get; }

        public WordCountReducer()
        {
            KeyValuePairs = new List<KeyValuePair<string, int>>();
        }

        public Task Reduce(string key, IEnumerable<int> values)
        {
            var result = 0;
            foreach (var value in values)
            {
                result += value;
            }
            KeyValuePairs.Add(new KeyValuePair<string, int>(key, result));
            return Task.FromResult(0);
        }
    }
}
