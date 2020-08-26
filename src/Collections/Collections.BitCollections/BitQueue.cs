using System;
using System.Collections.Generic;

namespace Groundbeef.Collections.BitCollections
{
    public class BitQueue : BitArrayList, ICloneable
    {
        protected int m_growthFactor = 150;

        public BitQueue()
        {
            m_storage = new ulong[DefaultCapacity];
        }

        private BitQueue(ulong[] storage, sbyte loOffset, sbyte hiOffset, int elements, int count) : base(storage, loOffset, hiOffset, elements, count)
        { }

        public bool IsEmpty => Count == 0;

        public virtual object Clone()
        {
            return new BitQueue(m_storage, m_loOffset, m_hiOffset, m_elements, m_count);
        }

        public virtual void Enqueue(bool item)
        {
            EnsureCapacity(m_elements + 1);
            m_hiOffset++;
            EnforceOffsetRange();
            WriteBitAt(m_storage.AsSpan(m_count - 1), m_hiOffset, item);
            UpdateElementsCount();
        }

        public virtual bool Dequeue()
        {
            bool item = ReadBitAt(m_storage, m_loOffset);
            m_loOffset++;
            EnforceOffsetRange();
            UpdateElementsCount();
            return item;
        }

        public virtual bool Peek()
        {
            return ReadBitAt(m_storage, m_loOffset);
        }

        public virtual bool Contains(bool item)
        {
            for (int i = m_loOffset; i < m_elements; i++)
            {
                if (ReadBitAt(m_storage, i) == item)
                    return true;
            }
            return false;
        }

        protected override void EnsureCapacity(int elementCount)
        {
            if (elementCount + m_loOffset + 8 >= m_count * BitsInLong)
            {
                int length = (int)(m_count * m_growthFactor / 1.00f);
                Array.Resize(ref m_storage, length);
            }
        }

        #region SyncronizedBitQueue
        public static BitQueue Syncronized(BitQueue queue) => new SyncronizedBitQueue(queue);

        private class SyncronizedBitQueue : BitQueue
        {
            private readonly BitQueue _queue;
            private readonly object _syncRoot;

            public SyncronizedBitQueue(BitQueue queue)
            {
                _queue = queue;
                _syncRoot = queue.SyncRoot;
            }

            public override bool IsSynchronized => true;

            public override object SyncRoot => _syncRoot;

            public override int Count
            {
                get
                {
                    lock (_syncRoot)
                        return _queue.Count;
                }
            }

            public override int DataBlockCount
            {
                get
                {
                    lock (_syncRoot)
                        return _queue.DataBlockCount;
                }
            }

            public override void Clear()
            {
                lock (_syncRoot)
                    _queue.Clear();
            }

            public override object Clone()
            {
                lock (_syncRoot)
                    return _queue.Clone();
            }

            public override void TrimToSize()
            {
                lock (_syncRoot)
                    _queue.TrimToSize();
            }

            public override void CopyTo(Array array, int arrayIndex)
            {
                lock (_syncRoot)
                    _queue.CopyTo(array, arrayIndex);
            }

            public override void CopyTo(bool[] array, int arrayIndex)
            {
                lock (_syncRoot)
                    _queue.CopyTo(array, arrayIndex);
            }

            public override void CopyTo(ulong[] array, int arrayIndex)
            {
                lock (_syncRoot)
                    _queue.CopyTo(array, arrayIndex);
            }

            public override IEnumerator<bool> GetEnumerator()
            {
                lock (_syncRoot)
                    return _queue.GetEnumerator();
            }

            public override void Enqueue(bool item)
            {
                lock (_syncRoot)
                    _queue.Enqueue(item);
            }

            public override bool Dequeue()
            {
                lock (_syncRoot)
                    return _queue.Dequeue();
            }

            public override bool Peek()
            {
                lock (_syncRoot)
                    return _queue.Peek();
            }

            public override bool Contains(bool item)
            {
                lock (_syncRoot)
                    return _queue.Peek();
            }
        }
        #endregion
    }
}