using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Eksmaru {
    public static class XmlExt {
        public static XmlDocument LoadFromPath(String xmlPath) {
            if (!File.Exists(xmlPath))
                throw new FileNotFoundException(xmlPath);

            String content = File.ReadAllText(xmlPath);
            XmlDocument xmlDoc = Load(content);

            return xmlDoc;
        }

        public static XmlDocument Load(String xmlContent) {
            String content = xmlContent.Trim();
            if (String.IsNullOrEmpty(content))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            return xmlDoc;
        }

        public static XmlAttribute GetAttribute(this XmlNode node, String name) {
            if (node != null && node.Attributes != null) {
                XmlAttribute attr = node.Attributes[name];
                if (attr != null)
                    return attr;
            }

            return null;
        }

        public static void AssignAttributeTo(this XmlDocument xmlDoc, XmlNode node, String name, String value) {
            if (xmlDoc != null && node != null && node.Attributes != null) {
                XmlAttribute attr = xmlDoc.CreateAttribute(name);
                attr.Value = value;

                node.Attributes.Append(attr);
            }
        }

        public static String GetAttributeValue(this XmlNode node, String name) {
            XmlAttribute attr = GetAttribute(node, name);
            if (attr != null)
                return attr.Value;

            return String.Empty;
        }

        public static String GetNodeValue(XmlDocument xmlDoc, String selector) {
            XmlNode node = xmlDoc.SelectSingleNode(selector);
            return node.InnerText;
        }

        public static IList<String> GetNodesValue(XmlDocument xmlDoc, String selector) {
            var values = new List<String>();
            XmlNodeList docs = xmlDoc.SelectNodes(selector);
            foreach (XmlNode doc in docs)
                values.Add(doc.InnerText);

            return values;
        }
    }
}