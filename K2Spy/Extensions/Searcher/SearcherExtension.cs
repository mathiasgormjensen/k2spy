using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Searcher
{
    internal class SearcherExtension : Model.ISearchPaneExtension, Model.ISearcherContextMenuExtension
    {
        public string DisplayName => "Searcher";

        public event EventHandler CloseSearchPane;
        public event EventHandler OpenSearchPane;

        public void ActivateSearchPane(K2SpyContext k2SpyContext)
        {
            this.m_TreeViewSearch.Activate();
        }

        private TreeViewSearch m_TreeViewSearch;

        public Control CreateSearchPaneControl(K2SpyContext k2SpyContext)
        {
            this.m_TreeViewSearch?.Dispose();
            this.m_TreeViewSearch = new TreeViewSearch();
            this.m_TreeViewSearch.Initialize(k2SpyContext);
            this.m_TreeViewSearch.ClosePanel += (sender, e) => this.CloseSearchPane?.Invoke(this, EventArgs.Empty);
            k2SpyContext.Disconnecting += async (sender, e) =>
            {
                k2SpyContext.ActionQueue.Queue(this.m_TreeViewSearch.Clear);
                this.CloseSearchPane?.Invoke(this, EventArgs.Empty);
            };
            return this.m_TreeViewSearch;
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            if (treeNode != null)
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Open", Properties.Resources.GoToReference_16x, (sender, e) =>
                    {
                        k2SpyContext.MainForm.Try(() => k2SpyContext.TreeView.SelectNode(treeNode));
                    })
                };
            }
            return null;
        }
    }
}
