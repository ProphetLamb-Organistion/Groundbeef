using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Groundbeef.IO
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class BinaryReaderExtention
    {
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using var ms = new MemoryStream();
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                ms.Write(buffer, 0, count);
            return ms.ToArray();
        }
    }
}
