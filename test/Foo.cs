using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Groundbeef.UnitTest
{
    internal interface IFoo
    {
        string Name { get; set; }
    }

    internal class Foo : IFoo, IEquatable<Foo>
    {
        public Foo(string name, double precision)
        {
            Name = name;
            Precision = precision;
        }

        public string Name { get; set; }
        public double Precision { get; set; }

        public bool Equals([AllowNull] Foo other)
        {
            return !(other is null) && Name == other.Name && Precision == other.Precision;
        }
    }
}
