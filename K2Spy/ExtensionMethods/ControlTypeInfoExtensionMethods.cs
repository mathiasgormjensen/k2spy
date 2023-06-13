using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ControlTypeInfoExtensionMethods
    {
        public static System.Xml.XmlDocument ToXml(this SourceCode.Forms.Management.ControlTypeInfo that)
        {
            Type type = that.GetType();
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", null));
            document.AppendChild(document.CreateComment(" Please note: This XML-format is not compatible with controlutil.exe "));
            document.AppendChild(document.CreateElement(type.FullName));
            ControlTypeInfoExtensionMethods.ToXml(document.DocumentElement, that, new object[0]);
            return document;
        }

        private static void ToXml(System.Xml.XmlNode parentNode, object currentItem, object[] parents)
        {
            if (currentItem == null)
                return;
            if (parents.Contains(currentItem))
                return;

            object[] parentsAndCurrent = parents.Union(new object[] { currentItem }).ToArray();
            System.Xml.XmlDocument document = parentNode as System.Xml.XmlDocument ?? parentNode.OwnerDocument;
            Type type = currentItem.GetType();
            parentNode.AppendChild(document.CreateComment(" Type: " + type.FullName + " "));

            if (type.IsValueType || type == typeof(string))
            {
                parentNode.InnerText = Convert.ToString(currentItem);
            }
            else if (currentItem is System.Collections.IEnumerable)
            {
                foreach (object item in (System.Collections.IEnumerable)currentItem)
                {
                    Type type2 = item.GetType();
                    System.Xml.XmlElement element = (System.Xml.XmlElement)parentNode.AppendChild(document.CreateElement(type2.FullName));
                    ToXml(element, item, parentsAndCurrent);
                }
            }
            else
            {
                foreach (System.Reflection.FieldInfo field in type.GetFields())
                {
                    System.Xml.XmlElement element = (System.Xml.XmlElement)parentNode.AppendChild(document.CreateElement(field.Name));
                    object value = field.GetValue(currentItem);
                    ToXml(element, value, parentsAndCurrent);
                }

                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    if (property.GetIndexParameters().Count() == 0)
                    {
                        System.Xml.XmlElement element = (System.Xml.XmlElement)parentNode.AppendChild(document.CreateElement(property.Name));
                        object value = property.GetValue(currentItem);
                        ToXml(element, value, parentsAndCurrent);
                    }
                }
            }
        }
    }
}
