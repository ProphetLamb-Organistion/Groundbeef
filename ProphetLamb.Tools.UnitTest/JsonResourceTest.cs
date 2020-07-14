using System.Globalization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

using ProphetLamb.Tools.Core;
using ProphetLamb.Tools.JsonResources;

namespace ProphetLamb.Tools.UnitTest
{
    public class JsonResourceTest
    {
        private const string resDir = ".\\resource_test";
        private static readonly CultureInfo german = CultureInfo.GetCultureInfo("de-de"),
                                            english = CultureInfo.GetCultureInfo("en-us");
        private ResourceManager resourceManager;

        [SetUp]
        public void SetUp()
        {
            // Clear directory
            if (Directory.Exists(resDir))
                Directory.Delete(resDir, true);
            Directory.CreateDirectory(resDir);
        }

        [Test]
        public void AddResourcesTest()
        {
            resourceManager = new ResourceManager("CommonResource", resDir, german);
            using (var writer = new ResourceWriter(resourceManager, german))
            {
                writer.AddResource("first", "Hallo Welt!");
                writer.AddResource("0to100", new Int32Range(0, 100));
            }
            using (var writer = new ResourceWriter(resourceManager, english))
            {
                writer.AddResource("first", "Hello World!");
                writer.AddResource("0to100", new Int32Range(0, 100));
            }
        }
    }
}