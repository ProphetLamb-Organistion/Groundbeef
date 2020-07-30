using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

namespace Groundbeef.Text
{
    public enum CharEqualityComparisions { Default, CaseInsensitive, InvariantCaseInsensitivie }
    public class CharEqualityComparer_InvariantCaseInsensitive : IEqualityComparer<char>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(char x, char y) => Char.ToLowerInvariant(x) == Char.ToLowerInvariant(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(char obj) => obj.GetHashCode();
    }
    public class CharEqualityComparer_CaseInsensitive : IEqualityComparer<char>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(char x, char y) => Char.ToLower(x) == Char.ToLower(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(char obj) => obj.GetHashCode();
    }
}