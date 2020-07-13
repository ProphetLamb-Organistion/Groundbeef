using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ProphetLamb.Tools.Localisation.JsonResources
{
    internal class ResourceSet : IDisposable
    {
        private ConcurrentDictionary<string, object> resourceTable,
                                                     caseInsenstiveTable;
        private readonly string assFileName;

        internal event EventHandler ResourceFileLoadedEvent;

        internal ResourceSet(string fileName)
        {
            assFileName = fileName;
        }

        /// <summary>
        /// Indicates that the resource tables is loaded from the associated file.
        /// </summary>
        internal bool Loaded { get; private set; } = false;

        internal static bool ThrowExceptionOnResourceMiss { get; set; } = true;

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
        internal string GetString(string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase) is string str ? str : throw new InvalidCastException("The object at the key is no string.");
        }

        /// <summary>
        /// Returns the object associated with the <paramref cref="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        internal object GetObject(string key, bool ignoreCase = false)
        {
            return InternalGetObject(key, ignoreCase);
        }

        protected object InternalGetObject(string key, bool ignoreCase)
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

        private async Task ReadResources()
        {
            Loaded = false;
            IEnumerable<KeyValuePair<string, object>> kvpEnu;
            using (var sr = new StreamReader(assFileName))
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