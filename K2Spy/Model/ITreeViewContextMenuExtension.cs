using System.Windows.Forms;

namespace K2Spy.Model
{
    public interface ITreeViewContextMenuExtension : IExtension
    {
        ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode);
    }
}
