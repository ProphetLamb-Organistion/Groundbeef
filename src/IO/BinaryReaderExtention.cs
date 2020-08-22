using Groundbeef.Collections;

using System.Collections.Generic;
using System.IO;

namespace Groundbeef.IO
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class BinaryReaderExtention
    {
        public static IEnumerable<byte[]> ReadBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, bufferSize)) != 0)
                yield return buffer.GetRange(0, count);
        }

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
