using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Collections.Spans
{
    public static unsafe class BitwiseExtention
    {
        /*
         * Contrary to C++, the C# specification dictates that the long datatype consists of one QWORD (64bit or 8bytes) of continues memory.
         * On 32bit Systems this is archived by allocating a tuple of two DWORDs (32bit or 4byte) consecutive,
         * in turn this results in non atomic operations on the long datatype when compiling for 32bit (csproj profile: x86 or AnyCPU).
         * Hence using DWORD operations is much faster then psudo QWORD when not executing in a 64bit environment.
         */
        #region Boolean
        /// <summary>
        /// Indicates whether the bit at the specified significance is set.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBitAt(this Span<byte> span, int bitSignificance)
         => GetBitAt((ReadOnlySpan<byte>)span, bitSignificance);

        /// <summary>
        /// Indicates whether the bit at the specified significance is set.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBitAt(this ReadOnlySpan<byte> span, int bitSignificance)
        {
#if BIG_EDIAN
            return ((span[bitOffset/8] >> (bitOffset % 8)) & 0x01) != 0;
#else
            return ((span[^(bitSignificance/8)] >> (bitSignificance % 8)) & 0x01) != 0;
#endif
        }

        /// <summary>
        /// Combines two spans using biwise AND. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> And(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => And(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise AND. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> And(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => And((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise AND. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> And(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrAnd(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrAnd(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = *(ulong*)(leftPtr + i) | *(ulong*)(rightPtr + i);
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = *(uint*)(leftPtr + i) | *(uint*)(rightPtr + i);
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = *(uint*)(leftPtr + i) | *(uint*)(rightPtr + i);
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)((*(leftPtr + i) & *(rightPtr + i)) & 0xFF);
        }

        /// <summary>
        /// Combines two spans using biwise OR. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Or(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => Or(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise OR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Or(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => Or((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise OR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> Or(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrOr(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrOr(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = *(ulong*)(leftPtr + i) | *(ulong*)(rightPtr + i);
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = *(uint*)(leftPtr + i) | *(uint*)(rightPtr + i);
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = *(uint*)(leftPtr + i) | *(uint*)(rightPtr + i);
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)((*(leftPtr + i) | *(rightPtr + i)) & 0xFF);
        }

        /// <summary>
        /// Combines two spans using biwise NAND. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Nand(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => Nand(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise NAND. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Nand(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => Nand((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise NAND. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> Nand(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrNand(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrNand(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = ~(*(ulong*)(leftPtr + i) & *(ulong*)(rightPtr + i));
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) & *(uint*)(rightPtr + i));
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) & *(uint*)(rightPtr + i));
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)(~(*(leftPtr + i) & *(rightPtr + i)) & 0xFF);
        }

        /// <summary>
        /// Combines two spans using biwise NOR. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Nor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => Nor(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise NOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Nor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => Nor((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise NOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> Nor(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrNor(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrNor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = ~(*(ulong*)(leftPtr + i) | *(ulong*)(rightPtr + i));
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) | *(uint*)(rightPtr + i));
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) | *(uint*)(rightPtr + i));
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)(~(*(leftPtr + i) | *(rightPtr + i)) & 0xFF);
        }

        /// <summary>
        /// Combines two spans using biwise XOR. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Xor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => Xor(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise XOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Xor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => Xor((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise XOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> Xor(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrXor(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrXor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = *(ulong*)(leftPtr + i) ^ *(ulong*)(rightPtr + i);
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = *(uint*)(leftPtr + i) ^ *(uint*)(rightPtr + i);
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = *(uint*)(leftPtr + i) ^ *(uint*)(rightPtr + i);
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)((*(leftPtr + i) ^ *(rightPtr + i)) & 0xFF);
        }

        /// <summary>
        /// Combines two spans using biwise XNOR. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the left operand span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Xnor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand)
         => Xnor(leftOperand, rightOperand, leftOperand);

        /// <summary>
        /// Combines two spans using biwise XNOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> Xnor(this Span<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
         => Xnor((ReadOnlySpan<byte>)leftOperand, rightOperand, result);

        /// <summary>
        /// Combines two spans using biwise XNOR. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> Xnor(this ReadOnlySpan<byte> leftOperand, in ReadOnlySpan<byte> rightOperand, in Span<byte> result)
        {
            fixed(byte* leftPtr = &MemoryMarshal.GetReference(leftOperand))
            fixed(byte* rightPtr = &MemoryMarshal.GetReference(rightOperand))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrXnor(leftPtr, rightPtr, outPtr, Math.Min(leftOperand.Length, rightOperand.Length));
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrXnor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
#if WIN64
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = ~(*(ulong*)(leftPtr + i) ^ *(ulong*)(rightPtr + i));
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) ^ *(uint*)(rightPtr + i));
                i+=4;
            }
