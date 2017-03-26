using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapReduce.Net.Test.Context;

namespace MapReduce.Net.Test.Reducers
{
    class WordCountReducer : IReducer<string, IEnumerable<int>, WordCountContext>
    {
        public Task Reduce(string key, IEnumerable<int> values, WordCountContext context)
        {
            var result = 0;
            foreach (var value in values)
            {
                result += value;
            }
            context.Save(key, result);
            return Task.FromResult(0);
        }
    }
}
