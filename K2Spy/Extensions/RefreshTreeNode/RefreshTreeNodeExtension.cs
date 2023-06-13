using K2Spy.ExtensionMethods;
using K2Spy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static K2Spy.MainForm;

namespace K2Spy.Extensions.RefreshTreeNode
{
    internal class RefreshTreeNodeExtension : Model.IExtension, Model.ITreeViewContextMenuExtension, Model.ILeftAlignedCommandBarExtension, Model.ISelectedNodeChangedExtension, ISelectedNodeResetExtension
    {
        private ToolStripButton m_RefreshToolStripButton = null;

        public string DisplayName => "Refresh tree node";

        public ToolStripItem[] CreateCommandBarItems(K2SpyContext k2SpyContext)
        {
            ToolStripButton refreshToolStripButton = new ToolStripButton("Refresh", Properties.Resources.Refresh_16x, async (sender, e) =>
            {
                await k2SpyContext.MainForm.TryAsync(async () =>
                {
                    using (StopWatch.CurrentMethod())
                    {
                        using (new DisableControlContext(this.m_RefreshToolStripButton))
                        {
                            await (k2SpyContext.TreeView.SelectedNode as K2SpyTreeNode)?.RefreshAsync();
                        }
                    }
                    Serilog.Log.Debug("Category data count: " + k2SpyContext.Cache.CategoryDataCache.Count);
                    Application.DoEvents();
                });
            });
            refreshToolStripButton.Enabled = false;
            refreshToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.m_RefreshToolStripButton = refreshToolStripButton;
            return new ToolStripItem[] { refreshToolStripButton };
        }

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("Refresh",Properties.Resources.Refresh_16x, (sender, e) =>
                {
                    k2SpyContext.MainForm.Try(() =>
                    {
                        this.m_RefreshToolStripButton.PerformClick();
                    });
                }).With(key => key.Enabled = treeNode?.CanRefresh == true)
            };
        }

        public void SelectedNodeChanged(TreeNode treeNode)
        {
            if (this.m_RefreshToolStripButton != null)
                this.m_RefreshToolStripButton.Enabled = (treeNode as K2SpyTreeNode)?.CanRefresh == true;
        }

        public void SelectedNodeReset()
        {
            this.SelectedNodeChanged(null);
        }
    }
}
