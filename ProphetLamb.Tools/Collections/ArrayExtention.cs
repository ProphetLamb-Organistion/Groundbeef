using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProphetLamb.Tools.Collections
{
    /// <summary>
    /// Collection of extention functions for arrays, and generic arrays: 
    /// SortByKeys(keys), FindFirst(predicate), FindLast(predicate), FindAll(predicate), IndexOf(element|predicate), IndexOfLast(element|predicate), IndexOfAll(element|predicate), GetHashCode(fromValues)
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class ArrayExtention
    {
        #region SortByKeys
        /// <summary>
        /// Sorts a one-dimesional array into a new array by swapping each element to the index indicated by <paramref name="keys"/>. 
        /// The length of both arrays must be equal.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T[]"/> that contains the elements to be sorted.</param>
        /// <param name="keys">The one-dimensional <see cref="Int32[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        public static T[] SortByKeys<T>(this T[] array, in int[] keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (keys.Length == 0) throw new ArgumentException(nameof(keys), ExceptionResource.ARRAY_NOTEMPTY);
            var newArray = new T[array.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                newArray.SetValue(array.GetValue(keys[i]), i);
            }
            return newArray;
        }

        /// <summary>
        /// Sorts a one-dimesional array by swapping each element at <paramref name="items"/> to the index indicated by <paramref name="keys"/>. 
        /// The length of both arrays must be equal.
        /// </summary>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> that contains the elements to be sorted.</param>
        /// <param name="keys">The one-dimensional <see cref="Int32[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        public static T[] SortByKeys<T>(this ReadOnlySpan<T> span, in int[] keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (span.Length == 0) throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (keys.Length == 0) throw new ArgumentException(nameof(keys), ExceptionResource.ARRAY_NOTEMPTY);
            var newArray = new T[span.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                newArray.SetValue(span[keys[i]], i);
            }
            return newArray;
        }
        #endregion

        #region GetHashCode
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
        #endregion

        public const int ParallelizationLength = 1024;

        #region IndexOf
        public static unsafe int IndexOf(this Array array, int offset, int count, in Predicate<object> match, bool allowParallel = true)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = offset + count;
            if (array.Length < endIndex)
                throw new IndexOutOfRangeException();
            int index = -1;
            if (count >= ParallelizationLength && allowParallel)
            {
                var cts = new CancellationTokenSource();
                Parallel.For(offset, endIndex, CreateOptions(cts), (int i) => 
                {
                    if (match(array.GetValue(i)))
                    {
                        index = i;
                        cts.Cancel();
                    }
                });
            }
            else
            {
                for (int i = offset; i < endIndex; i++)
                {
                    if (match(array.GetValue(i)))
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }
        #endregion

        internal static ParallelOptions CreateOptions(CancellationTokenSource src)
        {
            return new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount * 2, CancellationToken = src.Token};
        }
    }
}
