using System.Runtime.InteropServices;
using System;
using System.Runtime.CompilerServices;

namespace Groundbeef.Collections.Spans
{
    public static unsafe class BitwiseExtention
    {
        #region Boolean
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
        public static void ArrAnd(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
            if (len >= 8)
            {
                for (; i < len; i += 8)
                    *(ulong*)outPtr[i] = *(ulong*)(leftPtr + i) & *(ulong*)(rightPtr + i);
                i -= 8; // Offset overshoot
            }
            if (len - i >= 4)
            {
                *(uint*)outPtr[i] = *(uint*)(leftPtr + i) & *(uint*)(rightPtr + i);
                i+=4;
            }
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
        public static void ArrOr(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
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
        public static void ArrNand(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
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
        public static void ArrNor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
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
        public static void ArrXor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
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
        public static void ArrXnor(byte* leftPtr, byte* rightPtr, byte* outPtr, int len)
        {
            int i = 0;
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
            for (; i != len; i++)
                *(outPtr + i) = (byte)(~(*(leftPtr + i) ^ *(rightPtr + i)) & 0xFF);
        }
        #endregion

        #region Masked
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
        public static void ArrAndMask(byte* inPtr, byte* outPtr, int len, ulong mask)
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
        public static void ArrAndMask(byte* inPtr, byte* outPtr, int len, uint mask)
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
        public static void ArrAndMask(byte* inPtr, byte* outPtr, int len, byte mask)
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
        public static void ArrOrMask(byte* inPtr, byte* outPtr, int len, ulong mask)
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
        public static void ArrOrMask(byte* inPtr, byte* outPtr, int len, uint mask)
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
        public static void ArrOrMask(byte* inPtr, byte* outPtr, int len, byte mask)
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
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span, int n)
         => LeftShift(span, span, n);

        /// <summary>
        /// Shifts all bits in the span to the left by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> LeftShift(this Span<byte> span, in Span<byte> result, int n)
         => LeftShift((ReadOnlySpan<byte>)span, result, n);

        /// <summary>
        /// Shifts all bits in the span to the left by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> LeftShift(this ReadOnlySpan<byte> span, in Span<byte> result, int n)
        {
            if (span.Length == 0)
                throw new ArgumentException(nameof(span));
            if (span.Length < result.Length)
                throw new IndexOutOfRangeException();
            if ((uint)n > 64u) // Casting a negative to unsinged will result in a big number, have to cast other operand to unsigned as well, else the signed comparison will be used.
                throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0)
            {
                span.CopyTo(result);
                return result;
            }
            fixed (byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ProtLShift(inPtr, outPtr, span.Length, n);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtLShift(byte* inPtr, byte* outPtr, int len, int n)
        {
            int i = 0;
            ulong carryMask = 0;
            if (len >= 8)
            {
                for(; i < len; i += 8)
                {
                    ulong tmp = *(ulong*)(inPtr + i);
                    *(ulong*)(outPtr + i) = (tmp << n) | carryMask;
                    carryMask = tmp >> (64 - n);
                }
                if (i == len + 8)
                    return;
                i -= 7; // Offset overshoot
            }
            if (i < len)
            {
                ulong tmp = 0;
                int shift = ((len + 1) / 8 - 1) * 8; // Round up to 8 bytes
                for (int j = i; j < len; j++)
                    tmp |= (ulong)*(inPtr + j) << ((shift - j) * 8) & 0xFF;
                tmp = (tmp << n) | carryMask;
                for (int j = i; j < len; j++)
                    *(outPtr + j) = (byte)(tmp >> ((shift - i) * 8) & 0xFF);
            }
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
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference to the span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span, int n)
         => RightShift(span, span, n);

        /// <summary>
        /// Shifts all bits in the span to the right by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> RightShift(this Span<byte> span, in Span<byte> result, int n)
         => RightShift((ReadOnlySpan<byte>)span, result, n);

        /// <summary>
        /// Shifts all bits in the span to the right by a specified ammount. Writes to the result span.
        /// </summary>
        /// <param name="n">The amount by shich to shift. Must be greater or equal to one and smaller or equal to 64.</param>
        /// <returns>The reference of the result span passed as parameter.</returns>
        public static Span<byte> RightShift(this ReadOnlySpan<byte> span, in Span<byte> result, int n)
        {
            if (span.Length == 0)
                throw new ArgumentException(nameof(span));
            if (span.Length < result.Length)
                throw new IndexOutOfRangeException();
            if ((uint)n > 64u)
                throw new ArgumentOutOfRangeException(nameof(n));
            if (n == 0)
            {
                span.CopyTo(result);
                return result;
            }
            fixed (byte* inPtr = &MemoryMarshal.GetReference(span))
            fixed(byte* outPtr = &MemoryMarshal.GetReference(result))
            {
                ProtRShift(inPtr, outPtr, span.Length, n);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ProtRShift(byte* inPtr, byte* outPtr, int len, int n)
        {
            int i = len - 8;
            ulong carryMask = 0;
            if (len >= 8)
            {
                for (; i >= 0; i -= 8)
                {
                    ulong tmp = *(ulong*)(inPtr + i);
                    *(ulong*)outPtr = tmp >> n | carryMask;
                    carryMask = tmp & ((1ul << n) - 1);
                }
                if (i == -8)
                    return;
                i += 7; // Offset overshoot
            }
            if (i < 0)
            {
                ulong tmp = 0;
                for (int j = 0; j < i; j++)
                    tmp |= ((ulong)*(inPtr + j) & 0xFF) << ((i - j) * 8);
                tmp = tmp >> n | carryMask;
                for (int j = 0; j < i; j++)
                    *(outPtr + j) = (byte)((tmp >> ((i - j) * 8)) & 0xFF);
            }
        }
        #endregion
    }
}