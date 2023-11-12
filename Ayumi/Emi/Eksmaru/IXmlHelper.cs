using System.Collections.Generic;
using System.Xml;

namespace Eksmaru {
    public interface IXmlHelper {
        void AssignAttributeTo(XmlDocument xmlDoc, XmlNode node, System.String name, System.String value);
        XmlAttribute GetAttribute(XmlNode node, System.String name);
        System.String GetAttributeValue(XmlNode node, System.String name);
        IList<System.String> GetMultipleNodeValue(XmlDocument xmlDoc, System.String selector);
        System.String GetNodeValue(XmlDocument xmlDoc, System.String selector);
        XmlDocument Load(System.String xmlContent);
        XmlDocument LoadFromPath(System.String xmlPath);
    }
}