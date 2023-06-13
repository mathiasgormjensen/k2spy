using System.Windows.Forms;

namespace K2Spy.Model
{
    public interface IAnalyzerContextMenuExtension : IExtension
    {
        ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode);
    }
}
