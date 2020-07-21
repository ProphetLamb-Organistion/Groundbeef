using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

using Newtonsoft.Json;
using ProphetLamb.Tools.IO;
using ProphetLamb.Tools.Reflection;
using ProphetLamb.Tools.Events;

namespace ProphetLamb.Tools.Json.Settings
{
    public interface ISettingsProvider
    {
        event PropertyChangedEventHandler? SettingsChanged;
        string? FileName { get; }
        object? GetValue(string? propertyName);
        void SetValue(string? propertyName, object? value);
    }

    public interface ISettingsProvider<T_POGO> : ISettingsProvider { }

    public class SettingsProvider<T_POGO> : ISettingsProvider<T_POGO>, IDisposable where T_POGO : class, new()
    {
        /// <summary>
        /// Notifies subscribers when a property in the POGO class was changed.
        /// </summary>
        public event PropertyChangedEventHandler? SettingsChanged;

        private readonly Dictionary<string, PropertyInfo> _properties;
        private readonly FileSystemWatcher _watcher;
        private readonly FileStream _stream;
        private readonly Timer _readDelayTimer,
                               _serializeDelayTimer;
        private readonly T_POGO _settings;
        private JsonSerializerSettings? _serializerSettings;
        private bool _lastWriteSelf;

        private SettingsProvider(T_POGO? instance, string name, string directory, JsonSerializerSettings? serializerSettings)
        {
            string filePath = Path.Combine(directory, name);

            _settings = instance ?? DefaultSettings;

            // Watch for changes in the file
            _watcher = new FileSystemWatcher(directory, name);
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            _watcher.Changed += Watcher_FileChanged;

            _stream = FileHelper.Open(filePath);

            // Delay after a change to the file before reading
            _readDelayTimer = new Timer();
            _readDelayTimer.Interval = 500;
            _readDelayTimer.Elapsed += (s, e) => Task.Run(ReadSettings);

            // Delay after a changed property before serializing and writing
            _serializeDelayTimer = new Timer();
            _serializeDelayTimer.Interval = 500;
            _serializeDelayTimer.Elapsed += (s, e) => Task.Run(WriteSettings);

            _serializerSettings = serializerSettings;

            FileName = Path.Combine(directory, name);

            // Create dictionary of all public, instance properties of the POGO.
            _properties = new Dictionary<string, PropertyInfo>(typeof(T_POGO)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanRead)
                .Select(prop => KeyValuePair.Create(prop.Name, prop)));

            // If no initial settings are provided attempt to read them from the file.
            if (_settings is null)
                ReadSettings();
            // Write the provided settings object to the associated file.
            else
                WriteSettings();
        }

        /// <summary>
        /// Gets or sets the value of the property, by the name of the property.
        /// </summary>
        /// <value>The new value that will be assigned to the property.</value>
        public object? this[string propertyName]
        {
            get => GetValue(propertyName);
            set => SetValue(propertyName, value);
        }

