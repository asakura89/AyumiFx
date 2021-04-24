using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Eksmaru;
using Itsu;

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

                return storageRoot
                    .SelectNodes("item")
                    .Cast<XmlNode>()
                    .Select(node => node.GetAttributeValue("key"));
            }
        }

        XmlNode GetItemNode(String key) => storageRoot.SelectSingleNode($"storage/item[@key='{key}']");

        T AssignNodeValueToSimpleType<T>(XmlNode node, Type nodeType) {
            String nodeValue = node.GetAttributeValue("value");
            return (T) Convert.ChangeType(nodeValue, nodeType);
        }

        T AssignNodeValueToDateTime<T>(XmlNode node, Type nodeType) {
            String nodeValue = node.GetAttributeValue("value");
            DateTime datetime = nodeValue.FromIsoDateTime();

            return (T) Convert.ChangeType(datetime, nodeType);
        }

        T AssignNodeValueToTimeSpan<T>(XmlNode node, Type nodeType) {
            String nodeValue = node.GetAttributeValue("value");
            var timespan = TimeSpan.Parse(nodeValue);

            return (T) Convert.ChangeType(timespan, nodeType);
        }

        //IEnumerable<T> 

        T AssignNodeValueToEnumerable<T>(XmlNode node, Type nodeType) {
            XmlNodeList items = node.SelectNodes("item");
            foreach (XmlNode item in items) {
                
            }

                // NOTE: in case of array
                Type elementType = nodeType.GetElementType();
            if (elementType == null) {
                Type[] genericTypes = nodeType.GetGenericArguments();
                if (genericTypes.Length < 0)
                    throw new ArrayTypeMismatchException($"Unknown IEnumerable element type from {nodeType.FullName}.");

                elementType = genericTypes[0];
            }

            //Type genericIEnumType = typeof(IEnumerable<>).MakeGenericType(elementType);
            Object genericIEnum = typeof(Enumerable)
                .GetMethod("Empty", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(elementType)
                .Invoke(null, null);

            XmlNodeList docs = node.SelectNodes("item");
            foreach (XmlNode doc in docs) {
                Object items = typeof(Enumerable)
                    .GetMethod("Repeat", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(elementType)
                    .Invoke(null, new Object[] { doc.GetAttributeValue("value"), 1 });

                genericIEnum = typeof(Enumerable)
                    .GetMethod("Concat", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(elementType)
                    .Invoke(null, new [] { genericIEnum, items });
            }

            if (nodeType.IsArray) {
                ArrayList arr = new ArrayList();
                arr.Add()
                    Enumerable.ToArray(genericIEnum as IEnumerable)
            //return (T) Convert.ChangeType(genericIEnum, nodeType);
            }
        }

        //T AssignNodeValueToEnumerable<T>(XmlNode node, Type nodeType) {
        //    String nodeValue = node.GetAttributeValue("value");
        //    //DateTime datetime = nodeValue.FromIsoDateTime();

        //    //return (T) Convert.ChangeType(datetime, nodeType);
        //}

        // IMPORTANT: REFACTOR ASAP!!
        T AssignNodeValueToObject<T>(XmlNode node) {
            //Type objType = typeof(T); //obj.GetType();
            String nodeType = node.GetAttributeValue("type");
            if (typeof(T).FullName != nodeType)
                throw new InvalidOperationException($"Node {node.GetAttributeValue("key")} and object have different type.");

            var actualNodeType = Type.GetType(nodeType);
            if (actualNodeType.FullName == "System.DateTime")
                return AssignNodeValueToDateTime<T>(node, actualNodeType);
            else if (actualNodeType.FullName == "System.TimeSpan")
                return AssignNodeValueToTimeSpan<T>(node, actualNodeType);
            else if (actualNodeType.IsSimpleType())
                return AssignNodeValueToSimpleType<T>(node, actualNodeType);
            //else if (actualNodeType is IDictionary)
            //    return AssignNodeValueToDictionary<T>(node, actualNodeType);
            else if (typeof(IEnumerable).IsAssignableFrom(actualNodeType))
                return AssignNodeValueToEnumerable<T>(node, actualNodeType);
            //else
            //    return AssignNodeValueToPoco<T>(node, actualNodeType);
            else
                return default(T);
        }

        public T Get<T>(String key) {
            if (!loaded)
                Load(path);

            XmlNode item = GetItemNode(key);
            if (item == null)
                return default(T);

            //T obj = Activator.CreateInstance<T>();
            //AssignNodeToObject(item, obj);

            //return obj;
            //AssignNodeToObject<>();

            return AssignNodeValueToObject<T>(item);
        }

        XmlNode CreateItemNode(XmlDocument root, String key, String type) {
            XmlNode node = root.CreateElement("item", null);
            root.AssignAttributeTo(node, "key", key);
            root.AssignAttributeTo(node, "type", type);

            return node;
        }

        void AssignSimpleValueToNode(XmlDocument root, XmlNode node, String value) =>
            root.AssignAttributeTo(node, "value", value);

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

        void AssignPocoToNode<T>(XmlDocument root, XmlNode node, T value) {
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

        //String DetermineObjectType<T>(T obj) {
        //    if (obj == null)
        //        return "null";
        //    else if (obj.GetType().Name == "DateTime")
        //        return "datetime";
        //    else if (obj.GetType().IsSimpleType())
        //        return "simple";
        //    else if (obj is IDictionary dictionary)
        //        return "dictionary";
        //    else if (obj is IEnumerable enumerable)
        //        return "enumerable";
        //    else
        //        return "object";
        //}

        // IMPORTANT: REFACTOR ASAP!!
        void AssignValueToNode<T>(XmlDocument root, XmlNode node, T value) {
            if (value == null)
                AssignSimpleValueToNode(root, node, "null");
            else if (value.GetType().Name == "DateTime")
                AssignDateTimeToNode(root, node, Convert.ToDateTime(value));
            else if (value.GetType().IsSimpleType())
                AssignSimpleValueToNode(root, node, value.ToString());
            else if (value is IDictionary dictionary)
                AssignDictionaryToNode(root, node, dictionary);
            else if (value is IEnumerable enumerable)
                AssignEnumerableToNode(root, node, enumerable);
            else
                AssignPocoToNode(root, node, value);
        }

        void RemoveItemNode(XmlNode node) => storageRoot.SelectSingleNode("storage").RemoveChild(node);

        void AddItemNode(XmlNode node) => storageRoot.SelectSingleNode("storage").AppendChild(node);

        public void Set<T>(String key, T value) {
            if (!loaded)
                Load(path);

            XmlNode node = GetItemNode(key);
            Boolean newNode = node == null;
            XmlNode updated = CreateItemNode(storageRoot, key, value.GetType().ToString());
            AssignValueToNode(storageRoot, updated, value);

            if (!newNode)
                RemoveItemNode(node);

            AddItemNode(updated);
            storageRoot.Save(path);
        }

        public void Remove(String key) {
            if (!loaded)
                Load(path);

            XmlNode node = GetItemNode(key);
            if (node != null) {
                RemoveItemNode(node);
                storageRoot.Save(path);
            }
        }

        public void Clear() => InitStorage(path);
    }
}
