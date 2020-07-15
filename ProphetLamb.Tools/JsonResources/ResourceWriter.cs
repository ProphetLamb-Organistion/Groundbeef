using Newtonsoft.Json;

using ProphetLamb.Tools.Core;
using ProphetLamb.Tools.IO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            // Generate ResourceGroups form resoruceSet
            Task<ResourceGroup[]> resourceGroups = Task.Run(() => resourceSet.GroupBy(kvp => kvp.Value.GetType())
                .Select(grouping => new ResourceGroup(grouping.Key, grouping.Select(kvp => kvp.Key).ToList(), grouping.Select(kvp => kvp.Value).ToList())).ToArray());
            // Open file
            using var jsonWriter = new JsonTextWriter(writer = new StreamWriter(resourceFileName, append: false));
            // Create a serializer with the ResourceGroupConverter specific settings for the current culture.
            JsonSerializer serializer = JsonSerializer.Create(ResourceGroupConverter.SettingsFactory(culture));
            // Serialize the resourceGroups to the stream.
            resourceGroups.Wait();
            serializer.Serialize(jsonWriter, resourceGroups.Result);
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