using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections
{
    public interface IDictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        // Inherited from IEnumerator<KeyValuePair<TKey, TValue>>:
        // new KeyValuePair<TKey, TValue> Current { get; }

        // new TKey Key { get; }

        // new TValue Value { get; }
    }

    /// <summary>
    /// Generic implementation of the interface <see cref="IDictionaryEnumerator"/>.
    /// </summary>
    /// <typeparam name="TKey">The <see cref="Type"/> of the keys.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type"/> of the values.</typeparam>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator<TKey, TValue> where TKey : notnull
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

        /// <summary>
        /// Initializes a new instance of <see cref="DictionaryEnumerator{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The source dictionary</param>
        public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            _enumerator = dictionary.GetEnumerator();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DictionaryEnumerator{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The source enumerable</param>
        public DictionaryEnumerator(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            _enumerator = dictionary.GetEnumerator();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public DictionaryEntry Entry
        {
            get
            {
                (TKey key, TValue value) = _enumerator.Current;
                return new DictionaryEntry(key, value);
            }
        }

        public KeyValuePair<TKey, TValue> Current { get => _enumerator.Current; }

        object IEnumerator.Current => Entry;

        public TKey Key => _enumerator.Current.Key;

        public TValue Value => _enumerator.Current.Value;

        object IDictionaryEnumerator.Key => Key;

        [MaybeNull]
        object IDictionaryEnumerator.Value => Value;

        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}