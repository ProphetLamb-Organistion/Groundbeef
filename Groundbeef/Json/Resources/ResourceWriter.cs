using System;
using System.Globalization;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Groundbeef.IO;


namespace Groundbeef.Json.Resources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceWriter : System.Resources.IResourceWriter
    {
        private readonly CultureInfo _culture;
        private readonly string _resourceFileName;
        private readonly ResourceSet _resourceSet;
        private StreamWriter? _writer = null!;

        public ResourceWriter(in ResourceManager resourceManager, in CultureInfo resourceCulture)
        {
            _culture = resourceCulture ?? CultureInfo.InvariantCulture;
            // Locate the associated resource file.
            _resourceFileName = resourceManager.GetResourceFileName(_culture);
            // Attempt to load resource from resource manage, then from file
            if (resourceManager.TryGetResourceSet(_culture, out ResourceSet? resourceSet))
            {
                _resourceSet = resourceSet??throw new InvalidOperationException(); // Exception is unreachable, but keeps the compiler happy.
            }
            else if (File.Exists(_resourceFileName))
            {
                // Attempt to read existing resource set.
                using var reader = new ResourceReader(resourceManager, _culture);
                _resourceSet = new ResourceSet(reader);
                resourceManager.AddResourceSet(_culture, _resourceSet);
            }
            else
            {
                // Add new resource set to manager
                FileHelper.Create(_resourceFileName).Dispose();
                _resourceSet = new ResourceSet();
                resourceManager.AddResourceSet(_culture, _resourceSet);
            }
        }

        public void AddResource(string name, byte[] value)
        {
            _resourceSet.Add(name, value);
        }

        public void AddResource(string name, object value)
        {
            _resourceSet.Add(name, value);
        }

        public void AddResource(string name, string value)
        {
            _resourceSet.Add(name, value);
        }

        public void Close()
        {
            Generate();
            _writer?.Close();
        }

        public void Generate()
        {
            // Dispose existing stream; avoid collission from multiple writers flushing.
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
            lock (_resourceSet)
            {
                _writer = new StreamWriter(_resourceFileName, append: false);
                // Generate ResourceGroups form resoruceSet
                ResourceGroup[] resourceGroups = _resourceSet.GroupBy(kvp => kvp.Value?.GetType()??typeof(object))
                    .Select(grouping => new ResourceGroup(grouping.Key, grouping.Select(kvp => kvp.Key).ToList(), grouping.Select(kvp => kvp.Value).ToList())).ToArray();
                // Open file
                using var jsonWriter = new JsonTextWriter(_writer);
                // Create a serializer with the ResourceGroupConverter specific settings for the current culture.
                JsonSerializer serializer = JsonSerializer.Create(ResourceGroupConverter.SettingsFactory(_culture));
                // Serialize the resourceGroups to the stream.
                //resourceGroups.Wait();
                serializer.Serialize(jsonWriter, resourceGroups);
            }
        }

        #region IDisposable support
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                    _writer?.Dispose();
                }
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