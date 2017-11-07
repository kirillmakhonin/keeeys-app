using System.IO;

namespace Keeeys.Droid.PlatformSpecific
{
    public class FileHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public static void RemoveFile(string filename)
        {
            System.IO.File.Delete(filename);
        }

        public static bool IsFileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
    }
}