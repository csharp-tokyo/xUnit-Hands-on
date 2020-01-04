using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncAwait
{
    public static class Files
    {
        public static async Task<string> ReadAllTextAsync(string file)
        {
            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
