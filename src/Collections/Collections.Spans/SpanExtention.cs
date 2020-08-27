using System;
using System.Runtime.InteropServices;

namespace Groundbeef.Collections.Spans
{
    public static class SpanExtention
    {
        #region Assign
        // Source: http://www.pinvoke.net/default.aspx/msvcrt/memset.html
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int c, int byteCount);

        /// <summary>
        /// Assigns the value to all elements in the span. PInvokes memset.
        /// Chars in the span ought to be ASCII or Windows 1252 characters: [0..255]
        /// </summary>
        /// <param name="value">Must be convertible to the 'unsigned char' datatype. Allows only ASCII or Windows 1252 values: [0..255].</param>
        public static unsafe void Assign(this Span<char> span, char value)
        {
            if ((value & 0xFF) != value)
                throw new ArgumentException(nameof(value));
            fixed(char* spanPtr = &MemoryMarshal.GetReference(span))
            {
                MemSet(new IntPtr(spanPtr), value, span.Length);
            }
        }

        /// <summary>
        /// Assings the value to all elements in the span. PInvokes memset.
        /// </summary>
        public static unsafe void Assign(this Span<byte> span, byte value)
        {
            fixed(byte* spanPtr = &MemoryMarshal.GetReference(span))
            {
                MemSet(new IntPtr(spanPtr), value, span.Length);
            }
        }
        #endregion

        #region SpanSplitEnumerator
        /// <summary>
        /// Enumerates slices of a <see cref="Span{T}"/> separated by a specific <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="options">The split options.</param>
        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, in T separator, StringSplitOptions options = StringSplitOptions.None) where T : IEquatable<T>
        => Split(span, separator, 0, span.Length, options);

        /// <summary>
        /// Enumerates slices of a portion of a <see cref="Span{T}"/> separated by a specific <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="index">The start index of the first slice.</param>
        /// <param name="options">The split options.</param>
        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, in T separator, int index, StringSplitOptions options = StringSplitOptions.None) where T : IEquatable<T>
        => Split(span, separator, index, span.Length - index, options);

        /// <summary>
        /// Enumerates slices of a portion of a <see cref="Span{T}"/> separated by a specific <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="index">The start index of the first slice.</param>
        /// <param name="count">The number of elements </param>
        /// <param name="options">The split options.</param>
        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, in T separator, int index, int count, StringSplitOptions options = StringSplitOptions.None) where T : IEquatable<T>
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = index + count;
            if (length > span.Length)
                throw new IndexOutOfRangeException();
            if (span.IsEmpty)
                return new SpanSplitEnumerator<T>();
            return new SpanSplitEnumerator<T>(span, separator.Equals, index, length, options);
        }

        public static SpanSplitEnumerator<T> SplitWhere<T>(this ReadOnlySpan<T> span, in Predicate<T> match, StringSplitOptions options = StringSplitOptions.None)
        => SplitWhere(span, match, 0, span.Length, options);

        public static SpanSplitEnumerator<T> SplitWhere<T>(this ReadOnlySpan<T> span, in Predicate<T> match, int index, StringSplitOptions options = StringSplitOptions.None)
        => SplitWhere(span, match, index, span.Length - index, options);

        /// <summary>
        /// Enumerates slices of a portion of a <see cref="Span{T}"/> separated when a specified condition is met.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> signaling the elements at which the <see cref="Span{T}"/> is to be split.</param>
        /// <param name="index">The start index of the first slice.</param>
        /// <param name="count">The number of elements </param>
        /// <param name="options">The split options.</param>
        public static SpanSplitEnumerator<T> SplitWhere<T>(this ReadOnlySpan<T> span, in Predicate<T> match, int index, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = index + count;
            if (length > span.Length)
                throw new IndexOutOfRangeException();
            if (span.IsEmpty)
                return new SpanSplitEnumerator<T>();
            return new SpanSplitEnumerator<T>(span, match, index, length, options);
        }
        #endregion
    }
}