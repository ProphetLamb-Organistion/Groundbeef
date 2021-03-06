using Groundbeef.SharedResources;

using System;
using System.IO;
using System.Security.Cryptography;

namespace Groundbeef.IO
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
        /// Indicates whether the file at the path specified is a directory or not.
        /// </summary>
        /// <param name="filePath">The path leading to the file incuding the name and extention.</param>
        /// <returns><see cref="true"/> if the file is a directory; otherwise, <see cref="false"/>.</returns>
        public static bool IsDirectory(string filePath)
        {
            return IsDirectory(new FileInfo(filePath));
        }
        /// <summary>
        /// Indicates whether the <see cref="FileInfo"/> is a directory or not.
        /// </summary>
        /// <param name="fileInfo">The <see cref="FileInfo"/>.</param>
        /// <returns><see cref="true"/> if the file is a directory; otherwise, <see cref="false"/>.</returns>
        public static bool IsDirectory(this FileInfo fileInfo)
        {
            return (fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>
        /// Returns the hash of a file, using the <see cref="HashAlorithm"/> specified.
        /// </summary>
        /// <param name="fileName">The <see cref="String"/> defining the path the file, its name and extention.</param>
        /// <param name="hashAlgorithm">The new instance of the <see cref="HashAlorithm"/> used.</param>
        /// <returns>The hash of the file.</returns>
        public static byte[] GetFileHash(string fileName, HashAlgorithm hashAlgorithm)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(ExceptionResource.FILE_NOTONDEVICE, fileName);
            using var br = new BinaryReader(FileHelper.Open(fileName));
            int bufferSize = 4096; // sizeof one page
            byte[] readBuffer = br.ReadBytes(bufferSize);
            while (readBuffer.Length == bufferSize) // While reading complete blocks
            {
                // Combine the hash code
                hashAlgorithm.TransformBlock(readBuffer, 0, bufferSize, readBuffer, 0);
            }
            // Finialize
            hashAlgorithm.TransformFinalBlock(readBuffer, 0, readBuffer.Length);
            return hashAlgorithm.Hash;
        }

        /// <summary>
        /// Indicates whether the path is a relative path.
        /// </summary>
        public static bool IsRelativePath(in ReadOnlySpan<char> path) => IsRelativePath(path, 0, out _);

        internal static bool IsRelativePath(in ReadOnlySpan<char> path, int startIndex, out int relativeIndicatorEndIndex)
        {
            if (startIndex + 3 >= path.Length)
                throw new IndexOutOfRangeException();
            char lo = path[startIndex];
            char lo2 = path[startIndex + 1];
            relativeIndicatorEndIndex = startIndex + 2;
            bool isRelativePath = lo == '.' && (lo2 == '\\' || lo2 == '/');
            if (lo2 == '\\' || lo2 == '/')
                relativeIndicatorEndIndex++;
            return isRelativePath;
        }
    }
}