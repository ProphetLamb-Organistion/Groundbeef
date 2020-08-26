using NUnit.Framework;

using Groundbeef.Collections.Spans;

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Groundbeef.UnitTest
{
    public class BitwiseSpanTest
    {
        public const ulong taste = 0xBEEFCACEBEEFCACE;

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ShiftLongTest()
        {
            ulong integer = taste;
            Span<byte> span = BitConverter.GetBytes(integer);
            Assert.AreEqual(integer, BitConverter.ToUInt64(span));
            integer <<= 1;
            var shifted = span.LeftShift(new byte[8]);
            Assert.AreEqual(integer, BitConverter.ToUInt64(shifted));
            integer >>= 1;
            shifted.RightShift();
            Assert.AreEqual(integer, BitConverter.ToUInt64(shifted));
            Assert.Pass();
        }

        [Test]
        public void ShiftNotEven()
        {
            // Fill test array
            byte[] chunk = BitConverter.GetBytes(taste);
            // 32bit / 4 byte sized array
            byte[] data = new byte[chunk.Length / 2];
            Array.Copy(chunk, 0, data, 0, chunk.Length / 2);
            // Validate leftshift
            Span<byte> span = ((ReadOnlySpan<byte>)data).LeftShift(new byte[data.Length]);
            Assert.AreEqual(0x7DDF959C, BitConverter.ToUInt32(span));
            // Validate rightshift
            span.RightShift();
            Assert.AreEqual(0x3EEFCACE, BitConverter.ToUInt32(span));

            Assert.Pass();
        }

        [Test]
        public void ShiftExceptionTest()
        {
            Span<byte> span = BitConverter.GetBytes(taste);
            // Negative
            bool argExc = false;
            try
            {
                span.LeftShift(-1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                argExc = true;
            }
            Assert.IsTrue(argExc);
            // To great
            argExc = false;
            try
            {
                span.LeftShift(65);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                argExc = true;
            }
            Assert.IsTrue(argExc);

            Assert.Pass();
        }
    }
}
