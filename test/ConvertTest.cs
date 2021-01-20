using Groundbeef.BinaryEncoding;
using Groundbeef.Text;

using NUnit.Framework;

using System;
using System.Text;

namespace Groundbeef.UnitTest
{
    public class Z85EncodingTest
    {
        public const string c_loremIpsu = "Laborum adipisci in vel aut tempora et. Asperiores perferendis unde ut natus pariatur et. Vel aut placeat sit provident. Magni consequatur similique sapiente illum ut est";

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestBase85()
        {
            // Encode
            string test = StringHelper.RandomString(100_000);
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