using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using System;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceWriter : System.Resources.IResourceWriter
    {
        private readonly ResourceManager resourceManager;
        private readonly CultureInfo culture;
        private ResourceSet resourceSet;
        private string resourceFileName;

        public ResourceWriter(in ResourceManager resourceManager, in CultureInfo resourceCulture)
        {
            culture = resourceCulture??CultureInfo.InvariantCulture;
            this.resourceManager = resourceManager;
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
            using var sw = new StreamWriter(resourceFileName, append: false);
            JsonSerializer serializer = JsonSerializer.CreateDefault();
            serializer.Culture = CultureInfo.InvariantCulture;
            serializer.Serialize(sw, resourceSet.ResourceTable, ResourceSet.SerializedType);
        }

        public void Generate()
        {
            // Locate the associated resource file.
            string fileName = resourceManager.GetResourceFileName(culture);
            if (!resourceManager.TryGetResourceSet(culture, out resourceSet))
            {
                // Create a new ResourceSet/ file
                File.CreateText(fileName);
                resourceSet = new ResourceSet();
                resourceManager.AddResourceSet(culture, resourceSet);
            }
            // Locate resource file
            resourceFileName = resourceManager.GetResourceFileName(culture);
        }

        #region IDisposable support
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Close();
                resourceSet = null; // null the reference but dont dispose.
                resourceFileName = null;
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