using System.IO;
using System.Reflection;
using System.Text;

namespace MapReduce.Net.Benchmark
{
    static class FileUtil
    {
        public static string ReadFile(Assembly assembly, string fileName)
        {
            var resourceStream = assembly.GetManifestResourceStream(fileName);

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
