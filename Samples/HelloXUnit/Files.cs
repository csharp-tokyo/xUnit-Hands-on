using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HelloXUnit
{
    public class Files
    {
        public static bool DeleteIfExist(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            File.Delete(file);
            return true;
        }

        public static async Task<string> ReadAllTextAsync(string file)
        {
            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
