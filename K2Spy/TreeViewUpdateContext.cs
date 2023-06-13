using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class TreeViewUpdateContext : IDisposable
    {
        #region Private Fields

        private System.Windows.Forms.TreeView m_TreeView;

        #endregion

        #region Constructors

        private TreeViewUpdateContext(System.Windows.Forms.TreeView treeView)
        {
            this.m_TreeView = treeView;
            this.m_TreeView?.BeginUpdate();
        }

        #endregion

        #region Public Methods

        public static TreeViewUpdateContext CreateIfNecessary(System.Windows.Forms.TreeView treeView)
        {
            return new TreeViewUpdateContext(treeView);
        }

        public static TreeViewUpdateContext CreateIfNecessary(System.Windows.Forms.TreeNode node, bool onlyIfExpanded = false)
        {
            if (node != null && node.TreeView != null && (true || (node.IsVisible && (!onlyIfExpanded || node.IsExpanded))))
                return new TreeViewUpdateContext(node.TreeView);
            return null;
        }

        public void Dispose()
        {
            this.m_TreeView?.EndUpdate();
            System.Windows.Forms.Application.DoEvents();
        }

        #endregion
    }
}