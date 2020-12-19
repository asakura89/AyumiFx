using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Eksmaru;

namespace Haru {
    public interface IStorage {
        IEnumerable<String> Keys { get; }
        T Get<T>(String key);
        void Set<T>(String key, T value);
        void Remove(String key);
        void Clear();
    }

    public static class XmlStorageExt {
        public static Boolean IsSimpleType(this Type type) => type.Name == "String" || (type.IsValueType && type.Name != "Void");

        public static DateTime ToUtc(this DateTime datetime, TimeZoneInfo timezone = null) {
            if (datetime.Kind == DateTimeKind.Utc)
                return datetime;

            if (datetime == DateTime.MinValue || datetime == DateTime.MaxValue)
                return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);

            if (datetime.Kind == DateTimeKind.Local)
                return TimeZoneInfo.ConvertTimeToUtc(datetime);

            if (timezone == null)
                return TimeZoneInfo.ConvertTimeToUtc(datetime, TimeZoneInfo.Local);

            return TimeZoneInfo.ConvertTimeToUtc(datetime, timezone);
        }

        public static String ToIsoDateTime(this DateTime datetime) {
            datetime = datetime.ToUtc();
            return $"{datetime:yyyyMMddTHHmmss}:{datetime.Ticks}Z";
        }
    }

    public class XmlStorage : IStorage {
        readonly String path;
        XmlDocument storageRoot;
        Boolean loaded;

        public XmlStorage() : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml")) { }

        public XmlStorage(String path) {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            this.path = path;
        }

        void InitStorage(String path) {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty));
            xmlDoc.AppendChild(xmlDoc.CreateElement("storage"));

            xmlDoc.Save(path);
        }

        void Load(String path) {
            if (!File.Exists(path))
                InitStorage(path);

            storageRoot = XmlExt.LoadFromPath(path);
            if (storageRoot.SelectSingleNode("storage") == null)
                throw new InvalidOperationException("xml file contains invalid storage tag.");

            loaded = true;
        }

        public IEnumerable<String> Keys {
            get {
                if (!loaded)
                    Load(path);

                IList<XmlNode> itemNodes = storageRoot
                    .SelectNodes("item")
                    .Cast<XmlNode>()
                    .ToList();

                return itemNodes.Select(node => node.GetAttributeValue("key"));
            }
        }

        public T Get<T>(String key) => throw new NotImplementedException();

        XmlNode CreateItemNode(XmlDocument root, String key, String type) {
            XmlNode node = root.CreateElement("item", null);
            root.AssignAttributeTo(node, "key", key);
            root.AssignAttributeTo(node, "type", type);

            return node;
        }

        void AssignSimpleValueToNode<T>(XmlDocument root, XmlNode node, T value) =>
            root.AssignAttributeTo(node, "value", value.ToString());

        void AssignDateTimeToNode(XmlDocument root, XmlNode node, DateTime value) =>
            root.AssignAttributeTo(node, "value", value.ToIsoDateTime());

        void AssignEnumerableToNode(XmlDocument root, XmlNode node, IEnumerable value) {
            foreach (Object item in value) {
                Type itemType = item.GetType();
                XmlNode itemNode = CreateItemNode(root, itemType.Name, itemType.ToString());
                AssignValueToNode(root, itemNode, item);

                node.AppendChild(itemNode);
            }
        }

        void AssignDictionaryToNode(XmlDocument root, XmlNode node, IDictionary value) {
            ICollection keys = value.Keys;
            foreach (Object key in keys) {
                Object item = value[key];
                XmlNode itemNode = CreateItemNode(root, key.ToString(), item.GetType().ToString());
                AssignValueToNode(root, itemNode, item);

                node.AppendChild(itemNode);
            }
        }

        void AssignObjectToNode<T>(XmlDocument root, XmlNode node, T value) {
            PropertyInfo[] properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties) {
                XmlNode propertyNode = CreateItemNode(root, property.Name, property.PropertyType.ToString());
                Object propertyValue = property.GetValue(value, null);
                AssignValueToNode(root, propertyNode, propertyValue);

                node.AppendChild(propertyNode);
            }

            FieldInfo[] fields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo field in fields) {
                XmlNode fieldNode = CreateItemNode(root, field.Name, field.FieldType.ToString());
                Object fieldValue = field.GetValue(value);
                AssignValueToNode(root, fieldNode, fieldValue);

                node.AppendChild(fieldNode);
            }
        }

        void AssignValueToNode<T>(XmlDocument root, XmlNode node, T value) {
            if (value == null)
                AssignSimpleValueToNode(root, node, "null");
            else if (value.GetType().Name == "DateTime")
                AssignDateTimeToNode(root, node, Convert.ToDateTime(value));
            else if (value.GetType().IsSimpleType())
                AssignSimpleValueToNode(root, node, value);
            else if (value is IDictionary dictionary)
                AssignDictionaryToNode(root, node, dictionary);
            else if (value is IEnumerable enumerable)
                AssignEnumerableToNode(root, node, enumerable);
            else
                AssignObjectToNode(root, node, value);
        }

        public void Set<T>(String key, T value) {
            if (!loaded)
                Load(path);

            XmlNode node = storageRoot.SelectSingleNode($"storage/item[@key='{key}']");
            Boolean newNode = node == null;
            XmlNode udpated = CreateItemNode(storageRoot, key, value.GetType().ToString());
            AssignValueToNode(storageRoot, udpated, value);

            if (!newNode)
                storageRoot.SelectSingleNode("storage").RemoveChild(node);

            storageRoot.SelectSingleNode("storage").AppendChild(udpated);
            storageRoot.Save(path);
        }

        public void Remove(String key) {
            if (!loaded)
                Load(path);

            XmlNode node = storageRoot.SelectSingleNode($"storage/item[@key='{key}']");
            if (node != null) {
                storageRoot.SelectSingleNode("storage").RemoveChild(node);
                storageRoot.Save(path);
            }
        }

        public void Clear() => InitStorage(path);
    }
}
