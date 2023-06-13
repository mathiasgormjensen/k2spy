using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    /*
<file>
	<name>justincase.txt</name>
	<content>U3ViIHRlc3QoKQ0KRGltIHdiIEFzIFdvcmtib29rDQpGb3IgRWFjaCB3YiBJbiBBcHBsaWNhdGlvbi5Xb3JrYm9va3MNCkRpbSB5IEFzIFZhcmlhbnQNCidTZXQgeSA9IEFjdGl2ZVdvcmtib29rLlZCUHJvamVjdC5WQkNvbXBvbmVudHMNCkRpbSB4IEFzIE5ldyBWQkFDb2RlQ2xlYW5lci5Db2RlQ2xlYW5lcg0KDQpJZiB4Lkhhc0NvZGVUb0NsZWFuKHdiLlZCUHJvamVjdC5WQkNvbXBvbmVudHMpIFRoZW4NCiAgICB4LkV4cG9ydE1vZHVsZXMgd2IsIENyZWF0ZURpcmVjdG9yeSgiYzpcYmVmb3JlXCIgJiB3Yi5OYW1lKQ0KICAgIElmIHguQ2xlYW5Qcm9qZWN0KHdiKSBPciBUcnVlIFRoZW4NCiAgICAgICAgeC5FeHBvcnRNb2R1bGVzIHdiLCBDcmVhdGVEaXJlY3RvcnkoImM6XGFmdGVyXCIgJiB3Yi5OYW1lKQ0KICAgICAgICAnTXNnQm94IHdiLk5hbWUNCiAgICBFbmQgSWYNCkVuZCBJZg0KDQpOZXh0DQonTXNnQm94IFZCQUNvZGVDbGVhbmVyLkNvZGVDbGVhbmVyLkhhc0NvZGVUb0NsZWFuDQpFbmQgU3ViDQoNCkZ1bmN0aW9uIENyZWF0ZURpcmVjdG9yeShwYXRoIEFzIFN0cmluZykNCiAgICBPbiBFcnJvciBSZXN1bWUgTmV4dA0KICAgIE1rRGlyIHBhdGgNCiAgICBPbiBFcnJvciBHb1RvIDANCiAgICBDcmVhdGVEaXJlY3RvcnkgPSBwYXRoDQpFbmQgRnVuY3Rpb24NCg0KUHJpdmF0ZSBTdWIgUHJpdmF0ZUJsaXZlckFsZHJpZ0thbGR0KCkNCiAgICAnIGRlbm5lIGJsaXZlciBhbGRyaWcga2FsZHQNCkVuZCBTdWINCg==</content>
</file>
*/
    [System.Xml.Serialization.XmlType("file")]
    public class FileObject
    {
        public const string Scnull = "<file><name>scnull</name><content>scnull</content></file>";

        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("content")]
        public string ContentAsBase64 { get; set; }

        public static FileObject Deserialize(string xml)
        {
            return Xml.DeserializeString<FileObject>(xml);
        }

        public static FileObject FromFile(string filePath)
        {
            FileObject fileObject = new FileObject();
            fileObject.Name = System.IO.Path.GetFileName(filePath);
            fileObject.ContentAsBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
            return fileObject;
        }

        public string Serialize()
        {
            return Xml.SerializeToString(this, false, true);
        }
    }
}