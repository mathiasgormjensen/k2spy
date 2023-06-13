using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Bookmarks
{
    internal class BookmarksPlugin : Model.ITreeViewContextMenuExtension, Model.IDelayedSelectedNodeChangedExtension, Model.IToolsMenuExtension,Model.IInitializedExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension
    {
        #region Private Fields

        private ToolStripMenuItem m_ToolsAddBookmarkMenuItem = null;
        private ToolStripMenuItem m_ToolsRemoveBookmarkMenuItem = null;
        private Bookmarks m_Bookmarks;

        #endregion

        #region Constructors

        public BookmarksPlugin()
        {
            this.m_Bookmarks = new Bookmarks();
        }

        #endregion

        #region Public Properties

        public string DisplayName => "Bookmarks";

        #endregion

        #region Public Methods

        public ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            string fullPath = treeNode.FullPath;
            if (!string.IsNullOrEmpty(fullPath))
            {
                if (this.m_Bookmarks.IsBookmarked(fullPath))
                    return new ToolStripItem[] { this.CreateRemoveBookmarkMenuItem(context, () => fullPath) };
                else
                    return new ToolStripItem[] { this.CreateAddBookmarkMenuItem(context, () => fullPath) };
            }
            return null;
        }

        protected ToolStripMenuItem CreateRemoveBookmarkMenuItem(K2SpyContext context, Func<string> getPath)
        {
            return new ToolStripMenuItem("Remove bookmark", Properties.Resources.ClearIcon16, (sender, e) =>
            {
                context.MainForm.Try(() =>
                {
                    string fullPath = getPath();
                    this.m_Bookmarks.Remove(fullPath);
                    this.UpdateToolsMenu(fullPath);
                });
            });
        }

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext context)
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            Func<string> getPath = () =>
            {
                return context.TreeView.SelectedNode?.FullPath;
            };
            list.Add(this.m_ToolsAddBookmarkMenuItem = this.CreateAddBookmarkMenuItem(context, getPath));
            list.Add(this.m_ToolsRemoveBookmarkMenuItem = this.CreateRemoveBookmarkMenuItem(context, getPath));
            list.Add(new ToolStripSeparator());
            list.Add(new ToolStripMenuItem("Bookmarks", Properties.Resources.BookmarkMainMenuItem_16x, (sender, e) =>
            {
                Trier.Try(() =>
                {
                    using (ManageBookmarks dlg = new ManageBookmarks(context, this.m_Bookmarks))
                    {
                        dlg.ShowDialog(context.MainForm);
                    }
                });
            }));
            return list.ToArray();
        }

        public void SelectedNodeChangedDelayed(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, CancellationTokenSource cancellationTokenSource)
        {
            string fullPath = treeNode?.FullPath;
            this.UpdateToolsMenu(fullPath);
        }

        public async Task InitializedAsync(K2SpyContext k2SpyContext)
        {
            this.m_Bookmarks.Load(k2SpyContext);
        }

        #endregion

        #region Protected Methods

        protected ToolStripMenuItem CreateAddBookmarkMenuItem(K2SpyContext context, Func<string> getPath)
        {
            return new ToolStripMenuItem("Add bookmark", Properties.Resources.Bookmark_16x, (sender, e) =>
            {
                context.MainForm.Try(() =>
                {
                    string fullPath = getPath();
                    this.m_Bookmarks.Add(fullPath);
                    this.UpdateToolsMenu(fullPath);
                });
            });
        }

        protected void UpdateToolsMenu(string fullPath)
        {
            bool notEmpty = !string.IsNullOrEmpty(fullPath);
            bool isBookmarked = notEmpty && this.m_Bookmarks.IsBookmarked(fullPath);
            this.m_ToolsAddBookmarkMenuItem?.With(key => key.Enabled = notEmpty && !isBookmarked);
            this.m_ToolsRemoveBookmarkMenuItem?.With(key => key.Enabled = notEmpty && isBookmarked);
        }

        #endregion
    }
}
