using K2Spy.ExtensionMethods;
using System;
using System.Windows.Forms;

namespace K2Spy
{
    internal class K2SpyTreeNodeListViewItem : ListViewItem
    {
        #region Private Fields

        private K2SpyTreeNode m_Source;

        #endregion

        #region Constructors

        public K2SpyTreeNodeListViewItem(K2SpyTreeNode source)
        {
            this.m_Source = source;
            this.ImageIndexFrom(source);
            // base.ImageIndex = Math.Max(0, source.TreeView.ImageList.Images.IndexOfKey(source.ImageKey));
            base.Text = source.Text;
            base.SubItems.Add(source.FullPath);
        }

        #endregion

        #region Public Methods

        public void SelectTreeNode()
        {
            this.m_Source.TreeView.SelectedNode = this.m_Source;
            this.m_Source.TreeView.Focus();
        }

        #endregion
    }
}