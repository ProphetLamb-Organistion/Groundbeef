using System.Linq;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.BinaryEncoding
{
    /// <summary>
    /// Encodes binary data to the range of all printable ASCII characters 20..7E
    /// </summary>
    [ComVisible(true)]
    public static class Base94
    {
        // Divisor and multiplier weights for consecutive bytes for both encoding and decoding.
        private const int Num0 = 0xD151F, Num1 = 0x2341, Num2 = 0x5F;

        private static readonly byte[] s_decoder = Enumerable.Range(0x20, 0x5E).Cast<byte>().ToArray();

        private static unsafe void Decode(ReadOnlySpan<char> chars, Span<byte> base96)
        {
            Debug.Assert(chars.Length * 8 >= base96.Length * 3);
            int charsLength = chars.Length,
                base96Length = base96.Length;
            fixed(byte* outPtr = &MemoryMarshal.GetReference(base96))
            fixed(char* charsPtr = &MemoryMarshal.GetReference(chars))
            unchecked
            {
                char* inPtr = charsPtr;
                for (int i = 0; i < base96Length; inPtr += 8)
                {
                    
                }
            }
        }
    }
}