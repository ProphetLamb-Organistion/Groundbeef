using System;
using System.Collections.Concurrent;

namespace ProphetLamb.Tools
{
    /// <summary>
    /// Streamline add and remove methods on <see cref="ConcurrentDictionary{TKey, TValue}"/> with <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>
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
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.AddOrUpdate(key, value, (k, oldValue) => value);
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see cref="true"/> if the element is successfully found and removed; otherwise, <see cref="false"/>. This method returns <see cref="false"/> if key is not found in the <see cref="ConcurrentDictionary{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryRemove(key, out _);
        }
    }
}
