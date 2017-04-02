using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.DataBatchProcessors
{
    public class WordCountDataBatchProcessor : IDataBatchProcessor<string, List<string>>
    {
        public Task<List<string>> Run(string inputData, int numberOfChunks = 4)
        {
            var result = new List<string>();
            var lines = inputData.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            var linesPerChunk = lines.Count / numberOfChunks;
            if (linesPerChunk == 0)
            {
                return Task.FromResult(lines);
            }
            int tracker = 0;
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (tracker != 0 && tracker % linesPerChunk == 0)
                {
                    result.Add(sb.ToString());
                    sb = new StringBuilder();
                }
                sb.Append(" ");
                sb.Append(line);
                tracker += 1;
            }
            result.Add(sb.ToString());
            
            return Task.FromResult(result);
        }
    }
}
