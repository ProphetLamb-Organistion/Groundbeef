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
        internal const string ResFileExtention = ".json";

        private readonly string resourceRootPath,
                                baseFilePath;
        private readonly Dictionary<string, ResourceSet> resSets = new Dictionary<string, ResourceSet>();
        private ResourceSet currentResSet;
        private CultureInfo currentCulture;
        private bool currentResSetLoaded;

        public event ValueChangedEventHandler<CultureInfo> CultureChangedEvent;

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>. Deriving the resource location from the calling assembly's location.
        /// </summary>
        /// <param name="baseName"></param>
        public ResourceManager(string baseName)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            Assembly assembly = Assembly.GetCallingAssembly();
            resourceRootPath = assembly.Location;
            if (typeof(object).Assembly.FullName.Equals(assembly.FullName, StringComparison.InvariantCulture))
                throw new InvalidOperationException();
            // If the assemblyPath is a manifest-file
            if (!Directory.Exists(resourceRootPath))
                resourceRootPath = new FileInfo(resourceRootPath).DirectoryName;
            baseFilePath = GetBaseFileName(resourceRootPath, baseName);
            currentCulture = CultureInfo.InvariantCulture;
            ChangeCulture(null, currentCulture).Wait();
        }

        public ResourceManager(string baseName, string resourcePath)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            resourceRootPath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
            baseFilePath = GetBaseFileName(resourceRootPath, baseName);
            currentCulture = CultureInfo.InvariantCulture;
            ChangeCulture(null, currentCulture).Wait();
        }

        public ResourceManager(string baseName, string resourcePath, CultureInfo defaultCulture)
        {
            BaseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            resourceRootPath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
            baseFilePath = GetBaseFileName(resourceRootPath, baseName);
            currentCulture = defaultCulture??CultureInfo.InvariantCulture;
            ChangeCulture(null,currentCulture).Wait();
        }

        public object this[string key] => GetObject(key);

        /// <summary>
        /// Gets the first component of the name of a resource file. Format: [baseName].[culture identifier].json
        /// </summary>
        /// <value></value>
        public string BaseName { get; }

        public CultureInfo Culture
        {
            get => currentCulture;
            set => ChangeCulture(currentCulture, currentCulture = value).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetString(string key, bool caseInsensitive = false)
        {
            return GetString(key, currentCulture, caseInsensitive);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetString(string key, CultureInfo culture, bool caseInsensitive = false)
        {
            VerifyGet(key, culture);
            return currentResSet.GetString(key, caseInsensitive);
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetObject(string key, bool caseInsensitive = false)
        {
            return GetObject(key, currentCulture, caseInsensitive);
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="caseInsensitive">Whether the key is treated case insensitive.</param>
        public object GetObject(string key, CultureInfo culture, bool caseInsensitive = false)
        {
            VerifyGet(key, culture);
            return currentResSet.GetObject(key, caseInsensitive);
        }

        private async Task ChangeCulture(CultureInfo oldCulture, CultureInfo newCulture)
        {
            // Cultures must be different
            if (oldCulture?.Name.Equals(newCulture.Name, StringComparison.InvariantCulture) ?? false)
                return;
            // Unload previous ResourceSet if nessessary
            if (currentResSetLoaded)
                currentResSet.Unload();
            currentResSetLoaded = false;
            if (newCulture != null)
            {
                // Attempt to grab new resource set from resSets
                if (!resSets.TryGetValue(newCulture.Name, out currentResSet))
                {
                    // Create new ResourceSet from file
                    string fileName = GetResourceFileName(newCulture);
                    if (!File.Exists(fileName))
                        throw new FileNotFoundException();
                    currentResSet = new ResourceSet(fileName);
                    resSets.Add(newCulture.Name, currentResSet);
                }
                await currentResSet.Load();
                currentResSetLoaded = true;
            }
            CultureChangedEvent?.Invoke(this, new ValueChangedEventArgs<CultureInfo>(oldCulture, newCulture));
        }

        private string GetResourceFileName(CultureInfo culture)
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
            sb.Append(ResFileExtention);
            return sb.ToString();
        }

        private void VerifyGet(string key, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or white space", nameof(key));
            if (culture is null)
                throw new ArgumentNullException(nameof(culture));
            // Change culture & ResourceSet to the requested culture
            if (!currentResSetLoaded || !culture.Name.Equals(currentCulture.Name))
                Culture = culture;
        }

        internal static string GetBaseFileName(string rootPath, string baseName)
        {
            string baseFilePath = Path.Combine(rootPath, baseName);
            if (!File.Exists(baseFilePath + ResFileExtention))
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
                    currentResSetLoaded = false;
                    lock(currentResSet)
                        currentResSet = null;
                    foreach(ResourceSet resSet in resSets.Values)
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
