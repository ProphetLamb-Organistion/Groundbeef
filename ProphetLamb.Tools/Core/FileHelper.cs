using System.IO;
namespace ProphetLamb.Tools.Core
{
    public static class FileHelper
    {
        public static FileStream Create(string fileName)
        {
            return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        }
    }
}