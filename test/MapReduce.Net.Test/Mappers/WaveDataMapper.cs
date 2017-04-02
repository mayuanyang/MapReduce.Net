using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.Mappers
{
    public class WaveDataMapper : IMapper<string, List<WaveData>, string, List<WaveData>>
    {
        private List<KeyValuePair<string, List<WaveData>>> _keyValuePairs;
        public WaveDataMapper()
        {
            _keyValuePairs = new List<KeyValuePair<string, List<WaveData>>>();
        }


        public Task<List<KeyValuePair<string, List<WaveData>>>> Map(string key, List<WaveData> value)
        {
            _keyValuePairs.Add(new KeyValuePair<string, List<WaveData>>(key, value));
            return Task.FromResult(_keyValuePairs);
        }
    }
}
