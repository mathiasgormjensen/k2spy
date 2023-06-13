using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string ReplaceInvalidFileNameChars(this string that, string replacement = "_")
        {
            if (!string.IsNullOrEmpty(that))
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    that = that.Replace(c.ToString(), replacement);
            return that;
        }

        public static string UpperFirstCharacter(this string that)
        {
            if (!string.IsNullOrWhiteSpace(that))
                return that.Substring(0, 1).ToUpperInvariant() + that.Substring(1);
            return that;
        }

        public static System.Xml.XPath.XPathDocument AsXPathDocument(this string that)
        {
            using (System.IO.StringReader reader = new System.IO.StringReader(that))
            {
                System.Xml.XPath.XPathDocument document = new System.Xml.XPath.XPathDocument(reader);
                return document;
            }
        }
    }
}