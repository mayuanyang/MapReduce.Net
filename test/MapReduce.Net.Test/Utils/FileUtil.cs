using System.IO;
using System.Reflection;
using System.Text;

namespace MapReduce.Net.Test.Utils
{
    static class FileUtil
    {
        public static string ReadFile(string fileName)
        {
            var resourceStream = typeof(FileUtil).GetTypeInfo().Assembly.GetManifestResourceStream(fileName);

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
