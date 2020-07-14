using System.Globalization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

using ProphetLamb.Tools.Core;
using ProphetLamb.Tools.JsonResources;
using System;

namespace ProphetLamb.Tools.UnitTest
{
    public class JsonResourceTest
    {
        private const string resDir = ".\\resource_test";
        private static readonly CultureInfo german = CultureInfo.GetCultureInfo("de-de"),
                                            english = CultureInfo.GetCultureInfo("en-us");
        private ResourceManager resourceManager;

        private IFoo foo;

        [SetUp]
        public void SetUp()
        {
            // Clear directory
            if (Directory.Exists(resDir))
                Directory.Delete(resDir, true);
            Directory.CreateDirectory(resDir);

            foo = new Foo("Schneider", 0.3146d, new Foo.Bar { Cheeta = "Morning".ToList() });
        }

        [Test]
        public void AddResourcesTest()
        {
            resourceManager = new ResourceManager("CommonResource", resDir);
            using (var writer = new ResourceWriter(resourceManager, german))
            {
                writer.AddResource("first", "Hallo Welt!");
                writer.AddResource("0to100", foo);
            }
            using (var writer = new ResourceWriter(resourceManager, english))
            {
                writer.AddResource("first", "Hello World!");
                writer.AddResource("0to100", foo);
            }
            Assert.AreEqual(2, resourceManager.Cultures.Count());
            // Clear resources
            resourceManager.Dispose();
            resourceManager = new ResourceManager("CommonResource", resDir);
            using (var reader = new ResourceReader(resourceManager, german))
                reader.ReadToEnd();
            using (var reader = new ResourceReader(resourceManager, english))
                reader.ReadToEnd();
            resourceManager.Culture = german;
            Assert.AreEqual("Hallo Welt!", resourceManager.GetString("first"));
            var fooRes = resourceManager.GetObject("0to100") as Foo;
            Assert.AreEqual(foo, fooRes);
            Assert.AreEqual("Hello World!", resourceManager.GetString("first", english));

            Assert.Pass();
        }
    }
}