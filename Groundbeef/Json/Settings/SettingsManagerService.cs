using System;
using System.Collections.Generic;

namespace Groundbeef.Json.Settings
{
    public static class SettingsManagerService
    {
        private static readonly Dictionary<Guid, ISettingsProvider> s_registeredProviders = new Dictionary<Guid, ISettingsProvider>();
        private static ISettingsProvider? s_provider;
        private static Guid? s_storeGuid;

        /// <summary>
        /// Registers a <see cref="ISettingsProvider"/> to the <see cref="SettingsManagerService"/>.
        /// </summary>
        /// <param name="provider"></param>
        /// <typeparam name="T_STORE"></typeparam>
        public static void RegisterProvider<T_STORE>(ISettingsProvider<T_STORE> provider)
        {
            Guid guid = typeof(T_STORE).GUID;
            if (!s_registeredProviders.TryAdd(guid, provider))
                throw new ArgumentException("A provider with this STORE already exists.", nameof(T_STORE));
        }

        /// <summary>
        /// Retrieves the <see cref="ISettingsProvider"/> associated with the STORE.
        /// </summary>
        public static ISettingsProvider<T_STORE> GetProvider<T_STORE>()
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_STORE).GUID);
            return (ISettingsProvider<T_STORE>)provider;
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        public static object? GetValue<T_STORE>(string propertyName)
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_STORE).GUID);
            return provider.GetValue(propertyName);
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="propertyName">The case-sensitive name of the property.</param>
        /// <param name="value">The new value that will be assigned to the property.</param>
        public static void SetValue<T_STORE>(string propertyName, object? value)
        {
            ISettingsProvider provider = EnsureProvider(typeof(T_STORE).GUID);
            provider.SetValue(propertyName, value);
        }

        private static ISettingsProvider EnsureProvider(Guid storeGuid)
        {
            if (s_provider is null || storeGuid != s_storeGuid)
            {
                Dictionary<Guid, ISettingsProvider> registeredProviders = s_registeredProviders;
                registeredProviders.TryGetValue(storeGuid, out ISettingsProvider? provider);
                s_provider = provider ?? throw new InvalidOperationException("No provider with this STORE exists.");
                s_storeGuid = storeGuid;
            }
            return s_provider;
        }
    }
}