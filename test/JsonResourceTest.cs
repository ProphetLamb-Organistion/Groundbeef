using Groundbeef.Json.Resources;
using Groundbeef.Text;

using NUnit.Framework;

using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Groundbeef.UnitTest
{
    public class JsonResourceTest
    {
        private const string c_resDir = ".\\resource_test";
        private static readonly CultureInfo s_german = CultureInfo.GetCultureInfo("de-de"),
                                            s_english = CultureInfo.GetCultureInfo("en-us");
        private ResourceManager _resourceManager;

        private IFoo _foo;

        [SetUp]
        public void SetUp()
        {
            // Clear directory
            if (Directory.Exists(c_resDir))
                Directory.Delete(c_resDir, true);
            Directory.CreateDirectory(c_resDir);

            _foo = new Foo("Schneider", 0.3146d);
        }

        [Test]
        public void FunctionalityTest()
        {
            _resourceManager = new ResourceManager("CommonResource", c_resDir);
            using (var writer = new ResourceWriter(_resourceManager, s_german))
            {
                writer.AddResource("first", "Hallo Welt!");
                writer.AddResource("0to100", _foo);
            }
            using (var writer = new ResourceWriter(_resourceManager, s_english))
            {
                writer.AddResource("first", "Hello World!");
                writer.AddResource("0to100", _foo);
            }
            Assert.AreEqual(2, _resourceManager.Cultures.Count());
            // Clear resources
            _resourceManager.Dispose();
            _resourceManager = new ResourceManager("CommonResource", c_resDir);
            using (var reader = new ResourceReader(_resourceManager, s_german))
                reader.ReadToEnd();
            using (var reader = new ResourceReader(_resourceManager, s_english))
                reader.ReadToEnd();
            _resourceManager.Culture = s_german;
            Assert.AreEqual("Hallo Welt!", _resourceManager.GetString("first"));
            var fooRes = _resourceManager.GetObject("0to100") as Foo;
            Assert.AreEqual(_foo, fooRes);
            Assert.AreEqual("Hello World!", _resourceManager.GetString("first", s_english));

            Assert.Pass();
        }

        [Test]
        public void PerformanceTest()
        {
            _resourceManager = new ResourceManager("PerfResource", c_resDir);
            using var rwGerman = new ResourceWriter(_resourceManager, s_german);
            using var rwEnglish = new ResourceWriter(_resourceManager, s_english);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < 2048; i++)
            {
                rwEnglish.AddResource("ResNr_" + i, StringHelper.RandomString(4196));
            }
            rwEnglish.Close();
            for (int i = 0; i < 2048; i++)
            {
                rwGerman.AddResource("ResNr_" + i, StringHelper.RandomString(4196));
            }
            rwGerman.Close();
            sw.Stop();
            Console.WriteLine("Write: " + sw.Elapsed);
            _resourceManager.Dispose();
            _resourceManager = new ResourceManager("PerfResource", c_resDir);
            sw.Reset();
            using var rrGerman = new ResourceReader(_resourceManager, s_german);
            using var rrEnglish = new ResourceReader(_resourceManager, s_english);
            var germanSet = new ResourceSet(rrGerman);
            var englishSet = new ResourceSet(rrEnglish);
            rrGerman.Close();
            rrEnglish.Close();
            sw.Stop();
            Console.WriteLine("Read: " + sw.Elapsed);
            foreach ((string key, object value) in germanSet)
            {
                if (!(value is string str && str.Length == 4196))
                    Assert.Fail();
            }
            foreach ((string key, object value) in englishSet)
            {
                if (!(value is string str && str.Length == 4196))
                    Assert.Fail();
            }
            Assert.Pass();
        }
    }
}