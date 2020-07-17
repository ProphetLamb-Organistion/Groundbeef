using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ProphetLamb.Tools.Collections;
using System.Linq;

namespace ProphetLamb.Tools.Converters
{
    /// <summary>
    /// Z85 Encoding for UTF8 strings
    /// </summary>
    [ComVisible(true)]
    public static class Base85
    {
        #region Encoder
        private static readonly char[] encoder = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#".ToCharArray();

        public static Span<byte> Encode(ReadOnlySpan<char> chars, int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int charsLength = chars.Length;
            if (charsLength < offset + count)
                throw new IndexOutOfRangeException();
            if (charsLength == 0)
                return Array.Empty<byte>(); // Nothing to do here
            int charsRemainder = count % 4,
                charsChunksLength = count - charsRemainder,
                base85Length = count * 5 / 4,
                base85ChunksLength = charsChunksLength * 5 / 4,
                base85AllocLength = base85ChunksLength + (charsRemainder == 0 ? 0 : 5);
            Span<byte> base85 = new byte[base85AllocLength];
            EncodeSpan(chars, base85);
            if (charsRemainder == 0)
                return base85;
            // Allocate one remainder chunk
            Span<char> leftoverChars = stackalloc char[4];
            for (int i = 0; i < charsRemainder; i++)
                leftoverChars[i] = chars[offset + charsChunksLength + i];
            EncodeSpan(leftoverChars, base85.Slice(base85ChunksLength));
            return base85.Slice(0, base85Length);
        }

        private static unsafe void EncodeSpan(ReadOnlySpan<char> chars, Span<byte> base85)
        {
            Debug.Assert(chars.Length*5 >= base85.Length*4);
            int charsLength = chars.Length,
                base85Length = base85.Length;
#pragma warning disable RCS1001
            fixed(byte *outPtr = &MemoryMarshal.GetReference(base85))
            fixed(char *base85Ptr = &MemoryMarshal.GetReference(chars))
            unchecked
#pragma warning restore RCS1001
            {
                char *inPtr = base85Ptr;
                for (int i = 0; i < base85Length; inPtr += 4)
                {
                    long value = (inPtr[0] << 24) | (inPtr[1] << 16) | (inPtr[2] << 8) | inPtr[3];
                    outPtr[i++] = (byte)encoder[(value / num0) % 0x55];
                    outPtr[i++] = (byte)encoder[(value / num1) % 0x55];
                    outPtr[i++] = (byte)encoder[(value / num2) % 0x55];
                    outPtr[i++] = (byte)encoder[(value / num3) % 0x55];
                    outPtr[i++] = (byte)encoder[value % 0x55];
                }
            }
        }
        #endregion

        #region Decoder
        private static readonly char[] decoder = new byte[]
        {
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x00..0x0F ASCII
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x10..0x1F
             0, 68,  0, 84, 83, 82, 72,  0, 75, 76, 70, 65,  0, 63, 62, 69, // 0x20..0x2F
             0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 64,  0, 73, 66, 74, 71, // 0x30..0x3F
            81, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, // 0x40..0x4F
            51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 77,  0, 78, 67,  0, // 0x50..0x5F
             0, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, // 0x60..0x6F
            25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 79,  0, 80,  0,  0, // 0x70..0x7F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x80..0x8F ASCII ext
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x90..0x9F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xA0..0xAF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xB0..0xBF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xC0..0xCF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xD0..0xDF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xE0..0xEF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xF0..0xFF
        }.Cast<char>().ToArray();

        // Divisor and multiplier weights for encoding and decoding respectively, for consecutive bytes.
        private const long num0 = 0x31C84B1, num1 = 0x95EED, num2 = 0x1C39, num3 = 0x55;

        public static Span<char> Decode(ReadOnlySpan<byte> base85, int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int base85Length = base85.Length;
            if (base85Length < offset + count)
                throw new IndexOutOfRangeException();
            if (base85Length == 0)
                return Array.Empty<char>(); // Nothing to do here
            int base85Remainder = count % 5, // Bytes remaining after all chunks were consumed
                base85ChunksLength = count - base85Remainder, // Length of all complete input chunks
                charsLength = count * 4 / 5, // Length of the result
                charsChunksLength = base85ChunksLength * 4 / 5, // Length of all complete output chunks
                charsAllocLength = charsChunksLength + (base85Remainder == 0 ? 0 : 4); // Allocate extra length for the remainder
            Span<char> chars = new char[charsAllocLength];
            DecodeSpan(base85.Slice(offset), chars);
            if (base85Remainder == 0)
                return chars;
            // Allocate one remainder chunk
            Span<byte> leftoverBytes = stackalloc byte[5];
            for (int i = 0; i < base85Remainder; i++)
                leftoverBytes[i] = base85[offset + base85ChunksLength + i];
            DecodeSpan(leftoverBytes, chars.Slice(charsChunksLength));
            return chars.Slice(0, charsLength); // Remove null-byte remainder bytes
        }

        private static unsafe void DecodeSpan(ReadOnlySpan<byte> base85, Span<char> chars)
        {
            Debug.Assert(chars.Length*5 >= base85.Length*4);
            int length = chars.Length;
#pragma warning disable RCS1001
            fixed(byte *charsPtr = &MemoryMarshal.GetReference(base85))
            fixed(char *outPtr = &MemoryMarshal.GetReference(chars))
            unchecked
#pragma warning restore RCS1001
            {
                byte *inPtr = charsPtr;
                for(int i = 0; i < length; inPtr+=5)
                {
                    long value = decoder[inPtr[0]] * num0
                               + decoder[inPtr[1]] * num1
                               + decoder[inPtr[2]] * num2
                               + decoder[inPtr[3]] * num3
                               + decoder[inPtr[4]];
                    outPtr[i++] = (char)((value >> 24) & 0xFF);
                    outPtr[i++] = (char)((value >> 16) & 0xFF);
                    outPtr[i++] = (char)((value >> 8) & 0xFF);
                    outPtr[i++] = (char)((value >> 0) & 0xFF);
                }
            }
        }
        #endregion
    }
}