using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Groundbeef.Reflection
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class EqualityComparers
    {
        private static readonly Dictionary<Guid, IEqualityComparer?> defaultComparerDictionary = new Dictionary<Guid, IEqualityComparer?>();

        /// <summary>
        /// Returns the default <see cref="IEqualityComparer"/> for the type specified.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The default <see cref="IEqualityComparer"/> for the type specified.</returns>
        public static IEqualityComparer? GetDefaultEqualityComparer(this Type type)
        {
            if (defaultComparerDictionary.TryGetValue(type.GUID, out IEqualityComparer? defaultComparer))
                return defaultComparer;
            PropertyInfo prop = typeof(IEqualityComparer<>)
                .MakeGenericType(type)
                .GetProperty("Default", BindingFlags.Public | BindingFlags.Static) ?? throw new InvalidOperationException("Property \"Default\" was not found.");
            defaultComparer = prop.GetValue(null) as IEqualityComparer;
            lock (defaultComparerDictionary)
                defaultComparerDictionary.Add(type.GUID, defaultComparer);
            return defaultComparer;
        }
    }
}