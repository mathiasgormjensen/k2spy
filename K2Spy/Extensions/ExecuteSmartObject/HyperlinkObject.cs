namespace K2Spy.Extensions.ExecuteSmartObject
{
    /*
    <hyperlink>
        <link>b</link>
        <display>a</display>
    </hyperlink>
             */
    [System.Xml.Serialization.XmlType("hyperlink")]
    public class HyperlinkObject
    {
        public const string Scnull = "<hyperlink><display>scnull</display><link>scnull</link></hyperlink>";

        [System.Xml.Serialization.XmlElement("display")]
        public string Display { get; set; }

        [System.Xml.Serialization.XmlElement("link")]
        public string Link { get; set; }

        public static HyperlinkObject Deserialize(string xml)
        {
            return Xml.DeserializeString<HyperlinkObject>(xml);
        }

        public string Serialize()
        {
            return Xml.SerializeToString(this, false, true);
        }
    }
}