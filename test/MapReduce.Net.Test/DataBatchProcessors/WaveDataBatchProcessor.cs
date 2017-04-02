using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.DataBatchProcessors
{
    public class WaveDataBatchProcessor : IDataBatchProcessor<List<WaveData>, List<List<WaveData>>>
    {
        public Task<List<List<WaveData>>> Run(List<WaveData> inputData, int numOfChunks = 4)
        {
            var linesPerChunk = (int)decimal.Ceiling(inputData.Count / (numOfChunks * 1.0m));
            var result = new List<List<WaveData>> ();
            if (linesPerChunk == 0)
            {
                result.Add(inputData);
                return Task.FromResult(result);
            }
            for (int i = 0; i < numOfChunks; i++)
            {
                result.Add(inputData.Skip(i * linesPerChunk).Take(linesPerChunk).ToList());
            }


            return Task.FromResult(result);
        }
    }
}
