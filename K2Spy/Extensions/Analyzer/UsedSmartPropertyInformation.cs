using System;
using System.Collections;

namespace K2Spy.Extensions.Analyzer
{
    public class UsedSmartPropertyInformation : IEqualityComparer
    {
        public UsedSmartPropertyInformation()
        {
        }

        public UsedSmartPropertyInformation(Guid smartObjectGuid, string propertyName)
        {
            this.SmartObjectGuid = smartObjectGuid;
            this.PropertyName = propertyName;
        }

        [System.Xml.Serialization.XmlAttribute]
        public Guid SmartObjectGuid { get; set; }

        [System.Xml.Serialization.XmlAttribute]
        public string PropertyName { get; set; }

        public override int GetHashCode()
        {
            return this.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(this, obj);
        }

        public new bool Equals(object x, object y)
        {
            if (x is UsedSmartPropertyInformation x2 && y is UsedSmartPropertyInformation y2)
            {
                if (x2.SmartObjectGuid.Equals(y2.SmartObjectGuid))
                {
                    return string.Equals(x2.PropertyName, y2.PropertyName);
                }
            }
            return false;
        }

        public int GetHashCode(object obj)
        {
            return $"{typeof(UsedSmartPropertyInformation).Name}:{this.SmartObjectGuid}:{this.PropertyName}".GetHashCode();
        }
    }
}