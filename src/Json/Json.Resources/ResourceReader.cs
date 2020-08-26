using Groundbeef.Collections;
using Groundbeef.SharedResources;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Groundbeef.Json.Resources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceReader : System.Resources.IResourceReader, IEnumerable<KeyValuePair<string, object?>>
    {
        private readonly StreamReader _reader;
        private readonly CultureInfo _culture;
        private IEnumerable<KeyValuePair<string, object?>>? _resourceSetDictionary = null;

        /// <summary>
        /// Initializes a new intance of <see cref="ResourceReader"/>. Reads to end.
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
            _culture = resourceCulture ?? CultureInfo.InvariantCulture;
            string fileName = resourceManager.GetResourceFileName(_culture);
            if (!File.Exists(fileName))
                throw new FileNotFoundException(ExceptionResource.FILE_NOTONDEVICE, fileName);
            _reader = new StreamReader(fileName);
            if (readToEnd)
                ReadToEnd();
        }

        /// <summary>
        /// Reads all data from the underlying stream. Required to enumerate.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object?>> ReadToEnd()
        {
            Debug.Assert(_reader != null, "reader != null");
            if (!_reader.EndOfStream)
            {
                // Deserialize the ResourceGroups
                JsonSerializer serializer = JsonSerializer.Create(ResourceGroupConverter.SettingsFactory(_culture));
                if (serializer.Deserialize(_reader, typeof(ResourceGroup[])) is ResourceGroup[] resourceGroups)
                    _resourceSetDictionary = resourceGroups.AsParallel().SelectMany(x => x.ToDictionary());
            }
            if (_resourceSetDictionary is null)
                _resourceSetDictionary = new Dictionary<string, object?>();
            return _resourceSetDictionary;
        }

        public void Close()
        {
            _reader.Close();
        }

        IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
        {
            if (_resourceSetDictionary is null)
                throw new InvalidOperationException();
            return _resourceSetDictionary.GetEnumerator();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            if (_resourceSetDictionary is null)
                throw new InvalidOperationException(ExceptionResource.RESOURCEREADER_REQUIRE_READTOEND);
            return _resourceSetDictionary.GetDictionaryEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_resourceSetDictionary is null)
                throw new InvalidOperationException(ExceptionResource.RESOURCEREADER_REQUIRE_READTOEND);
            return _resourceSetDictionary.GetEnumerator();
        }

        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    Close();
                _resourceSetDictionary = null;
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}