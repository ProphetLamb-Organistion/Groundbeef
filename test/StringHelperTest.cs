using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Groundbeef.Text;

using NUnit.Framework;

namespace Groundbeef.UnitTest
{
    public class StringHelperTest
    {
        [Test]
        public void SplitTest()
        {
            string input = "123456";
            string[] expected = new string[] { "123", "456" };
            string[] actual = input.Crop(3);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitTestNotEqual()
        {
            string input = "12345";
            string[] expected = new string[] { "123", "45" };
            string[] actual = input.Crop(3);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitTestDesiredlengthIsZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                string input = "12345";
                string[] actual = input.Crop(0);
            });
        }

        [Test]
        public void SplitTestDesiredlengthIsZeroWithEmptyString()
        {
            string input = "";
            string[] expected = new string[0];
            string[] actual = input.Crop(0);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitTestWillThrowBecauseOfStrict()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                string input = "12345";
                string[] actual = input.Crop(3, true);
            });
        }

        [Test]
        public void SplitTestUnicodeVariant()
        {
            string input = "𠀑𠀑𠀑a𠀑𠀑𠀑";
            string[] expected = new string[] { "𠀑𠀑", "𠀑a", "𠀑𠀑", "𠀑" };
            string[] actual = input.Crop(2);
            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void SplitTestUnicodeVariant1()
        {
            string input = "dž𠀑𠀑a𠀑é𠀑";
            string[] expected = new string[] { "dž𠀑", "𠀑a𠀑", "é𠀑" };
            string[] actual = input.Crop(3);
            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void SplitTestUnicodeVariant2()
        {
            string input = "éée\u0301éé";
            string[] expected = new string[] { "éé", "e\u0301é", "é" };
            string[] actual = input.Crop(2);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
