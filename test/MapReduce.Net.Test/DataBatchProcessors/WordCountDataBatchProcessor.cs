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
            int lengthPerLineItem = 500;
            inputData = inputData.Replace("\r\n", " ").Replace("\n", " ");
            if (inputData.Length <= lengthPerLineItem)
            {
                return Task.FromResult(new List<string> {inputData});
            }

            var lines = Enumerable.Range(0, inputData.Length / lengthPerLineItem).Select(i => inputData.Substring(i * lengthPerLineItem, lengthPerLineItem)).ToList();
            
            //var lines = inputData.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            
            return Task.FromResult(lines);
        }
    }
}
