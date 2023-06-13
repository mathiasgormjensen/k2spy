using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class XPathDocumentExtensionMethods
    {
        public static string[] SelectValues(this System.Xml.XPath.XPathDocument that, string xpath)
        {
            return that.Select(xpath, item => item.Value);
        }

        public static TValue[] Select<TValue>(this System.Xml.XPath.XPathDocument that, string xpath, Func<System.Xml.XPath.XPathItem, TValue> getValue)
        {
            return that.CreateNavigator().Select(xpath, getValue);
        }
    }
    public static class XPathNavigatorExtensionMethods
    {
        public static string[] SelectValues(this System.Xml.XPath.XPathNavigator that, string xpath)
        {
            return that.Select(xpath, item => item.Value);
        }

        public static TValue[] Select<TValue>(this System.Xml.XPath.XPathNavigator that, string xpath, Func<System.Xml.XPath.XPathItem, TValue> getValue)
        {
            List<TValue> list = new List<TValue>();
            foreach (System.Xml.XPath.XPathNavigator item in that.Select(xpath))
                list.Add(getValue(item));
            return list.ToArray();
        }
    }
}
