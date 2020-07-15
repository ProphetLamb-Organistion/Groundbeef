using System.IO;

namespace ProphetLamb.Tools.IO
{
    public static class FileHelper
    {
        public static FileStream Create(string fileName)
        {
            string dirPath = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        }
    }
}