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
            // Encode
            string test = StringHelper.RandomString(10000000);
            ReadOnlySpan<char> encoded = Base85.Encode(Encoding.UTF8.GetBytes(test), 0, test.Length);
            //Console.WriteLine(encoded.ToString());
            // Decode
            byte[] decoded = Base85.Decode(encoded, 0, encoded.Length).ToArray();
            //Console.WriteLine(Encoding.UTF8.GetString(decoded));
            Assert.AreEqual(test, decoded);

            Assert.Pass();
        }
    }
}