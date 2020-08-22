using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Groundbeef.Collections.BitCollections
{
    public class BitList : BitArrayList, IList<bool>
    {
        public BitList()
        {
            m_storage = new ulong[DefaultCapacity];
        }

        public BitList(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Value cannot be negative or zero.");
            m_storage = new ulong[capacity];
        }

        private BitList(ulong[] storage, sbyte loOffset, sbyte hiOffset, int elements, int count) : base(storage, loOffset, hiOffset, elements, count)
        { }

        public BitList(IEnumerable<bool> collection)
        {
            bool[] source;
            if (collection is ICollection<bool> c)
            {
                source = new bool[c.Count];
                c.CopyTo(source, 0);
            }
            else
            {
                source = new bool[System.Linq.Enumerable.Count(collection)];
                using IEnumerator<bool> en = collection.GetEnumerator();
                for(int i = 0; i < source.Length && en.MoveNext(); i++)
                    source[i] = en.Current;
            }
            m_count = (source.Length + 7) / BitsInLong + 1; // Round up integer division: (a - 1) / b + 1
            m_hiOffset = (sbyte)(m_count * BitsInLong - source.Length);
            m_storage = new ulong[DefaultCapacity];
            EnsureCapacity(source.Length);
            EnforceOffsetRange();
            CopyFromBooleanArray(ref source, ref m_storage, source.Length);
        }

        public bool this[int index] { get => GetValue(index); set => SetValue(index, value); }

        public virtual bool IsReadOnly => false;

        public bool IsEmpty => Count == 0;

        public virtual object Clone()
        {
            return new BitList(m_storage, m_loOffset, m_hiOffset, m_elements, m_count);
        }
        
        protected virtual bool GetValue(int index)
        {
            if ((uint)index >= (uint)m_elements)
                throw new IndexOutOfRangeException();
            return ReadBitAt(m_storage, index + m_loOffset);
        }

        protected virtual void SetValue(int index, bool value)
        {
            if ((uint)index >= (uint)m_elements)
                throw new IndexOutOfRangeException();
            WriteBitAt(m_storage.AsSpan(), index + m_loOffset, value);
        }

        public virtual void Add(bool item)
        {
            EnsureCapacity(m_elements + 1);
            m_hiOffset++;
            EnforceOffsetRange();
            WriteBitAt(m_storage.AsSpan(m_count - 1), m_hiOffset, item);
            UpdateElementsCount();
        }

        public virtual bool Contains(bool item) => IndexOf(item) != -1;

        public virtual int IndexOf(bool item)
        {
            for(int i = m_loOffset; i < m_elements; i++)
            {
                if (ReadBitAt(m_storage, i) == item)
                    return i - m_loOffset;
            }
            return -1;
        }

        public virtual void Insert(int index, bool item)
        {
            if ((uint)index >= (uint)m_elements)
                throw new IndexOutOfRangeException();
            if (index == m_elements - 1)
            {
                Add(item);
                return;
            }
            EnsureCapacity(m_elements + 1);
            int longIndex = (index + m_loOffset) / BitsInLong;
            int offset = index + m_loOffset - longIndex * BitsInLong;
            if (index * 2 < m_elements)
                InsertLeftShift(longIndex, offset, item);
            else
                InsertRightShift(longIndex, offset, item);
            UpdateElementsCount();
        }

        public virtual bool Remove(bool item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        public virtual void RemoveAt(int index)
        {
            if ((uint)index >= (uint)m_elements)
                throw new IndexOutOfRangeException();
            int longIndex = (index + m_loOffset) / BitsInLong;
            int offset = index + m_loOffset - longIndex * BitsInLong;
            if (index == 0)
            {
                m_loOffset++;
            }
            else if (index == m_elements - 1)
            {
                m_hiOffset--;
            }
            else if (index * 2 < m_elements)
            {
                RemoveRightShift(longIndex, offset);
                m_loOffset++;
            }
            else // Remove by left shift
            {
                RemoveLeftShift(longIndex, offset);
                m_loOffset--;
            }
            EnforceOffsetRange();
            UpdateElementsCount();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void InsertLeftShift(int longIndex, int offset, bool value)
        {
            m_loOffset--;
            EnforceOffsetRange();
            Span<ulong> span = m_storage.AsSpan(0, longIndex+1);
            LeftShiftArrayBit(span);
            // Correct the part of the long that was shifted
            RightShiftLongRightAt(m_storage, longIndex, offset);
            WriteBitAt(span, longIndex * BitsInLong + offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void InsertRightShift(int longIndex, int offset, bool value)
        {
            m_hiOffset++;
            EnforceOffsetRange();
            Span<ulong> span = m_storage.AsSpan(longIndex);
            RightShiftArrayBit(span);
            // Correct the part of the long that was shifted
            LeftShiftLongLeftAt(m_storage, longIndex, offset);
            WriteBitAt(span, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RemoveLeftShift(int longIndex, int offset)
        {
            Span<ulong> span = m_storage.AsSpan(longIndex+1);
            LeftShiftLongLeftAt(m_storage, longIndex, offset);
            LeftShiftArrayBit(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RemoveRightShift(int longIndex, int offset)
        {
            Span<ulong> span = m_storage.AsSpan(0, longIndex);
            RightShiftLongRightAt(m_storage, longIndex, offset);
            RightShiftArrayBit(span);
        }

        #region SyncronizedBitList
        public BitList Syncronized(BitList list) => new SyncronizedBitList(list);

        private class SyncronizedBitList : BitList
        {
            private readonly BitList _list;
            private readonly object _syncRoot;

            public SyncronizedBitList(BitList list)
            {
                _list = list;
                _syncRoot = list.SyncRoot;
            }

            public override bool IsSynchronized => true;

            public override object SyncRoot => _syncRoot;

            public override bool IsReadOnly
            {
                get
                {
                    lock (_syncRoot)
                        return _list.IsReadOnly;
                }
            }

            public override int Count
            {
                get
                {
                    lock (_syncRoot)
                        return _list.Count;
                }
            }

            public override int DataBlockCount
            {
                get
                {
                    lock (_syncRoot)
                        return _list.DataBlockCount;
                }
            }

            public override void Clear()
            {
                lock (_syncRoot)
                    _list.Clear();
            }

            public override object Clone()
            {
                lock (_syncRoot)
                    return _list.Clone();
            }

            public override void TrimToSize()
            {
                lock (_syncRoot)
                    _list.TrimToSize();
            }

            public override void CopyTo(Array array, int arrayIndex)
            {
                lock (_syncRoot)
                    _list.CopyTo(array, arrayIndex);
            }

            public override void CopyTo(bool[] array, int arrayIndex)
            {
                lock (_syncRoot)
                    _list.CopyTo(array, arrayIndex);
            }

            public override void CopyTo(ulong[] array, int arrayIndex)
            {
                lock (_syncRoot)
                    _list.CopyTo(array, arrayIndex);
            }

            public override IEnumerator<bool> GetEnumerator()
            {
                lock (_syncRoot)
                    return _list.GetEnumerator();
            }


            protected override bool GetValue(int index)
            {
                lock(_syncRoot)
                    return _list.GetValue(index);
            }

            protected override void SetValue(int index, bool value)
            {
                lock (_syncRoot)
                    _list.SetValue(index, value);
            }

            public override void Add(bool item)
            {
                lock (_syncRoot)
                    _list.Add(item);
            }

            public override bool Contains(bool item)
            {
                lock (_syncRoot)
                    return _list.Contains(item);
            }

            public override int IndexOf(bool item)
            {
                lock (_syncRoot)
                    return _list.IndexOf(item);
            }

            public override void Insert(int index, bool item)
            {
                lock (_syncRoot)
                    _list.Insert(index, item);
            }

            public override bool Remove(bool item)
            {
                lock (_syncRoot)
                    return _list.Remove(item);
            }

            public override void RemoveAt(int index)
            {
                lock (_syncRoot)
                     _list.RemoveAt(index);
            }
        }
        #endregion
    }
}