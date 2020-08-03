using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Groundbeef.Collections
{
    /// <summary>
    /// Collection of extention functions for arrays, and generic arrays: 
    /// SortByKeys(keys), FindFirst(predicate), FindLast(predicate), FindAll(predicate), IndexOf(element|predicate), IndexOfLast(element|predicate), IndexOfAll(element|predicate), GetHashCode(fromValues)
    /// </summary>
    public static partial class ArrayExtention
    {
        internal static void ValidateAngGetEndIndex(in Array? array, int index, int count, in object? condition, out int endIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (condition is null)
                throw new ArgumentNullException(nameof(condition));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            endIndex = index + count;
            if (array.Length < endIndex)
                throw new IndexOutOfRangeException();
        }

        internal static void ValidateAngGetEndIndex(IList? collection, int index, int count, in object? condition, out int endIndex)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (condition is null)
                throw new ArgumentNullException(nameof(condition));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ExceptionResource.INTEGER_POSITIVEZERO);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ExceptionResource.INTEGER_POSITIVEZERO);
            endIndex = index + count;
            if (collection.Count < endIndex)
                throw new IndexOutOfRangeException();
        }

        internal static ParallelOptions DefaultOptions()
        {
            return new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 };
        }

        internal static ParallelOptions DefaultOptions(in CancellationTokenSource src)
        {
            return new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2, CancellationToken = src.Token };
        }
    }
}