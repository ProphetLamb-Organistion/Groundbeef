using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Core
{
    [ComVisible(true)]
    public static class ObjectHelper
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

        /// <summary>
        /// Throws an ArgumentNullException if the object is null.
        /// </summary>
        /// <param name="parameterName">The parameter name of the object.</param>
        public static void ThrowOnNull(this object? self, in string? parameterName = null)
        {
            if (self is null)
                throw new ArgumentNullException(parameterName??"parameterName");
        }

        /// <summary>
        /// Dereferences the <see cref="Nullable"/> object, if the object is null throws a <see cref="ArgumentNullException"/>.
        /// </summary>
        public static T DerefOrThrow<T>(this T? self, in string? paramerterName = null) where T : class
        {
            return self??throw new ArgumentNullException(paramerterName??nameof(paramerterName));
        }

        /// <summary>
        /// Dereferences the <see cref="Nullable"/> object, if the object is null throws a <see cref="ArgumentNullException"/>.
        /// </summary>
        public static T DerefOrThrow<T>(this T? self, in string? paramerterName = null) where T : struct
        {
            return self??throw new ArgumentNullException(paramerterName??nameof(paramerterName));
        }
    }
}