using System;
using System.Collections;

namespace ProphetLamb.Tools.Core
{
    public static class EnumerableExtention
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>The number of elements in a sequence.</returns>
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));
            var en = enumerable.GetEnumerator();
            int count = 0;
            while (en.MoveNext())
                count++;
            return count;
        }
    }
}