using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ILBLI.Core
{
    /// <summary>
    /// XML文件操作相关类
    /// </summary>
    public class XMLManager
    {
        /// <summary>
        /// 读取整个xml文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadXml(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                return xmlDoc.InnerXml;
            }
            catch(Exception e)
            {
                throw new Exception($"读取XML信息失败{e.Message}");
            }
        }

        /// <summary>
        /// 读取某个节点的XML数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static List<XmlNode> ReadXml(string filePath, string nodeName)
        {
            List<XmlNode> listNode = new List<XmlNode>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode node = xmlDoc.SelectSingleNode(nodeName);
                XmlNodeList nodeList = node.ChildNodes;
                foreach (XmlNode p in nodeList)
                {
                    listNode.Add(p);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"获取XML节点失败{e.Message}");
            }

            return listNode;
        }

        /// <summary>
        /// 读取XML某个节点的值
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetValueByAttributeName(string attrName, XmlNode node)
        {
            string value = string.Empty;

            XmlElement element = node as XmlElement;
            value = element.GetAttribute(attrName);

            return value;
        }

        /// <summary>
        /// 读取XML某个节点的值
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetValueByTagName(string tagName, XmlNode node)
        {
            string value = string.Empty;

            XmlElement element = node as XmlElement;
            var nodes = element.GetElementsByTagName(tagName);
            if (nodes.Count > 0)
            {
                value = nodes[0].Value;
            }
            return value;
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML字符</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string xml) where T:class
        {
            try
            {
                using (StringReader reader = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(reader) as T;
                }
            }
            catch(Exception e)
            {
                throw new Exception($"从xml转换为实体类失败{e.Message}");
            }
        }

        /// <summary>
        /// 将实体类转换为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }
    }

}
