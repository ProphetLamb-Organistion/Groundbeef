using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ProphetLamb.Tools.JsonResources
{
    internal class ResourceSet : IDisposable
    {
        private ConcurrentDictionary<string, object> resourceTable,
                                                     caseInsenstiveTable;
        private readonly string fileName;

        internal event EventHandler ResourceFileLoadedEvent;

        /// <summary>
        /// Initializes a new instance of ResourceSet.
        /// </summary>
        /// <param name="fileName">The full path the the resource .json file.</param>
        internal ResourceSet(in string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Indicates that the resource tables is loaded from the associated file.
        /// </summary>
        internal bool Loaded { get; private set; } = false;

        /// <summary>
        /// Determins wherther to throw a ArgumentException if the specified key was not found or the <see cref="ResourceSet"/> is currently unloaded.
        /// </summary>
        /// <value>If <see cref="true"/> throws a ArguemntException; otherwise, returns <see cref="null"/>.</value>
        public static bool ThrowExceptionOnResourceMiss { get; set; } = true;

        /// <summary>
        /// Cerates a task that loads or refreshes the <see cref="ResourceSet"/> by reading the resource tables from the associated file.
        /// </summary>
        /// <param name="refresh">When <see cref="true"/> refeshes the tables when already loaded; otherwise, loads tables only if not already loaded.</param>
        internal async Task Load(bool refresh = false)
        {
            if (!Loaded || refresh)
                await ReadResources();
        }

        /// <summary>
        /// Unloads the <see cref="ResourceSet"/> by clearing the resource tables.
        /// </summary>
        internal void Unload()
        {
            Loaded = false;
            lock(resourceTable)
                resourceTable = null;
            lock(caseInsenstiveTable)
                caseInsenstiveTable = null;
        }

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

        /// <summary>
        /// Adds the specified key and value to the tables. If the key already exists overwrites the exisiting value.
        /// </summary>
        /// <param name="keyValuePair">The key value pair to add.</param>
        internal void Add(in KeyValuePair<string, object> keyValuePair)
        {
            if (String.IsNullOrWhiteSpace(keyValuePair.Key))
                throw new ArgumentException($"'{nameof(keyValuePair.Key)}' cannot be null or whitespace", nameof(keyValuePair));
            if (keyValuePair.Value is null)
                throw new ArgumentNullException(nameof(keyValuePair.Value));
            resourceTable.Add(keyValuePair);
            caseInsenstiveTable.Add(keyValuePair.Key.ToUpperInvariant(), keyValuePair.Value);
        }

        private object InternalGetObject(in string key, bool ignoreCase)
        {
            IDictionary<string, object> resources = null;
            string iKey = null;
            if (Loaded)
            {
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
            }
            if (!Loaded || (resources.TryGetValue(iKey, out object value) && ThrowExceptionOnResourceMiss))
                throw new ArgumentException("The key is not present in the dictionary or the ResourceSet is not loaded.");
            return value;
        }

        internal Task<string> WriteResources()
        {
            return Task.Run(() => JsonConvert.SerializeObject(resourceTable.AsEnumerable()));
        }

        private async Task ReadResources()
        {
            Loaded = false;
            IEnumerable<KeyValuePair<string, object>> kvpEnu;
            using (var sr = new StreamReader(fileName))
                kvpEnu = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, object>>>(await sr.ReadToEndAsync());
            lock(resourceTable)
                resourceTable = new ConcurrentDictionary<string, object>(kvpEnu);
            lock(caseInsenstiveTable)
                caseInsenstiveTable = new ConcurrentDictionary<string, object>(resourceTable.Select(kvp => new KeyValuePair<string, object>(kvp.Key.ToUpperInvariant(), kvp.Value)));
            Loaded = true;
            ResourceFileLoadedEvent?.Invoke(this, EventArgs.Empty);
        }

        #region  IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Unload();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
        #endregion
    }
}