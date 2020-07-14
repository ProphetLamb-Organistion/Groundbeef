using System.Linq;
using System.IO;
using System.Collections.Generic;
using ProphetLamb.Tools.Core;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProphetLamb.Tools.Events;

namespace ProphetLamb.Tools.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceManager : IDisposable
    {
        internal static readonly string ResourceFileExtention = ".json";

        private readonly Dictionary<string, ResourceSet> resourceSetTable = new Dictionary<string, ResourceSet>();
        private readonly string baseFilePath;
        private CultureInfo _resourceCulture = null;
        private ResourceSet _loadedResourceSet;

        public event ValueChangedEventHandler<CultureInfo> CultureChangedEvent;

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>. Deriving the resource location from the calling assembly's location. 
        /// A task loading the <see cref="CultureInfo.InvariantCulture"/> resources ist started, upon completion the <see cref="CultureChangedEvent"/> is raised.
        /// </summary>
        /// <param name="baseName">The first component of the name of a resource file. Format: [baseName].[culture identifier].json</param>
        public ResourceManager(in string baseName)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            Assembly assembly = Assembly.GetCallingAssembly();
            ResourceRootPath = assembly.Location;
            if (typeof(object).Assembly.FullName.Equals(assembly.FullName, StringComparison.InvariantCulture))
                throw new InvalidOperationException();
            // If the assemblyPath is a manifest-file
            if (!Directory.Exists(ResourceRootPath))
                ResourceRootPath = new FileInfo(ResourceRootPath).DirectoryName;
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName, false);
        }

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>.
        /// A task loading the <see cref="CultureInfo.InvariantCulture"/> resources ist started, upon completion the <see cref="CultureChangedEvent"/> is raised.
        /// </summary>
        /// <param name="baseName">The first component of the name of a resource file. Format: [baseName].[culture identifier].json</param>
        /// <param name="resourcePath">The path leading to the directory that contains the resource files.</param>
        public ResourceManager(in string baseName, in string resourcePath)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            ResourceRootPath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName, false);
        }

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>.
        /// A task loading the <see cref="CultureInfo.InvariantCulture"/> ist started, upon completion the <see cref="CultureChangedEvent"/> is raised.
        /// </summary>
        /// <param name="baseName">The first component of the name of a resource file. Format: [baseName].[culture identifier].json</param>
        /// <param name="resourcePath">The path leading to the directory that contains the resource files.</param>
        /// <param name="defaultCulture">The culture that will initially be accessed unless specifing otherwise; if <see cref="null"/> the <see cref="CultureInfo.InvariantCulture"/> is used.</param>
        public ResourceManager(in string baseName, in string resourcePath, in CultureInfo defaultCulture)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            ResourceRootPath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName, false);
            _resourceCulture = defaultCulture??CultureInfo.InvariantCulture;
        }
        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The case sensitive key.</param>
        public string this[in string key] => GetString(key);

        /// <summary>
        /// Gets the first component of the name of a resource file. Format: [baseName].[culture identifier].json
        /// </summary>
        public string BaseName { get; }

        /// <summary>
        /// Gets or sets the culture used when requesting resources, unless specified otherwise.
        /// </summary>
        public CultureInfo Culture
        {
            get => _resourceCulture;
            set => ChangeCulture(_resourceCulture, _resourceCulture = value);
        }

        /// <summary>
        /// Gets or sets the currently loaded resource set, corrosponding to <see cref="ResourceManager.Culture"/>.
        /// </summary>
        internal ResourceSet LoadedResourceSet { get => _loadedResourceSet; private set => _loadedResourceSet = value; }

        /// <summary>
        /// Gets the path leading to the directory that contains the resource files.
        /// </summary>
        internal string ResourceRootPath { get; }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public string GetString(in string key, bool caseInsensitive = false)
        {
            return GetString(key, _resourceCulture, caseInsensitive);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public string GetString(in string key, in CultureInfo culture, bool caseInsensitive = false)
        {
            VerifyGet(key, culture);
            return _loadedResourceSet.GetString(key, caseInsensitive);
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetObject(in string key, bool caseInsensitive = false)
        {
            return GetObject(key, _resourceCulture, caseInsensitive);
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetObject(in string key, in CultureInfo culture, bool caseInsensitive = false)
        {
            VerifyGet(key, culture);
            return _loadedResourceSet.GetObject(key, caseInsensitive);
        }

        internal void AddResourceSet(in CultureInfo resourceCulture, in ResourceSet resourceSet)
        {
            if (resourceCulture is null)
                throw new ArgumentNullException(nameof(resourceCulture));
            if (resourceSet is null)
                throw new ArgumentNullException(nameof(resourceSet));
            resourceCulture.VerifyCultureName(true);
            string cultureName = resourceCulture.Name,
                   fileName = GetResourceFileName(baseFilePath, resourceCulture);
            if (resourceSetTable.ContainsKey(cultureName))
                throw new ArgumentException("A resource set with the same culture already exisits");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The resource set .json-file does not exist in the device.", fileName);
            resourceSetTable.Add(cultureName, resourceSet);
        }

        internal bool TryGetResourceSet(in CultureInfo culture, out ResourceSet resourceSet)
        {
            if (culture is null)
                throw new ArgumentNullException(nameof(culture));
            culture.VerifyCultureName(true);
            return resourceSetTable.TryGetValue(culture.Name, out resourceSet);
        }

        public IEnumerable<CultureInfo> Cultures
        {
            get => resourceSetTable.Keys.Select(CultureInfo.GetCultureInfo);
        }

        public void Clean()
        {

            foreach((string key, ResourceSet value) in resourceSetTable)
            {
                // Skip current ResourceSet
                if (key.Equals(_resourceCulture.Name, StringComparison.InvariantCulture))
                    continue;
                // Dispose all other ResourceSets & remove entry
                value.Dispose();
                resourceSetTable.Remove(key);
            }
        }

        private void ChangeCulture(CultureInfo oldCulture, CultureInfo newCulture)
        {
            // Cultures must be different
            if (oldCulture != null || oldCulture.Name.Equals(newCulture.Name, StringComparison.InvariantCulture))
                return;
            if (newCulture != null)
            {
                newCulture.VerifyCultureName(true);
                // Attempt to grab new resource set from resSets
                if (!resourceSetTable.TryGetValue(newCulture.Name, out _loadedResourceSet))
                {
                    // Create new ResourceSet from file
                    _loadedResourceSet = new ResourceSet(new ResourceReader(this, newCulture));
                    resourceSetTable.Add(newCulture.Name, _loadedResourceSet);
                }
            }
            CultureChangedEvent?.Invoke(this, new ValueChangedEventArgs<CultureInfo>(oldCulture, newCulture));
        }

        internal string GetResourceFileName(in CultureInfo culture)
        {
            return GetResourceFileName(baseFilePath, culture);
        }

        internal static string GetResourceFileName(in string baseFilePath, in CultureInfo culture)
        {
            var sb = new StringBuilder(255);
            sb.Append(baseFilePath);
            // Culture is not invariant or neutral culture
            if (!culture.IsNeutralCulture && !culture.Name.Equals(CultureInfo.InvariantCulture.Name, StringComparison.InvariantCulture))
            {
                // Call internal VerifyCultureName function with throw exception flag.
                culture.VerifyCultureName(true);
                sb.Append('.').Append(culture.IetfLanguageTag);
            }
            sb.Append(ResourceFileExtention);
            return sb.ToString();
        }

        private void VerifyGet(in string key, in CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or white space", nameof(key));
            if (culture is null)
                throw new ArgumentNullException(nameof(culture));
            // Change culture & ResourceSet to the requested culture
            if (!culture.Name.Equals(_resourceCulture.Name))
                ChangeCulture(_resourceCulture, _resourceCulture = culture);
        }

        internal static string GetBaseFileName(in string rootPath, in string baseName, bool fileNotFoundException = true)
        {
            string baseFilePath = Path.Combine(rootPath, baseName);
            if (fileNotFoundException && !File.Exists(baseFilePath + ResourceFileExtention))
                throw new FileNotFoundException();
            return baseFilePath;
        }

        #region IDisposable members
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose all ResourceSets
                    lock(_loadedResourceSet)
                        _loadedResourceSet = null;
                    foreach(ResourceSet resSet in resourceSetTable.Values)
                    {
                        resSet?.Dispose();
                    }
                }
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
