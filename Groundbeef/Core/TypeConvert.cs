using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Core
{
    [ComVisible(true)]
    public static class TypeConvert
    {
        /// <summary>
        /// Casts the provided object to a specified type.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T Cast<T>(in object value)
        {
            return (T)value;
        }

        /// <summary>
        /// Converts the provided object to a specified type. Using the IConvetible interface.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The conversion type.</typeparam>
        public static T Convert<T>(in object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Guid"/> by combining a <see cref="Guid"/> with another. By appling the XOR operation to the first and last 8 bytes, of the 16-element byte arrays, crosswise.
        /// </summary>
        /// <param name="other">The <see cref="Guid"/> to combine with.</param>
        /// <returns>The <see cref="Guid"/> created by combining two <see cref="Guid"/>s.</returns>
        public static unsafe Guid Combine(this Guid self, Guid other)
        {
            Span<byte> a = self.ToByteArray();
            ReadOnlySpan<byte> b = other.ToByteArray();
            fixed (byte* aPtr = &MemoryMarshal.GetReference(a))
            fixed (byte* bPtr = &MemoryMarshal.GetReference(b))
            {
                Unsafe.WriteUnaligned(aPtr, *(ulong*)aPtr ^ *(ulong*)(bPtr + 8));
                Unsafe.WriteUnaligned(aPtr + 8, *(ulong*)(aPtr + 8) ^ *(ulong*)bPtr);
            }
            return new Guid(a);
        }
    }
}