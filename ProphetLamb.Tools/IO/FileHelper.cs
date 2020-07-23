using System;
using System.IO;

namespace ProphetLamb.Tools.IO
{
    public static class FileHelper
    {
        /// <summary>
        /// Creates or overwrites an existing file allowing other programs read and write access to the created file.
        /// </summary>
        /// <param name="fileName">The string specifing the path to the file and its name.</param>
        /// <returns>A filestream for the file specified.</returns>
        /// <exception name="ArgumentException">fileName null or whitespace</exception>
        public static FileStream Create(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(ExceptionResource.STRING_NULLWHITESPACE);
            string dirPath = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        /// <summary>
        /// Opens a file allowing other programs read and write access to the file. This operation also requires write access the file specified.
        /// </summary>
        /// <param name="fileName">The string specifing the path to the file and its name.</param>
        /// <returns>A filestream for the file specified.</returns>
        /// <exception name="ArgumentException">fileName null or whitespace</exception>
        /// <exception name="FileNotFoundException"></exception>
        public static FileStream Open(string fileName) => Open(fileName, false);
        /// <summary>
        /// Opens or creates a file allowing other programs read and write access to the file. This operation also requires write access the file specified.
        /// </summary>
        /// <param name="fileName">The string specifing the path to the file and its name.</param>
        /// <param name="create">If <see cref="true"/> creates the file specified if it doenst already exsit; otherwise, throws a FileNotFoundException.</param>
        /// <returns>A filestream for the file specified.</returns>
        /// <exception name="ArgumentException">fileName null or whitespace</exception>
        /// <exception name="FileNotFoundException"></exception>
        public static FileStream Open(string fileName, bool create)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(ExceptionResource.STRING_NULLWHITESPACE);
            string dirPath = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return new FileStream(fileName, create ? FileMode.OpenOrCreate : FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite); // in order for the fileshare to work rwx we need to specify wrx access.
        }

        /// <summary>
        /// Returns whether the file at the path specified is a directory or not.
        /// </summary>
        /// <param name="filePath">The path leading to the file incuding the name and extention.</param>
        /// <returns><see cref="true"/> if the file is a directory; otherwise, <see cref="false"/>.</returns>
        public static bool IsDirectory(string? filePath)
        {
            return IsDirectory(new FileInfo(filePath));
        }
        /// <summary>
        /// Returns whether the <see cref="FileInfo"/> is a directory or not.
        /// </summary>
        /// <param name="fileInfo">The <see cref="FileInfo"/>.</param>
        /// <returns><see cref="true"/> if the file is a directory; otherwise, <see cref="false"/>.</returns>
        public static bool IsDirectory(this FileInfo fileInfo)
        {
            return (fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}