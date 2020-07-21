using System;
using System.IO;
using System.Security.Cryptography;

namespace ProphetLamb.Tools.IO
{
    public static class FileHasher
    {
        /// <summary>
        /// Returns the SHA1 hash of the file specified
        /// </summary>
        /// <param name="fileName">The <see cref="string"/> defining the path the file, its name and extention.</param>
        /// <returns>The SHA1 hash of the file specified</returns>
        public static byte[] GetFileSHA1Hash(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(ExceptionResource.FILE_NOTONDEVICE, fileName);
            var hasher = SHA1.Create();
            using var br = new BinaryReader(FileHelper.Open(fileName));
            int bufferSize = 4096; // sizeof one page
            byte[] readBuffer = br.ReadBytes(bufferSize);
            while (readBuffer.Length == bufferSize) // While reading complete blocks
            {
                // Combine the hash code
                hasher.TransformBlock(readBuffer, 0, bufferSize, readBuffer, 0);
            }
            // Finialize
            hasher.TransformFinalBlock(readBuffer, 0, readBuffer.Length);
            return hasher.Hash;
        }
    }
}
