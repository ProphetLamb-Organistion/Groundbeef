using System;

namespace Groundbeef.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Flags]
    public enum ColorStyles : sbyte
    {
        /// <summary>Default must be used.</summary>
        None = 0,
        /// <summary>32bit integer in decimal.</summary>
        Integer = 1 << 0,
        /// <summary>32bit integer in hexadecimal, optionally with the hex specifier.</summary>
        HexInteger = 1 << 1,
        /// <summary>Known name of a <see cref="Color"/>.</summary>
        Name = 1 << 2,
        /// <summary>Three or four numeric values separated by a comma.</summary>
        Tuple = 1 << 3,
        /// <summary>All <see cref="ColorStyles"/>.</summary>
        Any = ~0
    }
}