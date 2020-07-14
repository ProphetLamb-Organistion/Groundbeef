using Newtonsoft.Json;

using ProphetLamb.Tools.Core;

using System.Globalization;
using System.IO;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceWriter : System.Resources.IResourceWriter
    {
        private readonly CultureInfo culture;
        private readonly string resourceFileName;
        private readonly ResourceSet resourceSet;
        private StreamWriter writer;

        public ResourceWriter(in ResourceManager resourceManager, in CultureInfo resourceCulture)
        {
            culture = resourceCulture ?? CultureInfo.InvariantCulture;
            // Locate the associated resource file.
            resourceFileName = resourceManager.GetResourceFileName(culture);
            // Attempt to load resource from resource manage, then from file
            bool hasResource;
            if (!(hasResource = resourceManager.TryGetResourceSet(culture, out resourceSet))
             && File.Exists(resourceFileName))
            {
                // Attempt to read existing resource set.
                using var reader = new ResourceReader(resourceManager, culture);
                resourceSet = ResourceSet.FromDictionary(reader.ReadToEnd());
            }
            else
            {
                FileHelper.Create(resourceFileName).Dispose();
                resourceSet = new ResourceSet();
            }
            // Add resource set to manager
            if (!hasResource)
                resourceManager.AddResourceSet(culture, resourceSet);
        }

        public void AddResource(string name, byte[] value)
        {
            resourceSet.Add(name, value);
        }

        public void AddResource(string name, object value)
        {
            resourceSet.Add(name, value);
        }

        public void AddResource(string name, string value)
        {
            resourceSet.Add(name, value);
        }

        public void Close()
        {
            Generate();
            writer.Close();
        }

        public void Generate()
        {
            // Dispose existing stream; avoid collission from multiple writers flushing.
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            // Open file
            writer = new StreamWriter(resourceFileName, append: false);
            // Serialize to stream
            JsonSerializer serializer = JsonSerializer.CreateDefault();
            serializer.Culture = CultureInfo.InvariantCulture;
            serializer.Serialize(writer, resourceSet.ResourceTable, typeof(System.Collections.Generic.IDictionary<string, object>));
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
                    writer.Dispose();
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