using System;
using System.Collections.Generic;

namespace Groundbeef.Collections
{
    public class GenericEqualityComparer<T> : EqualityComparer<T>
    {
        public Func<T, T, bool> EqualityFunction { get; set; }

        public Func<T, int> HashFunction { get; set; }

        public GenericEqualityComparer(Func<T, T, bool> equalityFunction)
        {
            EqualityFunction = equalityFunction;
        }
        public GenericEqualityComparer(Func<T, T, bool> equalityFunction, Func<T, int> hashFunction)
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
            return HashFunction(obj);
        }
    }
}