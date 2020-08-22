using Groundbeef.Collections;

using System.Collections.Generic;
using System.IO;

namespace Groundbeef.IO
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class BinaryReaderExtention
    {
        /// <summary>
        /// Enumerates bytes read from the <see cref="BinaryReader"/> in blocks with a maximum size of one page (4096).
        /// </summary>
        public static IEnumerable<byte[]> ReadBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, bufferSize)) != 0)
                yield return count == bufferSize ? buffer : buffer.GetRange(0, count);
        }

        /// <summary>
        /// Reads the <see cref="BinaryReader"/> to the end.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>A <see cref="byte[]"/> containing all data of the reader.</returns>
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using var ms = new MemoryStream();
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, bufferSize)) != 0)
                ms.Write(buffer, 0, count);
            return ms.ToArray();
        }
    }
}
