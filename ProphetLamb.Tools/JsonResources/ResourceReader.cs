using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System.IO;
using System;
using Newtonsoft.Json;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceReader : System.Resources.IResourceReader, IEnumerable<KeyValuePair<string, object>>
    {
        private readonly StreamReader reader;
        private IEnumerable<KeyValuePair<string, object>> resourceSetValue;

        /// <summary>
        /// Initializes a new intance of <see cref="ResourceReader"/>. To initially read the resource file specify readToEnd.
        /// </summary>
        /// <param name="resourceManager">The resource manager the resource file belongs to.</param>
        /// <param name="resourceCulture">The culture of the resource file.</param>
        public ResourceReader(in ResourceManager resourceManager, in CultureInfo resourceCulture) : this (resourceManager, resourceCulture, false) { }

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
            CultureInfo culture = resourceCulture??CultureInfo.InvariantCulture;
            string fileName = resourceManager.GetResourceFileName(culture);
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The resource .json-file does not exist on the device", fileName);
            reader = new StreamReader(fileName);
            if (readToEnd)
                ReadToEnd();
        }

        public IEnumerable<KeyValuePair<string, object>> ReadToEnd()
        {
            JsonSerializer serializer = JsonSerializer.CreateDefault();
            resourceSetValue = serializer.Deserialize(reader, ResourceSet.SerializedType) as IEnumerable<KeyValuePair<string, object>>;
            return resourceSetValue;
        }

        public void Close()
        {
            reader.Close();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            if (resourceSetValue is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetValue.GetEnumerator();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            if (resourceSetValue is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetValue.GetDictionaryEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (resourceSetValue is null)
                throw new InvalidOperationException("Call ReadToEnd before enumerating.");
            return resourceSetValue.GetEnumerator();
        }

        #region IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Close();
                resourceSetValue = null;
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