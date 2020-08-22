using Groundbeef.Core;

using NUnit.Framework;

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Groundbeef.UnitTest
{
    public class EnumTest
    {
        public enum TestEnum
        {
            [Display(Name = "Value 1")]
            Value1 = 0,
            [Display(Name = "Value 2")]
            Value2 = 3,
            [Display(Name = "Value 3")]
            Value3 = 923,
            [Display(Name = "Value 4")]
            Value4 = 232330
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ValueTest()
        {
            var testValues = EnumHelper<TestEnum>.GetValues();
            var values = new[] { TestEnum.Value1, TestEnum.Value2, TestEnum.Value3, TestEnum.Value4 };
            Assert.AreEqual(values.Length, testValues.Length);
            for (int i = 0; i < values.Length; i++)
                Assert.AreEqual(values[i], testValues[i]);
            Assert.Pass();
        }

        [Test]
        public void NameTest()
        {
            var testNames = EnumHelper<TestEnum>.GetNames().ToArray();
            var names = new[] { "Value1", "Value2", "Value3", "Value4" };
            Assert.AreEqual(names.Length, testNames.Length);
            for (int i = 0; i < names.Length; i++)
                Assert.AreEqual(names[i], testNames[i]);
            Assert.Pass();
        }

        [Test]
        public void DisplayTest()
        {
            var testDisplay = EnumHelper<TestEnum>.GetDisplayValues().ToArray();
            var display = new[] { "Value 1", "Value 2", "Value 3", "Value 4" };
            Assert.AreEqual(display.Length, testDisplay.Length);
            for (int i = 0; i < display.Length; i++)
                Assert.AreEqual(display[i], testDisplay[i]);
            Assert.Pass();
        }
    }
}
