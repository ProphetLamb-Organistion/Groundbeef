using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProphetLamb.Tools.Collections
{
    /// <summary>
    /// Adds additional functionallity to implementations of <see cref="IDictionary"/>.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class DictionaryExtention
    {
        /// <summary>
        /// Adds a range of <see cref="KeyValuePair"/>s to the <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePairs">The collection of <see cref="KeyValuePairs"/> to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, in IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            if (dictionary is System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>)
                throw new NotSupportedException(ExceptionResource.DICTIONARY_CONCURRENT_NOTSUPPORTED);
            if (keyValuePairs is null)
                throw new ArgumentNullException(nameof(keyValuePairs));
            foreach (KeyValuePair<TKey, TValue> kvpair in keyValuePairs)
                dictionary.Add(kvpair);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePair">The key value pair to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, in KeyValuePair<TKey, TValue> keyValuePair) where TKey : notnull
        {
            if (dictionary is System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>)
                throw new NotSupportedException(ExceptionResource.DICTIONARY_CONCURRENT_NOTSUPPORTED);
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// Enumerates the <see cref="KeyValuePair"/>s associated with the <paramref name="keys"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keys">The collection of <typeparamref name="TKey"/>s.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetMany<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys) where TKey : notnull
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            if (keys is null)
                throw new ArgumentNullException(nameof(keys));
            return from item in keys
                   select KeyValuePair.Create(item, dictionary[item]);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An enumerator that iterates through the collection.</returns>
        public static DictionaryEnumerator<TKey, TValue> GetDictionaryEnumerator<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            return new DictionaryEnumerator<TKey, TValue>(enumerable);
        }
    }

    /// <summary>
    /// Generic implementation of the interface <see cref="IDictionaryEnumerator"/>.
    /// </summary>
    /// <typeparam name="TKey">The <see cref="Type"/> of the keys.</typeparam>
    /// <typeparam name="TValue">The <see cref="Type"/> of the values.</typeparam>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IDisposable where TKey : notnull
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

        /// <summary>
        /// Initializes a new instance of <see cref="DictionaryEnumerator{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The source dictionary</param>
        public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            enumerator = dictionary.GetEnumerator();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DictionaryEnumerator{TKey,TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The source enumerable</param>
        public DictionaryEnumerator(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            enumerator = dictionary.GetEnumerator();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        public DictionaryEntry Entry
        {
            get
            {
                (TKey key, TValue value) = enumerator.Current;
                return new DictionaryEntry(key, value);
            }
        }

        public object Current { get => Entry; }

        public object Key { get => enumerator.Current.Key; }

        public object Value { get => enumerator.Current.Value; }

        public void Dispose()
        {
            enumerator.Dispose();
        }
    }
}
