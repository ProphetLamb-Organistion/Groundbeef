using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections
{
    /// <summary>
    /// Represents a bi-directionally accessible dictionary implementation of <typeparam name="TForward"/> and <typeparam name="TReverse"/>.
    /// </summary>
    /// <typeparam name="TForward">The forward key, and reverse value type of the <see cref="Map"/>.</typeparam>
    /// <typeparam name="TReverse">The forward value, and reverse key type of the <see cref="Map"/>.</typeparam>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class Map<TForward, TReverse> : IEnumerable<KeyValuePair<TForward, TReverse>> where TForward : notnull where TReverse : notnull
    {
        private readonly Dictionary<TForward, TReverse> _forward;
        private readonly Dictionary<TReverse, TForward> _reverse;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map(IEqualityComparer<TForward> forwardComparer, IEqualityComparer<TReverse> reverseComparer)
        {
            _forward = new Dictionary<TForward, TReverse>(forwardComparer);
            _reverse = new Dictionary<TReverse, TForward>(reverseComparer);
            Forward = new Indexer<TForward, TReverse>(_forward);
            Reverse = new Indexer<TReverse, TForward>(_reverse);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="Map"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="value1">The forward key, and reverse value.</param>
        /// <param name="value2">The forward value, and reverse key.</param>
        public void Add(in TForward value1, in TReverse value2)
        {
            _forward.Add(value1, value2);
            _reverse.Add(value2, value1);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="Map"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="value">The <see cref="KeyValuePair{TForward,TReverse}"/>.</param>
        public void Add(in KeyValuePair<TForward, TReverse> value)
        {
            _forward.Add(value);
            _reverse.Add(value.Value, value.Key);
        }

        /// <summary>
        /// Gets the indexer with the <typeparam name="TForward"/> key and <typeparam name="TReverse"/> value.
        /// </summary>
        public Indexer<TForward, TReverse> Forward { get; }

        /// <summary>
        /// Gets the indexer with the <typeparam name="TReverse"/> key and <typeparam name="TForward"/> value.
        /// </summary>
        public Indexer<TReverse, TForward> Reverse { get; }

        public IEnumerator<KeyValuePair<TForward, TReverse>> GetEnumerator() => _forward.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _forward.GetEnumerator();
    }

    /// <summary>
    /// Represents a wrapper class of <see cref="Dictionary{TKey, TValue}"/> tailored for the needs of <see cref="Map"/>.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class Indexer<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Indexer"> class that contains elements copied from the specified <see cref="Dictionary{TKey, TValue}">
        /// </summary>
        /// <param name="dictionary">The <see cref="Dictionary{TKey, TValue}"> whose elements are copied to the new <see cref="Indexer">.</param>
        internal Indexer(in Dictionary<TKey, TValue> dictionary)
        {
            _storage = dictionary;
        }

        public TValue this[TKey index]
        {
            get { return _storage[index]; }
            set { _storage[index] = value; }
        }

        public IEnumerable<TKey> Keys => _storage.Keys;

        public IEnumerable<TValue> Values => _storage.Values;

        public int Count => _storage.Count;

        public bool ContainsKey(TKey key) => _storage.ContainsKey(key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _storage.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _storage.GetEnumerator();
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _storage.GetEnumerator();
    }
}