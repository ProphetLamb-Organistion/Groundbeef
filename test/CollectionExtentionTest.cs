using NUnit.Framework;

namespace Groundbeef.UnitTest
{
    public class CollectionExtentionTest
    {
        public const string c_loremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        [SetUp]
        public void Setup()
        {
            string[] source = c_loremIpsu.Split(' ');
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