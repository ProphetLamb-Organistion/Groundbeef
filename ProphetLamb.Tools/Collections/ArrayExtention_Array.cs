#nullable enable

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProphetLamb.Tools.Collections
{
    public static partial class ArrayExtention
    {
        /// <summary>
        /// Sorts a one-dimesional array into a new array by swapping each element to the index indicated by <paramref name="keys"/>. 
        /// The length of both arrays must be equal.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that contains the elements to be sorted.</param>
        /// <param name="keys">The one-dimensional <see cref="Int32[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        public static Array SortByKeys(this Array array, in int[] keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (keys.Length == 0) throw new ArgumentException(nameof(keys), ExceptionResource.ARRAY_NOTEMPTY);
            Array newArray = Array.CreateInstance(typeof(object), keys.Length);
            for (int i = 0; i < keys.Length; i++)
            {
                newArray.SetValue(array.GetValue(keys[i]), i);
            }
            return newArray;
        }

        #region Predicate
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOf(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (match(array.GetValue(i)))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (match(array.GetValue(i)))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable<int> IndexOfAll(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (match(array.GetValue(i)))
                    yield return i;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int ParallelIndexOfAny(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            int ind = -1;
            var cts = new CancellationTokenSource();
            try
            {
                Parallel.For(index, endIndex, DefaultOptions(cts), (int i) => 
                {
                    if (match(array.GetValue(i)))
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
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int[] ParallelIndexOfAll(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<int>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) => 
            {
                if (match(array.GetValue(i)))
                    queue.Enqueue(i);
            });
            return queue.ToArray();
        }

        /// <summary>
        /// Searches for the specified object and returns the first occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static object? Find(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                object? value = array.GetValue(i);
                if (match(value))
                    return value;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the last occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static object? FindLast(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                object? value = array.GetValue(i);
                if (match(value))
                    return value;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable FindAll(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                object? value = array.GetValue(i);
                if (match(value))
                    yield return value;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns any occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>Any occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static object? ParallelFindAny(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);;
            int ind = -1;
            var cts = new CancellationTokenSource();
            try
            {
                Parallel.For(index, endIndex, DefaultOptions(cts), (int i) => 
                {
                    if (match(array.GetValue(i)))
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
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static Array ParallelFindAll(this Array array, int index, int count, Predicate<object?> match)
        {
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<object?>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) => 
            {
                object? value = array.GetValue(i);
                if (match(value))
                    queue.Enqueue(value);
            });
            return queue.ToArray();
        }
        #endregion

        #region Element
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOf(this Array array, int index, int count, in object? match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(array.GetValue(i)))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int IndexOfLast(this Array array, int index, int count, in object? match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (l_match.Equals(array.GetValue(i)))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static IEnumerable<int> IndexOfAll(this Array array, int index, int count, object? match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(array.GetValue(i)))
                    yield return i;
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based indicies of any occurrence within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of any occurrence of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int ParallelIndexOfAny(this Array array, int index, int count, in object? match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            int ind = -1;
            var cts = new CancellationTokenSource();
            try
            {
                Parallel.For(index, endIndex, DefaultOptions(cts), (int i) => 
                {
                    if (l_match.Equals(array.GetValue(i)))
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
        /// Searches for the specified object and returns the zero-based indicies of the all occurrences within the range of elements in the <see cref="Array"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based indicies of the all occurrences of item within the range of elements in the <see cref="Array"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">array.length < index + count<exception/>
        public static int[] ParallelIndexOfAll(this Array array, int index, int count, in object? match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            ValidateAngGetEndIndex(array, index, count, match, out int endIndex);
            var queue = new ConcurrentQueue<int>();
            Parallel.For(index, endIndex, DefaultOptions(), (int i) => 
            {
                if (l_match.Equals(array.GetValue(i)))
                    queue.Enqueue(i);
            });
            return queue.ToArray();
        }
        #endregion
    }
}

#nullable disable