using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.TreeNodeStateMemory
{
    internal class TreeNodeStateMemoryExtension : Model.IInternalExtension, Model.IInitializedExtension
    {
        #region Private Fields

        private TreeNodeState m_TreeNodeState;
        private List<K2SpyTreeNode> m_Nodes = new List<K2SpyTreeNode>();

        #endregion

        #region Constructors

        public TreeNodeStateMemoryExtension()
        {
            this.m_TreeNodeState = TreeNodeState.Load();
        }

        #endregion

        #region Public Properties

        public string DisplayName => "Tree node state memory";

        #endregion

        #region Public Methods

        public async Task InitializedAsync(K2SpyContext k2SpyContext)
        {
            k2SpyContext.Connected += this.K2SpyContext_Connected;

            k2SpyContext.TreeView.AfterCollapse += this.TreeView_AfterCollapse;
            k2SpyContext.TreeView.AfterExpand += this.TreeView_AfterExpand;
            k2SpyContext.TreeView.AfterSelect += this.TreeView_AfterSelect;
        }

        #endregion

        #region Protected Methods

        protected async Task ApplyExpandedStateAsync(TreeNodeCollection nodes, bool apply, bool recursive)
        {
            foreach (TreeNode child in nodes)
                await this.ApplyExpandedStateAsync(child, apply, recursive);

        }

        protected async Task ApplyExpandedStateAsync(TreeNode node, bool apply, bool recursive)
        {
            if (node.TreeView == null)
                return;

            if (recursive)
                await this.ApplyExpandedStateAsync(node.Nodes, apply, true);

            if (node is K2SpyTreeNode k2SpyTreeNode && !this.m_Nodes.Contains(k2SpyTreeNode))
            {
                k2SpyTreeNode.Refreshed += async (sender, e) =>
                {
                    if (k2SpyTreeNode.Nodes.Count > 0)
                    {
                        //using (new LayoutSuspender(k2SpyTreeNode.TreeView))
                        {
                            using (TreeViewUpdateContext.CreateIfNecessary(k2SpyTreeNode))
                            {
                                await this.ApplyExpandedStateAsync(k2SpyTreeNode.Nodes, true, true);
                            }
                        }
                    }
                };
            }

            if (apply && this.m_TreeNodeState.ExpandedTreeNodes.Contains(node.FullPath))
            {
                TreeNode parent = node.Parent;
                if (parent != null)
                {
                    //node.TreeView.BeginUpdate();
                    int index = parent.Nodes.IndexOf(node);
                    parent.Nodes.Remove(node);
                    if (node is IInitializeChildren initializeChildren)
                        await initializeChildren.InitializeChildrenAsync();

                    node.Expand();
                    parent.Nodes.Insert(index, node);
                    //node.TreeView.EndUpdate();
                }
                else
                {
                    //node.TreeView.BeginUpdate();
                    node.Expand();
                    //node.TreeView.EndUpdate();
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task K2SpyContext_Connected(object sender, EventArgs e)
        {
            K2SpyContext context = (K2SpyContext)sender;
            await this.ApplyExpandedStateAsync(context.TreeView.Nodes, true, true);
            if (!string.IsNullOrEmpty(this.m_TreeNodeState.SelectedTreeNode))
            {
                await context.TreeView.SelectNodeByPathAsync(this.m_TreeNodeState.SelectedTreeNode, false);
            }
        }

        private void TreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.m_TreeNodeState.SelectedTreeNode = e.Node?.FullPath;
            this.m_TreeNodeState.QueueSave();
        }

        private void TreeView_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.m_TreeNodeState.ExpandedTreeNodes.Add(e.Node.FullPath);
            this.m_TreeNodeState.QueueSave();
        }

        private void TreeView_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            while (this.m_TreeNodeState.ExpandedTreeNodes.Remove(e.Node.FullPath)) ;
            this.m_TreeNodeState.QueueSave();
        }

        #endregion
    }
}
