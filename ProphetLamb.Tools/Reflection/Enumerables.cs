using System;
using System.Collections.Generic;
using System.Linq;

namespace ProphetLamb.Tools.Reflection
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class Enumerables
    {
        private static readonly Type iEnumerableType = typeof(IEnumerable<>);

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

        private static readonly Dictionary<Guid, Type> genericArgumentDictionary = new Dictionary<Guid, Type>();
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
            if (genericArgumentDictionary.TryGetValue(type.GUID, out Type? genericArgument))
                return genericArgument;
            genericArgument = type.GetGenericArguments()?[0];
            if (genericArgument is null)
                throw new ArgumentException(ExceptionResource.TYPE_NOTGENERIC, nameof(type));
            lock(genericArgumentDictionary)
                genericArgumentDictionary.Add(type.GUID, genericArgument);
            return genericArgument;
        }
    }
}