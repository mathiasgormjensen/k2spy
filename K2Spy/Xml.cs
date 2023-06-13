using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public static class Xml
    {
        public static System.Xml.Serialization.XmlSerializer CreateSerializer<T>()
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return xmlSerializer;
        }

        public static string RemoveUtf8Bom(string xml)
        {
            string result = xml?.TrimStart((char)65279);
            return result;
        }

        public static string SerializeToString<T>(T value, bool indent = true, bool omitXmlDeclaration = false)
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                Xml.Serialize<T>(value, stream, indent, omitXmlDeclaration);
                stream.Flush();
                byte[] data = stream.ToArray();
                string result = Encoding.UTF8.GetString(data);
                return Xml.RemoveUtf8Bom(result);
            }
        }

        public static void Serialize<T>(T value, string path, bool indent = true, bool omitXmlDeclaration = false)
        {
            using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                Xml.Serialize(value, stream, indent, omitXmlDeclaration);
        }

        public static void Serialize<T>(T value, System.IO.Stream stream, bool indent = true, bool omitXmlDeclaration = false)
        {
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stream, new System.Xml.XmlWriterSettings() { Indent = indent, IndentChars = "\t", CloseOutput = false, OmitXmlDeclaration = omitXmlDeclaration }))
            {
                System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces(new System.Xml.XmlQualifiedName[] { System.Xml.XmlQualifiedName.Empty });
                Xml.CreateSerializer<T>().Serialize(writer, value, namespaces);
            }
        }

        public static T DeserializeString<T>(string xml, bool preserveWhitespace = true)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes))
                return Xml.Deserialize<T>(stream, preserveWhitespace);
        }

        public static T Deserialize<T>(string path, bool preserveWhitespace = true)
        {
            using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                return Xml.Deserialize<T>(stream, preserveWhitespace);
        }

        public static T Deserialize<T>(System.IO.Stream value, bool preserveWhitespace = true)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = Xml.CreateSerializer<T>();
            if (preserveWhitespace)
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.PreserveWhitespace = true;
                document.Load(value);
                using (System.Xml.XmlReader reader = document.CreateNavigator().ReadSubtree())
                    return (T)xmlSerializer.Deserialize(reader);
            }
            else
            {
                return (T)Xml.CreateSerializer<T>().Deserialize(value);
            }
        }

        public static string Linearize(string xml)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.PreserveWhitespace = false;
            document.LoadXml(xml);
            return document.OuterXml;
        }

        public static string PrettyPrint(System.Xml.XmlDocument document)
        {
            StringBuilder builder = new StringBuilder();
            System.Xml.XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(builder, xmlWriterSettings);
            document.WriteTo(xmlWriter);
            xmlWriter.Flush();
            return builder.ToString();
        }

        public static string PrettyPrint(string xml)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xml);

            return Xml.PrettyPrint(document);
        }
    }
}