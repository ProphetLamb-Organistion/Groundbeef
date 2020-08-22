using Groundbeef.SharedResources;

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.BinaryEncoding
{
    /// <summary>
    /// ZeroMQ Base85 Encoding Algorithm, see: https://rfc.zeromq.org/spec/32/
    /// </summary>
    [ComVisible(true)]
    public static class Base85
    {
        // Divisor and multiplier weights for consecutive bytes for both encoding and decoding.
        // Num_0 = 85^4, Num_1 = 85^3, Num_2 = 85^2, Num_3 = 85^1
        private const uint Num0 = 0x31C84B1, Num1 = 0x95EED, Num2 = 0x1C39, Num3 = 0x55;

        #region Decode
        /// <summary>
        /// Converts a base85 encoded <see cref="ReadOnlySpan<char>"/> to the byte represenatation.
        /// </summary>
        /// <param name="chars">The base85 encoded <see cref="ReadOnlySpan<char>"/> to decode.</param>
        /// <returns>The byte representation of the base85 encoded  <see cref="ReadOnlySpan<char>"/>.</returns>
        public static Span<byte> Decode(ReadOnlySpan<char> chars) => Decode(chars, 0, chars.Length);

        /// <summary>
        /// Converts a portion of a base85 encoded <see cref="ReadOnlySpan<char>"/> to the byte represenatation.
        /// </summary>
        /// <param name="chars">The base85 encoded <see cref="ReadOnlySpan<char>"/> to decode.</param>
        /// <param name="offset">The zero-based starting index of the section to decode.</param>
        /// <returns>The byte representation of a protion of the base85 encoded  <see cref="ReadOnlySpan<char>"/>.</returns>
        public static Span<byte> Decode(ReadOnlySpan<char> chars, int offset) => Decode(chars, offset, chars.Length - offset);

        /// <summary>
        /// Converts a portion of a base85 encoded <see cref="ReadOnlySpan<char>"/> to the byte represenatation.
        /// </summary>
        /// <param name="chars">The base85 encoded <see cref="ReadOnlySpan<char>"/> to decode.</param>
        /// <param name="offset">The zero-based starting index of the section to decode.</param>
        /// <param name="count">The number of chars in the section.</param>
        /// <returns>The byte representation of a protion of the base85 encoded  <see cref="ReadOnlySpan<char>"/>.</returns>
        public static Span<byte> Decode(ReadOnlySpan<char> chars, int offset, int count)
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
            Span<byte> bytes = new byte[base85AllocLength];
            DecodeSpan(chars, bytes);
            if (charsRemainder == 0)
                return bytes; // alloclength = length
            // Allocate one remainder chunk
            Span<char> leftoverChars = stackalloc char[4];
            chars.Slice(offset + charsChunksLength).CopyTo(leftoverChars);
            DecodeSpan(leftoverChars, bytes.Slice(base85ChunksLength));
            return bytes.Slice(0, base85Length);
        }

        private static readonly byte[] s_decoder = Encoding.ASCII.GetBytes("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#");

        private static unsafe void DecodeSpan(ReadOnlySpan<char> chars, Span<byte> base85)
        {
            Debug.Assert(chars.Length * 5 >= base85.Length * 4);
            int charsLength = chars.Length,
                base85Length = base85.Length;
            fixed (byte* outPtr = &MemoryMarshal.GetReference(base85))
            fixed (char* charsPtr = &MemoryMarshal.GetReference(chars))
                unchecked
                {
                    char* inPtr = charsPtr;
                    for (int i = 0; i < base85Length; inPtr += 4)
                    {
                        uint value = ((uint)inPtr[0] << 24) | ((uint)inPtr[1] << 16) | ((uint)inPtr[2] << 8) | inPtr[3];
                        outPtr[i++] = s_decoder[(value / Num0) % 0x55];
                        outPtr[i++] = s_decoder[(value / Num1) % 0x55];
                        outPtr[i++] = s_decoder[(value / Num2) % 0x55];
                        outPtr[i++] = s_decoder[(value / Num3) % 0x55];
                        outPtr[i++] = s_decoder[value % 0x55];
                    }
                }
        }
        #endregion

        #region Encoder
        /// <summary>
        /// Converts a <see cref="Span<byte>"/> to the base85 encoded representation.
        /// </summary>
        /// <param name="bytes">The <see cref="Span<byte>"/> to encode.</param>
        /// <returns>The base85 encoded representation of the <see cref="ReadOnlySpan<byte>"/>.</returns>
        public static Span<char> Encode(ReadOnlySpan<byte> bytes) => Encode(bytes, 0, bytes.Length);

        /// <summary>
        /// Converts a portion of a <see cref="Span<byte>"/> to the base85 encoded representation.
        /// </summary>
        /// <param name="bytes">The <see cref="Span<byte>"/> to encode.</param>
        /// <param name="offset">The zero-based starting index of the section to encode.</param>
        /// <returns>The base85 encoded representation of a portion of the <see cref="ReadOnlySpan<byte>"/>.</returns>
        public static Span<char> Encode(ReadOnlySpan<byte> bytes, int offset) => Encode(bytes, offset, bytes.Length - offset);

        /// <summary>
        /// Converts a portion of a <see cref="Span<byte>"/> to the base85 encoded representation.
        /// </summary>
        /// <param name="bytes">The <see cref="Span<byte>"/> to encode.</param>
        /// <param name="offset">The zero-based starting index of the section to encode.</param>
        /// <param name="count">The number of bytes in the section.</param>
        /// <returns>The base85 encoded representation of a portion of the <see cref="ReadOnlySpan<byte>"/>.</returns>
        public static Span<char> Encode(ReadOnlySpan<byte> bytes, int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int base85Length = bytes.Length;
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
            EncodeSpan(bytes.Slice(offset), chars);
            if (base85Remainder == 0)
                return chars;
            // Allocate one remainder chunk
            Span<byte> leftoverBytes = stackalloc byte[5];
            bytes.Slice(offset + base85ChunksLength).CopyTo(leftoverBytes);
            EncodeSpan(leftoverBytes, chars.Slice(charsChunksLength));
            return chars.Slice(0, charsLength); // Remove null-byte remainder bytes
        }

        private static readonly char[] s_encoder = new byte[]
        {
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x00..0x0F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x10..0x1F
             0, 68,  0, 84, 83, 82, 72,  0, 75, 76, 70, 65,  0, 63, 62, 69, // 0x20..0x2F
             0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 64,  0, 73, 66, 74, 71, // 0x30..0x3F
            81, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, // 0x40..0x4F
            51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 77,  0, 78, 67,  0, // 0x50..0x5F
             0, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, // 0x60..0x6F
            25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 79,  0, 80,  0,  0, // 0x70..0x7F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x80..0x8F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0x90..0x9F
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xA0..0xAF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xB0..0xBF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xC0..0xCF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xD0..0xDF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xE0..0xEF
             0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, // 0xF0..0xFF
        }.Select(Convert.ToChar).ToArray();

        private static unsafe void EncodeSpan(ReadOnlySpan<byte> base85, Span<char> chars)
        {
            Debug.Assert(chars.Length * 5 >= base85.Length * 4);
            int length = chars.Length;
            fixed (byte* charsPtr = &MemoryMarshal.GetReference(base85))
            fixed (char* outPtr = &MemoryMarshal.GetReference(chars))
                unchecked
                {
                    byte* inPtr = charsPtr;
                    for (int i = 0; i < length; inPtr += 5)
                    {
                        uint value = s_encoder[inPtr[0]] * Num0
                                   + s_encoder[inPtr[1]] * Num1
                                   + s_encoder[inPtr[2]] * Num2
                                   + s_encoder[inPtr[3]] * Num3
                                   + s_encoder[inPtr[4]];
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