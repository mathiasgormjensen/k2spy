using K2Spy.ExtensionMethods;
using ScintillaNET.Demo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace K2Spy.Extensions.XPathSearcher
{
    internal partial class XPathSearcher : BaseForm
    {
        #region Private Fields

        private object QueueUpdateVirtualListSizeActionKey = new object();
        private K2SpyContext m_K2SpyContext;
        private XPathSearcherExtension m_XPathSearcherExtension;
        private System.Threading.CancellationTokenSource m_CancellationTokenSource;
        private ListViewSortableVirtualList<SearchResultItem> m_VirtualList;

        #endregion

        #region Constructors

        public XPathSearcher(XPathSearcherExtension xpathSearcherExtension, K2SpyContext k2SpyContext)
        {
            this.m_XPathSearcherExtension = xpathSearcherExtension;
            this.m_K2SpyContext = k2SpyContext;
            InitializeComponent();

            Control control = this.paddedPanel1;
            control.Padding = new Padding(control.Padding.Left, control.Padding.Top, control.Padding.Right, 0);

            this.listView1.SetDoubleBuffered(true);

            this.listView1.SmallImageList = k2SpyContext.TreeView.ImageList;

            this.m_VirtualList = new ListViewSortableVirtualList<SearchResultItem>(k2SpyContext, this.listView1, (items, columnIndex, order) =>
            {
                if (order == SortOrder.None)
                    return items;

                switch (columnIndex)
                {
                    case 0:
                        items = items.OrderBy(key => key.NameColumn).ToArray();
                        break;
                    case 1:
                        items = items.OrderBy(key => key.ResultColumn).ToArray();
                        break;
                    case 2:
                        items = items.OrderBy(key => key.PositionColumn).ToArray();
                        break;
                    case 3:
                        items = items.OrderBy(key => key.PathColumn).ToArray();
                        break;
                    default:
                        throw new Exception($"The column {columnIndex} was unexpected");
                }

                if (order == SortOrder.Descending)
                    items = items.Reverse().ToArray();
                return items;
            }, item => item.CreateListViewItem());

            this.categoryScope1.Initialize(k2SpyContext);
            this.progressBarLabel1.Initialize(k2SpyContext);
        }

        #endregion

        #region Protected Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.m_CancellationTokenSource?.Cancel();
        }

        protected async override void OnCreateControl()
        {
            base.OnCreateControl();

            await this.TryAsync(async () =>
            {
                HotKeyManager.AddHotKey(this, () => { this.btnSearch.PerformClick(); }, Keys.Enter, true);
                HotKeyManager.AddHotKey(this, () => { this.btnSearch.PerformClick(); }, Keys.F5);
                HotKeyManager.AddHotKey(this, () => { this.btnStop.PerformClick(); }, Keys.Escape);
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.m_VirtualList.Clear();
        }

        #endregion

        #region Private Methods

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            Serilog.Log.Logger.CurrentMethod();
            await this.TryAsync(async () =>
            {
                this.m_CancellationTokenSource?.Cancel();
                System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
                this.m_CancellationTokenSource = cancellationTokenSource;

                this.m_VirtualList.Clear();
                this.listView1.VirtualListSize = 0;

                this.btnStop.Enabled = true;
                this.listView1.Items.Clear();
                string xpath = this.txtXPath.Text;

                ImageList imageList = this.listView1.SmallImageList;
                await Task.Run(async () =>
                {
                    Serilog.Log.Debug($"Searching for xpath {xpath}");
                    System.Xml.XPath.XPathExpression expression = System.Xml.XPath.XPathExpression.Compile(xpath);
                    TreeNode scope = null;// this.m_K2SpyContext.TreeView.Nodes.OfType<CategoryRootTreeNode>().Single();
                    if (this.categoryScope1.Scoped && !string.IsNullOrEmpty(this.categoryScope1.CategoryPath))
                        scope = await this.m_K2SpyContext.TreeView.GetNodeByPathAsync(this.categoryScope1.CategoryPath);

                    K2SpyTreeNode[] nodes = null;
                    if (scope != null)
                        nodes = scope.DescendantsOfType<K2SpyTreeNode>().ToArray();
                    else
                        nodes = this.m_K2SpyContext.TreeView.Nodes.DescendantsOfType<K2SpyTreeNode>().ToArray();
                    
                    nodes = nodes
                        .Where(key => key is CategoryDataTreeNode || key is ServiceInstanceTreeNode || key is ControlTypeInfoTreeNode)
                        .ToArray();
                    int i = 0;
                    DateTime begin = DateTime.Now;
                    try
                    {
                        foreach (K2SpyTreeNode node in nodes)
                        {
                            i++;

                            if (cancellationTokenSource.IsCancellationRequested == true)
                                return;

                            int percentage = Math.Min(100, Math.Max(0, i * 100 / nodes.Length));
                            this.progressBarLabel1.QueueUpdate($"Processing {node.FullPath} {percentage}%", percentage);

                            K2SpyTreeNode node2 = null;
                            if (node is CategoryDataTreeNode categoryDataTreeNode)
                                node2 = await categoryDataTreeNode.GetActAsAsync();
                            else
                                node2 = await node.GetActAsOrSelfAsync();
                            // K2SpyTreeNode node2 = await node.GetActAsAsync();
                            if (node2 != null)
                            {
                                System.Xml.XPath.XPathDocument document = await node2.GetFormattedDefinitionAsXPathDocumentAsync();
                                if (document != null)
                                {
                                    object evaluationResult = document.CreateNavigator().Evaluate(xpath);
                                    if (evaluationResult is System.Xml.XPath.XPathNodeIterator iterator)
                                    {
                                        foreach (System.Xml.XPath.XPathItem item in iterator)
                                        {
                                            this.m_VirtualList.Add(new SearchResultItem(node, item, imageList));
                                        }
                                        iterator = null;
                                    }
                                    else
                                    {
                                        this.m_VirtualList.Add(new SearchResultItem(node, evaluationResult, imageList));
                                    }
                                    evaluationResult = null;
                                }
                                document = null;
                            }
                        }
                    }
                    finally
                    {
                        if (!cancellationTokenSource.IsCancellationRequested)
                        {
                            TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                            this.progressBarLabel1.QueueUpdate($"Search completed in {timeSpan}...", 0);
                            this.m_K2SpyContext.ActionQueue.Queue(() =>
                            {
                                this.btnStop.Enabled = false;
                            });
                        }
                    }
                });
            }, () => this.m_K2SpyContext.ActionQueue.Queue(() =>
            {
                //this.btnStop.Enabled = false;
            }));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Serilog.Log.Logger.CurrentMethod();
            this.m_CancellationTokenSource?.Cancel();
            this.progressBarLabel1.QueueReset();
            this.btnStop.Enabled = false;
        }

        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                SearchResultItem item = this.m_VirtualList.SelectedItem;
                if (item != null)
                {
                    if (item.LineInfo != null)
                    {
                        TreeNode selectedNode = this.m_K2SpyContext.TreeView.SelectedNode;
                        if (selectedNode != item.K2SpyTreeNode)
                        {
                            this.m_K2SpyContext.TreeView.SelectNode(item.K2SpyTreeNode, true);
                            await Task.Delay(250);
                        }
                        ExtensionsManager.GetExtension<Extensions.DefinitionPane.DefinitionPaneExtension>().GoTo(item.LineInfo.LineNumber - 1, item.LineInfo.LinePosition - 1);
                    }
                    else
                    {
                        this.m_K2SpyContext.TreeView.SelectNode(item.K2SpyTreeNode, true);
                    }
                }
            });
        }

        #endregion
    }
}