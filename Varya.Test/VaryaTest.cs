using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Varya.Test {
    [TestClass]
    public class VaryaTest {
        [TestMethod]
        public void NormalTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "Hello ${Name}, how do you do?";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("Hello Mike, how do you do?", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoReplacementTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "I take ${Major} as my focused study.";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("I take Mike as my focused study.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NullReplacementTest() {
            String template = "If I had a job in ${OfficeName}, maybe I can buy those drinks.";
            String replaced = template.ReplaceWith(new Dictionary<String, String>());

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("If I had a job in Microsoft, maybe I can buy those drinks.", StringComparison.InvariantCultureIgnoreCase));

            IDictionary<String, String> replacements = null;
            replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("If I had a job in Microsoft, maybe I can buy those drinks.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoVarTest() {
            var replacements = new Dictionary<String, String> { ["Name"] = "Mike" };

            String template = "One day, I'll travel the world.";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("One day, I'll travel the world.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void ArrayTest() {
            var replacements = new Dictionary<String, IEnumerable<String>> { ["Fruits"] = new[] { "Apple", "Banana", "Grape", "Pineapple" } };

            String template = "There is discount for some fruits in supermarket. ${Fruits:0}, ${Fruits:1}, ${Fruits:2}, ${Fruits:3} are discounted.";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("There is discount for some fruits in supermarket. Apple, Banana, Grape, Pineapple are discounted.", StringComparison.InvariantCultureIgnoreCase));

            template = "I like ${Fruits:1} and ${Fruits:3}.";
            replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsFalse(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("I like Banana and Pineapple.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoArrayVarTest() {
            var replacements = new Dictionary<String, IEnumerable<String>> { ["Fruits"] = new[] { "Apple", "Banana", "Grape", "Pineapple" } };

            String template = "There is no anything. Nothing. I just dreamt about ${Dream}.";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("There is no anything. Nothing. I just dreamt about being a pilot.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NoArrayReplacementTest() {
            var replacements = new Dictionary<String, IEnumerable<String>> { ["Fruits"] = new[] { "Apple", "Banana", "Grape", "Pineapple" } };

            String template = "One o one I love my ${Family:0}, Two o two I love my ${Family:1}, Three o three I love my ${Family: 2}. One, Two, Three I love everybody.";
            String replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(replaced.Equals("One o one I love my ${Family:0}, Two o two I love my ${Family:1}, Three o three I love my ${Family: 2}. One, Two, Three I love everybody.", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void NullArrayReplacementTest() {
            String template = "Actually, you could try to enter ${CampusName:0}, ${CampusName:1}, or even ${CampusName:3}. They all good Campuses.";
            String replaced = template.ReplaceWith(new Dictionary<String, IEnumerable<String>>());

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("Actually, you could try to enter MIT, Yale, or even Harvard. They all good Campuses.", StringComparison.InvariantCultureIgnoreCase));

            IDictionary<String, IEnumerable<String>> replacements = null;
            replaced = template.ReplaceWith(replacements);

            Assert.IsNotNull(replaced);
            Assert.IsTrue(template.Equals(replaced, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(replaced.Equals("Actually, you could try to enter MIT, Yale, or even Harvard. They all good Campuses.", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
