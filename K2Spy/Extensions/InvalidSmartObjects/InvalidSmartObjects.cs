using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.InvalidSmartObjects
{
    internal partial class InvalidSmartObjects : BaseForm
    {
        #region Private Fields

        private K2SpyContext m_K2SpyContext;
        private ListViewSortableVirtualList<MatchData> m_ListViewSortableVirtual;

        #endregion

        #region Constructors

        public InvalidSmartObjects(K2SpyContext k2SpyContext)
        {
            this.m_K2SpyContext = k2SpyContext;

            InitializeComponent();

            //this.workingOverlay1.Show();
            //this.workingOverlay1.ResetVisibilityDepth();
            this.progressBarLabel1.Initialize(k2SpyContext);

            this.m_ListViewSortableVirtual = new ListViewSortableVirtualList<MatchData>(k2SpyContext, this.listView1, (matches, columnIndex, sortOrder) =>
            {
                if (sortOrder!= SortOrder.None)
                {
                    if (columnIndex == 0)
                        matches = matches.OrderBy(key => key.CategoryDataTreeNode.Text).ToArray();
                    else if (columnIndex == 1)
                        matches = matches.OrderBy(key => key.Error?.Message).ToArray();
                    else if (columnIndex == 2)
                        matches = matches.OrderBy(key => key.CategoryDataTreeNode.FullPath).ToArray();
                    else
                        throw new Exception($"The column {columnIndex} was unexpected");
                    if (sortOrder == SortOrder.Descending)
                        matches = matches.Reverse().ToArray();
                }
                return matches;
            }, match =>
            {
                ListViewItem item = new ListViewItem(match.CategoryDataTreeNode.Text);
                item.SubItems.Add(match.Error?.Message);
                item.SubItems.Add(match.CategoryDataTreeNode.FullPath);
                item.ImageIndex = this.listView1.SmallImageList.Images.IndexOfKey(match.CategoryDataTreeNode.ImageKey);
                item.Tag = match.CategoryDataTreeNode;
                return item;
            });

            this.listView1.OnOpenContextMenuStrip(async (sender, e) =>
            {
                await this.TryAsync(async () =>
                {
                    MatchData matchData = this.m_ListViewSortableVirtual.SelectedItem;
                    if (matchData != null)
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
                        await ContextMenuManager.ShowTreeViewContextMenuAsync(this.m_K2SpyContext, matchData.CategoryDataTreeNode, screenLocation);
                    }
                });
            });
        }

        #endregion

        #region Protected Methods

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await this.TryAsync(async () =>
            {
                this.listView1.Items.Clear();
                this.listView1.SmallImageList = this.m_K2SpyContext.TreeView.ImageList;
                await this.PopulateInvalidSmartObjects(this.m_K2SpyContext);
                if (this.listView1.Items.Count == 0)
                {
                    MessageBox.Show(this, "No invalid SmartObjects were found", base.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //base.Close();
                }
            });
        }

        private class MatchData
        {
            public MatchData(CategoryDataTreeNode categoryDataTreeNode, Exception error)
            {
                this.CategoryDataTreeNode = categoryDataTreeNode;
                this.Error = error;
            }

            public CategoryDataTreeNode CategoryDataTreeNode { get; private set; }

            public Exception Error { get; private set; }
        }

        protected async Task PopulateInvalidSmartObjects(K2SpyContext k2SpyContext)
        {
            this.listView1.Items.Clear();
            await Task.Run(async () =>
            {
                CategoryDataTreeNode[] allNodes = (await k2SpyContext.TreeView.Nodes.DescendantsOfTypeWhereAsync<CategoryDataTreeNode>(async key => (await key.GetActAsAsync()) is SmartObjectInfoTreeNode)).ToArray();
                int count = 0;
                SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = k2SpyContext.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                foreach (CategoryDataTreeNode node in allNodes)
                {
                    count++;
                    int percentage = (count * 100) / allNodes.Length;
                    percentage = Math.Min(100, Math.Max(0, percentage));
                    this.progressBarLabel1.QueueUpdate($"Inspecting {node.FullPath}...", percentage); ;
                    try
                    {
                        SourceCode.SmartObjects.Authoring.SmartObjectDefinition smartObjectDefinition = await k2SpyContext.Cache.SmartObjectDefinitionCache.GetAsync(node.DataAsGuid);
                        if (smartObjectDefinition.Inconsistent)
                        {
                            smartObjectManagementServer.GetSmartObjectDefinition(true, node.DataAsGuid);
                            throw new Exception($"{smartObjectDefinition.Metadata.DisplayName} is inconsistent, but unexpectedly GetSmartObjectDefinition did not throw an error");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.m_ListViewSortableVirtual.Add(new MatchData(node, ex));
                    }
                }
                this.progressBarLabel1.QueueReset();
            });
        }

        #endregion

        #region Private Methods

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                MatchData match = this.m_ListViewSortableVirtual.SelectedItem;
                if (match != null)
                {
                    CategoryDataTreeNode node = match.CategoryDataTreeNode;
                    this.m_K2SpyContext.TreeView.SelectNode(node, true);
                }
            });
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                MatchData match = this.m_ListViewSortableVirtual.SelectedItem;
                if (match != null)
                {
                    CategoryDataTreeNode node = match.CategoryDataTreeNode;
                    this.textBox1.Text = match.Error?.Message;
                }
                else
                {
                    this.textBox1.Text = "(click an invalid SmartObject to view the error details)";
                }
            });
        }

        #endregion
    }
}