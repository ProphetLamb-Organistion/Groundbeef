using System;
using System.Collections;
using System.Collections.Generic;

namespace Groundbeef.Json.Resources
{
    public class ResourceSet : IDisposable, IEnumerable<KeyValuePair<string, object?>>
    {
        private readonly Dictionary<string, object?> _resourceTable = new Dictionary<string, object?>(),
                                                    _caseInsenstiveTable = new Dictionary<string, object?>();

        public ResourceSet() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ResourceSet"/>.
        /// </summary>
        /// <param name="reader">The resource reader to read the resource from.</param>
        public ResourceSet(System.Resources.IResourceReader reader)
        {
            IDictionaryEnumerator en = reader.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key is string key)
                    Add(key, en.Value);
                else
                    throw new InvalidCastException(ExceptionResource.STRING_NULLEMPTY);
            }
        }

        /// <summary>
        /// Determins wherther to throw a ArgumentException if the specified key was not found or the <see cref="ResourceSet"/>.
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
            return InternalGetObject(key, ignoreCase) is string str ? str : throw new InvalidCastException(ExceptionResource.STRING_NULLEMPTY);
        }

        /// <summary>
        /// Returns the object associated with the <paramref cref="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public object? GetObject(in string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase);
        }

        /// <summary>
        /// Adds the specified key and value to the tables. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can not be null.</param>
        public void Add(in string key, in object? value)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentException(ExceptionResource.STRING_NULLWHITESPACE, nameof(key));
            _resourceTable.Add(key, value);
            _caseInsenstiveTable.Add(key.ToUpperInvariant(), value);
        }

        private object? InternalGetObject(in string key, bool ignoreCase)
        {
            string l_key;
            Dictionary<string, object?> resources;
            if (ignoreCase)
            {
                resources = _caseInsenstiveTable;
                l_key = key.ToUpperInvariant();
            }
            else
            {
                resources = _resourceTable;
                l_key = key;
            }
            if (!resources.TryGetValue(l_key, out object? value) && ThrowExceptionOnResourceMiss)
                throw new ArgumentException(ExceptionResource.RESOURCESET_RESOURCEMISS);
            return value;
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return _resourceTable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resourceTable.GetEnumerator();
        }

        #region  IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}