using System;
using System.Collections;
using System.Collections.Generic;

namespace Groundbeef.Collections
{
    public class BitQueue : IReadOnlyList<bool>, ICollection
    {
        public const int DefaultCapacity = 16;
        private ulong[] _storage = new ulong[DefaultCapacity];
        private int _count,
                    _elements;
        private byte _loOffset, _hiOffset;

        public bool IsEmpty => _count == 0;

        /// <summary>
        /// Gets the number of <see cref="Boolean"/> values enqueued in the <see cref="BitQueue"/>.
        /// </summary>
        public int Count => _elements;

        /// <summary>
        /// Gets the number of <see cref="ulong"/> elements used to store the <see cref="Boolean"/> values. 
        /// The ratio converges to 1 : 56.
        /// </summary>
        public int StorageCount => _count;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        /// <summary>
        /// Peeks at the element at the <paramref name="index"/> relative to the top.
        /// </summary>
        public bool this[int index] => PeekAt(index);

        /// <summary>
        /// Enqueues the value to the <see cref="BitQueue"/>.
        /// </summary>
        public void Enqueue(bool value)
        {
            if (_count == 0)
            {
                _count = 1;
            }
            // Current long is full, move to next
            else if (_hiOffset >= 56)
            {
                _count++;
                _hiOffset = 0;
            }
            // Enure at least one partially free long
            if (_count > _storage.Length)
                Array.Resize(ref _storage, _storage.Length + 4);
            WriteHiBitAtOffset(value);
            _hiOffset++;
            RefreshElementCount();
        }

        /// <summary>
        /// Dequeues the topmost value from the <see cref="BitQueue"/>.
        /// </summary>
        public bool Dequeue()
        {
            bool value = Peek();
            // Lo is empty get next long
            if (_loOffset >= 56)
                DropLoLong();
            else
                _loOffset++;
            RefreshElementCount();
            return value;
        }

        /// <summary>
        /// Peeks at the topmost value of the <see cref="BitQueue"/>.
        /// </summary>
        public bool Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Queue is empty.");
            return ReadLoBitAtOffset();
        }

        /// <summary>
        /// Peeks at the element at the <paramref name="index"/> relative to the top.
        /// </summary>
        public bool PeekAt(int index)
        {
            if ((uint)index > (uint)Count)
                throw new IndexOutOfRangeException();
            return ReadBitAt(index);
        }

        /// <summary>
        /// Dequeues the element at the <paramref name="index"/> relative to the top.
        /// </summary>
        public bool DequeueAt(int index)
        {
            bool value = PeekAt(index);
            if (index * 2 - _elements < 0)
            {
                RightShiftArrayBitsAt(index);
                IncrementLo();
            }
            else
            {
                LeftShiftArrayBitsAt(index);
                if (_hiOffset == 0)
                {
                    // Drop hi long
                    _count--;
                    _hiOffset = 55;
                }
                else
                {
                    _hiOffset--;
                }
            }
            RefreshElementCount();
            return value;
        }

        /// <summary>
        /// Clears all data from the <see cref="BitQueue"/>.
        /// </summary>
        public void Clear()
        {
            _count = 0;
            _loOffset = 0;
            _hiOffset = 0;
            _elements = 0;
        }

        public IEnumerator<bool> GetEnumerator() => InternalGetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => InternalGetEnumerator();

        /// <summary>
        /// Copies the internal <see cref="ulong[]"/> storage array with the length of <see cref="StorageCount"/> to the <paramref name="array"/> at the <paramref name="index"/> specified.
        /// </summary>
        /// <param name="array">The array with the element type <see cref="ulong"/> to copy the elements to.</param>
        /// <param name="index">The index at which to copy the first element.</param>
        public void CopyTo(Array array, int index)
        {
            Array.Copy(_storage, 0, array, index, _storage.Length);
        }

        private void RefreshElementCount()
        {
            _elements = Math.Max(0, (_count - 1) * 56 + _hiOffset - _loOffset);
        }

        private IEnumerator<bool> InternalGetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return ReadBitAt(i);
            }
        }

        private void IncrementLo()
        {
            if (_loOffset >= 56)
            {
                LeftShiftArrayElements();
                _loOffset = 0;
            }
            else
            {
                _loOffset++;
            }
        }

        

        private void DropLoLong()
        {
            LeftShiftArrayElements();
            _count--;
            _loOffset = 0;
        }

        private unsafe bool ReadBitAt(int index)
        {
            // Offset index by read head
            index += _loOffset;
            int element = index / 56,
                offset = index % 56;
            fixed(ulong* source = &_storage[element])
            {
                return ReadBit(source, offset);
            }
        }

        private void LeftShiftArrayElements()
        {
            Span<ulong> storage = _storage;
            for(int i = 0; i < _count; i++)
            {
                storage[i] = storage[i+1];
            }
        }

        private unsafe void LeftShiftArrayBitsAt(int offset)
        {
            int length = _count * 64 - offset - 1;
            fixed(ulong* storage = &_storage[0])
            {
                for(int i = offset; i < length; i++)
                    *(storage + i) = *(storage + i + 1);
            }
        }

        private unsafe void RightShiftArrayBitsAt(int offset)
        {
            fixed(ulong* storage = &_storage[0])
            {
                for(int i = offset; i > 0; i--)
                    *(storage + i) = *(storage + i - 1);
            }
        }

        private unsafe void WriteHiBitAtOffset(bool value)
        {
            fixed(ulong* hi = &_storage[_count - 1])
            {
                *(byte*)(hi + _hiOffset) &= (byte)(0xFF & (value ? 0 : 1));
            }
        }

        private unsafe bool ReadLoBitAtOffset()
        {
            fixed(ulong* lo = &_storage[0])
            {
                return ReadBit(lo, _loOffset);
            }
        }

        private unsafe bool ReadBit(ulong* source, int offset)
        {
            return (*(byte*)(source + offset) & (byte)0x01) == (byte)0x01;
        }
    }
}