using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maaya.Test {
    [TestClass]
    public class MaayaTest {
        [TestMethod]
        public void UrlTest() {
            String[] urls = new[] {
                "",
                null,
                " ",
                "/",
                "/Home/Index",
                "/Dashboard",
                "/Non",
                "~",
                "~/",
                "~/Home/Index",
                "~/Dashboard",
                "~/Non",
                //"/~/",
                "../",
                "../Home.aspx",
                "Home/Index.aspx",
                "http:web.development.net",
                "http:/web.development.net",
                "https:web.development.net",
                "https:/web.development.net",
                "http://web.development.net/Home/Index",
                "https://web.development.net/Home/Index",
                "https://google.com"
            };

            Boolean[] expecteds = new[] {
                false,
                false,
                false,
                true,
                true,
                true,
                true,
                false,
                true,
                true,
                true,
                true,
                //false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                true,
                true,
                false
            };

            for (Int32 idx = 0; idx < urls.Length; idx++) {
                Assert.AreEqual(expecteds[idx], urls[idx].IsLocalUrl(new Uri("http://web.development.net/")));
            }
        }
    }
}
