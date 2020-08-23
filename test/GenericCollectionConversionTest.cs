using Groundbeef.Reflection;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace Groundbeef.UnitTest
{
    public class GenericCollectionConversionTest
    {
        public const string c_loremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        IEnumerable<char> _enumerable;

        [SetUp]
        public void Setup()
        {
            _enumerable = c_loremIpsu.Select(x => x);
        }

        [Test]
        public void ArrayTest()
        {
            var array = (char[])CollectionsReflect.ToGenericArray(_enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(c_loremIpsu.Length, array.Length);

            Assert.Pass();
        }

        [Test]
        public void ListTest()
        {
            var array = (List<char>)CollectionsReflect.ToGenericList(_enumerable, out _);
            Assert.NotNull(array);
            Assert.AreEqual(c_loremIpsu.Length, array.Count);

            Assert.Pass();
        }
    }
}