using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Analyzer
{
    public class UsesInformation
    {
        public const string CurrentVersion = "2.3";

        public UsesInformation()
        {
            this.Version = UsesInformation.CurrentVersion;
        }

        [System.Xml.Serialization.XmlAttribute]
        public DateTime Timestamp { get; set; }

        public Guid[] Guids { get; set; }

        public string[] ProcessFullNames { get; set; }

        public string[] ControlNames { get; set; }

        public UsedSmartMethodInformation[] SmartMethods { get; set; }

        public UsedSmartPropertyInformation[] SmartProperties { get; set; }

        [System.Xml.Serialization.XmlAttribute]
        public string Version { get; set; }
    }
}