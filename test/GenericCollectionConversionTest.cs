using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using Groundbeef.Collections;
using Groundbeef.Core;
using Groundbeef.Reflection;

namespace Groundbeef.UnitTest
{
    public class GenericCollectionConversionTest
    {
        public const string LOREM_IPSU = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        IEnumerable<char> _enumerable;

        [SetUp]
        public void Setup()
        {
            _enumerable = LOREM_IPSU.Select(x => x);
        }

        [Test]
        public void ArrayTest()
        {
            var array = (char[])CollectionsReflect.ToGenericArray(_enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(LOREM_IPSU.Length, array.Length);

            Assert.Pass();
        }

        [Test]
        public void ListTest()
        {
            var array = (List<char>)CollectionsReflect.ToGenericList(_enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(LOREM_IPSU.Length, array.Count);

            Assert.Pass();
        }
    }
}