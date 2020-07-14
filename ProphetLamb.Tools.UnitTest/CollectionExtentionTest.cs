using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

using ProphetLamb.Tools.Core;

namespace ProphetLamb.Tools.UnitTest
{
    public class CollectionExtentionTest
    {
        public const string LoremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        ICollection coll1;
        Collection<string> coll2;

        [SetUp]
        public void Setup()
        {
            string[] source = LoremIpsu.Split(' ');
            coll1 = source;
            coll2 = new Collection<string>(source);
        }

        [Test]
        public void IndexOfTest()
        {
            const string probe = "et.";
            // ICollection
            Assert.AreEqual(6, coll1.IndexOf(x => x is string str && str == probe));
            Assert.AreEqual(13, coll1.IndexOfLast(x => x is string str && str == probe));
            Assert.AreEqual(2, coll1.IndexOfAll(x => x is string str && str == probe).Count());
            // Generic Collection
            Assert.AreEqual(6, coll2.IndexOf(x => x == probe));
            Assert.AreEqual(13, coll2.IndexOfLast(x => x == probe));
            Assert.AreEqual(2, coll2.IndexOfAll(x => x == probe).Count());

            Assert.Pass();
        }

        [Test]
        public void FindTest()
        {
            const string probe = "et.";
            // ICollection
            Assert.AreEqual(probe, coll1.Find(x => x is string str && str == probe));
            Assert.AreEqual(probe, coll1.FindLast(x => x is string str && str == probe));
            Assert.AreEqual(2, coll1.FindAll(x => x is string str && str == probe).Count());
            // Generic Collection
            Assert.AreEqual(probe, coll2.Find(x => x == probe));
            Assert.AreEqual(probe, coll2.FindLast(x => x == probe));
            Assert.AreEqual(2, coll2.FindAll(x => x == probe).Count());

            Assert.Pass();
        }
    }
}