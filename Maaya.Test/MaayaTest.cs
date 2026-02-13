using System;
using System.Linq;
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
                "https://google.com",
                "http://web.development.net/Home/Index?q=hello",
                "http://web.development.net/Home-Index",
                "http://web.development.net/Home/Index?q=<script>alert('yeehaw');</script>", // NOTE: will failed the IsWellFormedUriString
                "http://web.development.net/Home/Index<en-us>/[page].htm?v={value1}#x=[amount]",
                "http://web.development.net/Home/Index<en-us>/[page].htm?v={value1}#x=[amount]&q=<script>alert('yeehaw');</script>", // NOTE: fragment issue
                "http://web.development.net/Home/Index<en-us>/[page].htm?q=<script>alert('yeehaw');</script>&x=a+b&v={value1}#x=[amount]"
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
                false,
                true,
                true,
                false,
                false,
                false,
                false
            };

            for (Int32 idx = 0; idx < urls.Length; idx++)
                Assert.AreEqual(expecteds[idx], urls[idx].IsLocalUrl(new Uri("http://web.development.net/")));

            String[] toBeCleaneds = urls
                .Take(3)
                .Concat(urls
                    .Skip(urls.Length -9))
                .ToArray();

            String[] expecteds2 = new[] {
                String.Empty,
                null,
                String.Empty,
                "http://web.development.net/Home/Index",
                "https://web.development.net/Home/Index",
                "https://google.com/",
                "http://web.development.net/Home/Index?q=hello",
                "http://web.development.net/Home-Index",
                "http://web.development.net/Home/Index?q=%3Cscript%3Ealert%28%27yeehaw%27%29%3B%3C%2Fscript%3E",
                "http://web.development.net/Home/Index%3Cen-us%3E/%5Bpage%5D.htm?v=%7Bvalue1%7D#x%3D%5Bamount%5D",
                "http://web.development.net/Home/Index%3Cen-us%3E/%5Bpage%5D.htm?v=%7Bvalue1%7D#x%3D%5Bamount%5D%26q%3D%3Cscript%3Ealert%28%27yeehaw%27%29%3B%3C%2Fscript%3E",
                "http://web.development.net/Home/Index%3Cen-us%3E/%5Bpage%5D.htm?q=%3Cscript%3Ealert%28%27yeehaw%27%29%3B%3C%2Fscript%3E&x=a%20b&v=%7Bvalue1%7D#x%3D%5Bamount%5D"
            };

            for (Int32 idx = 0; idx < toBeCleaneds.Length; idx++)
                Assert.AreEqual(expecteds2[idx], toBeCleaneds[idx].AsCleanedLink());
        }
    }
}
