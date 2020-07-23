using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ProphetLamb.Tools.Collections.Concurrent
{
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
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in TKey key, TValue value) where TKey : notnull
        {
            dictionary.AddOrUpdate(key, value, (_, __) => value);
        }

        /// <summary>
        /// Adds the specified key and value to the <see cref="ConcurrentDictionary{TKey, TValue}"/>. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePair">The key value pair to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in KeyValuePair<TKey, TValue> keyValuePair) where TKey : notnull
        {
            TValue value = keyValuePair.Value;
            dictionary.AddOrUpdate(keyValuePair.Key, value, (_, __) => value);
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see cref="true"/> if the element is successfully found and removed; otherwise, <see cref="false"/>. This method returns <see cref="false"/> if key is not found in the <see cref="ConcurrentDictionary{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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
        public static void AddRange<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, in IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            foreach (var kvpair in keyValuePairs)
                dictionary.AddOrUpdate(kvpair.Key, kvpair.Value, (_, __) => kvpair.Value);
        }
    }
}