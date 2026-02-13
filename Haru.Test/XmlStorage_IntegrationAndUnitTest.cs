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

        const String AppName = "HaruTest";

        [Fact]
        public void DefaultConstructor_StorageLoaded_ShouldBeNull() {
            var storage = new XmlStorage(AppName);
            var root = typeof(XmlStorage)
                .GetField("docRoot", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(storage) as XmlDocument;

            Assert.Null(root);
        }

        [Fact]
        public void DefaultConstructor_StoragePath_ShouldBeDefault() {
            var storage = new XmlStorage(AppName);
            String defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            String actualPath = typeof(XmlStorage)
                .GetField("path", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(storage).ToString();

            Assert.Equal(defaultPath, actualPath);
        }

        [Fact]
        public void DefaultConstructor_StorageFile_ShouldNotBeCreated() {
            var storage = new XmlStorage(AppName);
            Assert.False(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml")));
        }

        [Fact]
        public void SecondConstructor_EmptyPath_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new XmlStorage(String.Empty, AppName));

            Assert.Throws<ArgumentNullException>(() => new XmlStorage(null, AppName));
        }

        [Fact]
        public void SecondConstructor_InvalidPath_DidntComplain() {
            new XmlStorage("https://invalid-path.com", AppName);
            new XmlStorage("C:\\nonExistentPath", AppName);
        }

        [Fact]
        public void GetKeys_StorageFile_ShouldBeCreated() {
            var storage = new XmlStorage(AppName);
            IEnumerable<String> keys = storage.Keys;

            String defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            Assert.True(File.Exists(defaultPath));

            String otherPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "other-storage.xml");
            var otherStorage = new XmlStorage(otherPath, AppName);
            IEnumerable<String> otherKeys = otherStorage.Keys;

            Assert.True(File.Exists(otherPath));

            Cleanup(defaultPath);
            Cleanup(otherPath);
        }

        [Fact]
        public void GetKeys_InvalidStorageFile_ThrowsException() {
            String invalidFilePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\invalid-storage.xml");
            var storage = new XmlStorage(invalidFilePath1, AppName);
            Assert.Throws<InvalidOperationException>(() => storage.Keys);

            String invalidFilePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data\\invalid-storage-2.xml");
            var otherStorage = new XmlStorage(invalidFilePath2, AppName);
            Assert.Throws<InvalidOperationException>(() => otherStorage.Keys);
        }
    }
}