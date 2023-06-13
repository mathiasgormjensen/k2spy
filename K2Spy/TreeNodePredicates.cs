using K2Spy.ExtensionMethods;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace K2Spy
{
    [Obsolete("", true)]
    public delegate bool TreeNodePredicate(TreeNode node);

    public delegate Task<bool> AsyncTreeNodePredicate(TreeNode node);

    public static class TreeNodePredicates
    {
        public static AsyncTreeNodePredicate BuildIsDefinitionXPathMatchPredicate(K2SpyContext k2SpyContext, string xpath, Guid? sanityCheckGuid)
        {
            return async node =>
            {
                if (node is K2SpyTreeNode node2)
                {
                    System.Xml.XPath.XPathDocument document = await node2.GetFormattedDefinitionAsXPathDocumentAsync();
                    bool xmlCheck = false;
                    if (document != null)
                    {
                        xmlCheck = document.CreateNavigator().SelectSingleNode(xpath) != null;
                        if (xmlCheck)
                        {

                        }
                    }
                    return xmlCheck;
                }
                return false;
            };
        }

        public static AsyncTreeNodePredicate BuildIsNodeTextMatchAsyncPredicate(string text)
        {
            return async node => node?.Text?.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static AsyncTreeNodePredicate BuildIsSearchableValueOrNodeTextMatchAsyncPredicate(string text)
        {
            AsyncTreeNodePredicate isNodeTextMatchPredicate = TreeNodePredicates.BuildIsNodeTextMatchAsyncPredicate(text);
            return async node =>
            {
                if ((node as K2SpyTreeNode)?.GetSearchableValues()?.Any(key => key.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0) == true)
                    return true;
                return await isNodeTextMatchPredicate(node);
            };
        }

        public static AsyncTreeNodePredicate BuildIsDefinitionMatchPredicate(string text)
        {
            return async node =>
            {
                if (node is K2SpyTreeNode node2)
                {
                    node2 = await node2.GetActAsOrSelfAsync();
                    Definition definition = await node2.GetFormattedDefinitionAsync();
                    return definition?.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                return false;
            };
        }

        //[Obsolete("", true)]
        //public static TreeNodePredicate BuildIsDefinitionMatchPredicate(K2SpyContext k2SpyContext, string text, bool considerXPath)
        //{
        //    if (considerXPath)
        //    {
        //        try
        //        {
        //            System.Xml.XPath.XPathExpression xpath = System.Xml.XPath.XPathExpression.Compile(text);
        //            if (xpath != null)
        //            {
        //                return TreeNodePredicates.BuildIsDefinitionXPathMatchPredicate(k2SpyContext, xpath);
        //            }
        //        }
        //        catch { }
        //    }
        //    return TreeNodePredicates.BuildIsDefinitionMatchPredicate(text);
        //}

        public static AsyncTreeNodePredicate BuildIsDefinitionMatchPredicate(Guid guid)
        {
            string[] variations = new string[] { guid.ToString().Replace("{", "").Replace("}", ""), guid.ToString(), guid.ToString("N") };
            return async node =>
            {
                if (node is K2SpyTreeNode node2)
                {
                    Definition formattedDefinition = await node2.GetFormattedDefinitionAsync();
                    if (formattedDefinition != null)
                    {
                        string definition = formattedDefinition.Text;
                        foreach (string variation in variations)
                            if (definition.IndexOf(variation, StringComparison.OrdinalIgnoreCase) >= 0)
                                return true;
                    }
                }
                return false;
            };// .IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static Task<bool> IsServiceObjectsRootOrServiceTreeNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is ServiceTypesRootTreeNode || node is ServiceTypeTreeNode);
        }

        public static Task<bool> IsServiceInstanceTreeNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is ServiceInstanceTreeNode);
        }

        public static Task<bool> IsCategoryTreeNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is CategoryTreeNode);
        }

        public static async Task<bool> IsCategoryTreeNodeOrCategoryDataTreeNodeAsync(TreeNode node)
        {
            if (await IsCategoryTreeNodeAsync(node))
                return true;
            if (node is CategoryDataTreeNode)
            {
                return true;
            }
            return false;
        }

        public static Task<bool> TrueAsync(TreeNode node)
        {
            return Task.FromResult(true);
        }

        public static Task<bool> IsDefinitionContainerNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is CategoryTreeNode || node is ServiceTypesRootTreeNode || node is ServiceTypeTreeNode);
        }

        public static Task<bool> IsDefinitionNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is CategoryDataTreeNode || node is ServiceInstanceTreeNode);
        }

        public static Task<bool> IsCategoryDataTreeNodeAsync(TreeNode node)
        {
            return Task.FromResult(node is CategoryDataTreeNode);
        }

        public static async Task<bool> IsCategoryDataFormOrViewNodeAsync(TreeNode node)
        {
            if (node is CategoryDataTreeNode)
            {
                SourceCode.Categories.Client.CategoryData source = await ((CategoryDataTreeNode)node).GetSourceAsync();
                if (source != null)
                {
                    if (source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.Form || source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.View)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static async Task<bool> IsCategoryDataFormViewNodeOrChildOfCategoryDataNodeAsync(TreeNode node)
        {
            if (await TreeNodePredicates.IsCategoryDataFormOrViewNodeAsync(node))
                return true;
            if (node.Parent is CategoryDataTreeNode)
                return true;
            return false;
        }

        public static async Task<bool> IsCategoryDataFormViewOrWorkflowNodeAsync(TreeNode node)
        {
            if (node is CategoryDataTreeNode)
            {
                SourceCode.Categories.Client.CategoryData source = ((CategoryDataTreeNode)node).GetSource();
                if (source != null)
                {
                    if (source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.Form || source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.View || source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.Workflow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static async Task<bool> IsCategoryDataFormViewWorkflowNodeOrChildOfCategoryDataAsync(TreeNode node)
        {
            if (await TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync(node))
                return true;
            if (node.Parent is CategoryDataTreeNode)
                return true;
            return false;
        }

        public static async Task<bool> IsCategoryDataFormNodeAsync(TreeNode node)
        {
            return await TreeNodePredicates.CategoryDataTreeNodeOfTypeAsync(node, SourceCode.Categories.Client.CategoryServer.dataType.Form);
        }

        public static async Task<bool> IsCategoryDataViewNodeAsync(TreeNode node)
        {
            return await TreeNodePredicates.CategoryDataTreeNodeOfTypeAsync(node, SourceCode.Categories.Client.CategoryServer.dataType.View);
        }

        public static async Task<bool> IsCategoryDataSmartObjectNodeAsync(TreeNode node)
        {
            return await TreeNodePredicates.CategoryDataTreeNodeOfTypeAsync(node, SourceCode.Categories.Client.CategoryServer.dataType.SmartObject);
        }

        public static async Task<bool> IsCategoryDataWorkflowNodeAsync(TreeNode node)
        {
            return await TreeNodePredicates.CategoryDataTreeNodeOfTypeAsync(node, SourceCode.Categories.Client.CategoryServer.dataType.Workflow);
        }

        private static async Task<bool> CategoryDataTreeNodeOfTypeAsync(TreeNode node, SourceCode.Categories.Client.CategoryServer.dataType type)
        {
            if (node is CategoryDataTreeNode categoryDataTreeNode)
            {
                SourceCode.Categories.Client.CategoryData categoryData = await categoryDataTreeNode.GetSourceAsync();
                if (categoryData.DataType == type)
                    return true;
            }
            return false;
        }
    }
}