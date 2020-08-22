using System.Collections.Generic;
using System.Linq;
using System;
using Groundbeef.Collections;
using NUnit.Framework;

namespace Groundbeef.UnitTest
{
    public class SpanSplitTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Test()
        {
            string test = "Hello World! I can talk  can you? ";
            ReadOnlySpan<char> span = test;
            IList<string> ranges = span.Split(' ').ToList().Select(r => test[r]).ToArray();
            IList<string> parts = span.ToString().Split(' ');
            Assert.AreEqual(parts.Count, ranges.Count);
            for (int i = 0; i < ranges.Count; i++)
                Assert.AreEqual(parts[i], ranges[i]);
            Assert.Pass();
        }
    }
}