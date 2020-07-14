using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using ProphetLamb.Tools.Core;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class TypeHelper
    {
        private static readonly Type iEnumerableType = typeof(IEnumerable<>);
        private static readonly string iEnumerableName = nameof(IEnumerable<object>);


        /// <summary>
        /// Casts the provided object to a specified type.
        /// </summary>
        /// <param name="input">The source object.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T CastObject<T>(in object input)
        {
            return (T)input;
        }

        /// <summary>
        /// Converts the provided object to a specified type. Using the IConvetible interface.
        /// </summary>
        /// <param name="input">The source object.</param>
        /// <typeparam name="T">The conversion type.</typeparam>
        public static T ConvertObject<T>(in object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        /// <summary>
        /// Returns whether the type implements the IEnumerable<> interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see cref="true"/> if the type implements the IEnumerable<> interface; otherwise, <see cref="false"/>.</returns>
        public static bool IsGenericIEnumerable(in Type type)
        {
            return type != typeof(string) && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == iEnumerableType);
        }

        private static readonly ConcurrentDictionary<Guid, Type[]> genericArgumentsLookupCache = new ConcurrentDictionary<Guid, Type[]>();
        /// <summary>
        /// Returns the generic type argument of any enumerable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The generic type argument of any enumerable type.</returns>
        public static Type GetEnumerableBaseType(in Type type)
        {
            // Prefer to return the base type of the IEnumerable<> interface.
            Type enumerableInterface =type.GetInterfaces().Find(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableInterface != null)
                return GetEnumerableBaseType(enumerableInterface);
            // Grab the generic arguments.
            if (!genericArgumentsLookupCache.TryGetValue(type.GUID, out Type[] genericArguments))
            {
                genericArguments = type.GetGenericArguments();
                genericArgumentsLookupCache.Add(type.GUID, genericArguments);
            }
            if (genericArguments.Length == 0)
                throw new ArgumentException("The type does not have exactly one generic type argument.", nameof(type));
            var elementType = genericArguments[0];
            return elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? elementType.GetGenericArguments()[0]
                : elementType;
        }
    }
}
