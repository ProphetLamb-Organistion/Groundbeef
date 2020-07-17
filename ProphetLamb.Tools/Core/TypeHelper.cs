
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ProphetLamb.Tools.Collections.Concurrent;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class TypeHelper
    {
        private static readonly Type iEnumerableType = typeof(IEnumerable<>);

        /// <summary>
        /// Casts the provided object to a specified type.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T CastObject<T>(in object value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return (T)value;
        }

        /// <summary>
        /// Converts the provided object to a specified type. Using the IConvetible interface.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The conversion type.</typeparam>
        public static T ConvertObject<T>(in object value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Returns whether the type implements the IEnumerable<> interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see cref="true"/> if the type implements the IEnumerable<> interface; otherwise, <see cref="false"/>.</returns>
        public static bool IsGenericIEnumerable(in Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return type != typeof(string) && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == iEnumerableType);
        }

        private static readonly ConcurrentDictionary<Guid, Type> genericArgumentLookupCache = new ConcurrentDictionary<Guid, Type>();
        /// <summary>
        /// Returns the generic type argument of any enumerable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The generic type argument of any enumerable type.</returns>
        public static Type GetEnumerableElementType(in Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            // Grab the generic arguments.
            if (!genericArgumentLookupCache.TryGetValue(type.GUID, out Type genericArgument))
            {
                genericArgument = type.GetGenericArguments()?[0];
                if (genericArgument is null)
                    throw new ArgumentException("The type does not have at least one generic type argument.", nameof(type));
                genericArgumentLookupCache.Add(type.GUID, genericArgument);
            }
            return genericArgument;
        }
    }
}
