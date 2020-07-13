using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceWriter : System.Resources.IResourceWriter
    {
        private readonly ResourceSet resourceSet;
        private readonly string resourceFileName;

        public ResourceWriter(in ResourceManager sourceResourceManager, in CultureInfo culture)
        {
            // Locate the associated resource file.
            string fileName = sourceResourceManager.GetResourceFileName(culture??CultureInfo.InvariantCulture);
            if (!sourceResourceManager.resourceSetTable.TryGetValue(culture.Name, out resourceSet))
            {
                // Create a new ResourceSet/ file
                File.CreateText(fileName);
                resourceSet = new ResourceSet(fileName);
            }
            // Locate resource file
            resourceFileName = sourceResourceManager.GetResourceFileName(culture);
            if (!resourceSet.Loaded)
                resourceSet.Load().Wait();
        }

        public void AddResource(string name, byte[] value)
        {
            
        }

        public void AddResource(string name, object value)
        {
            resourceSet.Add(name, value);
        }

        public void AddResource(string name, string value)
        {
            resourceSet.Add(name, value);
        }

        public async void Close()
        {
            using var sr = new StreamWriter(resourceFileName);
            await sr.WriteLineAsync(await resourceSet.WriteResources());
            await sr.FlushAsync();
        }

        public void Generate()
        {
            throw new System.NotImplementedException();
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
            Dispose(disposing: true);
        }
        #endregion
    }
}