using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections
{
    public interface IPartitionedEnumerator<T> : IEnumerator<T>
    {
        bool MoveNextSatisfied();
        bool MoveNextFalsified();
        bool IsSatisfied { get; }
    }

    public interface IPartitionedEnumerable<T> : IEnumerable<T>
    {
        new IPartitionedEnumerator<T> GetEnumerator();
    }

    public enum PartitionedEnumeratorOperatingMode
    {
        Undefinded,
        Partitioned,
        Mixed,
    }

    public class PartitionedEnumerator<T> : IPartitionedEnumerator<T>
    {
        private readonly IEnumerator<T> _sourceEnumerator;
        private Queue<T> _satisfiedQueue = new Queue<T>(),
                         _falsifiedQueue = new Queue<T>();
        private readonly Predicate<T> _partitioner;
        private bool _isSatisfied;
        private T _current = default!;
        private PartitionedEnumeratorOperatingMode _operatingMode;
        private BitQueue _queue = new BitQueue();

        internal PartitionedEnumerator(in IEnumerable<T> source, in Predicate<T> partitioner)
        {
            _sourceEnumerator = source.GetEnumerator();
            _partitioner = partitioner;
        }

        public bool IsSatisfied => _isSatisfied;

        public T Current => _current;

        [MaybeNull]
        object IEnumerator.Current => _current;

        public PartitionedEnumeratorOperatingMode OperatingMode => _operatingMode;

        public bool MoveNextSatisfied()
        {
            ProtectOperatingMode(false);
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
            ProtectOperatingMode(false);
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
            ProtectOperatingMode(true);
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

        private void ProtectOperatingMode(bool mixed)
        {
            if (_operatingMode == PartitionedEnumeratorOperatingMode.Undefinded)
                _operatingMode = mixed ? PartitionedEnumeratorOperatingMode.Mixed : PartitionedEnumeratorOperatingMode.Partitioned;
            else if (_operatingMode != (mixed ? PartitionedEnumeratorOperatingMode.Mixed : PartitionedEnumeratorOperatingMode.Partitioned))
                throw new NotSupportedException("This operation is not supported in the current operating mode.");
        }

        public void Reset()
        {
            _sourceEnumerator.Reset();
            _satisfiedQueue.Clear();
            _falsifiedQueue.Clear();
            _isSatisfied = false;
            _current = default!;
            _queue.Clear();
        }

        private void Enqueue(T value, bool satisfied)
        {
            (satisfied ? _satisfiedQueue : _falsifiedQueue).Enqueue(value);
            _queue.Enqueue(satisfied);
        }

        private bool TryDequeue(bool satisfied)
        {
           return (satisfied ? _satisfiedQueue : _falsifiedQueue).TryDequeue(out _current);
        }

        private bool DeqeueAny()
        {
            if (_queue.IsEmpty)
                return false;
            _isSatisfied = _queue.Dequeue();
            _current = (_isSatisfied ? _satisfiedQueue : _falsifiedQueue).Dequeue();
            return true;
        }

        #region IDisposable members
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
                    _queue = null!;
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

        public IPartitionedEnumerator<T> GetEnumerator() => new PartitionedEnumerator<T>(_source, _partitioner);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() =>  new PartitionedEnumerator<T>(_source, _partitioner);

        IEnumerator IEnumerable.GetEnumerator() =>  new PartitionedEnumerator<T>(_source, _partitioner);
    }
}