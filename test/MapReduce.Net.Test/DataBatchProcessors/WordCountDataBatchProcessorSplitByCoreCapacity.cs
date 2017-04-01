using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.DataBatchProcessors
{
    public class WordCountDataBatchProcessorSplitByCoreCapacity : IDataBatchProcessor<string, List<string>>
    {
        public Task<List<string>> Run(string inputData)
        {
            var result = new List<string>();
            var lines = inputData.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            var linesPerCore = lines.Count / (Environment.ProcessorCount / 2);
            if (linesPerCore == 0)
            {
                return Task.FromResult(lines);
            }
            int tracker = 0;
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                // Split by every 20 lines
                if (tracker != 0 && tracker % linesPerCore == 0)
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

        public static IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }
    }
}
