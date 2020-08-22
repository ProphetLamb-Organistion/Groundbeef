using Groundbeef.Json.Settings;

using NUnit.Framework;

using System;
using System.Drawing;

namespace Groundbeef.UnitTest
{
    public class JsonSettingsTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        public string settingsPath = @".\config\appsettings.json";

        [Test]
        public void ProviderTest()
        {
            using (var prv = SettingsProvider<MyStorage>.Create(settingsPath))
            {
                // Write
                prv["HomeDir"] = "$home";
                prv["WorkingMode"] = 69;
                prv["Backcolor"] = Color.Gainsboro;
                // Write to readonly
                Assert.Catch(typeof(InvalidOperationException), () => prv["Appname"] = "Badname");
            }
            using (var prv = SettingsProvider<MyStorage>.Create(settingsPath))
            {
                // Read
                Assert.AreEqual("$home", prv["HomeDir"]);
                Assert.AreEqual(69, prv["WorkingMode"]);
                Assert.AreEqual(Color.Gainsboro, prv["Backcolor"]);
                // Read non exisiting
                Assert.Catch(typeof(ArgumentNullException), () => _ = prv[null]);
                Assert.Catch(typeof(ArgumentException), () => _ = prv["IDONTEXIST"]);
            }

            Assert.Pass();
        }

        [Test]
        public void ManagerServiceTest()
        {
            var prv = SettingsProvider<MyStorage>.Create(settingsPath);
            prv["HomeDir"] = "$home";
            prv["WorkingMode"] = 69;
            prv["Backcolor"] = Color.Gainsboro;
            SettingsManagerService.RegisterProvider(prv);
            Assert.AreEqual("$home", SettingsManagerService.GetValue<MyStorage>("HomeDir"));
            Assert.AreEqual(69, SettingsManagerService.GetValue<MyStorage>("WorkingMode"));
            Assert.AreEqual(Color.Gainsboro, SettingsManagerService.GetValue<MyStorage>("Backcolor"));

            Assert.Pass();
        }

        [SettingsStorage]
        public class MyStorage
        {
            public string HomeDir { get; set; }
            public Color Backcolor { get; set; }
            public int WorkingMode { get; set; }
            public double MaxPrecision { get; set; }
            public string Appname { get; } = "FooBaaar!";
        }
    }
}