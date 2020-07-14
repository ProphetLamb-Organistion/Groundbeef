using System.Security.Cryptography;
using System.Linq;
using System;
using NUnit.Framework;

using ProphetLamb.Tools.Core;
using System.Collections.Generic;

namespace ProphetLamb.Tools.UnitTest
{
    public class ArrayExtentionTest
    {
        public const string LoremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        string[] array1;
        Array array2;
        int[] keys;

        [SetUp]
        public void Setup()
        {
            array1 = LoremIpsu.Split(' ');
            array2 = array1;
            // randomize keys
            keys = new int[array1.Length];
            for (int i = 0; i < array1.Length; i++)
                keys[i] = i;
            var rnd = new Random();
            keys = keys.OrderBy(x => rnd.Next()).ToArray();
        }

        [Test]
        public void SortByKeysTest()
        {
            string[] sorted = array1.SortByKeys(keys);
            Assert.Pass();
        }

        [Test]
        public void IndexOfTest()
        {
            const string probe = "et.";
            // Generic Array
            Assert.AreEqual(6, array1.IndexOf(x => x == probe));
            Assert.AreEqual(13, array1.IndexOfLast(x => x == probe));
            Assert.AreEqual(2, array1.IndexOfAll(x => x == probe).Count());
            // Array
            Assert.AreEqual(6, array2.IndexOf(x => x is string str && str == probe));
            Assert.AreEqual(13, array2.IndexOfLast(x => x is string str && str == probe));
            Assert.AreEqual(2, array2.IndexOfAll(x => x is string str && str == probe).Count());
            // span
            ReadOnlySpan<string> span = array1.AsSpan();
            Assert.AreEqual(6, span.IndexOf(x => x == probe));
            Assert.AreEqual(13, span.IndexOfLast(x => x == probe));
        }

        [Test]
        public void FindTest()
        {
            const string probe = "et.";
            Assert.AreEqual(probe, array1.FindFirst(x => x == probe));
            Assert.AreEqual(probe, array1.FindLast(x => x == probe));
            Assert.AreEqual(2, array1.FindAll(x => x == probe).Count());
        }

        [Test]
        public void HashCodeTest()
        {
            string[] array2 = array1.Clone() as string[],
                     array3 = LoremIpsu.Split(' ');
            int hash1 = array1.GetHashCode(true),
                hash2 = array2.GetHashCode(true),
                hash3 = array3.GetHashCode(true);
            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(hash1, hash3);
        }
    }
}