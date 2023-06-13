using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace K2Spy.Extensions.Searcher
{
    internal partial class TreeViewSearch : UserControl
    {
        #region Private Fields

        private System.Threading.CancellationTokenSource m_SearchCancellationTokenSource;
        private K2SpyContext m_Context;
        private ListViewSortableVirtualList<K2SpyTreeNode> m_ListViewSortableVirtualList;

        #endregion

        #region Constructors

        public TreeViewSearch()
        {
            InitializeComponent();

            this.SetDoubleBuffered(true);

            foreach (ToolStripMenuItem item in this.tsddbtnSearchType.DropDownItems)
            {
                item.Click += (sender, e) =>
                {
                    this.Try(() =>
                    {
                        this.tsddbtnSearchType.DropDownItems.OfType<ToolStripMenuItem>().ToList().ForEach(key => key.Checked = false);
                        item.Checked = true;
                        this.tsddbtnSearchType.Text = item.Text;
                        this.tsddbtnSearchType.Image = item.Image;
                        this.tstxtSearch.Focus();
                        this.tstxtSearch.SelectAll();
                        if (base.Visible)
                            this.PerformSearchAsync();
                        this.tstxtSearch.Size = this.tstxtSearch.GetPreferredSize(this.tstxtSearch.Size);
                    });
                };
            }
            this.tsddbtnSearchType.DropDownItems[0].PerformClick();
            this.m_ListViewSortableVirtualList = new ListViewSortableVirtualList<K2SpyTreeNode>(this.m_Context, this.listView1, (items, columnIndex, order) =>
            {
                if (items != null && order != SortOrder.None)
                {
                    if (columnIndex == 0)
                        items = items.OrderBy(key => key.Text).ToArray();
                    else if (columnIndex == 1)
                        items = items.OrderBy(key => key.FullPath).ToArray();
                    else
                        throw new Exception($"The column {columnIndex} was unexpected");

                    if (order == SortOrder.Descending)
                        items = items.Reverse().ToArray();
                }

                return items;
            }, node => new K2SpyTreeNodeListViewItem(node));

            this.listView1.OnOpenContextMenuStrip(async (sender, e) =>
            {
                await this.TryAsync(async () =>
                {
                    K2SpyTreeNode match = this.m_ListViewSortableVirtualList.SelectedItem;
                    if (match != null)
                    {
                        Point screenLocation = e.MouseScreenLocation;
                        if (e.Context == OpenContextMenuStripContext.Keyboard)
                        {
                            int selectedIndex = this.listView1.SelectedIndices.OfType<int>().First();
                            Rectangle rectangle = this.listView1.Items[selectedIndex].Bounds;
                            screenLocation = this.listView1.PointToScreen(rectangle.Location);
                            screenLocation.X += rectangle.Width / 2;
                            screenLocation.Y += rectangle.Height / 2;
                        }
                        await ContextMenuManager.ShowSearcherContextMenuAsync(this.m_Context, match, screenLocation);
                    }
                });
            });
        }

        #endregion

        #region Public Events

        public event EventHandler ClosePanel;

        #endregion

        #region Public Methods

        public void Activate()
        {
            this.tstxtSearch.Focus();
            this.tstxtSearch.SelectAll();
        }

        public void Clear()
        {
            this.tsbtnStopSearch.PerformClick();
            this.tstxtSearch.Text = "";
            this.progressBar1.Value = 0;
            this.listView1.VirtualListSize = 0;
            this.m_ListViewSortableVirtualList?.Clear();
            this.tsbtnScope.Checked = false;
            this.categoryScope1.CategoryPath = "";
        }

        public void Initialize(K2SpyContext k2SpyContext)
        {
            this.m_Context = k2SpyContext;
            this.listView1.SmallImageList = k2SpyContext.TreeView.ImageList;
            this.categoryScope1.Initialize(k2SpyContext);
        }

        #endregion

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            this.toolStrip.UpdateFont(base.Font);
        }

        #region Private Methods

        private async Task PerformSearchAsync()
        {
            this.m_SearchCancellationTokenSource?.Cancel();
            System.Threading.CancellationToken cancellationToken = (this.m_SearchCancellationTokenSource = new System.Threading.CancellationTokenSource()).Token;
            this.tsbtnStopSearch.Enabled = false;
            this.progressBar1.Value = 0;

            string text = this.tstxtSearch.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                this.m_ListViewSortableVirtualList?.Clear();
                this.listView1.VirtualListSize = 0;
                return;
            }

            K2SpyTreeNode scopeRoot = null;
            if (this.categoryScope1.Scoped && !string.IsNullOrEmpty(this.categoryScope1.CategoryPath))
                scopeRoot = (K2SpyTreeNode)await this.m_Context.TreeView.GetNodeByPathAsync(this.categoryScope1.CategoryPath);

            TreeViewSearchSettings searchSettings;
            if (this.tsmiSearchTreeNodes.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.TrueAsync, TreeNodePredicates.TrueAsync, TreeNodePredicates.BuildIsSearchableValueOrNodeTextMatchAsyncPredicate(text));
            }
            else if (this.tsmiSearchAllDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsDefinitionContainerNodeAsync, TreeNodePredicates.IsDefinitionNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else if (this.tsmiSearchFormDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else if (this.tsmiSearchViewDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataViewNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else if (this.tsmiSearchSmartObjectDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataSmartObjectNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else if (this.tsmiSearchWorkflowDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataWorkflowNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else if (this.tsmiSearchServiceInstanceDefinitions.Checked)
            {
                searchSettings = new TreeViewSearchSettings(scopeRoot, TreeNodePredicates.IsServiceObjectsRootOrServiceTreeNodeAsync, TreeNodePredicates.IsServiceInstanceTreeNodeAsync, TreeNodePredicates.BuildIsDefinitionMatchPredicate(text));
            }
            else
            {
                throw new Exception("The type of search could not be determined");
            }

            await Task.Delay(500);
            if (cancellationToken.IsCancellationRequested)
                return;
            this.tsbtnStopSearch.Enabled = true;
            this.listView1.VirtualListSize = 0;
            this.m_ListViewSortableVirtualList.Clear();
            this.progressBar1.Value = 0;

            object actionKey = new object();
            object percentageKey = new object();
            await K2SpyTreeViewSearcher.PerformSearchAsync(this.m_Context, searchSettings, async match =>
            {
                this.m_ListViewSortableVirtualList.Add(match);
                this.m_Context.ActionQueue.QueueOnce(actionKey, () =>
                {
                    this.listView1.VirtualListSize = this.m_ListViewSortableVirtualList.Length;
                });
            }, (status, percentage) =>
            {
                this.m_Context.ActionQueue.QueueOnce(percentageKey, () => this.progressBar1.Value = percentage);
                this.m_Context.QueueSetStatus(status, percentage == 0);
            }, this.m_SearchCancellationTokenSource.Token);
            this.m_Context.ActionQueue.QueueOnce(percentageKey, () => this.progressBar1.Value = 0);
            if (this.m_SearchCancellationTokenSource.Token == cancellationToken || this.m_SearchCancellationTokenSource.IsCancellationRequested)
            {
                this.tsbtnStopSearch.Enabled = false;
            }
        }

        private void tsbtnCloseSearchPanel_Click(object sender, EventArgs e)
        {
            this.Clear();
            this.ClosePanel?.Invoke(this, EventArgs.Empty);
        }

        private void tsbtnStopSearch_Click(object sender, EventArgs e)
        {
            this.m_SearchCancellationTokenSource?.Cancel();
        }

        private async void tstxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await this.PerformSearchAsync();
            }
        }

        private async void tstxtSearch_TextChanged(object sender, EventArgs e)
        {
            await this.PerformSearchAsync();
        }

        private void paneHeader_CloseClicked(object sender, EventArgs e)
        {
            this.ClosePanel?.Invoke(this, EventArgs.Empty);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Try(() =>
            {
                K2SpyTreeNode node = this.m_ListViewSortableVirtualList.SelectedItem;
                if (node != null)
                {
                    this.m_Context.TreeView.SelectNode(node);
                }
            });
        }

        private async void tsbtnScope_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                this.tsbtnScope.Checked = !this.tsbtnScope.Checked;
                this.categoryScope1.Visible = this.tsbtnScope.Checked;
                this.categoryScope1.Scoped = this.tsbtnScope.Checked;

                if (!string.IsNullOrEmpty(this.categoryScope1.CategoryPath))
                {
                    await this.PerformSearchAsync();
                }

                // determine if we need to perform a new search
            });
        }

        private async void categoryScope1_CategoryPathChanged(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (this.categoryScope1.Scoped)
                {
                    await this.PerformSearchAsync();
                }
            });
        }

        #endregion
    }
}