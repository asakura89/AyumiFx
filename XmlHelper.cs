using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Eksmaru {
    public class XmlHelper : IXmlHelper {
        public XmlDocument LoadFromPath(String xmlPath) {
            if (!File.Exists(xmlPath))
                throw new FileNotFoundException(xmlPath);

            String content = File.ReadAllText(xmlPath);
            XmlDocument xmlDoc = Load(content);

            return xmlDoc;
        }

        public XmlDocument Load(String xmlContent) {
            String content = xmlContent.Trim();
            if (String.IsNullOrEmpty(content))
                return null;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            return xmlDoc;
        }

        public XmlAttribute GetAttribute(XmlNode node, String name) {
            if (node != null) {
                XmlAttribute attr = node.Attributes[name];
                if (attr != null)
                    return attr;
            }

            return null;
        }

        public void AssignAttributeTo(XmlDocument xmlDoc, XmlNode node, String name, String value) {
            if (xmlDoc != null && node != null) {
                XmlAttribute attr = GetAttribute(node, name);
                if (attr == null) {
                    attr = xmlDoc.CreateAttribute(name);
                    node.Attributes.Append(attr);
                }

                attr.Value = value;
            }
        }

        public String GetAttributeValue(XmlNode node, String name) {
            XmlAttribute attr = GetAttribute(node, name);
            if (attr != null)
                return attr.Value;

            return String.Empty;
        }

        public String GetNodeValue(XmlDocument xmlDoc, String selector) {
            XmlNode node = xmlDoc.SelectSingleNode(selector);
            return node.InnerText;
        }

        public IList<String> GetMultipleNodeValue(XmlDocument xmlDoc, String selector) {
            var values = new List<String>();
            XmlNodeList docs = xmlDoc.SelectNodes(selector);
            foreach (XmlNode doc in docs)
                values.Add(doc.InnerText);

            return values;
        }
    }
}