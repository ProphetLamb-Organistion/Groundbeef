using Groundbeef.Collections.BitCollections;
using Groundbeef.SharedResources;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections
{
    public interface IPartitionedEnumerator<T> : IEnumerator<T>
    {
        bool IsSatisfied { get; }
        bool MoveNextSatisfied();
        bool MoveNextFalsified();
        IEnumerator<T> SynchronizedSatisfiedEnumerator();
        IEnumerator<T> SynchronizedFalisifiedEnumerator();
    }

    public interface IPartitionedEnumerable<T> : IEnumerable<T>
    {
        new IPartitionedEnumerator<T> GetEnumerator();
    }

    /// <summary>
    /// Enumerates a sequence, partitioning elements based on a condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PartitionedEnumerator<T> : IPartitionedEnumerator<T>
    {
        private readonly IEnumerator<T> _sourceEnumerator;
        private Queue<T> _satisfiedQueue = new Queue<T>(),
                         _falsifiedQueue = new Queue<T>();
        private readonly Predicate<T> _partitioner;
        private bool _isSatisfied;
        private T _current = default!;
        private BitList _queue = new BitList();
        private sbyte _state = -1; // -1 := No state; 0 := Normal; -2 := Failure, Disposed or Ended.
        private SyncronizedSinglePartitionEnumerator<T>? _falsifiedEn,
                                                         _satisfiedEn;

        internal PartitionedEnumerator(in IEnumerable<T> source, in Predicate<T> partitioner)
        {
            _sourceEnumerator = source.GetEnumerator();
            _partitioner = partitioner;
        }

        public bool IsSatisfied => _isSatisfied;

        public T Current
        {
            get
            {
                if (_state != 0)
                    throw new InvalidOperationException(ExceptionResource.ENUMERATOR_STATE_ABNORMAL);
                return _current;
            }
        }

        [MaybeNull]
        object IEnumerator.Current => Current;

        internal sbyte State => _state;

        public bool MoveNextSatisfied()
        {
            if (_state == -2)
                return false;
            // Attempt to get queued entry
            if (TryDequeue(true))
                return true;
            // Seek next satisfied
            bool result;
            while ((result = _sourceEnumerator.MoveNext()) && !_partitioner(_sourceEnumerator.Current))
                Enqueue(_sourceEnumerator.Current, false);
            if (result)
            {
                _state = 0;
                _current = _sourceEnumerator.Current;
                _isSatisfied = true;
                return true;
            }
            else
            {
                _state = -2;
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public bool MoveNextFalsified()
        {
            if (_state == -2)
                return false;
            // Attempt to get queued entry
            if (TryDequeue(false))
                return true;
            // Seek next satisfied
            bool result;
            while ((result = _sourceEnumerator.MoveNext()) && _partitioner(_sourceEnumerator.Current))
                Enqueue(_sourceEnumerator.Current, true);
            if (result)
            {
                _state = 0;
                _current = _sourceEnumerator.Current;
                _isSatisfied = false;
                return true;
            }
            else
            {
                _state = -2;
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public bool MoveNext()
        {
            if (_state == -2)
                return false;
            //Attempt to get queued entry
            if (TryDeqeueAny())
                return true;
            // Get next entry
            if (_sourceEnumerator.MoveNext())
            {
                _state = 0;
                _current = _sourceEnumerator.Current;
                _isSatisfied = _partitioner(_current);
                return true;
            }
            else
            {
                _state = -2;
                _current = default!;
                _isSatisfied = false;
                return false;
            }
        }

        public IEnumerator<T> SynchronizedSatisfiedEnumerator()
        {
            if (_satisfiedEn is null || _satisfiedEn.State == -2)
                return _satisfiedEn = new SyncronizedSinglePartitionEnumerator<T>(this, true);
            return _satisfiedEn;
        }

        public IEnumerator<T> SynchronizedFalisifiedEnumerator()
        {
            if (_falsifiedEn is null || _falsifiedEn.State == -2)
                return _falsifiedEn = new SyncronizedSinglePartitionEnumerator<T>(this, true);
            return _falsifiedEn;
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
            _queue.Add(satisfied);
        }

        private bool TryDequeue(bool satisfied)
        {
            if (!satisfied || !_queue.Remove(satisfied))
                return false;
            _current = (satisfied ? _satisfiedQueue : _falsifiedQueue).Dequeue();
            _isSatisfied = satisfied;
            return true;
        }

        private bool TryDeqeueAny()
        {
            if (_queue.IsEmpty)
                return false;
            _isSatisfied = _queue[0];
            _queue.RemoveAt(0);
            _current = (_isSatisfied ? _satisfiedQueue : _falsifiedQueue).Dequeue();
            return true;
        }

        #region IDisposable members
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _state = -2;
                    _sourceEnumerator.Dispose();
                    _satisfiedQueue = null!;
                    _falsifiedQueue = null!;
                    _queue = null!;
                    _current = default!;
                    _sourceEnumerator.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
        #endregion

        #region Syncronized
        private class SyncronizedSinglePartitionEnumerator<T2> : IEnumerator<T2>
        {
            private readonly PartitionedEnumerator<T2> _syncEn;
            private readonly bool _partition;
            private sbyte _state = -1;

            public SyncronizedSinglePartitionEnumerator(PartitionedEnumerator<T2> enumerator, bool partition)
            {
                _syncEn = enumerator;
                _partition = partition;
            }

            public T2 Current
            {
                get
                {
                    if (_state != 0)
                        throw new InvalidOperationException(ExceptionResource.ENUMERATOR_STATE_ABNORMAL);
                    return Current;
                }
            }

            [MaybeNull]
            object IEnumerator.Current => Current;

            internal sbyte State => _state;

            public bool MoveNext()
            {
                lock (_syncEn)
                {
                    if (_state == -2)
                        return false;
                    _state = _syncEn.State;
                    return _partition ? _syncEn.MoveNextSatisfied() : _syncEn.MoveNextFalsified();
                }
            }

            public void Reset()
            {
                lock (_syncEn)
                    _syncEn.Reset();
            }

            #region IDisposable members
            private bool _disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposedValue)
                {
                    if (disposing)
                    {
                        _state = -2;
                    }
                    _disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(disposing: true);
            }
            #endregion
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}