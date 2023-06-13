using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace K2Spy.Plugins.NavigationHistory
{
    [Model.IgnoreExtension]
    internal class NavigationHistoryExtension : Model.IInternalExtension, Model.ILeftAlignedCommandBarExtension, Model.ISelectedNodeChangedExtension, Model.ISelectedNodeResetExtension
    {
        #region Private Fields

        private ToolStripButton m_BackButton;
        private ToolStripButton m_ForwardButton;
        private bool m_HistoryTrackingDisabled;
        private List<TreeNode> m_History = new List<TreeNode>();
        private int m_HistoryIndexPlusOne = 0;
        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Public Properties

        public string DisplayName => "Navigation history";

        #endregion

        #region Public Methods

        public ToolStripItem[] CreateCommandBarItems(K2SpyContext k2SpyContext)
        {
            this.m_K2SpyContext = k2SpyContext;

            this.m_BackButton = new ToolStripButton();
            this.m_BackButton.Image = Properties.Resources.Backwards_16x;
            this.m_BackButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.m_BackButton.Text = "Back";
            this.m_BackButton.Enabled = false;
            this.m_BackButton.Click += (sender, e) =>
            {
                k2SpyContext.MainForm.Try(() =>
                {
                    this.m_HistoryTrackingDisabled = true;
                    TreeNode node = null;
                    if (this.m_IsNullSelection)
                        node = this.m_History[(this.m_HistoryIndexPlusOne) - 1];
                    else
                        node = this.m_History[(--this.m_HistoryIndexPlusOne) - 1];
                    this.m_K2SpyContext.TreeView.SelectedNode = node;
                    this.m_K2SpyContext.TreeView.Focus();
                    if (this.m_HistoryIndexPlusOne == 1)
                    {

                    }
                    this.m_HistoryTrackingDisabled = false;
                });
            };

            this.m_ForwardButton = new ToolStripButton();
            this.m_ForwardButton.Image = Properties.Resources.Forward_16x;
            this.m_ForwardButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.m_ForwardButton.Text = "Forward";
            this.m_ForwardButton.Enabled = false;
            this.m_ForwardButton.Click += (sender, e) =>
            {
                k2SpyContext.MainForm.Try(() =>
                {
                    this.m_HistoryTrackingDisabled = true;
                    TreeNode node = this.m_History[this.m_HistoryIndexPlusOne++];
                    this.m_K2SpyContext.TreeView.SelectedNode = node;
                    this.m_K2SpyContext.TreeView.Focus();
                    this.m_HistoryTrackingDisabled = false;
                });
            };

            k2SpyContext.Disconnecting += async (sender, e) =>
            {
                k2SpyContext.MainForm.Try(() =>
                {
                    this.m_History.Clear();
                    this.m_HistoryIndexPlusOne = 0;
                    this.m_BackButton.Enabled = false;
                    this.m_ForwardButton.Enabled = false;
                });
            };

            return new ToolStripItem[] { this.m_BackButton, this.m_ForwardButton };
        }

        private bool m_IsNullSelection = false;
        public void SelectedNodeChanged(TreeNode treeNode)
        {
            this.m_IsNullSelection = treeNode == null;
            if (!this.m_HistoryTrackingDisabled)
            {
                if (this.m_History.LastOrDefault() != treeNode)
                {
                    this.m_History.RemoveRange(this.m_HistoryIndexPlusOne, this.m_History.Count - this.m_HistoryIndexPlusOne);
                    if (!this.m_IsNullSelection)
                        this.m_History.Add(treeNode);
                    this.m_HistoryIndexPlusOne = this.m_History.Count;
                }
            }
            this.m_BackButton.Enabled = this.m_HistoryIndexPlusOne > 1 || this.m_IsNullSelection;
            this.m_ForwardButton.Enabled = this.m_HistoryIndexPlusOne < this.m_History.Count;
        }

        public void SelectedNodeReset()
        {
            this.SelectedNodeChanged(null);
        }

        #endregion
    }
}
