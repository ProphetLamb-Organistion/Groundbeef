using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ProphetLamb.Tools.Collections
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class CollectionHashing
    {
        /// <summary>
        /// Serves as the default hash function. If <paramref name="fromValues"/> is <see cref="true"/> returns the combined hashcode of all elements widthin the <see cref="Array"/>; otherwise returns the default hashcode.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="fromValues">Indicates that the hashcode should be derived from the elements of the array instead.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int GetHashCode(this Array array, bool fromValues)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (!fromValues)
                return array.GetHashCode();
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            int c = 0;
            for (int i = 0; i < length; i++)
            {
                object value = array.GetValue(i) ?? throw new NullReferenceException(ExceptionResource.VALUE_NOTNULL);
                c = CombineHashCodes(c, value.GetHashCode());
            }
            return c;
        }

        /// <summary>
        /// Serves as the default hash function. If <paramref name="fromValues"/> is <see cref="true"/> returns the combined hashcode of all elements widthin the <see cref="ReadOnlySpan{T}"/>; otherwise returns the default hashcode.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="fromValues">Indicates that the hashcode should be derived from the elements of the array instead.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int GetHashCode<T>(this ReadOnlySpan<T> span, bool fromValues)
        {
            int length = span.Length;
            if (!fromValues)
                return span.GetHashCode();
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            int c = 0;
            for (int i = 0; i < length; i++)
            {
                object value = span[i] ?? throw new NullReferenceException(ExceptionResource.VALUE_NOTNULL);
                c = CombineHashCodes(c, value.GetHashCode());
            }
            return c;
        }

        /// <summary>
        /// Serves as the default hash function. If <paramref name="fromValues"/> is <see cref="true"/> returns the combined hashcode of all elements widthin the <see cref="IList"/>; otherwise returns the default hashcode.
        /// </summary>
        /// <param name="list">The list containing the elements.</param>
        /// <param name="fromValues">Indicates that the hashcode should be derived from the elements of the list instead.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int GetHashCode(this IList list, bool fromValues)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (!fromValues)
                return list.GetHashCode();
            int length = list.Count;
            if (length == 0)
                throw new ArgumentException(nameof(list), ExceptionResource.LIST_NOTEMPTY);
            int c = 0;
            for (int i = 0; i < length; i++)
            {
                object value = list[i] ?? throw new NullReferenceException(ExceptionResource.VALUE_NOTNULL);
                c = CombineHashCodes(c, value.GetHashCode());
            }
            return c;
        }

        /// <summary>
        /// Serves as the default hash function. If <paramref name="fromValues"/> is <see cref="true"/> returns the combined hashcode of all elements widthin the <see cref="IEnumerable"/>; otherwise returns the default hashcode.
        /// </summary>
        /// <param name="enumerable">The enumerable containing the elements.</param>
        /// <param name="fromValues">Indicates that the hashcode should be derived from the elements of the enumerable instead.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int GetHashCode(this IEnumerable enumerable, bool fromValues)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));
            if (!fromValues)
                return enumerable.GetHashCode();
            int c = 0;
            IEnumerator en = enumerable.GetEnumerator();
            if (!en.MoveNext())
                throw new ArgumentException(nameof(enumerable), ExceptionResource.ENUMERABLE_NOTEMPTY);
            do
            {
                object value = en.Current ?? throw new NullReferenceException(ExceptionResource.VALUE_NOTNULL);
                c = CombineHashCodes(c, value.GetHashCode());
            }
            while (en.MoveNext());
            return c;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        internal static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }
    }
}