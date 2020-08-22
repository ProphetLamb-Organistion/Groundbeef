using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using Groundbeef.Collections;
using Groundbeef.Core;

namespace Groundbeef.UnitTest
{
    public class CollectionExtentionTest
    {
        public const string LOREM_IPSU = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        [SetUp]
        public void Setup()
        {
            string[] source = LOREM_IPSU.Split(' ');
        }

        [Test]
        public void IndexOfTest()
        {
            Assert.Pass();
        }

        [Test]
        public void FindTest()
        {
            Assert.Pass();
        }
    }
}