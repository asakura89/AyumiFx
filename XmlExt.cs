using System;
using System.IO;
using System.Xml;

namespace Eksmaru {
    public static class XmlExt {
        public static XmlDocument LoadFromPath(String path) {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            String content = File.ReadAllText(path);
            return Load(content);
        }

        public static XmlDocument Load(String xml) {
            if (String.IsNullOrEmpty(xml) || String.IsNullOrEmpty(xml.Trim()))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        public static XmlNode GetAttribute(this XmlNode node, String name) {
            if (node != null && node.Attributes != null) {
                XmlAttribute attr = node.Attributes[name];
                if (attr != null)
                    return (XmlNode) attr;
            }

            return null;
        }

        public static String GetAttributeValue(this XmlNode node, String name) {
            XmlNode attr = GetAttribute(node, name);
            if (attr != null)
                return attr.Value;

            return String.Empty;
        }
    }
}