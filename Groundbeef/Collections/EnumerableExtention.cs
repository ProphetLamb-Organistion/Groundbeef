using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace Groundbeef.Collections
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class EnumerableExtention
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>The number of elements in a sequence.</returns>
        public static int QuickCount(this IEnumerable enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));
            if (enumerable is IList list)
                return list.Count;
            else
                return enumerable.Cast<object>().Count();
        }

        public static IList<T> CastList<T>(this IList list)
        {
            int length = list.Count;
            var cast = new List<T>(length);
            for (int i = 0; i < length; i++)
                cast.Add((T)list[i]);
            return cast;
        }
    }
}