using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#nullable enable
namespace Groundbeef.Collections
{
    public interface IRange
    {
        object? Minimum { get; }
        object? Maximum { get; }
        bool HasValue { get; }

        bool Contains(object value);
    }


    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public readonly struct Range<T> : IRange, IEquatable<Range<T>?>, IEquatable<object?> where T : IComparable<T>
    {
        [AllowNull]
        private readonly T _minimum, _maximum;
        private readonly bool _hasValue;
        /// <summary>
        /// Initializes a new instance of <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="minimum">The minimum value in the <see cref="Range{T}"/>.</param>
        /// <param name="maximum">The maximum value in the <see cref="Range{T}"/>.</param>
        public Range([AllowNull] T minimum, [AllowNull] T maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
            _hasValue = true;
        }

        /// <summary>
        /// Gets the minimum value in the <see cref="Range{T}"/>.
        /// </summary>
        [AllowNull]
        public T Minimum
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("The Range must be fully assigned.");
                return _minimum;
            }
        }

        /// <summary>
        /// Gets the maximum value in the <see cref="Range{T}"/>.
        /// </summary>
        [AllowNull]
        public T Maximum
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("The Range must be fully assigned.");
                return _maximum;
            }
        }

        /// <summary>
        /// Indicates whether the <see cref="Minimum"/> and <see cref="Maximum"/> value is set.
        /// </summary>
        public bool HasValue => _hasValue;

        /// <summary>
        /// Returns a new <see cref="Range{T}"/> unifing this instance and the <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The other <see cref="Range{T}"/>.</param>
        /// <returns>A new <see cref="Range{T}"/> unifing this instance and the <paramref name="other"/>.</returns>
        public Range<T> Unify(Range<T>? other)
        {
            if (!(other is Range<T> range))
                return this;
            if (!HasValue || !other.HasValue)
                throw new InvalidOperationException("Both Ranges must be fully assigned.");
            return new Range<T>(
                _minimum.CompareTo(range._minimum) == -1 ? _minimum : range._minimum,
                _maximum.CompareTo(range._maximum) == -1 ? range._maximum : _maximum);
        }

        /// <summary>
        /// Returns a new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to extend to <see cref="Range{T}"/> to.</param>
        /// <returns>A new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.</returns>
        public Range<T> Expand(T value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (!HasValue)
                throw new InvalidOperationException("The Range must be fully assigned.");
            return new Range<T>(
                _minimum.CompareTo(value) == -1 ? _minimum : value,
                _maximum.CompareTo(value) == -1 ? value : _maximum);
        }

        /// <summary>
        /// Returns whether the <paramref name="other"/> <see cref="Range{T}"/> intersects with this instance.
        /// </summary>
        /// <param name="other">The other <see cref="Range{T}"/>.</param>
        /// <returns><see cref="true"/> if the <paramref name="other"/> <see cref="Range{T}"/> intersects with this instance; otherwise, <see cref="false"/>.</returns>
        public bool Intersects(Range<T>? other)
        {
            if (!(other is Range<T> range) || !_hasValue || !range._hasValue)
                return false;
            int minMax = _minimum.CompareTo(range._maximum),
                maxMin = _maximum.CompareTo(range._minimum),
                minMin = _minimum.CompareTo(range._minimum),
                maxMax = _maximum.CompareTo(range._maximum);
            return (minMax <= 0 && minMin >= 0) || (maxMin <= 0 && maxMax >= 0);
        }

        /// <summary>
        /// Returns whether the <paramref name="value"/> is contained widthin the <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><see cref="true"/> if the <paramref name="value"/> is contained widthin the <see cref="Range{T}"/>; otherwise, <see cref="false"/>.</returns>
        public bool Contains(T value)
        {
            return _minimum.CompareTo(value) <= 0 && _maximum.CompareTo(value) >= 0;
        }

        private string GetDebuggerDisplay() => ToString();

        #region IRange members
        object? IRange.Minimum => Minimum;
        object? IRange.Maximum => Maximum;
        bool IRange.HasValue => HasValue;

        bool IRange.Contains(object value) => value is T v && Contains(v);
        #endregion

        #region IEquatable members
        public bool Equals(Range<T>? other)
        {
            // Other not null
            return other is Range<T> range
                // Both without value => equal
                && ((!_hasValue && !range._hasValue)
                // Both have value and values are equal => equal
                || (_hasValue && range._hasValue
                && _minimum.CompareTo(range._minimum) == 0
                && _maximum.CompareTo(range._maximum) == 0));
        }

        public override bool Equals(object? obj) => obj is Range<T> other && Equals(other);

        public override int GetHashCode() => HasValue ? HashCode.Combine(_minimum, _maximum) : 0;

        public static bool operator ==(Range<T>? left, Range<T>? right)
        {
            // Both are null => equal
            return (left is null && right is null)
                // If left is null, then right is not null => not equal, else check equality
                || (left?.Equals(right) ?? false);
        }
        public static bool operator !=(Range<T>? left, Range<T>? right)
        {
            return !(left == right);
        }
        #endregion

        #region Operators 
        /// <summary>
        /// Returns a new <see cref="Range{T}"/> unifing the <paramref name="left"/> and <paramref name="right"/> <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="left">The left <see cref="Range{T}"/>.</param>
        /// <param name="right">The right <see cref="Range{T}"/>.</param>
        /// <returns>A new <see cref="Range{T}"/> unifing the <paramref name="left"/> and <paramref name="right"/> <see cref="Range{T}"/>.</returns>
        public static Range<T>? Unify(Range<T>? left, Range<T>? right) => left?.Unify(right) ?? right;

        /// <summary>
        /// Returns a new <see cref="Range{T}"/> unifing the <paramref name="left"/> and <paramref name="right"/> <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="left">The left <see cref="Range{T}"/>.</param>
        /// <param name="right">The right <see cref="Range{T}"/>.</param>
        /// <returns>A new <see cref="Range{T}"/> unifing the <paramref name="left"/> and <paramref name="right"/> <see cref="Range{T}"/>.</returns>
        public static Range<T> operator +(Range<T> left, Range<T>? right) => Unify(left, right) ?? left;

        /// <summary>
        /// Returns a new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to extend to <see cref="Range{T}"/> to.</param>
        /// <returns>A new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.</returns>
        public static Range<T>? Expand(Range<T>? range, T value) => range?.Expand(value);

        /// <summary>
        /// Returns a new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to extend to <see cref="Range{T}"/> to.</param>
        /// <returns>A new <see cref="Range{T}"> to encompassing the <paramref name="value"/>.</returns>
        public static Range<T> operator +(Range<T> range, T value) => Expand(range, value) ?? range;

        /// <summary>
        /// Converts a <see cref="ValueTuple{T1, T2}"/> to a <see cref="Range{T}"/> where item1 is the  minimum, and item2 the maximum value.
        /// </summary>
        public static implicit operator Range<T>(ValueTuple<T, T> tuple) => new Range<T>(tuple.Item1, tuple.Item2);
        #endregion
    }

    public static class RangeExtentions
    {
        /// <summary>
        /// Returns a new <see cref="Range{Int32}"/> with the Minumum equal to the <see cref="Range.Start"/>, and the Maximum equal to the <see cref="Range.End"/>. 
        /// Requires <see cref="Index.IsFromEnd"/> to be false.
        /// </summary>
        /// <param name="range">The <see cref="Range"/>.</param>
        /// <returns>A new <see cref="Range{Int32}"/> with the Minumum equal to the <see cref="Range.Start"/>, and the Maximum equal to the <see cref="Range.End"/>.</returns>
        public static Range<int> ToGenericRange(this Range range)
        {
            if (range.Start.IsFromEnd || range.End.IsFromEnd)
                throw new NotSupportedException("Converting a System.Range to a Range<int> requires Index.IsFromEnd to be false.");
            return new Range<int>(range.Start.Value, range.End.Value);
        }

        /// <summary>
        /// Returns a new <see cref="Range"/> with the Start equal to <see cref="Range{Int32}.Minimum"/>, and the End equal to <see cref="Range{Int32}.Maximum"/>.
        /// </summary>
        /// <param name="range">The <see cref="Range{Int32}"/>.</param>
        /// <returns>Returns a new <see cref="Range"/> with the Start equal to <see cref="Range{Int32}.Minimum"/>, and the End equal to <see cref="Range{Int32}.Maximum"/>.</returns>
        public static Range ToIndexRange<T>(this Range<T> range) where T : IComparable<T>, IConvertible
        {
            int start = range.Minimum.ToInt32(CultureInfo.CurrentCulture.NumberFormat),
                end = range.Maximum.ToInt32(CultureInfo.CurrentCulture.NumberFormat);
            return new Range(new Index(start), new Index(end));
        }

        /// <summary>
        /// Casts the <see cref="IRange"/> to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type the range will be cast to.</typeparam>
        /// <returns>A new instance of <see cref="Range{T}"/>, cast from the <see cref="IRange"/>.</returns>
        public static Range<T> Cast<T>(IRange range) where T : IComparable<T> => new Range<T>((T)range.Minimum, (T)range.Maximum);

        /// <summary>
        /// Enumerates all elements in the range.
        /// </summary>
        public static System.Collections.Generic.IEnumerator<int> GetEnumerator(this Range<int> range, bool minimumInclusive = true, bool maximumInclusive = false)
        {
            int start = minimumInclusive ? range.Minimum : range.Minimum + 1,
                end = maximumInclusive ? range.Maximum + 1 : range.Maximum;
            for (int i = start; i < end; i++)
                yield return i;
        }
    }
}
#nullable disable