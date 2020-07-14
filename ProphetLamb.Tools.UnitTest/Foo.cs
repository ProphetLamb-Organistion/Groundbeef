using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ProphetLamb.Tools.UnitTest
{
    internal interface IFoo
    {
        string Name { get; set; }
        Foo.Bar ThisBar { get; }
    }

    internal class Foo : IFoo, IEquatable<Foo>
    {
        public Foo(string name, double precision, Bar thisBar)
        {
            Name = name;
            Precision = precision;
            ThisBar = thisBar;
        }

        public struct Bar : IEquatable<Bar>
        {
            public List<char> Cheeta { get; set; }

            public bool Equals([AllowNull] Bar other)
            {
                if (Cheeta is null && other.Cheeta is null)
                    return true;
                if (Cheeta is null || other.Cheeta is null || Cheeta.Count != other.Cheeta.Count)
                    return false;
                for (int i = 0; i < Cheeta.Count; i++)
                {
                    if (Cheeta[i] != other.Cheeta[i])
                        return false;
                }
                return true;
            }
        }

        public string Name { get; set; }
        public double Precision { get; set; }
        public Bar ThisBar { get; }

        public bool Equals([AllowNull] Foo other)
        {
            return !(other is null) && Name == other.Name && Precision == other.Precision && ThisBar.Equals(other.ThisBar);
        }
    }
}
