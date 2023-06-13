using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class K2SpyTreeNodeExtensionMethods
    {
        private static Dictionary<K2SpyTreeNode, System.Xml.XPath.XPathDocument> m_Dictionary = new Dictionary<K2SpyTreeNode, System.Xml.XPath.XPathDocument>();
      
        public static async Task<System.Xml.XPath.XPathDocument> GetFormattedDefinitionAsXPathDocumentAsync(this K2SpyTreeNode that)
        {
            return await that.GetFormattedDefinitionAsXPathDocumentAsync(Properties.Settings.Default.KeepXPathDocumentsInMemory);
        }

        internal static void ClearCache()
        {
            K2SpyTreeNodeExtensionMethods.m_Dictionary.Clear();
        }

        private static async Task<System.Xml.XPath.XPathDocument> GetFormattedDefinitionAsXPathDocumentAsync(this K2SpyTreeNode that,bool considerCache)
        {
            if (considerCache)
            {
                System.Xml.XPath.XPathDocument document = null;
                if (!K2SpyTreeNodeExtensionMethods.m_Dictionary.TryGetValue(that, out document))
                {
                    Task<Definition> task = that.GetFormattedDefinitionAsync();
                    if (task != null)
                    {
                        Definition definition = await task;
                        if (definition.Type == DefinitionType.XML)
                        {
                            if (!string.IsNullOrWhiteSpace(definition.Text))
                            {
                                document = definition.Text.AsXPathDocument();
                                that.Evicted += async (sender, e) => K2SpyTreeNodeExtensionMethods.m_Dictionary.Remove(that);
                                that.Disposed += (sender, e) => K2SpyTreeNodeExtensionMethods.m_Dictionary.Remove(that);
                            }
                        }
                    }
                    K2SpyTreeNodeExtensionMethods.m_Dictionary.Add(that, document);
                }
                return document;
            }
            else
            {
                Task<Definition> task = that.GetFormattedDefinitionAsync();
                if (task != null)
                {
                    Definition definition = await task;
                    if (definition.Type == DefinitionType.XML)
                    {
                        if (!string.IsNullOrWhiteSpace(definition.Text))
                        {
                            return definition.Text.AsXPathDocument();
                        }
                    }
                }
                return null;
            }
        }

        public static async Task<K2SpyTreeNode> GetActAsOrSelfAsync(this K2SpyTreeNode that, bool recursive = true)
        {
            if (recursive)
            {
                List<K2SpyTreeNode> list = new List<K2SpyTreeNode>();
                while (that is IActAsTreeNode)
                {
                    K2SpyTreeNode result = await that.GetActAsOrSelfAsync(false);
                    if (result == that)
                        return that;
                    if (list.Contains(result))
                        throw new Exception("Circular reference");
                    list.Add(result);
                    that = result;
                }
            }
            else if (that is IActAsTreeNode actAsTreeNode)
            {
                return (await actAsTreeNode.GetActAsAsync()) ?? that;
            }
            return that;
        }
    }
}
