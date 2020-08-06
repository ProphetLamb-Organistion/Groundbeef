using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Groundbeef.WPF
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class SyncronizedFilterManagerService
    {
        private static readonly Dictionary<string, SyncronizedFilter> s_registeredFilters = new Dictionary<string, SyncronizedFilter>();

        /// <summary>
        /// Registers a instance of filter with a unique name to the <see cref="SyncronizedFilterManagerService"/>.
        /// </summary>
        /// <param name="filter">The filter to register.</param>
        /// <returns><see cref="true"/> if the filter was registered successfully; otherwise, <see cref="false"/>.</returns>
        public static bool RegisterFilter(SyncronizedFilter filter)
        {
            if (!filter.HasName)
                throw new ArgumentException("The filter has no name.");
            return s_registeredFilters.TryAdd(filter.Name, filter);
        }

        /// <summary>
        /// Unregsitsters a already registered filter form the <see cref="SyncronizedFilterManagerService"/>. If the filter was unregistered successfully, then disposes the filter.
        /// </summary>
        /// <param name="filter">The filter to unregister.</param>
        /// <returns></returns>
        public static bool UnregisterFilter(SyncronizedFilter filter, bool disposes = true)
        {
            if (!filter.HasName)
                throw new ArgumentException("The filter has no name.");
            bool result = s_registeredFilters.Remove(filter.Name);
            if (result && disposes)
                filter.Dispose();
            return result;
        }

        public static bool AddView(string filterName, ItemsControl itemsControl) => GetFilter(filterName).AddView(itemsControl);

        public static void SetFilter(string filterName, Predicate<object?> filter) => GetFilter(filterName).Filter = filter;

        internal static SyncronizedFilter GetFilter(string filterName) => s_registeredFilters[filterName];
    }
}
