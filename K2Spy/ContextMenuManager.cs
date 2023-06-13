using K2Spy.ExtensionMethods;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public static class ContextMenuManager
    {
        private delegate ToolStripItem[] ToolStripItemsFactory(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode);

        public static async Task ShowTreeViewContextMenuAsync(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, Point screenLocation)
        {
            if (k2SpyContext == null || treeNode == null)
            {
                ContextMenuManager.Close();
            }
            else
            {
                K2SpyTreeNode actingAsOrSelfTreeNode = await treeNode.GetActAsOrSelfAsync();
                await ContextMenuManager.ShowContextMenuForNodeAsync(k2SpyContext, treeNode,
                            Extensions.ExtensionsManager.GetExtensions<Model.ITreeViewContextMenuExtension>(false)
                            .OrderByDescending(key => ((key as Model.ITreeViewContextMenuPriorityExtension)?.TreeViewContextMenuPriority) ?? 0)
                            .Select(key => (ToolStripItemsFactory)key.CreateTreeViewContextMenuItems)
                            .ToArray(),
                            screenLocation);
            }
        }

        public static async Task ShowSearcherContextMenuAsync(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, Point screenLocation)
        {
            if (k2SpyContext == null || treeNode == null)
            {
                ContextMenuManager.Close();
            }
            else
            {
                K2SpyTreeNode actingAsOrSelfTreeNode = await treeNode.GetActAsOrSelfAsync();
                await ContextMenuManager.ShowContextMenuForNodeAsync(k2SpyContext, treeNode,
                            Extensions.ExtensionsManager.GetExtensions<Model.ISearcherContextMenuExtension>(false)
                            .OrderByDescending(key => ((key as Model.ISearcherContextMenuPriorityExtension)?.SearcherContextMenuPriority) ?? 0)
                            .Select(key => (ToolStripItemsFactory)key.CreateSearcherContextMenuItems)
                            .ToArray(),
                            screenLocation);
            }
        }

        public static async Task ShowAnalyzerContextMenuAsync(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, Point screenLocation)
        {
            if (k2SpyContext == null || treeNode == null)
            {
                ContextMenuManager.Close();
            }
            else
            {
                K2SpyTreeNode actingAsOrSelfTreeNode = await treeNode.GetActAsOrSelfAsync();
                await ContextMenuManager.ShowContextMenuForNodeAsync(k2SpyContext, treeNode,
                            Extensions.ExtensionsManager.GetExtensions<Model.IAnalyzerContextMenuExtension>(false)
                            .OrderByDescending(key => ((key as Model.IAnalyzerContextMenuMenuPriorityExtension)?.AnalyzerContextMenuPriority) ?? 0)
                            .Select(key => (ToolStripItemsFactory)((a, b, c) => key.CreateAnalyzerContextMenuItems(k2SpyContext, analyzerNode, treeNode, actingAsOrSelfTreeNode)))
                            .ToArray(),
                            screenLocation);
            }
        }

        public static void Close()
        {
            ContextMenuManager.m_ContextMenuStrip?.Close();
        }

        private static ContextMenuStrip m_ContextMenuStrip;
        private static async Task ShowContextMenuForNodeAsync(K2SpyContext k2SpyContext, K2SpyTreeNode node, ToolStripItemsFactory[] factories, Point screenLocation)
        {
            ContextMenuManager.m_ContextMenuStrip?.Dispose();
            ContextMenuStrip cms = new ContextMenuStrip();
            if (node != null)
            {
                K2SpyContext context = k2SpyContext;
                K2SpyTreeNode actingAsTreeNode = await node.GetActAsOrSelfAsync();
                ContextMenuManager.m_ContextMenuStrip = cms;
                foreach (ToolStripItemsFactory factory in factories)
                {
                    ToolStripItem[] items = factory(context, node, actingAsTreeNode) ?? new ToolStripItem[0];
                    if (items?.Length > 0)
                    {
                        if (cms.Items.Count > 0)
                            cms.Items.Add(new ToolStripSeparator());

                        cms.Items.AddRange(items);
                    }
                }
            }
            if (cms.Items.Count > 0)
            {
                cms.Items.UpdateFont(k2SpyContext.MainForm.Font, true, true);
                cms.Show(screenLocation);
            }
        }
    }
}