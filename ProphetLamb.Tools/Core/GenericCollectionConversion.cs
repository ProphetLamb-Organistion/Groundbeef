using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace ProphetLamb.Tools.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class GenericCollectionConversion
    {
        public static readonly MethodInfo EnumerableToListMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToList), BindingFlags.Public | BindingFlags.Static),
                                          EnumerableToArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray), BindingFlags.Public | BindingFlags.Static);
        public static ConcurrentDictionary<Guid, MethodInfo> GenericToListMethodsDictionary = new ConcurrentDictionary<Guid, MethodInfo>();
        public static ConcurrentDictionary<Guid, MethodInfo> GenericToArrayMethodsDictionary = new ConcurrentDictionary<Guid, MethodInfo>();

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
            if (GenericToListMethodsDictionary.TryGetValue(sourceType.GUID, out MethodInfo genericToListMethod))
                return genericToListMethod;
            // Make generic method Enumerable.ToList<TSource>(IEnumerable<TSource)
            genericToListMethod = EnumerableToListMethod.MakeGenericMethod(sourceType);
            GenericToListMethodsDictionary.Add(sourceType.GUID, genericToListMethod);
            return genericToListMethod;
        }

        public static MethodInfo MakeToArrayMethod(in Type sourceType)
        {
            if (GenericToArrayMethodsDictionary.TryGetValue(sourceType.GUID, out MethodInfo genericToArrayMethod))
                return genericToArrayMethod;
            // Make generic method Enumerable.ToArray<TSource>(IEnumerable<TSource)
            genericToArrayMethod = EnumerableToArrayMethod.MakeGenericMethod(sourceType);
            GenericToArrayMethodsDictionary.Add(sourceType.GUID, genericToArrayMethod);
            return genericToArrayMethod;
        }
    }
}
