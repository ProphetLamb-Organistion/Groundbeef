using Groundbeef.Collections;

using NUnit.Framework;

using System;
using System.Linq;

using RNG = System.Security.Cryptography.RandomNumberGenerator;

namespace Groundbeef.UnitTest
{
    public class ArrayExtentionTest
    {
        public const string c_loremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        string[] _array1;
        Array _array2;
        int[] _keys;
        const string c_probe = "et.";

        [SetUp]
        public void Setup()
        {
            _array1 = c_loremIpsu.Split(' ');
            _array2 = _array1;
            // randomize keys
            _keys = new int[_array1.Length];
            for (int i = 0; i < _array1.Length; i++)
                _keys[i] = i;
            _keys = _keys.OrderBy(i => RNG.GetInt32(_array1.Length)).ToArray();
        }

        [Test]
        public void SortByKeysTest()
        {
            string[] sorted = _array1.SortByKeys(_keys);

            Assert.Pass();
        }

        [Test]
        public void TestArrayIndexOf()
        {
            Assert.AreEqual(6, _array2.IndexOf(0, _array2.Length, x => x is string str && str == c_probe));
            Assert.AreEqual(13, _array2.IndexOfLast(0, _array2.Length, x => x is string str && str == c_probe));
            Assert.AreEqual(2, _array2.IndexOfAll(0, _array2.Length, x => x is string str && str == c_probe).Count());
            // Parallel
            Assert.AreNotEqual(-1, _array2.ParallelIndexOfAny(0, _array2.Length, x => x is string str && str == c_probe));
            Assert.AreEqual(2, _array2.ParallelIndexOfAll(0, _array2.Length, x => x is string str && str == c_probe).Length);

            Assert.Pass();
        }

        [Test]
        public void IndexOfTest()
        {
            // Generic Array
            Assert.AreEqual(6, _array1.IndexOf(x => x == c_probe));
            Assert.AreEqual(13, _array1.IndexOfLast(x => x == c_probe));
            Assert.AreEqual(2, _array1.IndexOfAll(x => x == c_probe).Count());
            // Array
            Assert.AreEqual(6, _array2.IndexOf(x => x is string str && str == c_probe));
            Assert.AreEqual(13, _array2.IndexOfLast(x => x is string str && str == c_probe));
            Assert.AreEqual(2, _array2.IndexOfAll(x => x is string str && str == c_probe).Count());
            // span
            ReadOnlySpan<string> span = _array1.AsSpan();
            Assert.AreEqual(6, span.IndexOf(x => x == c_probe));
            Assert.AreEqual(13, span.IndexOfLast(x => x == c_probe));
        }

        [Test]
        public void FindTest()
        {
            Assert.AreEqual(c_probe, _array1.Find(x => x == c_probe));
            Assert.AreEqual(c_probe, _array1.FindLast(x => x == c_probe));
            Assert.AreEqual(2, _array1.FindAll(x => x == c_probe).Count());
        }

        [Test]
        public void HashCodeTest()
        {
            string[] array2 = _array1.Clone() as string[],
                     array3 = c_loremIpsu.Split(' ');
            int hash1 = _array1.GetHashCode(true),
                hash2 = array2.GetHashCode(true),
                hash3 = array3.GetHashCode(true);
            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(hash1, hash3);

            Assert.Pass();
        }
    }
}