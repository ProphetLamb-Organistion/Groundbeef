using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProphetLamb.Tools.Core
{
    /// <summary>
    /// Collection of extention functions for arrays, and generic arrays: 
    /// SortByKeys(keys), FindFirst(predicate), FindLast(predicate), FindAll(predicate), IndexOf(element|predicate), IndexOfLast(element|predicate), IndexOfAll(element|predicate), GetHashCode(fromValues)
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class ArrayExtention
    {
        /// <summary>
        /// Sorts a one-dimesional array into a new array by swapping each element to the index indicated by <paramref name="keys"/>. 
        /// The length of both arrays must be equal.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that contains the elements to be sorted.</param>
        /// <param name="keys">The one-dimensional <see cref="int[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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
        /// <param name="keys">The one-dimensional <see cref="int[]"/> that contains indicies.</param>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object FindFirst(this Array array, in Predicate<object> match)
        {
            return FindFirst(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object FindFirst(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindFirst(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static object FindFirst(this Array array, int startIndex, int count, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                object value = array.GetValue(i);
                if (match(value)) return value;
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindFirst<T>(this T[] array, in Predicate<T> match)
        {
            return FindFirst(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindFirst<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindFirst(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static T FindFirst<T>(this T[] array, int startIndex, int count, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array[i])) return array[i];
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindFirst<T>(this ReadOnlySpan<T> span, in Predicate<T> match)
        {
            return FindFirst(span, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindFirst<T>(this ReadOnlySpan<T> span, int startIndex, in Predicate<T> match)
        {
            return FindFirst(span, startIndex, span.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The first occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static T FindFirst<T>(this ReadOnlySpan<T> span, int startIndex, int count, in Predicate<T> match)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(span[i])) return span[i];
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object FindLast(this Array array, in Predicate<object> match)
        {
            return FindLast(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object FindLast(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindLast(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static object FindLast(this Array array, int startIndex, int count, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                object value = array.GetValue(i);
                if (match(value)) return value;
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindLast<T>(this T[] array, in Predicate<T> match)
        {
            return FindLast(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindLast<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindLast(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static T FindLast<T>(this T[] array, int startIndex, int count, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(array[i])) return array[i];
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindLast<T>(this ReadOnlySpan<T> span, in Predicate<T> match)
        {
            return FindLast(span, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindLast<T>(this ReadOnlySpan<T> span, int startIndex, in Predicate<T> match)
        {
            return FindLast(span, startIndex, span.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The last occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static T FindLast<T>(this ReadOnlySpan<T> span, int startIndex, int count, in Predicate<T> match)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(span[i])) return span[i];
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<object> FindAll(this Array array, in Predicate<object> match)
        {
            return FindAll(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<object> FindAll(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindAll(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<object> FindAll(this Array array, int startIndex, int count, Predicate<object> match)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                object value = array.GetValue(i);
                if (match(value)) yield return value;
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> FindAll<T>(this T[] array, in Predicate<T> match)
        {
            return FindAll(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> FindAll<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return FindAll(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<T> FindAll<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array[i])) yield return array[i];
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this Array array, in Predicate<object> match)
        {
            return IndexOf(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOf(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOf(this Array array, int startIndex, int count, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array.GetValue(i))) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this T[] array, in Predicate<T> match)
        {
            return IndexOf(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOf(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOf<T>(this T[] array, int startIndex, int count, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this T[] array, T element)
        {
            return IndexOf(array, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this T[] array, int startIndex, T element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOf(array, startIndex, array.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOf<T>(this T[] array, int startIndex, int count, T element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (comparer.Equals(array[i], element)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, in Predicate<T> match)
        {
            return IndexOf(span, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, in Predicate<T> match)
        {
            return IndexOf(span, startIndex, span.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, int count, in Predicate<T> match)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(span[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T element)
        {
            return IndexOf(span, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, T element)
        {
            return IndexOf(span, startIndex, span.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOf<T>(this ReadOnlySpan<T> span, int startIndex, int count, T element)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (comparer.Equals(span[i], element)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast(this Array array, in Predicate<object> match)
        {
            return IndexOfLast(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfLast(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOfLast(this Array array, int startIndex, int count, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(array.GetValue(i))) return i;
            }
            return -1;
        }


        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this T[] array, in Predicate<T> match)
        {
            return IndexOfLast(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfLast(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOfLast<T>(this T[] array, int startIndex, int count, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(array[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this T[] array, T element)
        {
            return IndexOfLast(array, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this T[] array, int startIndex, T element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfLast(array, startIndex, array.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOfLast<T>(this T[] array, int startIndex, int count, T element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (comparer.Equals(array[i], element)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, in Predicate<T> match)
        {
            return IndexOfLast(span, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int startIndex, in Predicate<T> match)
        {
            return IndexOfLast(span, startIndex, span.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int startIndex, int count, in Predicate<T> match)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(span[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, T element)
        {
            return IndexOfLast(span, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int startIndex, T element)
        {
            return IndexOf(span, startIndex, span.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ReadOnlySpan{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="span">The span containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static int IndexOfLast<T>(this ReadOnlySpan<T> span, int startIndex, int count, T element)
        {
            int length = span.Length;
            if (length == 0)
                throw new ArgumentException(nameof(span), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (comparer.Equals(span[i], element)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll(this Array array, in Predicate<object> match)
        {
            return IndexOfAll(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll(this Array array, int startIndex, in Predicate<object> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfAll(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<int> IndexOfAll(this Array array, int startIndex, int count, Predicate<object> match)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array.GetValue(i))) yield return i;
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll(this Array array, in object element)
        {
            return IndexOfAll(array, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll(this Array array, int startIndex, in object element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfAll(array, startIndex, array.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<int> IndexOfAll(this Array array, int startIndex, int count, object element)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<object> comparer = EqualityComparer<object>.Default;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (comparer.Equals(array.GetValue(i), element)) yield return i;
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, in Predicate<T> match)
        {
            return IndexOfAll(array, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int startIndex, in Predicate<T> match)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfAll(array, startIndex, array.Length - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array[i])) yield return i;
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, T element)
        {
            return IndexOfAll(array, 0, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int startIndex, T element)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return IndexOfAll(array, startIndex, array.Length - startIndex, element);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="Array"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The one-dimensional array containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="element">The object to locate in the <see cref="Array"/>.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static IEnumerable<int> IndexOfAll<T>(this T[] array, int startIndex, int count, T element)
        {
            int length = array.Length;
            if (length == 0)
                throw new ArgumentException(nameof(array), ExceptionResource.ARRAY_NOTEMPTY);
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (comparer.Equals(array[i], element)) yield return i;
            }
        }

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
