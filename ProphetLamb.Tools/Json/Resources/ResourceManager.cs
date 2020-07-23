using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using ProphetLamb.Tools.Core;
using ProphetLamb.Tools.Events;

namespace ProphetLamb.Tools.Json.Resources
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceManager : IDisposable
    {
        internal static readonly string ResourceFileExtention = ".json";

        private readonly Dictionary<string, ResourceSet> _resourceSetTable = new Dictionary<string, ResourceSet>();
        private readonly string _baseFilePath;
        private CultureInfo? _resourceCulture = null!;
        private ResourceSet? _loadedResourceSet = null!;

        public event ValueChangedEventHandler<CultureInfo?>? CultureChangedEvent;

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>. Deriving the resource location from the calling assembly's location. 
        /// A task loading the <see cref="CultureInfo.InvariantCulture"/> resources ist started, upon completion the <see cref="CultureChangedEvent"/> is raised.
        /// </summary>
        /// <param name="baseName">The first component of the name of a resource file. Format: [baseName].[culture identifier].json</param>
        public ResourceManager(in string baseName)
        {
            BaseName = baseName;
            Assembly assembly = Assembly.GetCallingAssembly();
            ResourceRootPath = assembly.Location;
            string stdAssemblyName = typeof(object).Assembly.FullName??throw new InvalidOperationException("Could not determine the name of the System assembly.");
            if (stdAssemblyName.Equals(assembly.FullName, StringComparison.InvariantCulture))
                throw new NotSupportedException("Cannot be called by the system assembly.");
            // If the assemblyPath is a manifest-file
            if (!Directory.Exists(ResourceRootPath))
                ResourceRootPath = new FileInfo(ResourceRootPath).DirectoryName;
            _baseFilePath = GetBaseFileName(ResourceRootPath, baseName, false);
        }

        /// <summary>
        /// Initializses a new instance of <see cref="ResourceManager"/>.
        /// A task loading the <see cref="CultureInfo.InvariantCulture"/> resources ist started, upon completion the <see cref="CultureChangedEvent"/> is raised.
        /// </summary>
        /// <param name="baseName">The first component of the name of a resource file. Format: [baseName].[culture identifier].json</param>
        /// <param name="resourcePath">The path leading to the directory that contains the resource files.</param>
        public ResourceManager(in string baseName, in string resourcePath)
        {
            BaseName = baseName;
            ResourceRootPath = resourcePath;
            _baseFilePath = GetBaseFileName(ResourceRootPath, baseName, false);
        }

        /// <summary>
        /// Gets or sets if key should be treated as case insensive by default or not.
        /// </summary>
        public static bool CaseInsensitiveDefault { get; set; } = true;

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        public string? this[in string key] => GetString(key);

        /// <summary>
        /// Gets the first component of the name of a resource file. Format: [baseName].[culture identifier].json
        /// </summary>
        public string BaseName { get; }

        /// <summary>
        /// Gets or sets the culture used when requesting resources, unless specified otherwise.
        /// </summary>
        public CultureInfo? Culture
        {
            get => _resourceCulture;
            set => ChangeCulture(_resourceCulture, _resourceCulture = value);
        }

        /// <summary>
        /// Gets or sets the currently loaded resource set, corrosponding to <see cref="ResourceManager.Culture"/>.
        /// </summary>
        internal ResourceSet? LoadedResourceSet
        {
            get => _loadedResourceSet; 
            private set => _loadedResourceSet = value;
        }

        /// <summary>
        /// Gets the path leading to the directory that contains the resource files.
        /// </summary>
        internal string ResourceRootPath { get; }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>. 
        /// Replaces the format item in a value string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args"> An object array that contains zero or more objects to format.</param>
        public string? GetStringFormat(in string key, params object?[] args)
        {
            return String.Format(GetString(key), args);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>. 
        /// Replaces one or more format items in a string with the string representation of a specified object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arg0">The frist object to format.</param>
        public string? GetStringFormat(in string key, in object? arg0)
        {
            return String.Format(GetString(key), arg0);
        }

        /// <summary>
        /// Returns the string value formatted with the format arguments associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// Replaces one or more format items in a string with the string representation of a specified object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arg0">The frist object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        public string? GetStringFormat(in string key, in object? arg0, in object? arg1)
        {
            return String.Format(GetString(key), arg0, arg1);
        }

        /// <summary>
        /// Returns the string value formatted with the format arguments associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// Replaces one or more format items in a string with the string representation of a specified object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arg0">The frist object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        public string? GetStringFormat(in string key, in object? arg0, in object? arg1, in object? arg2)
        {
            return String.Format(GetString(key), arg0, arg1, arg2);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public string GetString(in string key, bool? ignoreCase = null)
        {
            return GetString(key, _resourceCulture, ignoreCase);
        }

        /// <summary>
        /// Returns the string value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public string GetString(in string key, in CultureInfo? culture, bool? ignoreCase = null)
        {
            EnsureSet(key, culture);
#pragma warning disable CS8602, CS8604
            return _loadedResourceSet.GetString(key, ignoreCase ?? CaseInsensitiveDefault);
#pragma warning restore CS8602, CS8604
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public object? GetObject(in string key, bool? ignoreCase = null)
        {
            return GetObject(key, _resourceCulture, ignoreCase);
        }

        /// <summary>
        /// Returns the object value associated with the <paramref cref="key"/> with the culture <see cref="ResourceManager.Culture"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture of the resource value</param>
        /// <param name="ignoreCase">Whether the key is treated case insensitive.</param>
        public object? GetObject(in string key, in CultureInfo? culture, bool? ignoreCase = null)
        {
            EnsureSet(key, culture);
#pragma warning disable CS8602, CS8604
            return _loadedResourceSet.GetObject(key, ignoreCase ?? CaseInsensitiveDefault);
#pragma warning restore CS8602, CS8604
        }

        internal void AddResourceSet(in CultureInfo resourceCulture, in ResourceSet resourceSet, bool overwriteExisting = false)
        {
            resourceCulture.VerifyCultureName(true);
            string cultureName = resourceCulture.Name,
                   fileName = GetResourceFileName(_baseFilePath, resourceCulture);
            if (_resourceSetTable.ContainsKey(cultureName))
                throw new ArgumentException(ExceptionResource.RESOURCESET_CULTURE_EXISTS);
            if (!File.Exists(fileName))
                throw new FileNotFoundException(ExceptionResource.FILE_NOTONDEVICE, fileName);
            if (overwriteExisting && _resourceSetTable.TryGetValue(cultureName, out ResourceSet? dispose))
            {
                _resourceSetTable.Remove(cultureName);
                dispose.Dispose();
            }
            _resourceSetTable.Add(cultureName, resourceSet);
        }

        internal bool TryGetResourceSet(in CultureInfo culture, out ResourceSet? resourceSet)
        {
            string cultureName = culture.Name;
            CultureInfoExtention.VerifyCultureName(cultureName, true);
            return _resourceSetTable.TryGetValue(culture.Name, out resourceSet);
        }

        internal void SetResourceSet(in CultureInfo resourceCulture, in ResourceSet newValue)
        {
            string cultureName = resourceCulture.Name;
            CultureInfoExtention.VerifyCultureName(cultureName, true);

            _resourceSetTable[cultureName] = newValue;
        }

        public IEnumerable<CultureInfo> Cultures
        {
            get => _resourceSetTable.Keys.Select(CultureInfo.GetCultureInfo);
        }

        public void Clean()
        {

            foreach ((string key, ResourceSet value) in _resourceSetTable)
            {
                // Skip current ResourceSet
                if (!(_resourceCulture is null) && key.Equals(_resourceCulture.Name, StringComparison.InvariantCulture))
                    continue;
                // Dispose all other ResourceSets & remove entry
                value.Dispose();
                _resourceSetTable.Remove(key);
            }
        }

        private void ChangeCulture(CultureInfo? oldCulture, CultureInfo? newCulture)
        {
            // Cultures must be different
            if (oldCulture != null && newCulture != null && oldCulture.Name.Equals(newCulture.Name, StringComparison.InvariantCulture))
                return;
            if (newCulture != null)
            {
                newCulture.VerifyCultureName(true);
                // Attempt to grab new resource set from resSets
                if (!_resourceSetTable.TryGetValue(newCulture.Name, out _loadedResourceSet))
                {
                    using var reader = new ResourceReader(this, newCulture);
                    // Create new ResourceSet from file
                    _loadedResourceSet = new ResourceSet(reader);
                    _resourceSetTable.Add(newCulture.Name, _loadedResourceSet);
                }
            }
            CultureChangedEvent?.Invoke(this, new ValueChangedEventArgs<CultureInfo?>(oldCulture, newCulture));
        }

        internal string GetResourceFileName(in CultureInfo culture)
        {
            return GetResourceFileName(_baseFilePath, culture);
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

        private void EnsureSet(in string key, in CultureInfo? culture)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or white space", nameof(key));
            // Change culture & ResourceSet to the requested culture
            CultureInfo l_culture = culture??CultureInfo.InvariantCulture;
            if (!l_culture.Name.Equals(_resourceCulture?.Name))
                ChangeCulture(_resourceCulture, _resourceCulture = l_culture);
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
                    if (_loadedResourceSet != null)
                    {
                        _loadedResourceSet = null;
                    }
                    foreach (ResourceSet resSet in _resourceSetTable.Values)
                    {
                        lock (resSet)
                            resSet.Dispose();
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
