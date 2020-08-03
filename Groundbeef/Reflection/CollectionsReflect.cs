using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Groundbeef.Reflection
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class CollectionsReflect
    {
        #region Cast enumerable
        private static Dictionary<Guid, Invoker> s_enumerableCastMethods = new Dictionary<Guid, Invoker>();
        /// <summary>
        /// Casts the elements of an <see cref="IEnumerable"/> to the specified type.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable"/> that contains the elements to be cast to type <paramref name="resultType"/>.</param>
        /// <param name="resultType">The type to cast the elements of <paramref name="source"/> to.</param>
        /// <returns>The <see cref="IEnumerable"/> that contains the elements to be cast to <paramref name="resultType"/>.</returns>
        public static IEnumerable Cast(this IEnumerable source, Type resultType)
        {
            if (!s_enumerableCastMethods.TryGetValue(resultType.GUID, out Invoker castMethod))
            {
                castMethod = typeof(Enumerable).GetMethod("Cast", BindingFlags.Public | BindingFlags.Static).Invoke;
                lock (s_enumerableCastMethods)
                    s_enumerableCastMethods.Add(resultType.GUID, castMethod);
            }
            return (IEnumerable)(castMethod(null, new object[] { source }) ?? throw new NullReferenceException("Invoking the invoker failed."));
        }
        #endregion

        #region Convert collections
        private static readonly MethodInfo s_enumerableToListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList), BindingFlags.Public | BindingFlags.Static),
                                           s_enumerableToArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray), BindingFlags.Public | BindingFlags.Static);
        private static readonly Dictionary<Guid, Invoker> s_toListMethods = new Dictionary<Guid, Invoker>(),
                                                          s_toArrayMethods = new Dictionary<Guid, Invoker>();
        /// <summary>
        /// Converts a <see cref="IEnumerable{TSource}"/> to <see cref="List{TSource}"/> of the same TSource. 
        /// </summary>
        /// <param name="enumerable">The source <see cref="IEnumerable{T}"/> for the conversion.</param>
        /// <param name="sourceType">The type of TSource.</param>
        /// <returns>A <see cref="List{TSource}"/> with the data of <paramref name="enumerable"/>.</returns>
        public static object? ToGenericList(in object enumerable, out Type sourceType)
        {
            sourceType = TypeExtention.GetEnumerableElementType(enumerable.GetType());
            return MakeToListMethod(sourceType).Invoke(null, new[] { enumerable });
        }

        /// <summary>
        /// Converts a <see cref="IEnumerable{TSource}"/> to <see cref="T[]"/> of the same TSource. 
        /// </summary>
        /// <param name="enumerable">The source <see cref="IEnumerable{T}"/> for the conversion.</param>
        /// <param name="sourceType">The type of TSource.</param>
        /// <returns>A <see cref="List{TSource}"/> with the data of <paramref name="enumerable"/>.</returns>
        public static object? ToGenericArray(in object enumerable, out Type sourceType)
        {
            sourceType = TypeExtention.GetEnumerableElementType(enumerable.GetType());
            return MakeToArrayMethod(sourceType).Invoke(null, new[] { enumerable });
        }

        internal static Invoker MakeToListMethod(in Type sourceType)
        {
            if (s_toListMethods.TryGetValue(sourceType.GUID, out Invoker genericToListMethod))
                return genericToListMethod;
            // Make generic method Enumerable.ToList<TSource>(IEnumerable<TSource)
            genericToListMethod = s_enumerableToListMethod.MakeGenericMethod(sourceType).Invoke;
            lock (s_toListMethods)
                s_toListMethods.Add(sourceType.GUID, genericToListMethod);
            return genericToListMethod;
        }

        internal static Invoker MakeToArrayMethod(in Type sourceType)
        {
            if (s_toArrayMethods.TryGetValue(sourceType.GUID, out Invoker genericToArrayMethod))
                return genericToArrayMethod;
            // Make generic method Enumerable.ToArray<TSource>(IEnumerable<TSource)
            genericToArrayMethod = s_enumerableToArrayMethod.MakeGenericMethod(sourceType).Invoke;
            lock (s_toArrayMethods)
                s_toArrayMethods.Add(sourceType.GUID, genericToArrayMethod);
            return genericToArrayMethod;
        }
        #endregion

        #region Constructors
        private static readonly Dictionary<Guid, Invoker> s_listConstructors = new Dictionary<Guid, Invoker>();

        /// <summary>
        /// Initializes a new instance of <see cref="List{}"/> with the specified generic type.
        /// </summary>
        /// <param name="elementType">The generic type argument.</param>
        /// <returns>A new instance of <see cref="List{}"/>.</returns>
        public static IList MakeList(Type elementType)
        {
            return (IList)(MakeListCtor(elementType).Invoke(null, null) ?? throw new NullReferenceException("Invoking the invoker failed."));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="List{}"/> with the specified generic type and initial capacity.
        /// </summary>
        /// <param name="elementType">The generic type argument.</param>
        /// <param name="capacity">The intial capacity.</param>
        /// <returns>A new instance of <see cref="List{}"/>.</returns>
        public static IList MakeList(Type elementType, int capacity)
        {
            return (IList)(MakeListCtor(elementType, new[] { typeof(int) }).Invoke(null, new object[] { capacity }) ?? throw new NullReferenceException("Invoking the invoker failed."));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="List{}"/> with the specified generic type and initial capacity.
        /// </summary>
        /// <param name="elementType">The generic type argument.</param>
        /// <param name="collection">The <see cref="IEnumerable{}"/> with the same element type as specifed for the <see cref="List{}"/>.</param>
        /// <returns>A new instance of <see cref="List{}"/>.</returns>
        public static IList MakeList(Type elementType, IEnumerable collection)
        {
            return (IList)(MakeListCtor(elementType, new[] { typeof(IEnumerable<>).MakeGenericType(elementType) }).Invoke(null, new object[] { collection }) ?? throw new NullReferenceException("Invoking the invoker failed."));
        }

        private static Invoker MakeListCtor(Type elementType, Type[]? parameterTypes = null)
        {
            if (!s_listConstructors.TryGetValue(elementType.GUID, out Invoker ctor))
            {
                ctor = typeof(List<>).MakeGenericType(elementType).GetConstructor(parameterTypes ?? Array.Empty<Type>()).Invoke;
                lock (s_listConstructors)
                    s_listConstructors.Add(elementType.GUID, ctor);
            }
            return ctor;
        }
        #endregion
    }
}