        /// <summary>
        /// Gets the full path to the underlying file including its name and file extention.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets or sets the <see cref="JsonSerializerSettings"/> used for de-/serialization of the POGO.
        /// </summary>
        public JsonSerializerSettings? SerializerSettings
        {
            get => _serializerSettings;
            set
            {
                _serializerSettings = value;
                ReadSettings();
            }
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        public T? GetValue<T>(string? propertyName) where T : struct
        {
            return (T?)GetValue(propertyName);
        }
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        public object? GetValue(string? propertyName)
        {
            if (!(propertyName is string name))
                throw new ArgumentNullException(nameof(propertyName));
            if (!_properties.TryGetValue(name, out PropertyInfo? prop))
                throw new ArgumentException("No property with the name specified was found.", nameof(propertyName));
            return prop.GetValue(_settings);
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        /// <param name="value">The new value that will be assigned to the property.</param>
        public void SetValue(string? propertyName, object? value)
        {
            if (!(propertyName is string name))
                throw new ArgumentNullException(nameof(propertyName));
            if (!_properties.TryGetValue(name, out PropertyInfo? prop))
                throw new ArgumentException("No property with the name specified was found.", nameof(propertyName));
            InternalSetValue(prop, value);
        }

        private void InternalSetValue(PropertyInfo propertyInfo, object? newValue)
        {
            _serializeDelayTimer.Stop();
            if (!propertyInfo.CanWrite)
                throw new InvalidOperationException("The requested property cannot be set to the specified value.");
            object? oldValue = propertyInfo.GetValue(_settings);
            propertyInfo.SetValue(_settings, newValue);
            SettingsChanged?.Invoke(this, new PropertyChangedEventArgs(oldValue, newValue, propertyInfo.PropertyType, propertyInfo.Name));
            _serializeDelayTimer.Start();
        }

        private void Watcher_FileChanged(object sender, FileSystemEventArgs e)
        {
            if (_lastWriteSelf)
                _lastWriteSelf = false;
            else
                _readDelayTimer.Start();
        }

        private void ReadSettings()
        {
            T_POGO? newSettings;
            lock(_stream)
            {
                Stream stream = _stream;
                using var sr = new StreamReader(stream);
                // Create serializer
                JsonSerializer serializer;
                if (SerializerSettings is JsonSerializerSettings serializerSettings)
                    serializer = JsonSerializer.Create(serializerSettings);
                else
                    serializer = JsonSerializer.CreateDefault();
                // Deserialize json or create default
                newSettings = serializer.Deserialize(sr, typeof(T_POGO)) as T_POGO ?? DefaultSettings;
            }
            Parallel.ForEach(_properties.Values, (PropertyInfo prop) => 
            {
                object? newValue = prop.GetValue(newSettings);
                if (prop.PropertyType.GetDefaultEqualityComparer() is IEqualityComparer comparer)
                {
                    // Update settings value if it differs
                    if (!comparer.Equals(prop.GetValue(_settings), newValue))
                        InternalSetValue(prop, newValue);
                }
                else
                {
                    // Assign new settings value
                    InternalSetValue(prop, newValue);
                }
            });
        }

        private void WriteSettings()
        {
            lock(_stream)
            {
                Stream stream = _stream;
                using var sw = new StreamWriter(stream);
                // Create serializer
                JsonSerializer serializer;
                if (SerializerSettings is JsonSerializerSettings serializerSettings)
                    serializer = JsonSerializer.Create(serializerSettings);
                else
                    serializer = JsonSerializer.CreateDefault();
                // Serialize json
                serializer.Serialize(sw, _settings, typeof(T_POGO));
            }
        }

        #region Static members
        /// <summary>
        /// Returns a new instance of <see cref="SettingsProvider{T_POGO}"/>, using the default <see cref="JsonSerializationSettings"/>.
        /// </summary>
        /// <param name="fileName">The relative or full path to the file including its name and file extention.</param>
        /// <returns>A new instance of <see cref="SettingsProvider{T_POGO}"/>.</returns>
        public static SettingsProvider<T_POGO> Create(string? fileName) => Create(null, fileName, null);
        /// <summary>
        /// Returns a new instance of <see cref="SettingsProvider{T_POGO}"/>.
        /// </summary>
        /// <param name="fileName">The relative or full path to the file including its name and file extention.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used when serializing and deserializing the POGO object.</param>
        /// <returns>A new instance of <see cref="SettingsProvider{T_POGO}"/>.</returns>
        public static SettingsProvider<T_POGO> Create(string? fileName, JsonSerializerSettings? settings) => Create(null, fileName, settings);
        /// <summary>
        /// Returns a new instance of <see cref="SettingsProvider{T_POGO}"/>.
        /// </summary>
        /// <param name="instance">The initial settings instance.</param>
        /// <param name="fileName">The relative or full path to the file including its name and file extention.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used when serializing and deserializing the POGO object.</param>
        /// <returns>A new instance of <see cref="SettingsProvider{T_POGO}"/>.</returns>
        public static SettingsProvider<T_POGO> Create(T_POGO? instance, string? fileName, JsonSerializerSettings? settings)
        {
            string l_fileName = fileName??throw new ArgumentNullException(nameof(fileName)),
                   directory = Path.GetDirectoryName(l_fileName)??throw new ArgumentException("The path is not valid."),
                   name = Path.GetFileName(l_fileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(l_fileName))
                FileHelper.Create(l_fileName).Dispose();
            return new SettingsProvider<T_POGO>(instance, name, directory, settings);
        }

        /// <summary>
        /// Gets the default instance of the settings POGO
        /// </summary>
        public static T_POGO DefaultSettings { get; } = new T_POGO();

        #endregion

        #region IDisposable members
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watcher.Dispose();
                    _stream.Dispose();
                    _readDelayTimer.Dispose();
                    _serializeDelayTimer.Dispose();
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