#else
            if (len >= 4)
            {
                for (; i < len; i+= 4)
                    *(uint*)outPtr[i] = ~(*(uint*)(leftPtr + i) ^ *(uint*)(rightPtr + i));
            }
#endif
            for (; i != len; i++)
                *(outPtr + i) = (byte)(~(*(leftPtr + i) ^ *(rightPtr + i)) & 0xFF);
        }
        #endregion

        #region Masked
        /// <summary>
        /// Computes the bitmask to obtain <paramref name="bitCount"/> bits, begining at <paramref name="bitOffset"/>.
        /// Mutates the span passed as parameter.
        /// </summary>
        /// <param name="bitOffset">Number of bits from the 0th bit to the 1st bit to set.</param>
        /// <param name="bitCount">Number of bits from the <paramref name="bitOffset"/> to set.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> ComputeMask(in Span<byte> span, int bitOffset, int bitCount)
        {
            int bitLength = span.Length * 8;
            if (bitCount > bitLength)
                bitCount = bitLength;
            if (bitCount == bitLength)
            {
                if (bitOffset >= 0)
                {
                    // (1 << (count + offset))- 1;
                    MaskSignificantBitsExclusive(span, bitCount + bitOffset);
                    span.LeftShift(bitOffset);
                }
                else //if (offset < 0)
                {
                    // 0xFF... << offset
                    span.Assign(0xFF);
                }
            }
            else if (bitOffset < 0)
            {
                int totalBits = bitCount - bitOffset;
                if (totalBits <= 0)
                {
                    // Leave empty, nothing to fill: out of range
                }
                else
                {
                    // (1 << total) - 1
                    MaskSignificantBitsExclusive(span, totalBits);
                }
            }
            else
            {
                // ((1 << count) - 1) << offset
                MaskSignificantBitsExclusive(span, bitCount);
                span.LeftShift(bitOffset);
            }
            return span;
        }

        /// <summary>
        /// Performs following integer arithmetic operation on byte arrays:
        /// Little edian => (1 << n) - 1.
        /// Big edian => (1 >> n) - 1.
        /// </summary>
        /// <remarks>
        /// Originally not part of the shipped API design.
        /// Primerly used to generate compute masks.
        /// </remarks>
        public static void MaskSignificantBitsExclusive(in Span<byte> storage, int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            if (n > storage.Length)
            {
                storage.Assign(0xFF);
                return;
            }
            fixed(byte* storagePtr = &MemoryMarshal.GetReference(storage))
            {
                int bytes = n / 8,
                    remainder = n % 8;
#if BIG_EDIAN
                PInvoke.MemSet(storagePtr, 0xFF, bytes);
                if (remainder != 0)
                    *(storagePtr + (bytes + 1)*8) = (byte)((1 >> remainder) - 1);
#else
                PInvoke.MemSet(storagePtr + (storage.Length - bytes)*8, 0xFF, bytes);
                if (remainder != 0)
                    *(storagePtr + (storage.Length - bytes - 1)*8) = (byte)((1 << remainder) - 1);
#endif
            }
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> AndMask(this Span<byte> span, ulong mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrAndMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> AndMask(this Span<byte> span, uint mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrAndMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> AndMask(this Span<byte> span, byte mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrAndMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> AndMask(this ReadOnlySpan<byte> span, Span<byte> result, ulong mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrAndMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> AndMask(this ReadOnlySpan<byte> span, Span<byte> result, uint mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrAndMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to AND each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> AndMask(this ReadOnlySpan<byte> span, Span<byte> result, byte mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrAndMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrAndMask(byte* inPtr, byte* outPtr, int len, ulong mask)
        {
            int i = 0;
            if (len >= 8)
            {
                for(; i < len; i+= 8)
                    *(ulong*)(outPtr + i) = *(ulong*)(inPtr + i) & mask;
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)(outPtr + i) = *(uint*)(inPtr + i) & (uint)(mask >> 32);
                i += 4;
            }
            for (; i != len; i++)
                *(outPtr + i) = (byte)(*(inPtr + i) & (byte)(mask >> (32 + (len - i) * 8) & 0xFF) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrAndMask(byte* inPtr, byte* outPtr, int len, uint mask)
        {
            int i = 0;
            if (len >= 4)
            {
                for(; i < len; i+= 4)
                    *(uint*)(outPtr + i) = *(uint*)(inPtr + i) & mask;
                i -= 4;
            }
            for (; i != len; i++)
                *(outPtr + i) = (byte)(*(inPtr + i) & (byte)(mask >> ((len - i) * 8) & 0xFF) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrAndMask(byte* inPtr, byte* outPtr, int len, byte mask)
        {
            int rem = len;
            for (int i = 0; i != len; i++)
                *(outPtr + i) = (byte)(*(inPtr + i) & mask);
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> OrMask(this Span<byte> span, ulong mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrOrMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> OrMask(this Span<byte> span, uint mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrOrMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask. Mutates the span passed as this parameter.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        public static Span<byte> OrMask(this Span<byte> span, byte mask)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                ArrOrMask(spanPtr, spanPtr, span.Length, mask);
            }
            return span;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> OrMask(this ReadOnlySpan<byte> span, Span<byte> result, ulong mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrOrMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }
        
        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> OrMask(this ReadOnlySpan<byte> span, Span<byte> result, uint mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrOrMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }

        /// <summary>
        /// Masks each chunk - with the size of the mask - in the span with the value of mask.
        /// If the size of the span is not a multiple of the mask size, then the remaining byte will be computed with a part of the mask.
        /// </summary>
        /// <param name="result">The span to which to write the results of the operation.</param>
        /// <param name="mask">The mask to OR each chunk with.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> OrMask(this ReadOnlySpan<byte> span, Span<byte> result, byte mask)
        {
            if (result.Length < span.Length)
                throw new IndexOutOfRangeException();
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ArrOrMask(inPtr, outPtr, span.Length, mask);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrOrMask(byte* inPtr, byte* outPtr, int len, ulong mask)
        {
            int i = 0;
            if (len >= 8)
            {
                for(; i < len; i+= 8)
                    *(ulong*)(outPtr + i) = *(ulong*)(inPtr + i) | mask;
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)(outPtr + i) = *(uint*)(inPtr + i) | (uint)(mask >> 32);
                i += 4;
            }
            for (; i != len; i++)
                *(outPtr + i) = (byte)((*(inPtr + i) | (byte)(mask >> (32 + (len - i) * 8) & 0xFF)) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrOrMask(byte* inPtr, byte* outPtr, int len, uint mask)
        {
            int i = 0;
            if (len >= 4)
            {
                for(; i < len; i+= 4)
                    *(uint*)(outPtr + i) = *(uint*)(inPtr + i) | mask;
                i -= 4;
            }
            for (; i != len; i++)
                *(outPtr + i) = (byte)((*(inPtr + i) | (byte)(mask >> ((len - i) * 8) & 0xFF)) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ArrOrMask(byte* inPtr, byte* outPtr, int len, byte mask)
        {
            int rem = len;
            for (int i = 0; i != len; i++)
                *(outPtr + i) = (byte)(*(inPtr + i) | mask);
        }
        #endregion

        #region Shift
        /// <summary>
        /// Shifts all bits in the span to the left by one. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the span as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span)
         => LeftShift(span, span, 1);

        /// <summary>
        /// Shifts all bits in the span to the left by one. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span, in Span<byte> result)
         => LeftShift(span, result, 1);

        /// <summary>
        /// Shifts all bits in the span to the left by one. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this ReadOnlySpan<byte> span, in Span<byte> result)
         => LeftShift(span, result, 1);

        /// <summary>
        /// Shifts all bits in the span to the left by a specified ammount. Mutates the span passed as this parameter.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span, int n)
         => LeftShift(span, span, n);

        /// <summary>
        /// Shifts all bits in the span to the left by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span, in Span<byte> result, int n)
         => LeftShift((ReadOnlySpan<byte>)span, result, n);

        /// <summary>
        /// Shifts all bits in the span to the left by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> LeftShift(this ReadOnlySpan<byte> span, in Span<byte> result, int n)
        {
            int len = span.Length;
            if (len == 0)
                throw new ArgumentException(nameof(span));
            if (len > result.Length)
                throw new IndexOutOfRangeException();
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            // Doesnt modify the span
            if (n == 0)
            {
                if (span != result)
                    span.CopyTo(result);
                return result;
            }
            // Zeros all bits
            if (n > len * 8)
            {
                result.Assign(0x00);
                return result;
            }
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                // Shift n so that less then one byte is to be shifted
                // using memmove or memcopy depending on if the spans overlap
                int bytesToShift = n / 8;
                if (bytesToShift != 0)
                {
                    if (result.Overlaps(span))
                        PInvoke.MemMove(outPtr, inPtr + bytesToShift, len - bytesToShift);
                    else
                        PInvoke.MemCpy(outPtr, inPtr + bytesToShift, len - bytesToShift);
                    // Zero out dirty memory
                    PInvoke.MemSet(outPtr + len - bytesToShift, 0x00, bytesToShift);
                }
                // Shift by remaining bits.
                if (bytesToShift * 8 != n)
                    ProtLShiftBits(inPtr, outPtr, len, n % 8);
            }
            return result;
        }

        private static void ProtLShiftBits(byte* inPtr, byte* outPtr, int len, int bitsToShift)
        {
            Debug.Assert(bitsToShift < 8);
            Debug.Assert(len != 0);
#if WIN64
            ulong carryMask = 0;
            int chunksLen = len / 8 * 8;
            if (len % 8 != 0)
            {
                // Shift tailing bits
                ulong tmp = 0;
                PInvoke.MemCpy(&tmp, inPtr + chunksLen, len % 8);
                carryMask = tmp >> (64 - bitsToShift);
                PInvoke.MemCpy(outPtr + chunksLen, &tmp, len % 8);
            }
            if (len >= 8)
            {
                // Shift 8byte chunks
                for(int i = 0; i < chunksLen; i+=8)
                {
                    ulong tmp = *(ulong*)(inPtr + i);
                    *(ulong*)(outPtr + i) = (tmp << bitsToShift) | carryMask;
                    carryMask = tmp >> (64 - bitsToShift);
                }
            }
#else
            uint carryMask = 0;
            int chunksLen = len / 4 * 4;
            if (len % 4 != 0)
            {
                // Shift tailing bits
                uint tmp = 0;
                PInvoke.MemCpy(&tmp, inPtr + chunksLen, len % 4);
                carryMask = tmp >> (32 - bitsToShift);
                PInvoke.MemCpy(outPtr + chunksLen, &tmp, len % 4);
            }
            if (len >= 8)
            {
                // Shift 8byte chunks
                for(int i = 0; i < chunksLen; i+=4)
                {
                    uint tmp = *(uint*)(inPtr + i);
                    *(uint*)(outPtr + i) = (tmp << bitsToShift) | carryMask;
                    carryMask = tmp >> (32 - bitsToShift);
                }
            }
#endif
        }

        /// <summary>
        /// Shifts all bits in the span to the right by one. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference to the span as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span)
         => RightShift(span, span, 1);

        /// <summary>
        /// Shifts all bits in the span to the right by one. Mutates the span passed as this parameter.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span, in Span<byte> result)
         => RightShift(span, result, 1);

        /// <summary>
        /// Shifts all bits in the span to the right by one. Writes to the result span.
        /// </summary>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this ReadOnlySpan<byte> span, in Span<byte> result)
         => RightShift(span, result, 1);

        /// <summary>
        /// Shifts all bits in the span to the right by a specified ammount. Mutates the span passed as this parameter.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span, int n)
         => RightShift(span, span, n);

        /// <summary>
        /// Shifts all bits in the span to the right by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span, in Span<byte> result, int n)
         => RightShift((ReadOnlySpan<byte>)span, result, n);

        /// <summary>
        /// Shifts all bits in the span to the right by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by which to shift. Must be greater or equal to zero.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> RightShift(this ReadOnlySpan<byte> span, in Span<byte> result, int n)
        {
            int len = span.Length;
            if (len == 0)
                throw new ArgumentException(nameof(span));
            if (len > result.Length)
                throw new IndexOutOfRangeException();
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            // Doesnt modify the span
            if (n == 0)
            {
                if (span != result)
                    span.CopyTo(result);
                return result;
            }
            // Zeros all bits
            if (n > len * 8)
            {
                result.Assign(0x00);
                return result;
            }
            fixed(byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                // Shift so that less then one byte remains to be shifted
                // using memmove or memcopy depending on if the spans overlap
                int bytesToShift = n / 8;
                if (bytesToShift != 0)
                {
                    if (result.Overlaps(span))
                        PInvoke.MemMove(outPtr + bytesToShift, inPtr, len - bytesToShift);
                    else
                        PInvoke.MemCpy(outPtr + bytesToShift, inPtr, len - bytesToShift);
                    // Zero out dirty memory
                    PInvoke.MemSet(outPtr, 0x00, bytesToShift);
                }
                // Shift by remaining bits.
                if (bytesToShift * 8 != n)
                    ProtRShiftBits(inPtr, outPtr, len, n % 8);
            }
            return result;
        }

        private static void ProtRShiftBits(byte* inPtr, byte* outPtr, int len, int bitsToShift)
        {
            Debug.Assert(bitsToShift < 8);
            Debug.Assert(len != 0);
#if WIN64
            ulong carryMask = 0;
            int chunksLen = len / 8 * 8;
            if (len >= 8)
            {
                // Shift 8byte chunks
                for (int i = 0; i < chunksLen; i+=8)
                {
                    ulong tmp = *(ulong*)(inPtr + i);
                    *(ulong*)(outPtr + i) = (tmp >> bitsToShift) | carryMask;
                    // Left align bits that will overflow to the next chunk
                    carryMask = tmp << (64 - bitsToShift);
                }
            }
            if (len % 8 != 0)
            {
                // Shift tailing bits
                ulong tmp = 0;
                PInvoke.MemCpy(&tmp, inPtr + chunksLen, len % 8);
                tmp = (tmp >> bitsToShift) | carryMask;
                PInvoke.MemCpy(outPtr + chunksLen, &tmp, len % 8);
            }
#else
            uint carryMask = 0;
            int chunksLen = len / 4 * 4;
            if (len >= 4)
            {
                // Shift 8byte chunks
                for (int i = 0; i < chunksLen; i+=4)
                {
                    uint tmp = *(uint*)(inPtr + i);
                    *(uint*)(outPtr + i) = (tmp >> bitsToShift) | carryMask;
                    // Left align bits that will overflow to the next chunk
                    carryMask = tmp << (32 - bitsToShift);
                }
            }
            if (len % 4 != 0)
            {
                // Shift tailing bits
                uint tmp = 0;
                PInvoke.MemCpy(&tmp, inPtr + chunksLen, len % 4);
                tmp = (tmp >> bitsToShift) | carryMask;
                PInvoke.MemCpy(outPtr + chunksLen, &tmp, len % 4);
            }
#endif
        }
        #endregion
    }
}