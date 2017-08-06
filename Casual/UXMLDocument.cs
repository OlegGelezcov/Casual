using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_IOS
using System.Xml;
#else
using System.Xml.Linq;
#endif
using UnityEngine;

namespace Casual {

    public class UXMLDocument {

#if UNITY_IOS
        private XmlDocument m_Document;
#else
        private XDocument m_Document;
#endif

        public void Load(string resourcePath) {
            TextAsset asset = Resources.Load<TextAsset>(resourcePath);

#if UNITY_IOS
            m_Document = new XmlDocument();
            m_Document.LoadXml(asset.text);
#else
            m_Document = XDocument.Parse(asset.text);
#endif
        }

        public void Parse(string xml) {
#if UNITY_IOS
            m_Document = new XmlDocument();
            m_Document.LoadXml(xml);
#else
            m_Document = XDocument.Parse(xml);
#endif
        }

        public UXMLElement Element(string name) {
            if (m_Document != null) {
#if UNITY_IOS
                foreach(XmlNode node in m_Document.ChildNodes ) {
                    if(node.Name == name ) {
                        return new UXMLElement(node);
                    }
                }
#else
                var elem = m_Document.Element(name);
                if (elem != null) {
                    return new UXMLElement(elem);
                }
#endif
            }
            return null;
        }
    }


    public class UXMLElement {
#if UNITY_IOS
        private XmlNode m_Node;
#else
        private XElement m_Node;
#endif

#if UNITY_IOS
        public UXMLElement(XmlNode node) {
            m_Node = node;
        }
#else
        public UXMLElement(XElement node) {
            m_Node = node;
        }
#endif

        public override string ToString() {
#if UNITY_IOS
            return m_Node.OuterXml;
#else
            return m_Node.ToString();
#endif
        }

        public string GetString(string name) {
#if UNITY_IOS
            foreach(XmlAttribute attrib in m_Node.Attributes ) {
                if(attrib.Name == name ) {
                    return attrib.Value;
                }
            }
#else
            foreach (XAttribute attr in m_Node.Attributes()) {
                if (attr.Name == name) {
                    return attr.Value;
                }
            }
#endif
            return string.Empty;
        }

        public float GetFloat(string name) {
            string strVal = GetString(name);
            float val;
            if (float.TryParse(strVal, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out val)) {
                return val;
            }
            return 0.0f;
        }

        public T GetEnum<T>(string name) {
            return GetEnum<T>(name, default(T));
        }

        public T GetEnum<T>(string name, T defaultValue = default(T)) {
            string strVal = GetString(name);
            if(string.IsNullOrEmpty(strVal)) {
                return defaultValue;
            }
            return (T)(object)Enum.Parse(typeof(T), GetString(name));
        }

        public int GetInt(string name) {
            return GetInt(name, 0);
        }

        public int GetInt(string name, int defaultValue = 0) {
            string strVal = GetString(name);

            if(string.IsNullOrEmpty(strVal)) {
                return defaultValue;
            }
            int val;
            if(int.TryParse(strVal, out val)) {
                return val;
            }
            return defaultValue;
        }

        public string[] GetStringArray(string name) {
            string str = GetString(name);
            return str.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        public List<string> GetStringList(string name) {
            return new List<string>(GetStringArray(name));
        }

        public bool GetBool(string name) {
            return GetBool(name, false);
        }

        public bool GetBool(string name, bool defaultValue = false) {
            string strVal = GetString(name);
            if(string.IsNullOrEmpty(strVal)) {
                return defaultValue;
            }
            bool val;
            if (bool.TryParse(strVal, out val)) {
                return val;
            }
            return defaultValue;
        }

        public string value {
            get {
#if UNITY_IOS
                return m_Node.InnerText.Trim();
#else
                return m_Node.Value;
#endif
            }
        }

        public UXMLElement Element(string name) {
#if UNITY_IOS
            if(m_Node != null ) {
                foreach(XmlNode node in m_Node.ChildNodes ) {
                    if(node.Name == name ) {
                        return new UXMLElement(node);
                    }
                }
            }
#else
            if (m_Node != null) {
                var elem = m_Node.Element(name);
                if (elem != null) {
                    return new UXMLElement(elem);
                }
            }
#endif
            return null;
        }

        public List<UXMLElement> Elements(string name) {


#if UNITY_IOS
            List<UXMLElement> result = new List<UXMLElement>();
            if(m_Node != null ) {
                foreach(XmlNode node in m_Node.ChildNodes ) {
                    if(node.Name == name ) {
                        result.Add(new UXMLElement(node));
                    }
                }
            }
            return result;
#else
            if (m_Node != null) {
                return m_Node.Elements(name).Select(e => new UXMLElement(e)).ToList();
            }
            return new List<UXMLElement>();
#endif

        }
        public bool HasAttribute(string name) {
#if UNITY_IOS
            foreach(XmlAttribute attr in m_Node.Attributes ) {
                if(attr.Name == name ) {
                    return true;
                }
            }
#else
            foreach (var attr in m_Node.Attributes()) {
                if (attr.Name == name) {
                    return true;
                }
            }
#endif
            return false;
        }


    }

    public class UXMLAttribute {
        public string name { get; private set; }
        public string value { get; private set; }

        public UXMLAttribute(string name, string value) {
            this.name = name;
            this.value = value;
        }

        public UXMLAttribute(string name, int value)
            : this(name, value.ToString()) { }

        public UXMLAttribute(string name, float value)
            : this(name, value.ToString(System.Globalization.CultureInfo.InvariantCulture)) { }

        public UXMLAttribute(string name, bool value)
            : this(name, value.ToString()) { }

    }



    public class UXMLWriteElement {

        public string name { get; private set; }
        public List<UXMLWriteElement> childrens { get; private set; }
        public List<UXMLAttribute> attributes { get; private set; }

        public UXMLWriteElement(string name) {
            this.name = name;
            this.childrens = new List<UXMLWriteElement>();
            this.attributes = new List<UXMLAttribute>();
        }

        public void AddAttribute(string name, string value) {
            attributes.Add(new UXMLAttribute(name, value));
        }

        public void AddAttribute(string name, int value) {
            attributes.Add(new UXMLAttribute(name, value));
        }

        public void AddAttribute(string name, float value) {
            attributes.Add(new UXMLAttribute(name, value));
        }

        public void AddAttribute(string name, bool value) {
            attributes.Add(new UXMLAttribute(name, value));
        }

        public void Add(UXMLWriteElement element) {
            childrens.Add(element);
        }

        public override string ToString() {
#if UNITY_IOS
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true });
            WriteToXmlWriter(writer);
            writer.Close();
            return sb.ToString();
#else
            return WriteToXElement(null).ToString();
#endif
        }

#if UNITY_IOS
        private void WriteToXmlWriter(XmlWriter writer) {
            writer.WriteStartElement(name);
            foreach (var attr in attributes) {
                writer.WriteAttributeString(attr.name, attr.value);
            }
            foreach (var child in childrens) {
                child.WriteToXmlWriter(writer);
            }
            writer.WriteEndElement();
        }
#else
        private XElement WriteToXElement(XElement parent) {
            XElement current = new XElement(name);
            foreach (var attr in attributes) {
                current.SetAttributeValue(attr.name, attr.value);
            }

            foreach (var ch in childrens) {
                ch.WriteToXElement(current);
            }

            if (parent != null) {
                parent.Add(current);
            }

            if (parent != null) {
                return parent;
            } else {
                return current;
            }
        }
#endif

    }


}
