using System;
using System.Runtime.InteropServices;

namespace ProphetLamb.Tools
{
    /// <summary>
    /// Simpler implementation of a range structure then <see cref="Range"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [System.Runtime.InteropServices.ComVisible(true)]
    public readonly ref struct Int32Range
    {
        public readonly int Start;
        public readonly int End;
        public readonly int Count;

        /// <summary>
        /// Initializes a new instance of <see cref="Int32Range"/>.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Int32Range(int start, int end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException(nameof(start), ExceptionResource.RANGE_STARTEND_SMALLEREQUALS);
            Start = start;
            End = end;
            Count = end - start;
        }

        /// <summary>
        /// Adds a specific value to both the <see cref="Start"/> and <see cref="End"/> value of this instance.
        /// </summary>
        /// <param name="count">The value offset.</param>
        public Int32Range Offset(int count)
        {
            return new Int32Range(Start + count, End + count);
        }

        /// <summary>
        /// Returns whether a value is widthin the range. Including the lower bound and excluding the upper bound.
        /// </summary>
        /// <param name="value">The value tested.</param>
        /// <returns>True if the value is widthin the range, otherwise false</returns>
        public bool Contains(int value)
        {
            return Start <= value && value < End;
        }

        public bool Equals(Int32Range other) => Start == other.Start && End == other.End;
        public override int GetHashCode() => System.HashCode.Combine(Start, End);

        public static bool operator ==(Int32Range left, Int32Range right) => left.Equals(right);
        public static bool operator !=(Int32Range left, Int32Range right) => !(left == right);

        public static implicit operator Int32Range(Range range) => new Int32Range(range.Start.Value, range.End.Value);

        public static implicit operator Range(Int32Range range) => new Range(new Index(range.Start), new Index(range.End));
    }
}
