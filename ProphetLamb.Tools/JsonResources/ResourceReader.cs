using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using ProphetLamb.Tools.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceReader : System.Resources.IResourceReader, IEnumerable<KeyValuePair<string, object>>
    {
        private readonly StreamReader reader;
        private readonly ResourceManager resourceManager;
        private readonly CultureInfo culture;
        private IEnumerable<KeyValuePair<string, object>> resourceSetDictionary;

        /// <summary>
        /// Initializes a new intance of <see cref="ResourceReader"/>.
        /// </summary>
        /// <param name="resourceManager">The resource manager the resource file belongs to.</param>
        /// <param name="resourceCulture">The culture of the resource file.</param>
        public ResourceReader(in ResourceManager resourceManager, in CultureInfo resourceCulture) : this(resourceManager, resourceCulture, true) { }

        /// <summary>
        /// Initializes a new intance of <see cref="ResourceReader"/>.
        /// </summary>
        /// <param name="resourceManager">The resource manager the resource file belongs to.</param>
        /// <param name="resourceCulture">The culture of the resource file.</param>
        /// <param name="readToEnd">Whether to read the resource file to end or not.</param>
        public ResourceReader(in ResourceManager resourceManager, in CultureInfo resourceCulture, bool readToEnd)
        {
            if (resourceManager is null)
                throw new ArgumentNullException(nameof(resourceManager));
            this.resourceManager = resourceManager;
            culture = resourceCulture ?? CultureInfo.InvariantCulture;
            string fileName = resourceManager.GetResourceFileName(culture);
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The resource .json-file does not exist on the device", fileName);
            reader = new StreamReader(fileName);
            if (readToEnd)
                ReadToEnd();
        }

        /// <summary>
        /// Reads all data from the underlying stream. Required to enumerate.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> ReadToEnd()
        {
            // Deserialize the ResourceGroups
            JsonSerializer serializer = JsonSerializer.Create(ResourceGroupConverter.SettingsFactory(culture));
            var resourceGroups = serializer.Deserialize(reader, typeof(ResourceGroup[])) as ResourceGroup[];
            resourceSetDictionary = resourceGroups.AsParallel().SelectMany(x => x.ToDictionary());
            return resourceSetDictionary;
        }

        public void Close()
        {
            reader.Close();
            if (resourceSetDictionary != null)
            {
                // Add resource set to ResourceManager
                resourceManager.AddResourceSet(culture, ResourceSet.FromDictionary(resourceSetDictionary), true);
            }
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            if (resourceSetDictionary is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetDictionary.GetEnumerator();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            if (resourceSetDictionary is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetDictionary.GetDictionaryEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (resourceSetDictionary is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetDictionary.GetEnumerator();
        }

        #region IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Close();
                resourceSetDictionary = null;
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