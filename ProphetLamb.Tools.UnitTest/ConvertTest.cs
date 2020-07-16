using System.Runtime.InteropServices;
using System.Text;
using System;
using NUnit.Framework;
using ProphetLamb.Tools.Converters;

namespace ProphetLamb.Tools.UnitTest
{
    public class Z85EncodingTest
    {
        public const string LoremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public unsafe void TestBase85()
        {
            // Encode Hello World
            string test = "Hello World!";
            Span<byte> bytes = Base85.Encode(test, 0, test.Length);
            Console.WriteLine(String.Join(',',bytes.ToArray()));
            // Decode Hello World
            string decoded = Base85.Decode(bytes, 0, bytes.Length).ToString();
            Console.WriteLine(decoded);
            Assert.AreEqual(test, decoded);

            Assert.Pass();
        }
    }
}