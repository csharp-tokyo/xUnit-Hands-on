using System;
using System.IO;

namespace SetupTearDown
{
    public static class Files
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
    }
}
