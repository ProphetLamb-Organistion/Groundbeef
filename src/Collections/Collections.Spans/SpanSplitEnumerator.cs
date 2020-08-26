using System;
using System.Collections.Generic;

namespace Groundbeef.Collections.Spans
{
    public static class SpanExtention
    {
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
    }

    //Based on https://github.com/dotnet/runtime/issues/934
    public ref struct SpanSplitEnumerator<T>
    {
        private static readonly Range s_defaultValue = default!;
        private readonly ReadOnlySpan<T> _span;
        private readonly Predicate<T> _comparison;
        private int _index;
        private readonly int _length,
                             _startIndex;
        private readonly StringSplitOptions _options;
        private Range _current;

        internal SpanSplitEnumerator(in ReadOnlySpan<T> span, Predicate<T> comparison, int index, int length, StringSplitOptions options)
        {
            _span = span;
            _comparison = comparison;
            _index = index;
            _length = length;
            _startIndex = index;
            _options = options;
            _current = s_defaultValue;
        }

        public SpanSplitEnumerator<T> GetEnumerator() { return this; }

        public IList<Range> ToList()
        {
            var list = new List<Range>();
            while (MoveNext())
                list.Add(Current);
            return list;
        }

        public Span<Range> ToSpan()
        {
            Span<int> split = _span.IndexOfAll(_startIndex, _length - _startIndex, _comparison);
            Span<Range> ranges = new Range[split.Length + 1];
            int lastIndex = 0;
            int rangeIndex = 0;
            for (int i = 0; i < split.Length; i++)
            {
                int length = split[i] - lastIndex;
                if (_options == StringSplitOptions.None && length >= 2)
                {
                    ranges[rangeIndex] = lastIndex..(split[i]);
                    lastIndex = i + 1;
                    rangeIndex++;
                }
            }
            ranges[rangeIndex] = lastIndex..(_span.Length);
            return ranges.Slice(0, rangeIndex + 1);
        }

        public void Reset()
        {
            _index = _startIndex;
            _current = s_defaultValue;
        }

        public bool MoveNext()
        {
            if (_index == _length)
            {
                _current = _length.._length;
                _index++;
                return _options == StringSplitOptions.None;
            }
            if (_index > _length)
            {
                _current = s_defaultValue;
                return false;
            }
            int i;
            for (i = _index; i < _length; i++)
            {
                if (_comparison(_span[i]))
                    break;
            }
            // Empty entry
            if (i == _index + 1 && _options == StringSplitOptions.RemoveEmptyEntries)
            {
                _index = i;
                return MoveNext();
            }
            // Skip separator
            _current = _index..i;
            _index = i + 1;
            return true;
        }

        public Range Current { get { return _current; } }
    }
}