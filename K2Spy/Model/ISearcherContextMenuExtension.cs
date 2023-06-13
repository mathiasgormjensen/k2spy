using System.Windows.Forms;

namespace K2Spy.Model
{
    public interface ISearcherContextMenuExtension : IExtension
    {
        ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode);
    }
}
