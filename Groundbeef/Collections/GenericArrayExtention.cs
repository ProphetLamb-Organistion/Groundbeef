#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Groundbeef.Collections
{
    public static partial class ArrayExtention
    {
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

        #region Predicate
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/>, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int IndexOf<T>(this T[] array, Predicate<T> match) => IndexOf(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int IndexOf<T>(this T[] array, int index, Predicate<T> match) => IndexOf(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOf<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (match(array[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/>, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int IndexOfLast<T>(this T[] array, Predicate<T> match) => IndexOfLast(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast<T>(this T[] array, int index, Predicate<T> match) => IndexOfLast(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (match(array[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/>.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, Predicate<T> match) => IndexOfAll(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int index, Predicate<T> match) => IndexOfAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (match(array[i]))
                    yield return i;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/>, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, Predicate<T> match) => ParallelIndexOfAny(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, int index, Predicate<T> match) => ParallelIndexOfAny(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            int ind = -1;
            var cts = new CancellationTokenSource();
            try
            {
                Parallel.For(index, endIndex, DefaultOptions(cts), (int i) =>
                {
                    if (match(array[i]))
                    {
                        ind = i;
                        cts.Cancel();
                    }
                });
            }
            catch (OperationCanceledException) { }
            return ind;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/>.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, Predicate<T> match) => ParallelIndexOfAll(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, int index, Predicate<T> match) => ParallelIndexOfAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<int>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) =>
            {
                if (match(array[i]))
                    queue.Enqueue(i);
            });
            return queue.ToArray();
        }

        /// <summary>
        /// Searches for the specified object and returns the first occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first occurrence of item within the range of elements in the <see cref="T[]"/>, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static T Find<T>(this T[] array, Predicate<T> match) => Find(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static T Find<T>(this T[] array, int index, Predicate<T> match) => Find(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        /// <exception cref="InvalidOperationException">No match found</exception>
        public static T Find<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                T value = array[i];
                if (match(value))
                    return value;
            }
            throw new InvalidOperationException("Value not found");
        }

        /// <summary>
        /// Searches for the specified object and returns the last occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last occurrence of item within the range of elements in the <see cref="T[]"/>, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static T FindLast<T>(this T[] array, Predicate<T> match) => FindLast(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static T FindLast<T>(this T[] array, int index, Predicate<T> match) => FindLast(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        /// <exception cref="InvalidOperationException">No match found</exception>
        public static T FindLast<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                T value = array[i];
                if (match(value))
                    return value;
            }
            throw new InvalidOperationException("Value not found");
        }

        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/>.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static IEnumerable<T> FindAll<T>(this T[] array, Predicate<T> match) => FindAll(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index.<returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static IEnumerable<T> FindAll<T>(this T[] array, int index, Predicate<T> match) => FindAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable<T> FindAll<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                T value = array[i];
                if (match(value))
                    yield return value;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns any occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>Any occurrence of item within the range of elements in the <see cref="T[]"/> if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static T ParallelFindAny<T>(this T[] array, Predicate<T> match) => ParallelFindAny(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>Any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static T ParallelFindAny<T>(this T[] array, int index, Predicate<T> match) => ParallelFindAny(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>Any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static T ParallelFindAny<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            int i = ParallelIndexOfAny(array, index, count, match);
            return i == -1 ? throw new InvalidOperationException("Value not found.") : array[i];
        }

        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/>.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static Array ParallelFindAll<T>(this T[] array, Predicate<T> match) => ParallelFindAll(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static Array ParallelFindAll<T>(this T[] array, int index, Predicate<T> match) => ParallelFindAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static Array ParallelFindAll<T>(this T[] array, int index, int count, Predicate<T> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<T>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) =>
            {
                T value = array[i];
                if (match(value))
                    queue.Enqueue(value);
            });
            return queue.ToArray();
        }
        #endregion

        #region Element
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int IndexOf<T>(this T[] array, in T match) => IndexOf(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int IndexOf<T>(this T[] array, int index, in T match) => IndexOf(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOf<T>(this T[] array, int index, int count, in T match)
        {
            object l_match = match ?? throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(array[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast<T>(this T[] array, in T match) => IndexOfLast(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int IndexOfLast<T>(this T[] array, int index, in T match) => IndexOfLast(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast<T>(this T[] array, int index, int count, in T match)
        {
            object l_match = match ?? throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (l_match.Equals(array[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, T match) => IndexOfAll(array, 0, array.Length, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int index, T match) => IndexOfAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int index, int count, T match)
        {
            object l_match = match ?? throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(array[i]))
                    yield return i;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, in T match) => ParallelIndexOfAny(array, 0, array.Length, match);

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, int index, in T match) => ParallelIndexOfAny(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int ParallelIndexOfAny<T>(this T[] array, int index, int count, in T match)
        {
            object l_match = match ?? throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            int ind = -1;
            var cts = new CancellationTokenSource();
            try
            {
                Parallel.For(index, endIndex, DefaultOptions(cts), (int i) =>
                {
                    if (l_match.Equals(array[i]))
                    {
                        ind = i;
                        cts.Cancel();
                    }
                });
            }
            catch (OperationCanceledException) { }
            return ind;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/>.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, in T match) => ParallelIndexOfAll(array, 0, array.Length, match);

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, int index, in T match) => ParallelIndexOfAll(array, index, array.Length - index, match);
        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="T[]"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="T[]"/> that starts at index and contains count number of elements.</returns>
        /// <exception cref="ArgumentNullException">match is null<exception/>
        /// <exception cref="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception cref="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int[] ParallelIndexOfAll<T>(this T[] array, int index, int count, in T match)
        {
            object l_match = match ?? throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<int>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) =>
            {
                if (l_match.Equals(array[i]))
                    queue.Enqueue(i);
            });
            return queue.ToArray();
        }
        #endregion
    }
}

#nullable disable