using Groundbeef.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Groundbeef.Reflection
{
    public static class TypeExtention
    {
        private static readonly Type s_enumerableTypeDefinition = typeof(IEnumerable<>);
        private static readonly Dictionary<Guid, bool> s_typeInterfaces = new Dictionary<Guid, bool>();
        private static readonly Dictionary<Guid, Type> s_enumerableArguments = new Dictionary<Guid, Type>();

        /// <summary>
        /// Returns whether the interface specified is implemented by the <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of the interface to check for.</typeparam>
        /// <param name="preferGenericTypeDefinition">If the interface is a generic type, then specifies whether to check for the type definition instead of the specific implementation.</param>
        /// <returns><see cref="true"/> if the <see cref="Type"/> contains the interface specified; otherwise, <see cref="false"/>.</returns>
        public static bool HasInterface<T>(this Type type, bool preferGenericTypeDefinition = false) => HasInterface(type, typeof(T), preferGenericTypeDefinition);

        /// <summary>
        /// Returns whether the interface specified is implemented by the <see cref="Type"/>.
        /// </summary>
        /// <param name="interfaceType">The type of the interface to check for.</param>
        /// <param name="preferGenericTypeDefinition">If the interface is a generic type, then specifies whether to check for the type definition instead of the specific implementation.</param>
        /// <returns><see cref="true"/> if the <see cref="Type"/> contains the interface specified; otherwise, <see cref="false"/>.</returns>
        public static bool HasInterface(this Type type, Type interfaceType, bool preferGenericTypeDefinition = false)
        {
            Guid guid = type.GUID.Combine(interfaceType.GUID);
            if (s_typeInterfaces.TryGetValue(guid, out bool result))
                return result;
            if (!interfaceType.IsInterface)
                throw new ArgumentException("The type is no interface.");
            if (preferGenericTypeDefinition && interfaceType.IsGenericType)
                result = HasInterface(type, interfaceType.GetGenericTypeDefinition());
            else if (interfaceType.IsGenericTypeDefinition)
                result = type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().GUID == interfaceType.GUID);
            else
                result = type.GetInterfaces().Any(i => i.GUID == interfaceType.GUID);
            lock (s_typeInterfaces)
                s_typeInterfaces.Add(guid, result);
            return result;
        }

        /// <summary>
        /// Returns the generic type argument of any <see cref="IEnumerable{T}"/> type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The generic type argument of any <see cref="IEnumerable{T}"/> type.</returns>
        public static Type GetEnumerableElementType(in Type type)
        {
            if (s_enumerableArguments.TryGetValue(type.GUID, out Type genericArgument))
                return genericArgument;
            if (!type.HasInterface(s_enumerableTypeDefinition))
                throw new ArgumentException("The type does not have the generic type definition IEnumerable<>.");
            genericArgument = type.GetGenericArguments()[0];
            lock (s_enumerableArguments)
                s_enumerableArguments.Add(type.GUID, genericArgument);
            return genericArgument;
        }


        /// <summary>
        /// Filters a sequence by methods with the attribute specified.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        public static IEnumerable<MethodInfo?> WithAttribute<TAttribute>(this IEnumerable<MethodInfo?> methods) where TAttribute : Attribute => WithAttribute(methods, typeof(TAttribute));

        /// <summary>
        /// Filters a sequence by methods with the attribute specified.
        /// </summary>
        /// <param name="attribute">The type of the attribute.</param>
        public static IEnumerable<MethodInfo?> WithAttribute(this IEnumerable<MethodInfo?> methods, Type attributeType)
        {
            if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
                throw new ArgumentException("The type is no attribute type.");
            foreach(MethodInfo? current in methods)
            {
                if (!(current is null) && current.GetCustomAttributes(attributeType, false).Length != 0)
                    yield return current;
            }
        }
    }
}
