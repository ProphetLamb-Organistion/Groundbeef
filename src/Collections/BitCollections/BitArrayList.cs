using Groundbeef.SharedResources;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Collections.BitCollections
{
    public abstract class BitArrayList : IReadOnlyCollection<bool>, ICollection
    {
        public const int DefaultCapacity = 4;
        internal const int BitsInLong = sizeof(long) * 8;
        protected ulong[] m_storage = null!;
        private object? _syncRoot;
        protected int m_count, // Number of allocated long chunks
                      m_elements; // Number of elements between lo and incl. hi
        protected sbyte m_loOffset, // Offset in the first ulong in storage to the first bit
                        m_hiOffset; // Offset in the last ulong to the first empty bit

        protected BitArrayList()
        { }

        protected BitArrayList(ulong[] storage, sbyte loOffset, sbyte hiOffset, int elements, int count)
        {
            m_storage = storage;
            m_loOffset = loOffset;
            m_hiOffset = hiOffset;
            m_elements = elements;
            m_count = count;
        }

        /// <summary>
        /// Gets the number of <see cref="Boolean"/> elements in the collection.
        /// </summary>
        public virtual int Count => m_elements;

        /// <summary>
        /// Gets the number of <see cref="ulong"/> chunks allocated to encompass all <see cref="Boolean"/> elements in the collection. 
        /// <see cref="DataBlockCount"/> : <see cref="Count"/> converges to 1 : 64 on a 64bit platform and to 1 : 32 on a 32bit platform.
        /// </summary>
        public virtual int DataBlockCount => m_count;

        /// <summary>
        /// Indicates that the collection is syncronized.
        /// </summary>

        public virtual bool IsSynchronized
        {
            get => false;
        }

        /// <summary>
        /// Gets the <see cref="Object"/> that is the syncronization root for the current instance if <see cref="IsSyncronized"/>; otherwise, a reference to the instance itself.
        /// </summary>
        public virtual object SyncRoot
        {
            get
            {
                if (_syncRoot is null)
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        /// <summary>
        /// Clears all data from the collection.
        /// </summary>
        /// <remarks>Does not reallocate.</remarks>
        public virtual void Clear()
        {
            m_count = 0;
            m_elements = 0;
            m_hiOffset = 0;
            m_loOffset = 0;
        }

        /// <summary>
        /// Trims the <see cref="DataBlockCount"/> to minimally fit the <see cref="Count"/> by reallocating the storage array.
        /// </summary>
        public virtual void TrimToSize()
        {
            // Shift lo to zero
            while (m_loOffset / BitsInLong != 0)
                LeftShiftArrayLong(m_storage.AsSpan());
            while (m_loOffset != 0)
                LeftShiftArrayBit(m_storage.AsSpan());
            Array.Resize(ref m_storage, Math.Max(1, m_elements - 1) / BitsInLong + 1);
        }

        public virtual void CopyTo(Array array, int arrayIndex)
        {
            if (array.Rank != 1)
                throw new ArgumentException(ExceptionResource.ARRAY_MULTIRANK_NOTSUPPORTED);
            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException(ExceptionResource.ARRAY_LOWERBOUND_ZERO);
            if (arrayIndex + m_elements > array.Length)
                throw new IndexOutOfRangeException();
            if (array is bool[] boolean)
                CopyTo(boolean, arrayIndex);
            else if (array is ulong[] storage)
                CopyTo(storage, arrayIndex);
            else
                throw new ArgumentException(ExceptionResource.COLLECTION_INVALID_ELEMENTTYPE);
        }

        public virtual unsafe void CopyTo(bool[] array, int arrayIndex)
        {
            if (arrayIndex + m_elements > array.Length)
                throw new IndexOutOfRangeException();
            for (int i = m_loOffset; i < m_elements; i++)
                array.SetValue(ReadBitAt(m_storage, i), arrayIndex++);
        }

        public virtual void CopyTo(ulong[] array, int arrayIndex)
        {
            if (arrayIndex + m_elements > array.Length)
                throw new IndexOutOfRangeException();
            for (int i = 0; i < m_count; i++)
                array.SetValue(m_storage[i], arrayIndex++);
        }

        public virtual IEnumerator<bool> GetEnumerator()
        {
            for (int i = m_loOffset; i < m_elements; i++)
                yield return ReadBitAt(m_storage, i);
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Ensures that lo & hi are widthin [0,64) on 64bit or [0,32) on 32bit. Modulates m_count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void EnforceOffsetRange()
        {
            // Assume offsets to have a value of no more then twince the offset of bits in long, because it ensure capacity must be called after each chage in size
            if (m_hiOffset >= BitsInLong)
            {
                m_count++;
                m_hiOffset = 0;
            }
            else if (m_hiOffset < 0)
            {
                m_count--;
                m_hiOffset = BitsInLong - 1;
            }
            if (m_loOffset >= BitsInLong) // First ulong is empty
            {
                LeftShiftArrayLong(m_storage.AsSpan());
                m_count--;
                m_loOffset = 0;
            }
            else if (m_loOffset < 0)
            {
                RightShiftArrayLong(m_storage.AsSpan());
                m_count++;
                m_loOffset = 55;
            }
        }

        /// <summary>
        /// Ensures that the storage array has enouth capacity to hoist the <paramref name="elementCount"/>.
        /// </summary>
        protected virtual void EnsureCapacity(int elementCount)
        {
            if (elementCount + m_loOffset + 8 >= m_count * BitsInLong)
            {
                // The number of default capacities needed to satisfy the required element count.
                int incrementMult = Math.Max(1, (elementCount + m_loOffset + 7) / BitsInLong - m_count) / DefaultCapacity + 1; // Two roundup integer divisions: [...] BitsInLong + 1 - 1 - m_count [...]
                Array.Resize(ref m_storage, m_storage.Length + DefaultCapacity * incrementMult);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe static void RightShiftArrayLong(in Span<ulong> sourceSpan)
        {
            fixed (ulong* source = &MemoryMarshal.GetReference(sourceSpan))
            {
                for (int i = sourceSpan.Length - 2; i >= 0; i--)
                    source[i] = source[i + 1];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void RightShiftLongRightAt(in ulong[] sourceArray, int index, int offset)
        {
            ulong mask = 0xFFFFFFFFFFFFFFFF << offset;
            sourceArray[index] &= mask;
            sourceArray[index] |= (sourceArray[index] >> 1) & ~mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void RightShiftLongLeftAt(in ulong[] sourceArray, int index, int offset)
        {
            ulong mask = 0xFFFFFFFFFFFFFFFF << offset;
            sourceArray[index] &= ~mask;
            sourceArray[index] |= (sourceArray[index] >> 1) & mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe static void RightShiftArrayBit(in Span<ulong> sourceSpan)
        {
            fixed (ulong* source = &MemoryMarshal.GetReference(sourceSpan))
            {
                // Because of the dead byte at the last index we can shift a int32, int16, byte on 64bit, and a int16, byte on 32bit,
                // without going out of our allocated memory range.
                ulong* lastLong = &source[sourceSpan.Length - 1];
#if WIN64
                *(byte*)(lastLong + 48) = *(byte*)(lastLong + 49);
                *(ushort*)(lastLong + 32) = *(ushort*)(lastLong + 33);
                *(uint*)(lastLong + 0) = *(uint*)(lastLong + 1);
#else
                *(byte*)(lastLong + 16) = *(byte*)(lastLong + 17);
                *(ushort*)(lastLong + 0) = *(ushort*)(lastLong + 1);
#endif
                // Rightshift each element in our array
                for (int i = sourceSpan.Length - 2; i >= 0; i--)
                {
                    (source + 1)[i] = source[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe static void LeftShiftArrayLong(in Span<ulong> sourceSpan)
        {
            fixed (ulong* source = &MemoryMarshal.GetReference(sourceSpan))
            {
                for (int i = 1; i < sourceSpan.Length; i++)
                    source[i - 1] = source[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void LeftShiftLongLeftAt(in ulong[] sourceArray, int index, int offset)
        {
            ulong mask = 0xFFFFFFFFFFFFFFFF >> offset;
            sourceArray[index] &= mask;
            sourceArray[index] |= (sourceArray[index] << 1) & ~mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe static void LeftShiftArrayBit(in Span<ulong> sourceSpan)
        {
            fixed (ulong* source = &MemoryMarshal.GetReference(sourceSpan))
            {
                // Because of the dead byte at the last index we can shift a int32, int16, byte on 64bit, and a int16, byte on 32bit,
                // without going out of our allocated memory range.
                ulong* lastLong = &source[sourceSpan.Length - 1];
#if WIN64
                *(uint*)(lastLong + 1) = *(uint*)(lastLong + 0);
                *(ushort*)(lastLong + 33) = *(ushort*)(lastLong + 32);
                *(byte*)(lastLong + 49) = *(byte*)(lastLong + 48);
#else
                *(ushort*)(lastLong + 1) = *(ushort*)(lastLong + 0);
                *(byte*)(lastLong + 17) = *(byte*)(lastLong + 16);
#endif
                // Leftshift each element in our array
                for (int i = 0; i < sourceSpan.Length - 1; i++)
                {
                    (source - 1)[i] = source[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UpdateElementsCount()
        {
            m_elements = Math.Max(0, (m_count - 1) * BitsInLong + 1 + m_hiOffset - m_loOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected unsafe static void WriteBitAt(in Span<ulong> sourceSpan, int offset, bool value)
        {
            fixed (ulong* source = &MemoryMarshal.GetReference(sourceSpan))
            {
                *(byte*)(source + offset) = (byte)(value
                ? *(byte*)(source + offset) | 0x01 // Set bit at offset
                : *(byte*)(source + offset) & 0xFE); // Remove bit at offset
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool ReadBitAt(in ulong[] source, int index)
        {
            unchecked
            {
                return ((byte)(source[index / BitsInLong] << (index % BitsInLong)) & 0x01) == 0x01;
            }
        }

        protected static unsafe void CopyFromBooleanArray(ref bool[] sourceArray, ref ulong[] targetArray, int length)
        {
            fixed (bool* source = &sourceArray[0])
            fixed (ulong* target = &targetArray[0])
            {
                byte* sourceAsBytes = (byte*)source;
                ulong* value = stackalloc ulong[2];
                for (int i = 0; i < length; i++)
                {
                    // Asign value at the correct bit. 
                    // A boolean vale is a byte 0x00 if false; otherwise, 0x01 so we can simply shift it to the correct position;
                    *(byte*)(target + i) |= *(sourceAsBytes + i * 8);
                }
            }
        }
    }
}
