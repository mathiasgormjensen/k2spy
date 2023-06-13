using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    /*
    <image>
        <name>Close_8x_8x_RED.png</name>
        <content>iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAACDSURBVChTjZBLCkMhEAR7HnqLLHP/K3gKQRC8QbbiDye0eQuTTSwQG7vAUdEPGGPAWoud3juMMRARXJRKKYgxorV2K1g5pbQ6OjxQ772+Hk91zmmtdS1mnoUQlA7tr4L7ntmRJZJd/pXIdY/0H9pHVx8/Zs6pOecl7zMxU2JH5/DDBW83RP8X4l4/fwAAAABJRU5ErkJggg==</content>
    </image>
    */
    [System.Xml.Serialization.XmlType("image")]
    public class ImageObject
    {
        public const string Scnull = "<image><name>scnull</name><content>scnull</content></image>";

        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("content")]
        public string ContentAsBase64 { get; set; }

        public static ImageObject Deserialize(string xml)
        {
            return Xml.DeserializeString<ImageObject>(xml);
        }

        public static ImageObject FromFile(string filePath)
        {
            ImageObject imageObject = new ImageObject();
            imageObject.Name = System.IO.Path.GetFileName(filePath);
            imageObject.ContentAsBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
            return imageObject;
        }

        public string Serialize()
        {
            return Xml.SerializeToString(this, false, true);
        }
    }
}