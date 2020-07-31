using System;
using System.Collections.Generic;

#nullable enable
namespace Groundbeef.Collections
{
    /// <summary>
    /// Returns whether two values are equal.
    /// </summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <returns><see cref="true"/> if both value are equal; otherwise, <see cref="false"/>.</returns>
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate bool EqualityComparison<T>(T left, T right);

    /// <summary>
    /// Returns the Hashcode of a value using a specific function.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <returns>The Hashcode of a value using a specific function.</returns>
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate int HashCodeFunction<T>(T value);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class GenericEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// Gets or sets the annonymous <see cref="EqualityComparison{T}"/> used to check for equality.
        /// </summary>
        public EqualityComparison<T> EqualityFunction { get; set; }

        /// <summary>
        /// Gets or sets the annonymous function used to generate the hashcode of a value.
        /// </summary>
        public HashCodeFunction<T> HashFunction { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of <see cref="GenericEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="equalityFunction">The annonymous <see cref="EqualityComparison{T}"/> used to check for equality.</param>
        public GenericEqualityComparer(EqualityComparison<T> equalityFunction)
        {
            EqualityFunction = equalityFunction;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GenericEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="equalityFunction">The annonymous <see cref="EqualityComparison{T}"/> used to check for equality.</param>
        /// <param name="hashFunction">The annonymous function used to generate the hashcode of a value.</param>
        public GenericEqualityComparer(EqualityComparison<T> equalityFunction, HashCodeFunction<T> hashFunction)
        {
            EqualityFunction = equalityFunction;
            HashFunction = hashFunction;
        }

        public override bool Equals(T x, T y)
        {
            return EqualityFunction(x, y);
        }

        public override int GetHashCode(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            return HashFunction is null ? obj.GetHashCode() : HashFunction(obj);
        }
    }
}
#nullable disable