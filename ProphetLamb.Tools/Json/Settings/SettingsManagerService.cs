using System;
using System.Collections.Generic;

namespace ProphetLamb.Tools.Json.Settings
{
    public static class SettingsManagerService
    {
        private static readonly Dictionary<Guid, ISettingsProvider> s_registeredProviders = new Dictionary<Guid, ISettingsProvider>();
        private static ISettingsProvider? s_provider;
        private static Guid? s_pogoGuid;

        /// <summary>
        /// Registers a <see cref="ISettingsProvider"/> to the <see cref="SettingsManagerService"/>.
        /// </summary>
        /// <param name="provider"></param>
        /// <typeparam name="T_POGO"></typeparam>
        public static void RegisterProvider<T_POGO>(ISettingsProvider<T_POGO> provider)
        {
            Guid guid = typeof(T_POGO).GUID;
            if (!s_registeredProviders.TryAdd(guid, provider))
                throw new ArgumentException("A provider with this POGO already exists.", nameof(T_POGO));
        }

        /// <summary>
        /// Retrieves the <see cref="ISettingsProvider"/> associated with the POGO.
        /// </summary>
        public static ISettingsProvider<T_POGO> GetProvider<T_POGO>()
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_POGO).GUID);
            return (ISettingsProvider<T_POGO>)provider;
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        public static object? GetValue<T_POGO>(string? propertyName)
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_POGO).GUID);
            return provider.GetValue(propertyName);
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        /// <param name="value">The new value that will be assigned to the property.</param>
        public static void SetValue<T_POGO>(string? propertyName, object? value)
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_POGO).GUID);
            provider.SetValue(propertyName, value);
        }

        private static ISettingsProvider EnsureProvider(Guid pogoGuid)
        {
            if (s_provider is null || pogoGuid != s_pogoGuid)
            {
                Dictionary<Guid, ISettingsProvider> registeredProviders = s_registeredProviders;
                registeredProviders.TryGetValue(pogoGuid, out ISettingsProvider? provider);
                s_provider = provider ?? throw new InvalidOperationException("No provider with this POGO exists.");
                s_pogoGuid = pogoGuid;
            }
            return s_provider;
        }
    }
}