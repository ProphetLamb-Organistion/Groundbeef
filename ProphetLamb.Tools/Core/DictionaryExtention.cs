using System.Collections;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ProphetLamb.Tools
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, in IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            if (keyValuePairs is null)
                throw new ArgumentNullException(nameof(keyValuePairs));
            foreach (KeyValuePair<TKey, TValue> kvpair in keyValuePairs)
                dictionary.Add(kvpair);
        }

        /// <summary>
        /// Enumerates the <see cref="KeyValuePair"/>s associated with the <paramref name="keys"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keys">The collection of <typeparamref name="TKey"/>s.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetMany<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys) where TKey : notnull
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            if (keys is null)
                throw new ArgumentNullException(nameof(keys));
            return from item in keys
                   select KeyValuePair.Create(item, dictionary[item]);
        }
    }

    /// <summary>
    /// Streamline add and remove methods on <see cref="ConcurrentDictionary{TKey, TValue}"/> with <see cref="Dictionary{TKey, TValue}"/>
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class ConcurrentDictionaryExtention
    {
        /// <summary>
        /// Adds the specified key and value to the <see cref="ConcurrentDictionary{TKey, TValue}"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in TKey key, TValue value) where TKey : notnull
        {
            dictionary.AddOrUpdate(key, value, (k, oldValue) => value);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="ConcurrentDictionary{TKey, TValue}"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePair">The key value pair to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in KeyValuePair<TKey, TValue> keyValuePair) where TKey : notnull
        {
            TValue value = keyValuePair.Value;
            dictionary.AddOrUpdate(keyValuePair.Key, value, (k, oldValue) => value);
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see cref="true"/> if the element is successfully found and removed; otherwise, <see cref="false"/>. This method returns <see cref="false"/> if key is not found in the <see cref="ConcurrentDictionary{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in TKey key) where TKey : notnull
        {
            return dictionary.TryRemove(key, out _);
        }

        /// <summary>
        /// Adds a range of <see cref="KeyValuePair"/>s to the <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePairs">The collection of <see cref="KeyValuePairs"/> to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            foreach(var kvpair in keyValuePairs)
                dictionary.AddOrUpdate(kvpair.Key, kvpair.Value, (k, oldValue) => kvpair.Value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>An enumerator that iterates through the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DictionaryEnumerator<TKey, TValue> GetDictionaryEnumerator<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryEnumerator<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An enumerator that iterates through the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
             enumerator.Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
