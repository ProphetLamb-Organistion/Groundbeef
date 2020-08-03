using System.Linq;
using System.Collections.Generic;
using System;

namespace Groundbeef.Collections
{
    //From https://github.com/dotnet/runtime/issues/934
    public static class SpanExtention
    {
        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, in T separator, StringSplitOptions options = StringSplitOptions.None) where T : IEquatable<T>
        => Split(span, separator, 0, span.Length, options);

        public static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, in T separator, int index, StringSplitOptions options = StringSplitOptions.None) where T : IEquatable<T>
        => Split(span, separator, index, span.Length - index, options);

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

        public ref struct SpanSplitEnumerator<T>
        {
            private static readonly Range s_defaultValue = default(Range);
            private readonly ReadOnlySpan<T> _span;
            private readonly Predicate<T> _comparison;
            private int _index;
            private readonly int _length;
            private readonly StringSplitOptions _options;
            private Range _current;

            internal SpanSplitEnumerator(in ReadOnlySpan<T> span, Predicate<T> comparison, int index, int length, StringSplitOptions options)
            {
                _span = span;
                _comparison = comparison;
                _index = index;
                _length = length;
                _options = options;
                _current = s_defaultValue;
            }

            public SpanSplitEnumerator<T> GetEnumerator() { return this;  }


            // TODO: Something smart with this, maybe count separators
            public IList<Range> ToList()
            {
                var list = new List<Range>();
                while (MoveNext())
                    list.Add(Current);
                return list;
            }

            public void Reset()
            {
                _index = 0;
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
                for(i = _index; i < _length; i++)
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
                _index = i+1;
                return true;
            }

            public Range Current { get => _current; }
        }
    }
}