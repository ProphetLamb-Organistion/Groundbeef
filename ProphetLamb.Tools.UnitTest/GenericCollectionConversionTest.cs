using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using ProphetLamb.Tools.Collections;
using ProphetLamb.Tools.Core;
using ProphetLamb.Tools.Reflection;

namespace ProphetLamb.Tools.UnitTest
{
    public class GenericCollectionConversionTest
    {
        public const string LoremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        IEnumerable<char> enumerable;

        [SetUp]
        public void Setup()
        {
            enumerable = LoremIpsu.Select(x => x);
        }

        [Test]
        public void ArrayTest()
        {
            var array = (char[])CollectionsConvert.ToGenericArray(enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(LoremIpsu.Length, array.Length);

            Assert.Pass();
        }

        [Test]
        public void ListTest()
        {
            var array = (List<char>)CollectionsConvert.ToGenericList(enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(LoremIpsu.Length, array.Count);

            Assert.Pass();
        }
    }
}