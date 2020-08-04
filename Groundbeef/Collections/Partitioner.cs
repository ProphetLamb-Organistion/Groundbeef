using System.Collections.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Groundbeef.Collections
{
    public interface IPartitionEnumerator<T> : IEnumerator<T>
    {
        bool MoveNextSatisfied();
        bool MoveNextFalsified();
        bool IsSatisfied { get; }
    }

    public interface IPartitionedEnumerable<T> : IEnumerable<T>
    {
        new IPartitionEnumerator<T> GetEnumerator();
        IEnumerator<T> GetSatisfiedEnumerator();
        IEnumerator<T> GetFalsifiedEnumerator();
    }

    public class PartitionEnumerator<T> : IPartitionEnumerator<T>
    {
        private readonly IEnumerator<T> _sourceEnumerator;
        private Queue<T> _satisfiedQueue = new Queue<T>(),
                         _falsifiedQueue = new Queue<T>();
        private readonly Predicate<T> _partitioner;
        private bool _isSatisfied;
        private T _current = default!;
        /* 
         * When enqueing assign the boolean value indicating whether the value satisfied to condition to a ulong at a offset.
         * That offset is between [1..56). Before enqueing the offset if incremented; before dequeing the offset is decremented.
         * When enqueing, if the offset would be greater then 56, then we push the ulong to the queue, and reset.
         * When autodequeing, if the offset would equal one, then we try to pop a ulong from the queue; if that fails or both queues are empty then the dequeing fails.
         */
        private Queue<ulong> _dequeueOrderQueue = new Queue<ulong>();
        private ulong _dequeueOrder;
        private byte _dequeueOrderOffset = 1;

        internal PartitionEnumerator(in IEnumerable<T> source, in Predicate<T> partitioner)
        {
            _sourceEnumerator = source.GetEnumerator();
            _partitioner = partitioner;
        }

        public bool IsSatisfied => _isSatisfied;

        public T Current => _current;

        [MaybeNull]
        object IEnumerator.Current => _current;

        public bool MoveNextSatisfied()
        {
            // Attempt to get queued entry
            if (TryDequeue(true))
            {
                _isSatisfied = true;
                return true;
            }
            // Seek next satisfied
            bool result;
            while((result = _sourceEnumerator.MoveNext()) && !_partitioner(_sourceEnumerator.Current))
                Enqueue(_sourceEnumerator.Current, false);
            if (result)
            {
                _current = _sourceEnumerator.Current;
                _isSatisfied = true;
                return true;
            }
            else
            {
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public bool MoveNextFalsified()
        {
            // Attempt to get queued entry
            if (TryDequeue(false))
            {
                _isSatisfied = false;
                return true;
            }
            // Seek next satisfied
            bool result;
            while((result = _sourceEnumerator.MoveNext()) && _partitioner(_sourceEnumerator.Current))
                Enqueue(_sourceEnumerator.Current, true);
            if (result)
            {
                _current = _sourceEnumerator.Current;
                _isSatisfied = false;
                return true;
            }
            else
            {
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public bool MoveNext()
        {
            //Attempt to get queued entry
            if (DeqeueAny())
                return true;
            // Get next entry
            if (_sourceEnumerator.MoveNext())
            {
                _current = _sourceEnumerator.Current;
                _isSatisfied = _partitioner(_current);
                return true;
            }
            else
            {
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public void Reset()
        {
            _sourceEnumerator.Reset();
            _satisfiedQueue.Clear();
            _falsifiedQueue.Clear();
            _isSatisfied = false;
            _current = default!;
            _dequeueOrderQueue.Clear();
            _dequeueOrderOffset = 1;
            _dequeueOrder = 0L;
        }

        private void Enqueue(T value, bool satisfied)
        {
            (satisfied ? _satisfiedQueue : _falsifiedQueue).Enqueue(value);
            if (_dequeueOrderOffset++ >= 56)
            {
                _dequeueOrderQueue.Enqueue(_dequeueOrder);
                _dequeueOrder = 0L;
                _dequeueOrderOffset = 1;
            }
            WriteBitAtOffset(satisfied);
        }

        private bool TryDequeue(bool satisfied)
        {
            return (satisfied ? _satisfiedQueue : _falsifiedQueue).TryDequeue(out _current);
        }

        private bool DeqeueAny()
        {
            if (_dequeueOrderOffset-- == 0 && !_dequeueOrderQueue.TryDequeue(out _dequeueOrder))
                return false;
            _isSatisfied = ReadBitAtOffsetAndLeftShift();
            return (_isSatisfied ? _satisfiedQueue : _falsifiedQueue).TryDequeue(out _current);
        }

        private unsafe void WriteBitAtOffset(bool satisfied)
        {
            // Mask bit at dequeue order offset with whether the value satisfied the condition or not
            fixed(ulong* dequeueOrderPtr = &_dequeueOrder)
            {
                *(byte*)(dequeueOrderPtr + _dequeueOrderOffset) &= (byte)(0xFF & (satisfied ? 0 : 1));
            }
        }

        private unsafe bool ReadBitAtOffsetAndLeftShift()
        {
            int result;
            fixed(ulong* dequeueOrderPtr = &_dequeueOrder)
            {
                result = *(byte*)(dequeueOrderPtr + _dequeueOrderOffset) & 0x01;
            }
            //Remove bit read
            _dequeueOrder <<= 1;
            return result == 0x01;
        }

        #region IDisposable support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _sourceEnumerator.Dispose();
                    _satisfiedQueue = null!;
                    _falsifiedQueue = null!;
                    _dequeueOrderQueue = null!;
                    _current = default!;
                    _sourceEnumerator.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
        #endregion
    }

    public class PartitionedEnumerable<T> : IPartitionedEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Predicate<T> _partitioner;

        internal PartitionedEnumerable(IEnumerable<T> source, Predicate<T> partitioner)
        {
            _source = source;
            _partitioner = partitioner;
        }

        public IPartitionEnumerator<T> GetEnumerator() => new PartitionEnumerator<T>(_source, _partitioner);

        public IEnumerator<T> GetFalsifiedEnumerator() => this.Where(o => !_partitioner(o)).GetEnumerator();

        public IEnumerator<T> GetSatisfiedEnumerator() => this.Where(o => _partitioner(o)).GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() =>  new PartitionEnumerator<T>(_source, _partitioner);

        IEnumerator IEnumerable.GetEnumerator() =>  new PartitionEnumerator<T>(_source, _partitioner);
    }
}