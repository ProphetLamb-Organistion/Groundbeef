using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Groundbeef.Text
{
    public enum CharEqualityComparisions { Default, CaseInsensitive, InvariantCaseInsensetive }

    public sealed class CharEqualityComparer_InvariantCaseInsensitive : IEqualityComparer<char>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(char x, char y) => Char.ToLowerInvariant(x) == Char.ToLowerInvariant(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(char obj) => obj.GetHashCode();
    }
    public sealed class CharEqualityComparer_CaseInsensitive : IEqualityComparer<char>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(char x, char y) => Char.ToLower(x) == Char.ToLower(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(char obj) => obj.GetHashCode();
    }
}