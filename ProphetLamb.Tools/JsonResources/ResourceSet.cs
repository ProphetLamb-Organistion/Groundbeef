using System;
using System.Collections;
using System.Collections.Generic;

namespace ProphetLamb.Tools.JsonResources
{
    internal class ResourceSet : IDisposable, IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> resourceTable = new Dictionary<string, object>(),
                                           caseInsenstiveTable = new Dictionary<string, object>();

        public ResourceSet() {}

        /// <summary>
        /// Initializes a new instance of <see cref="ResourceSet"/>.
        /// </summary>
        /// <param name="reader">The resource reader to read the resource from.</param>
        public ResourceSet(System.Resources.IResourceReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            IDictionaryEnumerator en = reader.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key is string key)
                    Add(key, en.Value);
                else
                    throw new InvalidCastException("One or more keys in the dictonary are null or not a string.");
            }
        }

        /// <summary>
        /// Determins wherther to throw a ArgumentException if the specified key was not found or the <see cref="ResourceSet"/> is currently unloaded.
        /// </summary>
        /// <value>If <see cref="true"/> throws a ArguemntException; otherwise, returns <see cref="null"/>.</value>
        public bool ThrowExceptionOnResourceMiss { get; set; } = true;

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

        private object InternalGetObject(in string key, bool ignoreCase)
        {
            string iKey;
            Dictionary<string, object> resources;
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