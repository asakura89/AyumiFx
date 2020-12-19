using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Xunit;

namespace Haru.Test {
    public class XmlStorage_IntegrationAndUnitTest {
        void Cleanup(String path) {
            if (File.Exists(path))
                File.Delete(path);
        }

        [Fact]
        public void DefaultConstructor_StorageLoaded_ShouldBeNull() {
            var storage = new XmlStorage();
            var root = typeof(XmlStorage)
                .GetField("storageRoot", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(storage) as XmlDocument;

            Assert.Null(root);
        }

        [Fact]
        public void DefaultConstructor_StoragePath_ShouldBeDefault() {
            var storage = new XmlStorage();
            String defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            String actualPath = typeof(XmlStorage)
                .GetField("path", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(storage).ToString();

            Assert.Equal(defaultPath, actualPath);
        }

        [Fact]
        public void DefaultConstructor_StorageFile_ShouldNotBeCreated() {
            var storage = new XmlStorage();
            Assert.False(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml")));
        }

        [Fact]
        public void SecondConstructor_EmptyPath_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new XmlStorage(String.Empty));

            Assert.Throws<ArgumentNullException>(() => new XmlStorage(null));
        }

        [Fact]
        public void SecondConstructor_InvalidPath_DidntComplain() {
            new XmlStorage("https://invalid-path.com");
            new XmlStorage("C:\\nonExistentPath");
        }

        [Fact]
        public void GetKeys_StorageFile_ShouldBeCreated() {
            var storage = new XmlStorage();
            IEnumerable<String> keys = storage.Keys;

            String defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            Assert.True(File.Exists(defaultPath));

            String otherPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "other-storage.xml");
            var otherStorage = new XmlStorage(otherPath);
            IEnumerable<String> otherKeys = otherStorage.Keys;

            Assert.True(File.Exists(otherPath));

            Cleanup(defaultPath);
            Cleanup(otherPath);
        }

        [Fact]
        public void GetKeys_InvalidStorageFile_ThrowsException() {
            String invalidFilePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\invalid-storage.xml");
            var storage = new XmlStorage(invalidFilePath1);
            Assert.Throws<InvalidOperationException>(() => storage.Keys);

            String invalidFilePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\invalid-storage-2.xml");
            var otherStorage = new XmlStorage(invalidFilePath2);
            Assert.Throws<InvalidOperationException>(() => otherStorage.Keys);
        }
    }
}