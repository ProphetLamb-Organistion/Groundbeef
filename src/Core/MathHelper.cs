using Groundbeef.SharedResources;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Groundbeef.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static partial class MathHelper
    {
        /// <summary>
        /// Indicates whether the value is a power of two.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(this int x) => (x & (x - 1)) == 0;

        /// <summary>
        /// Indicates whether the value is a power of two.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOfTwo(this long x) => (x & (x - 1)) == 0;

        /// <summary>
        /// Rounds the value to the nearest 32bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundInt32(this float value) => (int)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest 64bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RoundInt64(this float value) => (long)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest 32bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundInt32(this double value) => (int)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest 64bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RoundInt64(this double value) => (long)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest 32bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundInt32(this decimal value) => (int)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest 64bit signed integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RoundInt64(this decimal value) => (long)Math.Round(value);

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            Comparison<T> comp = Comparer<T>.Default.Compare;
            T max = values[0];
            for (int i = 0; i < values.Length; i++)
                max = comp(max, values[i]) < 0 ? values[i] : max;
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            Comparison<T> comp = Comparer<T>.Default.Compare;
            T min = values[0];
            for (int i = 0; i < values.Length; i++)
                min = comp(min, values[i]) > 0 ? values[i] : min;
            return min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (T Min, T Max) MinMax<T>(params T[] values) where T : IComparable<T>
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            Comparison<T> comp = Comparer<T>.Default.Compare;
            T max = values[0],
              min = values[0];
            for (int i = 0; i < values.Length; i++)
            {
                max = comp(max, values[i]) < 0 ? values[i] : max;
                min = comp(min, values[i]) > 0 ? values[i] : min;
            }
            return (max, min);
        }


        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static byte Max(params byte[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            byte max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static byte Min(params byte[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            byte min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (byte Min, byte Max) MinMax(params byte[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            byte lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static int Max(params int[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            int max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static int Min(params int[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            int min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the difference between the highest and lowerst value of the provided values.
        /// </summary>
        public static int Range(params int[] values)
        {
            (int min, int max) = MinMax(values);
            return max - min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (int Min, int Max) MinMax(params int[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            int lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static long Max(params long[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            long max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static long Min(params long[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            long min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the difference between the highest and lowerst value of the provided values.
        /// </summary>
        public static long Range(params long[] values)
        {
            (long min, long max) = MinMax(values);
            return max - min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (long Min, long Max) MinMax(params long[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            long lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static float Max(params float[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            float max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static float Min(params float[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            float min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the difference between the highest and lowerst value of the provided values.
        /// </summary>
        public static float Range(params float[] values)
        {
            (float min, float max) = MinMax(values);
            return max - min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (float Min, float Max) MinMax(params float[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            float lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static double Max(params double[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            double max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static double Min(params double[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the difference between the highest and lowerst value of the provided values.
        /// </summary>
        public static double Range(params double[] values)
        {
            (double min, double max) = MinMax(values);
            return max - min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (double Min, double Max) MinMax(params double[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            double lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static decimal Max(params decimal[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            decimal max = values[0];
            for (int i = 1; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }

        /// <summary>
        /// Returns the highest value of the provided values.
        /// </summary>
        public static decimal Min(params decimal[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            decimal min = values[0];
            for (int i = 1; i < values.Length; i++)
                min = Math.Min(min, values[i]);
            return min;
        }

        /// <summary>
        /// Returns the difference between the highest and lowerst value of the provided values.
        /// </summary>
        public static decimal Range(params decimal[] values)
        {
            (decimal min, decimal max) = MinMax(values);
            return max - min;
        }

        /// <summary>
        /// Returns the minimum and maximum value of the provided value.
        /// </summary>
        public static (decimal Min, decimal Max) MinMax(params decimal[] values)
        {
            if (values.Length == 0)
                throw new ArgumentException(ExceptionResource.COLLECTION_NOT_EMPTY);
            decimal lower = values[0],
                    upper = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                lower = Math.Min(lower, values[i]);
                upper = Math.Max(upper, values[i]);
            }
            return (lower, upper);
        }
    }
}
