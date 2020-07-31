using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

#nullable enable
namespace Groundbeef.Collections
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class EnumerableExtention
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <param name="collection">The collection to count the elements of.</param>
        /// <returns>The number of elements in a sequence.</returns>
        public static int QuickCount(this IEnumerable collection)
        {
            // Most senior interface in the inheritence that has a count property
            if (collection is ICollection c)
                return c.Count;
            else
                return collection.Cast<object>().Count();
        }

        public static IList<T> CastList<T>(this IList list)
        {
            int length = list.Count;
            // Cast list items to array
            var items = new T[length];
            for(int i = 0; i < length; i++)
                items[i] = (T)list[i];
            // The List<T>(IEnumerable<T>) constructor casts the array to ICollection<T> and calls CopyTo(_items) which is implemented via Array.Copy.
            return new List<T>(items);
        }
    }
}
#nullable disable