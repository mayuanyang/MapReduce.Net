using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MapReduce.Net.Test.Reducers
{
    public class WaveDataReducer : IReducer<string, List<KeyValuePair<string, List<WaveData>>>, string, WaveDataAverage>
    {

        public Task<List<KeyValuePair<string, WaveDataAverage>>> Reduce(string key, List<KeyValuePair<string, List<WaveData>>> values)
        {
            var result = new List<KeyValuePair<string, WaveDataAverage>>();
            var flatternList = values.SelectMany(x => x.Value).ToList();
            var siteGroups = flatternList.GroupBy(x => x.Site, y => y);
            foreach (var siteGroup in siteGroups)
            {
                //var totalSeconds = siteGroup.Sum(x => x.Seconds);
                var totalHsig = siteGroup.Sum(x => x.Hsig);
                var totalHmax = siteGroup.Sum(x => x.Hmax);
                var totalTp = siteGroup.Sum(x => x.Tp);
                var totalTz = siteGroup.Sum(x => x.Tz);
                var totalSst = siteGroup.Sum(x => x.Sst);
                var totalDirection = siteGroup.Sum(x => x.Direction);
                var count = siteGroup.Count();
                var avg = new WaveDataAverage
                {
                    Site = key,
                    //Seconds = totalSeconds / count,
                    Hisg = totalHsig / count,
                    Hmax = totalHmax / count,
                    Tp = totalTp / count,
                    Tz = totalTz / count,
                    Sst = totalSst / count,
                    Direction = totalDirection / count
                };
                result.Add(new KeyValuePair<string, WaveDataAverage>(siteGroup.Key, avg));
            }

            return Task.FromResult(result);
        }
    }
}
