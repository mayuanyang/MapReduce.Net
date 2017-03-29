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
            int half = lines.Count / 2;
            var list = new List<string>();
            var str = "";
            for (int i = 0; i < half; i++)
            {
                str += " " + lines[i];
            }
            list.Add(str);
            str = "";
            for (int i = half; i < lines.Count; i++)
            {
                str += " " + lines[i];
            }
            list.Add(str);
            return Task.FromResult(list);
        }
    }
}
