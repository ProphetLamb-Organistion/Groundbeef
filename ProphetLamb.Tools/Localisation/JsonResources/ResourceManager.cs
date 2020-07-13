using System.IO;
using System.Collections.Generic;
using ProphetLamb.Tools.Core;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProphetLamb.Tools.Events;

namespace ProphetLamb.Tools.Localisation.JsonResources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceManager : IDisposable
    {
        internal const string ResourceFileExtention = ".json";

        internal readonly Dictionary<string, ResourceSet> resourceSetTable = new Dictionary<string, ResourceSet>();
        private readonly string baseFilePath;
        private CultureInfo _resourceCulture;
        private ResourceSet _loadedResourceSet;
        private bool hasLoadedResourceSet;

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
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName);
            _resourceCulture = CultureInfo.InvariantCulture;
            ChangeCulture(null, _resourceCulture).ConfigureAwait(false);
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
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName);
            _resourceCulture = CultureInfo.InvariantCulture;
            ChangeCulture(null, _resourceCulture).ConfigureAwait(false);
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
            baseFilePath = GetBaseFileName(ResourceRootPath, baseName);
            _resourceCulture = defaultCulture??CultureInfo.InvariantCulture;
            ChangeCulture(null,_resourceCulture).ConfigureAwait(false);
        }

        public object this[in string key] => GetObject(key);

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
            set => ChangeCulture(_resourceCulture, _resourceCulture = value).ConfigureAwait(false);
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
        public object GetString(in string key, bool caseInsensitive = false)
        {
            return GetString(key, _resourceCulture, caseInsensitive);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetString(in string key, in CultureInfo culture, bool caseInsensitive = false)
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

        private async Task ChangeCulture(CultureInfo oldCulture, CultureInfo newCulture)
        {
            // Cultures must be different
            if (oldCulture?.Name.Equals(newCulture.Name, StringComparison.InvariantCulture) ?? false)
                return;
            // Unload previous ResourceSet if nessessary
            if (hasLoadedResourceSet)
                _loadedResourceSet.Unload();
            hasLoadedResourceSet = false;
            if (newCulture != null)
            {
                // Attempt to grab new resource set from resSets
                if (!resourceSetTable.TryGetValue(newCulture.Name, out _loadedResourceSet))
                {
                    // Create new ResourceSet from file
                    string fileName = GetResourceFileName(newCulture);
                    if (!File.Exists(fileName))
                        throw new FileNotFoundException();
                    _loadedResourceSet = new ResourceSet(fileName);
                    resourceSetTable.Add(newCulture.Name, _loadedResourceSet);
                }
                await _loadedResourceSet.Load();
                hasLoadedResourceSet = true;
            }
            CultureChangedEvent?.Invoke(this, new ValueChangedEventArgs<CultureInfo>(oldCulture, newCulture));
        }

        internal string GetResourceFileName(in CultureInfo culture)
        {
            var sb = new StringBuilder(255);
            sb.Append(baseFilePath);
            // Culture is not invariant or neutral culture
            if (!culture.IsNeutralCulture && !culture.Name.Equals(CultureInfo.InvariantCulture.Name, StringComparison.InvariantCulture))
            {
                // Call internal VerifyCultureName function with throw exception flag.
                culture.VerifyCultureName(true);
                sb.Append('.').Append(culture.Name);
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
            if (!hasLoadedResourceSet || !culture.Name.Equals(_resourceCulture.Name))
                Culture = culture;
        }

        internal static string GetBaseFileName(in string rootPath, in string baseName)
        {
            string baseFilePath = Path.Combine(rootPath, baseName);
            if (!File.Exists(baseFilePath + ResourceFileExtention))
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
                    hasLoadedResourceSet = false;
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
