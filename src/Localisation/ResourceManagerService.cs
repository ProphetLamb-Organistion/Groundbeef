using Groundbeef.Events;

using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Groundbeef.Localisation
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class ResourceManagerService
    {
        private static readonly Dictionary<string, System.Resources.ResourceManager> s_managers;

        public static event ValueChangedEventHandler<Locale?>? LocaleChanged;

        /// <summary>
        /// Current application locale
        /// </summary>
        public static Locale? CurrentLocale { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ResourceManager class.
        /// </summary>
        static ResourceManagerService()
        {
            s_managers = new Dictionary<string, System.Resources.ResourceManager>();
            // Set to default culture
            ChangeLocale(CultureInfo.CurrentCulture.IetfLanguageTag);
        }

        /// <summary>
        /// Retreives a string resource with the given key from the given /// resource manager. 
        /// Will load the string relevant to the current culture.
        /// </summary>
        /// <param name="managerName">Name of the ResourceManager</param>
        /// <param name="resourceKey">Resource to lookup</param>
        public static string? GetResourceString(in string managerName, in string resourceKey)
        {
            string? resource = null!;
            if (s_managers.TryGetValue(managerName, out var manager))
                resource = manager.GetString(resourceKey);
            return resource;
        }

        /// <summary>
        /// Changes the current locale
        /// </summary>
        /// <param name="newLocaleName">IETF locale name (e.g. en-US, en-GB)</param>
        public static void ChangeLocale(in string newLocaleName)
        {
            CultureInfo newCultureInfo = new CultureInfo(newLocaleName);
            Thread.CurrentThread.CurrentCulture = newCultureInfo;
            Thread.CurrentThread.CurrentUICulture = newCultureInfo;

            Locale newLocale = new Locale() { Name = newLocaleName, RTL = newCultureInfo.TextInfo.IsRightToLeft };
            Locale? oldLocale = CurrentLocale?.Clone();
            CurrentLocale = newLocale;

            LocaleChanged?.Invoke(null, new ValueChangedEventArgs<Locale?>(oldLocale, newLocale));
        }

        /// <summary>
        /// Fires the LocaleChange event to reload bindings
        /// </summary>
        public static void Refresh()
        {
            ChangeLocale(CultureInfo.CurrentCulture.IetfLanguageTag);
        }

        /// <summary>
        /// Register a ResourceManager, does not fire a refresh
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        public static void RegisterManager(in string managerName, in System.Resources.ResourceManager manager)
        {
            RegisterManager(managerName, manager, false);
        }

        /// <summary>
        /// Register a ResourceManager
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        /// <param name="refresh">Whether to fire the LocaleChanged event to refresh bindings</param>
        public static void RegisterManager(in string managerName, in System.Resources.ResourceManager manager, bool refresh)
        {
            if (!s_managers.TryGetValue(managerName, out _))
                s_managers.Add(managerName, manager);
            if (refresh)
                Refresh();
        }

        /// <summary>
        /// Remove a ResourceManager
        /// </summary>
        /// <param name="name">Name of the manager to remove</param>
        public static void UnregisterManager(in string name)
        {
            if (!s_managers.TryGetValue(name, out _))
                s_managers.Remove(name);
        }
    }
}
