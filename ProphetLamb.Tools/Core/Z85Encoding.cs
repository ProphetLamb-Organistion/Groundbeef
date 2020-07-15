using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ProphetLamb.Tools.Core
{
    [ComVisible(true)]
    public class Z85Encoding
    {
        private static readonly char[] encoder = 
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 
            'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 
            'Y', 'Z', '.', '-', ':', '+', '=', '^', '!', '/', 
            '*', '?', '&', '<', '>', '(', ')', '[', ']', '{', 
            '}','@','%','$','#'
        };
        private static readonly byte[] decoder = 
        {
            0x00, 0x44, 0x00, 0x54, 0x53, 0x52, 0x48, 0x00, 
            0x4B, 0x4C, 0x46, 0x41, 0x00, 0x3F, 0x3E, 0x45, 
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
            0x08, 0x09, 0x40, 0x00, 0x49, 0x42, 0x4A, 0x47, 
            0x51, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 
            0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 
            0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 
            0x3B, 0x3C, 0x3D, 0x4D, 0x00, 0x4E, 0x43, 0x00, 
            0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 
            0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 
            0x21, 0x22, 0x23, 0x4F, 0x00, 0x50, 0x00, 0x00
        };

        public unsafe string EncodeASCII(in byte[] data, int offset, int count)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            int length = data.Length;
            if (length == 0)
                return String.Empty;
            if (offset < 0 || length <= offset)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || length <= offset)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset + count >= data.Length)
                throw new IndexOutOfRangeException("The sum of offset and count must be smaller then the length of the data.");
            string result = StringHelper.FastAllocateString(GetCharsCount(count));
            fixed(byte *inBytes = data) { fixed (char *outChars = result)
            {
                InternalEncode(outChars, inBytes + offset, count);
            }}
            return result;
        }

        public unsafe byte[] DecodeASCII(in string data, int offset, int count)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            int length = data.Length;
            if (length == 0)
                return Array.Empty<byte>();
            if (offset < 0 || length <= offset)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || length <= offset)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset + count >= data.Length)
                throw new IndexOutOfRangeException("The sum of offset and count must be smaller then the length of the data.");
            var result = new byte[GetBytesCount(count)];
            fixed(char *inChars = data) { fixed (byte *outBytes = result)
            {
                InternalDecode(outBytes, inChars + offset, count);
            }}
            return result;
        }

        private int GetCharsCount(int bytesCount)
        {
            int count = bytesCount * 5 / 4;
            // Only four byte chunks are allowed
            return count += count % 4;
        }

        private int GetBytesCount(int charsCount)
        {
            int count = charsCount * 4 / 5;
            // Only five char chunks are allowed
            return count += count % 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private unsafe void InternalEncode(in char *outChars, in byte *inBytes, int bytesCount)
        {
            unchecked
            {
                int charIndex = 0,
                    byteIndex = 0,
                    value = 0;
                while (byteIndex < bytesCount)
                {
                    // Accumulate in base256
                    value = (value * 256) + inBytes[byteIndex++];
                    if (byteIndex % 4 == 0)
                    {
                        // Value is in base256
                        int div = 85*85*85*85;
                        // Split into base85
                        while (div != 0)
                        {
                            outChars[charIndex++] = encoder[value / div % 85];
                            div /= 85;
                        }
                        value = 0;
                    }
                }
                outChars[charIndex] = '\0'; // null terminator
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private unsafe void InternalDecode(in byte *outBytes, in char *inChars, int charsCount)
        {
            unchecked
            {
                int charIndex = 0,
                    byteIndex = 0,
                    value = 0;
                while (charIndex < charsCount)
                {
                    // Accumulate in base85
                    value = (value * 85) + decoder[inChars[charIndex++] - 32];
                    // Mergt to base256
                    if (charIndex % 5 == 0)
                    {
                        int div = 256 * 256 * 256;
                        while (div != 0)
                        {
                            outBytes[byteIndex++] = (byte)(value / div % 256);
                            div /= 256;
                        }
                        value = 0;
                    }
                }
            }
        }
    }
}