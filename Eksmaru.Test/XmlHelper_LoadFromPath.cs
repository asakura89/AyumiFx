using System;
using System.IO;
using System.Xml;
using Xunit;

namespace Eksmaru.Test {
    public class XmlHelper_LoadFromPath {
        String GetFilePath(String filename) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", filename);

        [Fact]
        public void LoadFromPath_EmptyPath_Error() =>
            Assert
                .Throws<FileNotFoundException>(() =>
                    new XmlHelper()
                        .LoadFromPath(""));

        [Fact]
        public void LoadFromPath_NonExistentFilePath_Error() =>
            Assert
                .Throws<FileNotFoundException>(() =>
                    new XmlHelper()
                        .LoadFromPath(GetFilePath("")));

        [Fact]
        public void LoadFromPath_NonXmlFilePath_Error() =>
            Assert
                .Throws<XmlException>(() =>
                    new XmlHelper()
                        .LoadFromPath(GetFilePath(".editorconfig")));

        [Fact]
        public void LoadFromPath_CorrectPathCorrectFile_Ok() {
            XmlDocument doc = new XmlHelper().LoadFromPath(GetFilePath("valid.xml"));
            Assert.NotNull(doc);
            Assert.True(doc.HasChildNodes);
        }
    }
}
