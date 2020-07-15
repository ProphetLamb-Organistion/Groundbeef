using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ProphetLamb.Tools.Collections.Concurrent;

namespace ProphetLamb.Tools.JsonResources
{
    internal class ResourceSet : IDisposable, IEnumerable<KeyValuePair<string, object>>
    {
        private ConcurrentDictionary<string, object> resourceTable = new ConcurrentDictionary<string, object>(),
                                                     caseInsenstiveTable = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// Creates a new instance of <see cref="ResourceSet"/> from a <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The data source dictionary for the new <see cref="ResourceSet"/>.</param>
        /// <returns>A new instance of <see cref="ResourceSet"/> from a <paramref name="dictionary"/>.</returns>
        public static ResourceSet FromDictionary(in IEnumerable<KeyValuePair<string, object>> dictionary)
        {
            var resourceSet = new ResourceSet();
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            foreach ((string key, object value) in dictionary)
                resourceSet.Add(key, value);
            return resourceSet;
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
        public string GetString(in string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase) is string str ? str : throw new InvalidCastException("The object at the key is no string.");
        }

        /// <summary>
        /// Returns the object associated with the <paramref cref="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public object GetObject(in string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase);
        }

        /// <summary>
        /// Adds the specified key and value to the tables. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can not be null.</param>
        public void Add(in string key, in object value)
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
            ConcurrentDictionary<string, object> resources;
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
            if (!resources.TryGetValue(iKey, out object value) && ThrowExceptionOnResourceMiss)
                throw new ArgumentException("The key is not present in the dictionary or the ResourceSet is not loaded.");
            return value;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return resourceTable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return resourceTable.GetEnumerator();
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
        #endregion
    }
}