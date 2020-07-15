using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProphetLamb.Tools.Collections.Concurrent;

namespace ProphetLamb.Tools.Collections
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class GenericCollectionConversion
    {
        private static readonly MethodInfo enumerableToListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList), BindingFlags.Public | BindingFlags.Static),
                                           enumerableToArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray), BindingFlags.Public | BindingFlags.Static);
        private static readonly ConcurrentDictionary<Guid, MethodInfo> genericToListMethodsDictionary = new ConcurrentDictionary<Guid, MethodInfo>(),
                                                                       genericToArrayMethodsDictionary = new ConcurrentDictionary<Guid, MethodInfo>();

        /// <summary>
        /// Converts a <see cref="IEnumerable{TSource}"/> to <see cref="List{TSource}"/> of the same TSource. 
        /// </summary>
        /// <param name="enumerable">The source <see cref="IEnumerable{T}"/> for the conversion.</param>
        /// <param name="sourceType">The type of TSource.</param>
        /// <returns>A <see cref="List{TSource}"/> with the data of <paramref name="enumerable"/>.</returns>
        public static object ToGenericList(in object enumerable, out Type sourceType)
        {
            sourceType = TypeHelper.GetEnumerableBaseType(enumerable.GetType());
            return MakeToListMethod(sourceType).Invoke(null, new[] { enumerable });
        }

        /// <summary>
        /// Converts a <see cref="IEnumerable{TSource}"/> to <see cref="T[]"/> of the same TSource. 
        /// </summary>
        /// <param name="enumerable">The source <see cref="IEnumerable{T}"/> for the conversion.</param>
        /// <param name="sourceType">The type of TSource.</param>
        /// <returns>A <see cref="List{TSource}"/> with the data of <paramref name="enumerable"/>.</returns>
        public static object ToGenericArray(in object enumerable, out Type sourceType)
        {
            sourceType = TypeHelper.GetEnumerableBaseType(enumerable.GetType());
            return MakeToArrayMethod(sourceType).Invoke(null, new[] { enumerable });
        }

        public static MethodInfo MakeToListMethod(in Type sourceType)
        {
            if (genericToListMethodsDictionary.TryGetValue(sourceType.GUID, out MethodInfo genericToListMethod))
                return genericToListMethod;
            // Make generic method Enumerable.ToList<TSource>(IEnumerable<TSource)
            genericToListMethod = enumerableToListMethod.MakeGenericMethod(sourceType);
            genericToListMethodsDictionary.Add(sourceType.GUID, genericToListMethod);
            return genericToListMethod;
        }

        public static MethodInfo MakeToArrayMethod(in Type sourceType)
        {
            if (genericToArrayMethodsDictionary.TryGetValue(sourceType.GUID, out MethodInfo genericToArrayMethod))
                return genericToArrayMethod;
            // Make generic method Enumerable.ToArray<TSource>(IEnumerable<TSource)
            genericToArrayMethod = enumerableToArrayMethod.MakeGenericMethod(sourceType);
            genericToArrayMethodsDictionary.Add(sourceType.GUID, genericToArrayMethod);
            return genericToArrayMethod;
        }
    }
}
