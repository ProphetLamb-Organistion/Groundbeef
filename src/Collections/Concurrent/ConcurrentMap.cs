using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections.Concurrent
{
    /// <summary>
    /// Represents a bi-directionally accessible, threadsafe dictionary implementation of <typeparam name="TForward"/> and <typeparam name="TReverse"/>.
    /// </summary>
    /// <typeparam name="TForward">The forward key, and reverse value type of the <see cref="ConcurrentMap"/>.</typeparam>
    /// <typeparam name="TReverse">The forward value, and reverse key type of the <see cref="ConcurrentMap"/>.</typeparam>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ConcurrentMap<TForward, TReverse> : IEnumerable<KeyValuePair<TForward, TReverse>> where TForward : notnull where TReverse : notnull
    {
        private readonly ConcurrentDictionary<TForward, TReverse> forward = new ConcurrentDictionary<TForward, TReverse>();
        private readonly ConcurrentDictionary<TReverse, TForward> reverse = new ConcurrentDictionary<TReverse, TForward>();

        /// <summary>
        /// Initializes a new instance of <see cref="ConcurrentMap"/> class.
        /// </summary>
        public ConcurrentMap()
        {
            Forward = new ConcurrentIndexer<TForward, TReverse>(forward);
            Reverse = new ConcurrentIndexer<TReverse, TForward>(reverse);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="ConcurrentMap"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="value1">The forward key, and reverse value.</param>
        /// <param name="value2">The forward value, and reverse key.</param>
        public void Add(in TForward value1, in TReverse value2)
        {
            forward.Add(value1, value2);
            reverse.Add(value2, value1);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="ConcurrentMap"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="value">The <see cref="KeyValuePair{TForward,TReverse}"/>.</param>
        public void Add(in KeyValuePair<TForward, TReverse> value)
        {
            forward.Add(value);
            reverse.Add(value.Value, value.Key);
        }

        /// <summary>
        /// Gets the threadsafe indexer with the <typeparam name="TForward"/> key and <typeparam name="TReverse"/> value.
        /// </summary>
        public ConcurrentIndexer<TForward, TReverse> Forward { get; }

        /// <summary>
        /// Gets the threadsafe indexer with the <typeparam name="TReverse"/> key and <typeparam name="TForward"/> value.
        /// </summary>
        public ConcurrentIndexer<TReverse, TForward> Reverse { get; }

        public IEnumerator<KeyValuePair<TForward, TReverse>> GetEnumerator() => forward.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => forward.GetEnumerator();
    }

    /// <summary>
    /// Represents a threadsafe wrapper class of <see cref="ConcurrentDictionary{TKey, TValue}"/> tailored for the needs of <see cref="ConcurrentMap"/>.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ConcurrentIndexer<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Indexer"> class that contains elements copied from the specified <see cref="ConcurrentDictionary{TKey, TValue}">
        /// </summary>
        /// <param name="dictionary">The <see cref="ConcurrentDictionary{TKey, TValue}"> whose elements are copied to the new <see cref="Indexer">.</param>
        internal ConcurrentIndexer(in ConcurrentDictionary<TKey, TValue> dictionary)
        {
            storage = dictionary;
        }

        public TValue this[TKey index]
        {
            get { return storage[index]; }
            set { storage[index] = value; }
        }

        public IEnumerable<TKey> Keys => storage.Keys;

        public IEnumerable<TValue> Values => storage.Values;

        public int Count => storage.Count;

        public bool ContainsKey(TKey key) => storage.ContainsKey(key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => storage.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => storage.GetEnumerator();
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => storage.GetEnumerator();
    }
}