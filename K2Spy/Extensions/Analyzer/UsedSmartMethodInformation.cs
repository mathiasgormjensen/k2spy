using System;
using System.Collections;

namespace K2Spy.Extensions.Analyzer
{
    public class UsedSmartMethodInformation : IEqualityComparer, IEquatable<UsedSmartMethodInformation>
    {
        public UsedSmartMethodInformation()
        {
        }

        public UsedSmartMethodInformation(Guid smartObjectGuid, string methodName)
        {
            this.SmartObjectGuid = smartObjectGuid;
            this.MethodName = methodName;
        }

        [System.Xml.Serialization.XmlAttribute]
        public Guid SmartObjectGuid { get; set; }

        [System.Xml.Serialization.XmlAttribute]
        public string MethodName { get; set; }

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
            if (x is UsedSmartMethodInformation x2 && y is UsedSmartMethodInformation y2)
            {
                if (x2.SmartObjectGuid.Equals(y2.SmartObjectGuid))
                {
                    return string.Equals(x2.MethodName, y2.MethodName);
                }
            }
            return false;
        }

        public int GetHashCode(object obj)
        {
            return $"{typeof(UsedSmartMethodInformation).Name}:{this.SmartObjectGuid}:{this.MethodName}".GetHashCode();
        }

        public bool Equals(UsedSmartMethodInformation other)
        {
            return this.Equals(this, other);
        }
    }
}