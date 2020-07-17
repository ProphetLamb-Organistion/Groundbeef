#nullable enable

using System;
using System.Collections.Generic;

namespace ProphetLamb.Tools.Collections
{
    public static partial class ArrayExtention
    {
        /// <summary>
        /// Sorts aspan into a new span by swapping each element to the index indicated by <paramref name="keys"/>. 
        /// The length of both spans must be equal.
        /// </summary>
        /// <param name="span">The one-dimensional <see cref="ReadOnlySpan{T}"/> that contains the elements to be sorted.</param>
        /// <param name="keys">The one-dimensional <see cref="Int32[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        public static Span<T> SortByKeys<T>(this ReadOnlySpan<T> span, in int[] keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            if (span.Length == 0) throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (keys.Length == 0) throw new ArgumentException(nameof(keys), ExceptionResource.ARRAY_NOTEMPTY);
            var newArray = new T[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                newArray.SetValue(span[keys[i]], i);
            }
            return newArray;
        }

        #region Predicate
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int index, int count, Predicate<object?> match)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            for (int i = index; i < endIndex; i++)
            {
                if (match(span[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int index, int count, Predicate<object?> match)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (match(span[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static Span<int> IndexOfAll<T>(this ReadOnlySpan<T> span, int index, int count, Predicate<object?> match)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            var queue = new Queue<int>();
            for (int i = index; i < endIndex; i++)
            {
                if (match(span[i]))
                    queue.Enqueue(i);
            }
            return queue.ToArray();
        }

        /// <summary>
        /// Searches for the specified object and returns all occurrences within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>All occurrences of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static Span<T> FindAll<T>(this ReadOnlySpan<T> span, int index, int count, Predicate<object?> match)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            var queue = new Queue<T>();
            for (int i = index; i < endIndex; i++)
            {
                T value = span[i];
                if (match(value))
                    queue.Enqueue(value);
            }
            return queue.ToArray();
        }
        #endregion

        #region Element
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int index, int count, in T match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(span[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements, if found; otherwise, -1.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int index, int count, in T match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            for (int i = endIndex - 1; i >= index; i--)
            {
                if (l_match.Equals(span[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the all occurrences within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="span">The source span.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate<object>"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the all occurrences of item within the range of elements in the <see cref="ReadOnlySpan{T}"/> that starts at index and contains count number of elements.</returns>
        /// <exception name="ArgumentNullException">match is null<exception/>
        /// <exception name="ArgumentOutOfRangeException">index < 0 || count < 0<exception/>
        /// <exception name="IndexOutOfRangeException">span.length < index + count<exception/>
        public static Span<int> IndexOfAll<T>(this ReadOnlySpan<T> span, int index, int count, T match)
        {
            object l_match = match??throw new ArgumentNullException(nameof(match));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = index + count;
            if (span.Length < endIndex)
                throw new IndexOutOfRangeException();
            var queue = new Queue<int>();
            for (int i = index; i < endIndex; i++)
            {
                if (l_match.Equals(span[i]))
                    queue.Enqueue(i);
            }
            return queue.ToArray();
        }
        #endregion
    }
}

#nullable disable