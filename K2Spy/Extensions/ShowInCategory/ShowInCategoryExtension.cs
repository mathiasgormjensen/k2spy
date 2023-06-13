using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.GoToCategory
{
    internal class ShowInCategoryExtension : Model.ITreeViewContextMenuExtension
    {
        public string DisplayName => "Go to category";

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            Guid guid = Guid.Empty;
            string processFullName = null;
            if (treeNode is SmartObjectInfoTreeNode smartObjectInfoTreeNode)
            {
                guid = smartObjectInfoTreeNode.SmartObjectGuid;
            }
            else if (treeNode is ViewInfoTreeNode viewInfoTreeNode)
            {
                guid = viewInfoTreeNode.ViewGuid;
            }
            else if (treeNode is FormInfoTreeNode formInfoTreeNode)
            {
                guid = formInfoTreeNode.FormGuid;
            }
            else if (treeNode is WorkflowTreeNode processSetTreeNode)
            {
                processFullName = processSetTreeNode.ProcessFullName;
            }
            else if (treeNode is StyleProfileInfoTreeNode styleProfileInfoTreeNode)
            {
                guid = styleProfileInfoTreeNode.CacheKey;
            }
            if (guid != Guid.Empty || !string.IsNullOrEmpty(processFullName))
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Show in category", null, (sender, e) =>
                    {
                        context.MainForm.Try(()=>
                        {
                            CategoryRootTreeNode root = context.TreeView.Nodes.OfType<CategoryRootTreeNode>().Single();
                            CategoryDataTreeNode[] candidates =root.DescendantsWhere(descendant => true).OfType<CategoryDataTreeNode>().ToArray();
                            K2SpyTreeNode match = null;
                            if (guid != Guid.Empty)
                            {
                                match = root.DescendantsOfTypeWhere<CategoryDataTreeNode>(node => node.DataAsGuid == guid).Single();
                            }
                            else
                            {
                                match = root.DescendantsOfTypeWhere<CategoryDataTreeNode>(node => node.Data.Equals(processFullName, StringComparison.OrdinalIgnoreCase)).Single();
                            }
                            context.TreeView.SelectNode(match,true);
                        });
                    })
                };
            }
            return null;
        }
    }
}
