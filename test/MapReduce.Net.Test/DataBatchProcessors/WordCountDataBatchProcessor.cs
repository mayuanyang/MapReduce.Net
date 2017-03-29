using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapReduce.Net.Test.DataBatchProcessors
{
    public class WordCountDataBatchProcessor : IDataBatchProcessor<string, List<string>>
    {
        public Task<List<string>> Run(string inputData)
        {
            var lines = inputData.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            return Task.FromResult(lines);
        }
    }
}
