using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Groundbeef.Collections
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class CollectionExtention
    {
        /// <summary>
        /// Adds a range of elements to the collection by repeatetly calling the <see cref="IList{T}.Add(T)"/> function.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="target">The target <see cref="ICollection{T}"/>.</param>
        /// <param name="source">The source <see cref="IEnumerable{T}"/></param>
        /// <exception cref="ArgumentNullException">If the <paramref name="source"/> or <paramref name="target"/> is null.</exception>
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (source is null) throw new ArgumentNullException(nameof(source));
            using var en = source.GetEnumerator();
            while (en.MoveNext()) target.Add(en.Current);
        }

        /// <summary>
        /// Adds a range of elements to the collection by repeatetly calling the <see cref="IList{T}.Add(T)"/> function using the <paramref name="selector"/> to convert the elements.
        /// </summary>
        /// <typeparam name="TTarget">The type of elements in the <paramref name="target"/>.</typeparam>
        /// <typeparam name="TSource">The type of elements in the <paramref name="source"/>.</typeparam>
        /// <param name="target">The target <see cref="ICollection{TTarget}"/>.</param>
        /// <param name="source">The source <see cref="IEnumerable{TSource}"/></param>
        /// <exception cref="ArgumentNullException">If the <paramref name="source"/>, <paramref name="target"/> or <paramref name="selector"/> is null.</exception>
        public static void AddRange<TTarget, TSource>(this ICollection<TTarget> target, IEnumerable<TSource> source, Func<TSource, TTarget> selector)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            using var en = source.GetEnumerator();
            while (en.MoveNext()) target.Add(selector(en.Current));
        }

        /// <summary>
        /// Adds all days between <paramref name="start"/> and <paramref name="end"/> to the collection.
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <param name="start">The frist day that will be added to the collection.</param>
        /// <param name="end">The last day that will be added to the collection.</param>
        /// <returns>The reference to the <see cref="ICollection{DateTime}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="start"/> is greater then <paramref name="end"/>.</exception>
        public static ICollection<DateTime> AddDays(this ICollection<DateTime> collection, DateTime start, DateTime end)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (end.Date < start.Date)
                throw new ArgumentOutOfRangeException(nameof(end), "The end date must be greater or equal to the start date.");
            while (start.Date != end.Date)
            {
                collection.Add(start);
                // Increment
                start = start.AddDays(1);
            }
            collection.Add(start);
            return collection;
        }

        /// <summary>
        /// Adds and Removes elements from <paramref name="target"/> so that it only contains elements from <paramref name="source"/> constraint by <paramref name="filter"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="target">The target <see cref="IList{T}"/>.</param>
        /// <param name="source">The source<see cref="IList{T}"/>.</param>
        /// <param name="filter">The filter desciding which elements from <paramref name="source"/> to keep in <paramref name="target"/>.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="source"/>, <paramref name="target"/> or <paramref name="filter"/> is null.</exception>
        public static void Filter<T>(this IList<T> target, IList<T> source, Predicate<T> filter)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (filter is null) throw new ArgumentNullException(nameof(filter));
            for (int i = 0; i < source.Count; i++)
            {
                var index = target.IndexOf(source[i]);
                if (filter(source[i]))
                {
                    if (index == -1)
                        target.Add(source[i]);
                }
                else
                {
                    if (index != -1)
                        target.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOf<T>(this ICollection<T> collection, Predicate<T> match)
        {
            return IndexOf(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IndexOf<T>(this ICollection<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return IndexOf(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int IndexOf<T>(this ICollection<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(collection[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOf(this ICollection collection, Predicate<object> match)
        {
            return IndexOf(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IndexOf(this ICollection collection, int startIndex, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return IndexOf(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the first occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int IndexOf(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            int endIndex = startIndex + count;
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            int i = 0;
            var em = collection.GetEnumerator();
            while (i < startIndex && em.MoveNext()) i++; //skip until i=startindex
            while (i < endIndex && em.MoveNext()) //take until i=endindex-1
            {
                if (match(em.Current)) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOfLast<T>(this IList<T> collection, Predicate<T> match)
        {
            return IndexOfLast(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IndexOfLast<T>(this IList<T> collection, int startIndex, Predicate<T> match)
        {
            return IndexOfLast(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int IndexOfLast<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(collection[i])) return i;
            }
            return -1;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOfLast(this ICollection collection, Predicate<object> match)
        {
            return IndexOfLast(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int IndexOfLast(this ICollection collection, int startIndex, Predicate<object> match)
        {
            return IndexOfLast(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the zero-based index of the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The zero-based index of the last occurence of the specified element or -1 if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static int IndexOfLast(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEnumerator en = collection.GetEnumerator();
            int i = 0,
                last = -1;
            while (i < startIndex && en.MoveNext()) i++;
            while (i < endIndex && en.MoveNext())
            {
                if (match(en.Current)) last = i;
                i++;
            }
            return last;
        }

        /// <summary>
        /// Searches the elements in the <see cref="IList{T}"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<int> IndexOfAll<T>(this IList<T> collection, in Predicate<T> match)
        {
            return IndexOfAll(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<int> IndexOfAll<T>(this IList<T> collection, int startIndex, in Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return IndexOfAll(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IEnumerable<int> IndexOfAll<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            int length = collection.Count;
            if (length == 0)
                throw new ArgumentException(nameof(collection), ExceptionResource.ARRAY_NOTEMPTY);
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
                if (match(collection[i])) yield return i;
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<int> IndexOfAll(this ICollection collection, in Predicate<object> match)
        {
            return IndexOfAll(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<int> IndexOfAll(this ICollection collection, int startIndex, in Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return IndexOfAll(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and enumerates the zero-based index of all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> use to locate the object.</param>
        /// <returns>The zero-based index of all occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IEnumerable<int> IndexOfAll(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            int length = collection.Count;
            if (length == 0)
                throw new ArgumentException(nameof(collection), ExceptionResource.ARRAY_NOTEMPTY);
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (endIndex > length)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEnumerator en = collection.GetEnumerator();
            int i = 0;
            while (i < startIndex && en.MoveNext()) i++;
            while (i < endIndex && en.MoveNext())
            {
                if (match(en.Current)) yield return i;
                i++;
            }
        }

        #nullable disable
        /// <summary>
        /// Searches the elements in the <see cref="IList{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Find<T>(this IList<T> collection, Predicate<T> match)
        {
            return Find(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T Find<T>(this IList<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return Find(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T Find<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(collection[i]))
                    return collection[i];
            }
            return default(T);
        }

        /// <summary>
        /// Searches the elements in the <see cref="IList{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T FindLast<T>(this IList<T> collection, Predicate<T> match)
        {
            return FindLast(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T FindLast<T>(this IList<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return FindLast(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T FindLast<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                if (match(collection[i]))
                    return collection[i];
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="IList{T}"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> FindAll<T>(this IList<T> collection, Predicate<T> match)
        {
            return FindAll(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> FindAll<T>(this IList<T> collection, int startIndex, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return FindAll(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="IList{T}"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IEnumerable<T> FindAll<T>(this IList<T> collection, int startIndex, int count, Predicate<T> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(collection[i]))
                    yield return collection[i];
            }
        }

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static object Find(this ICollection collection, Predicate<object> match)
        {
            return Find(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static object Find(this ICollection collection, int startIndex, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return Find(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the first occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static object Find(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEnumerator en = collection.GetEnumerator();
            int i = 0;
            while (i < startIndex && en.MoveNext()) i++;
            while (i < endIndex && en.MoveNext())
            {
                object current = en.Current;
                if (match(current))
                    return current;
                i++;
            }
            return default;
        }

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static object FindLast(this ICollection collection, Predicate<object> match)
        {
            return FindLast(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static object FindLast(this ICollection collection, int startIndex, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return FindLast(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and returns the last occurence.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>The frist occurence of the specified element or <see cref="default"/> if no match was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static object FindLast(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEnumerator en = collection.GetEnumerator();
            int i = 0;
            object last = default;
            while (i < startIndex && en.MoveNext()) i++;
            while (i < endIndex && en.MoveNext())
            {
                object current = en.Current;
                if (match(current))
                    last = current;
                i++;
            }
            return last;
        }
        #nullable enable

        /// <summary>
        /// Searches the elements in the <see cref="ICollection"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable FindAll(this ICollection collection, Predicate<object> match)
        {
            return FindAll(collection, 0, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable FindAll(this ICollection collection, int startIndex, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            return FindAll(collection, startIndex, collection.Count - startIndex, match);
        }

        /// <summary>
        /// Searches a portion of the elements in the <see cref="ICollection"/> for the specified element and enumerates all occurences.
        /// </summary>
        /// <param name="collection">The collection containing the elements.</param>
        /// <param name="startIndex">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="match">The <see cref="Predicate{Object}"/> use to locate the object.</param>
        /// <returns>All occurences of the specified element.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IEnumerable FindAll(this ICollection collection, int startIndex, int count, Predicate<object> match)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            int endIndex = startIndex + count;
            if (startIndex + count > collection.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            IEnumerator en = collection.GetEnumerator();
            int i = 0;
            while (i < startIndex && en.MoveNext()) i++;
            while (i < endIndex && en.MoveNext())
            {
                object current = en.Current;
                if (match(current))
                    yield return current;
                i++;
            }
        }

        /// <summary>
        /// Sorts the elements in the <see cref="IList{T}"/> using defaul comparer <see cref="Comparer{T}.Default"/> to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        public static void Sort<T>(this IList<T> collection) where T : IComparable<T>
        {
            Sort(collection, 0);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using defaul comparer <see cref="Comparer{T}.Default"/> to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex) where T : IComparable<T>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            Sort(collection, startIndex, collection.Count - startIndex);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using defaul comparer <see cref="Comparer{T}.Default"/> to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex, int count) where T : IComparable<T>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            IntrospectiveSort(collection, startIndex, count, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts the elements in the <see cref="IList{T}"/> using a provided <see cref="Comparison{T}"/> delegate to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        public static void Sort<T>(this IList<T> collection, Comparison<T> comparison)
        {
            Sort(collection, 0, comparison);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using a provided <see cref="Comparison{T}"/> delegate to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex, Comparison<T> comparison)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            Sort(collection, startIndex, collection.Count - startIndex, comparison);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using a provided <see cref="Comparison{T}"/> delegate to compare collection elements.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex, int count, Comparison<T> comparison)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (comparison == null)
                throw new ArgumentNullException(nameof(comparison));
            IntrospectiveSort(collection, startIndex, count, Comparer<T>.Create(comparison));
        }

        /// <summary>
        /// Sorts the elements in the <see cref="IList{T}"/> using the specified or default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used when comparing collection elements, or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public static void Sort<T>(this IList<T> collection, IComparer<T> comparer)
        {
            Sort(collection, 0, comparer);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using the specified or default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used when comparing collection elements, or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex, IComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            Sort(collection, startIndex, collection.Count - startIndex, comparer);
        }

        /// <summary>
        /// Sorts a portion of the elements in the <see cref="IList{T}"/> using the specified or default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">The collection to sort.</param>
        /// <param name="startIndex">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used when comparing collection elements, or null to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public static void Sort<T>(this IList<T> collection, int startIndex, int count, IComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (comparer == null)
                comparer = Comparer<T>.Default;
            IntrospectiveSort(collection, startIndex, count, comparer);
        }

        #region IntrospectiveSort
        // All source code in this region is based on the reference source of following classes:
        // System.Collections.Generic.GenericArraySortHelper<T> and System.Collections.Generic.IntrospectiveSortUtilities

        private const int IntrosortSizeThreshold = 16;

        internal static void IntrospectiveSort<T>(IList<T> keys, int left, int length, IComparer<T> comparer)
        {
            if (keys is null)
                throw new ArgumentNullException(nameof(keys));
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));
            if (left < 0)
                throw new ArgumentOutOfRangeException(nameof(left), ExceptionResource.INTEGER_POSITIVEZERO);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), ExceptionResource.INTEGER_POSITIVEZERO);
            if (length + left > keys.Count)
                throw new IndexOutOfRangeException(ExceptionResource.INDEX_UPPERLIMIT);
            if (length < 2)
                return;
            IntroSort(keys, left, length + left - 1, 2 * FloorLog2(keys.Count), comparer);
        }

        private static void IntroSort<T>(IList<T> keys, int lo, int hi, int depthLimit, IComparer<T> comparer)
        {
            while (hi > lo)
            {
                int partitionSize = hi - lo + 1;
                if (partitionSize <= IntrosortSizeThreshold)
                {
                    if (partitionSize == 1)
                        return;
                    if (partitionSize == 2)
                    {
                        SwapIfGreater(keys, comparer, lo, hi);
                        return;
                    }
                    if (partitionSize == 3)
                    {
                        SwapIfGreater(keys, comparer, lo, hi - 1);
                        SwapIfGreater(keys, comparer, lo, hi);
                        SwapIfGreater(keys, comparer, hi - 1, hi);
                        return;
                    }
                    InsertionSort(keys, lo, hi, comparer);
                    return;
                }

                if (depthLimit == 0)
                {
                    Heapsort(keys, lo, hi, comparer);
                    return;
                }
                depthLimit--;

                int p = PickPivotAndPartition(keys, lo, hi, comparer);
                // Note we've already partitioned around the pivot and do not have to move the pivot again.
                IntroSort(keys, p + 1, hi, depthLimit, comparer);
                hi = p - 1;
            }
        }

        private static void SwapIfGreater<T>(IList<T> keys, IComparer<T> comparer, int a, int b)
        {
            if (a != b)
            {
                if (comparer.Compare(keys[a], keys[b]) > 0)
                {
                    T key = keys[a];
                    keys[a] = keys[b];
                    keys[b] = key;
                }
            }
        }

        private static void InsertionSort<T>(IList<T> keys, int lo, int hi, IComparer<T> comparer)
        {
            int i, j;
            T t;
            for (i = lo; i < hi; i++)
            {
                j = i;
                t = keys[i + 1];
                while (j >= lo && comparer.Compare(t, keys[j]) < 0)
                {
                    keys[j + 1] = keys[j];
                    j--;
                }
                keys[j + 1] = t;
            }
        }

        private static void Heapsort<T>(IList<T> keys, int lo, int hi, IComparer<T> comparer)
        {
            int n = hi - lo + 1;
            for (int i = n / 2; i >= 1; i--)
            {
                DownHeap(keys, i, n, lo, comparer);
            }
            for (int i = n; i > 1; i--)
            {
                Swap(keys, lo, lo + i - 1);
                DownHeap(keys, 1, i - 1, lo, comparer);
            }
        }

        private static int PickPivotAndPartition<T>(IList<T> keys, int lo, int hi, IComparer<T> comparer)
        {
            // Compute median-of-three.  But also partition them, since we've done the comparison.
            int middle = lo + ((hi - lo) / 2);

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            SwapIfGreater(keys, comparer, lo, middle);  // swap the low with the mid point
            SwapIfGreater(keys, comparer, lo, hi);   // swap the low with the high
            SwapIfGreater(keys, comparer, middle, hi); // swap the middle with the high

            T pivot = keys[middle];
            Swap(keys, middle, hi - 1);
            int left = lo, right = hi - 1;  // We already partitioned lo and hi and put the pivot in hi - 1.  And we pre-increment & decrement below.

            while (left < right)
            {
                while (comparer.Compare(keys[++left], pivot) < 0) ;
                while (comparer.Compare(pivot, keys[--right]) < 0) ;

                if (left >= right)
                    break;

                Swap(keys, left, right);
            }

            // Put pivot in the right location.
            Swap(keys, left, (hi - 1));
            return left;
        }

        private static void DownHeap<T>(IList<T> keys, int i, int n, int lo, IComparer<T> comparer)
        {
            T d = keys[lo + i - 1];
            int child;
            while (i <= n / 2)
            {
                child = 2 * i;
                if (child < n && comparer.Compare(keys[lo + child - 1], keys[lo + child]) < 0)
                {
                    child++;
                }
                if (comparer.Compare(d, keys[lo + child - 1]) >= 0)
                    break;
                keys[lo + i - 1] = keys[lo + child - 1];
                i = child;
            }
            keys[lo + i - 1] = d;
        }

        private static void Swap<T>(IList<T> a, int i, int j)
        {
            if (i != j)
            {
                T t = a[i];
                a[i] = a[j];
                a[j] = t;
            }
        }

        private static int FloorLog2(int n)
        {
            int result = 0;
            while (n >= 1)
            {
                result++;
                n /= 2;
            }
            return result;
        }
        #endregion
    }
}
