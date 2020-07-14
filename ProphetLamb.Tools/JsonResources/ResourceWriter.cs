using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using System;
using ProphetLamb.Tools.Core;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceWriter : System.Resources.IResourceWriter
    {
        private readonly ResourceManager resourceManager;
        private readonly CultureInfo culture;
        private readonly string resourceFileName;
        private readonly ResourceSet resourceSet;
        private StreamWriter writer;

        public ResourceWriter(in ResourceManager resourceManager, in CultureInfo resourceCulture)
        {
            culture = resourceCulture??CultureInfo.InvariantCulture;
            this.resourceManager = resourceManager;
            // Locate the associated resource file.
            resourceFileName = resourceManager.GetResourceFileName(culture);
            // Attempt to load resource from resource manage, then from file
            bool hasResource;
            if (!(hasResource = resourceManager.TryGetResourceSet(culture, out resourceSet))
             && File.Exists(resourceFileName))
            {
                // Attempt to read existing resource set.
                using var reader = new ResourceReader(resourceManager, culture);
                resourceSet = new ResourceSet(reader.ReadToEnd());
            }
            else
            {
                FileHelper.Create(resourceFileName).Dispose();
                resourceSet = new ResourceSet();
            }
            // Add resource set to manager
            if (!hasResource)
                resourceManager.AddResourceSet(culture, resourceSet);
            // Open file
            writer = new StreamWriter(resourceFileName, append: false);
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
            JsonSerializer serializer = JsonSerializer.CreateDefault();
            serializer.Culture = CultureInfo.InvariantCulture;
            serializer.Serialize(writer, resourceSet.ResourceTable, ResourceSet.SerializedType);
        }

        #region IDisposable support
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Close();
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