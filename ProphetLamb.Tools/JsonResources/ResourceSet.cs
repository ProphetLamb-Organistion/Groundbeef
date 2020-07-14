using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Collections;

namespace ProphetLamb.Tools.JsonResources
{
    internal class ResourceSet : IDisposable, IEnumerable<KeyValuePair<string, object>>
    {
        internal static readonly Type SerializedType = typeof(IEnumerable<KeyValuePair<string, object>>);
        private ConcurrentDictionary<string, object> resourceTable = new ConcurrentDictionary<string, object>(),
                                                     caseInsenstiveTable = new ConcurrentDictionary<string, object>();

        internal ResourceSet() { }
        internal ResourceSet(IEnumerable<KeyValuePair<string, object>> dictionary)
        {
            foreach((string key, object value) in dictionary)
                Add(key, value);
        }

        /// <summary>
        /// Determins wherther to throw a ArgumentException if the specified key was not found or the <see cref="ResourceSet"/> is currently unloaded.
        /// </summary>
        /// <value>If <see cref="true"/> throws a ArguemntException; otherwise, returns <see cref="null"/>.</value>
        public static bool ThrowExceptionOnResourceMiss { get; set; } = true;

        /// <summary>
        /// Returns the string associated with the <paramref cref="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        internal string GetString(in string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase) is string str ? str : throw new InvalidCastException("The object at the key is no string.");
        }

        /// <summary>
        /// Returns the object associated with the <paramref cref="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        internal object GetObject(in string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase);
        }

        /// <summary>
        /// Adds the specified key and value to the tables. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can not be null.</param>
        internal void Add(in string key, in object value)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace", nameof(key));
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            resourceTable.Add(key, value);
            caseInsenstiveTable.Add(key.ToUpperInvariant(), value);
        }

        internal IEnumerable<KeyValuePair<string, object>> ResourceTable { get => resourceTable.AsEnumerable(); }

        private object InternalGetObject(in string key, bool ignoreCase)
        {
            string iKey;
            IDictionary<string, object> resources;
            if (ignoreCase)
            {
                resources = caseInsenstiveTable;
                iKey = key.ToUpperInvariant();
            }
            else
            {
                resources = resourceTable;
                iKey = key;
            }
            if (resources.TryGetValue(iKey, out object value) && ThrowExceptionOnResourceMiss)
                throw new ArgumentException("The key is not present in the dictionary or the ResourceSet is not loaded.");
            return value;
        }

        #region  IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                resourceTable = null;
                caseInsenstiveTable = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return resourceTable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return resourceTable.GetEnumerator();
        }
        #endregion
    }
}