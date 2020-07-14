using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

using ProphetLamb.Tools.Core;

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
        public void Tests()
        {
            char[] array = (char[])GenericCollectionConversion.ToGenericArray(enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(LoremIpsu.Length, array.Length);
        }
    }
}