using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.GoToDefinition
{
    internal class GoToDefinitionExtension : Model.ITreeViewContextMenuExtension
    {
        public string DisplayName => "Go to definition";

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            if (!(treeNode is K2SpyTreeNodeClone) && actingAsOrSelfTreeNode != null && treeNode != actingAsOrSelfTreeNode)
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Go to definition", null, (sender, e) =>
                    {
                        context.MainForm.Try(() => context.TreeView.SelectNode(actingAsOrSelfTreeNode));
                    })
                };
            }
            return null;
        }
    }
}
