using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private class AnalyzerTreeNode : TreeNode
        {
            private AnalyzerTreeNode(K2SpyTreeNode source)
            {
                this.Source = source;
                this.ImageKeyFrom(source);
                base.Text = source.Text;
                base.ToolTipText = source.FullPath;

                source.Disposed += (sender, e) =>
                {
                    base.Remove();
                };
            }

            public K2SpyTreeNode Source { get; private set; }

            public static async Task<AnalyzerTreeNode> CreateAsync(K2SpyTreeNode source)
            {
                AnalyzerTreeNode analyzerTreeNode = new AnalyzerTreeNode(source);


                //if (SmartObjectMethodsTreeNode.IsSupported(source))
                //    base.Nodes.Add(new SmartObjectMethodsTreeNode(source));

                //if (SmartObjectPropertiesTreeNode.IsSupported(source))
                //    base.Nodes.Add(new SmartObjectPropertiesTreeNode(source));

                if (await UsesTreeNode.IsSupportedAsync(source))
                    analyzerTreeNode.Nodes.Add(new UsesTreeNode(source));

                //if (BruteforceUsesTreeNode.IsSupported(source))
                //    base.Nodes.Add(new BruteforceUsesTreeNode(source));

                if (await UsedByTreeNode.IsSupportedAsync(source))
                    analyzerTreeNode.Nodes.Add(new UsedByTreeNode(source));

                //if (BruteforceUsedByTreeNode.IsSupported(source))
                //    base.Nodes.Add(new BruteforceUsedByTreeNode(source));

                if (ServiceInstanceSmartObjectsTreeNode.IsSupported(source))
                    analyzerTreeNode.Nodes.Add(new ServiceInstanceSmartObjectsTreeNode(source));

                //analyzerTreeNode.Nodes.Add(new LoadingTreeNode());

                return analyzerTreeNode;
            }

            public static void InsertInAlphabeticOrder(TreeNode parent, AnalyzerTreeNode node)
            {
                using (new LayoutSuspender(parent.TreeView))
                {
                    List<AnalyzerTreeNode> list = parent.Nodes.OfType<AnalyzerTreeNode>().ToList();
                    list.Add(node);
                    int offset = parent.Nodes.OfType<TreeNode>().TakeWhile(key => !(key is AnalyzerTreeNode)).Count();
                    list = list.OrderBy(key => key.Text).ToList();
                    int index = list.IndexOf(node) + offset;
                    parent.Nodes.Insert(index, node);
                }
                Application.DoEvents();
            }
        }
    }
}
