using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Varya.Test {
    [TestClass]
    public class VaryaTest {
        readonly String DataDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        protected String GetDataPath(String filename) {
            if (!Directory.Exists(DataDirPath))
                Directory.CreateDirectory(DataDirPath);

            return Path.Combine(DataDirPath, filename);
        }

        readonly String OutputDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

        protected String GetOutputPath(String filename) {
            if (!Directory.Exists(OutputDirPath))
                Directory.CreateDirectory(OutputDirPath);

            return Path.Combine(OutputDirPath, filename);
        }

        [TestMethod]
        public void NormalTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "Hello ${Name}, how do you do?";
            String replaced = template.ReplaceWithDictionary(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("Hello Mike, how do you do?", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ComplexNormalTest() {
            var replacements = new Dictionary<String, String> {
                ["WidgetId"] = "simple-progress",
                ["IconColor"] = "dark-blue",
                ["Counter"] = "150",
                ["Suffix"] = "x",
                ["Icon"] = "fe-pencil",
                ["Label1"] = "Edit Count",
                ["Label2"] = "Edit Status:",
                ["Label3"] = "Safe",
                ["Progress"] = "50",
                ["Day"] = new Func<String>(() => new DateTime(2019, 1, 3).DayOfWeek.ToString())(),
                ["Date"] = new Func<String>(() => new DateTime(2019, 1, 3).ToString("dd MMMM yyyy"))()
            };

            String template = File.ReadAllText(GetDataPath("template.html"));
            String replaced = template.ReplaceWithDictionary(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));

            String replacedTemplate = File.ReadAllText(GetDataPath("replaced-template.html"));
            Assert.IsTrue(replaced.Equals(replacedTemplate, StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoReplacementTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "I take ${Major} as my focused study.";
            String replaced = template.ReplaceWithDictionary(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("I take Mike as my focused study.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NullReplacementTest() {
            String template = "If I had a job in ${OfficeName}, maybe I can buy those drinks.";
            String replaced = template.ReplaceWithDictionary(new Dictionary<String, String>());

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("If I had a job in Microsoft, maybe I can buy those drinks.", StringComparison.InvariantCultureIgnoreCase));

            IDictionary<String, String> replacements = null;
            replaced = template.ReplaceWithDictionary(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("If I had a job in Microsoft, maybe I can buy those drinks.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoVarTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "One day, I'll travel the world.";
            String replaced = template.ReplaceWithDictionary(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("One day, I'll travel the world.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ConfigAndEncryptedConfigTest() {
            String template = ConfigurationManager.AppSettings.Get("Api.Url") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("http://api.devsvr.net/?item=kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM&field=MousePad", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void UrlEncodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_1") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("The%20payload%20contains%20message", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ConfigInsideUrlEncodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_2") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("http%3A%2F%2Fapi.devsvr.net%2F%3Fitem%3DkntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM%26field%3DMousePad", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ConfigInsideUrlDecodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_3") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("http://api.devsvr.net/?item=kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM&field=MousePad", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void EncryptedConfigInsideHtmlEncodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_4") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("MousePad", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ConfigInsideHtmlDecodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_5") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ComplexUrlEncodeTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_6") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));

            // NOTE: this is expected broken, because it can't resolve most outer var caused by one inner var is not resolved.
            Assert.IsTrue(replaced.Equals("${urle:Now complex value with kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM and ${Name} also 00:00:01:00 are used.}", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ComplexUrlEncodeTest_2() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_6") ?? String.Empty;
            String replaced = template.ReplaceWithDictionary(new Dictionary<String, String> { ["Name"] = "Mike" });

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("Now%20complex%20value%20with%20kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM%20and%20Mike%20also%2000%3A00%3A01%3A00%20are%20used.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ComplexUrlEncodeTest_3() {
            String template = ConfigurationManager.AppSettings.Get("Http.Payload_7") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("http://api.devsvr.net/?item=kntprZ8Q0IGliqYY7DztEffC02czEsPDokscXOZmIwM%26field%3DMousePad", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void VarTimespanMinuteTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.Timeout") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("00:00:01:00", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void VarTimespanHourTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.RepeatInterval") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("00:04:00:00", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void VarTimespanSecondTest() {
            String template = ConfigurationManager.AppSettings.Get("Http.RetryInterval") ?? String.Empty;
            String replaced = template.Resolve();

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("00:00:00:30", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
