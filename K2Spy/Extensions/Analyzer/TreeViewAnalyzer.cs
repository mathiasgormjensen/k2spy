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

namespace K2Spy.Extensions.Analyzer
{
    internal partial class TreeViewAnalyzer : UserControl
    {
        #region Private Fields

        private K2SpyContext m_Context;

        #endregion

        #region Constructors

        public TreeViewAnalyzer()
        {
            InitializeComponent();
            this.treeView1.SetDoubleBuffered(true);
            this.treeView1.EnableSelectNodeOnRightClick();
            this.treeView1.OnOpenContextMenuStrip(async (sender, e) =>
            {
                if (this.treeView1.SelectedNode is AnalyzerTreeNode analyzerTreeNode)
                {
                    Point screenLocation = e.MouseScreenLocation;
                    if (e.Context == OpenContextMenuStripContext.Keyboard)
                    {
                        Rectangle rectangle = analyzerTreeNode.Bounds;
                        screenLocation = this.treeView1.PointToScreen(rectangle.Location);
                        screenLocation.X += rectangle.Width / 2;
                        screenLocation.Y += rectangle.Height / 2;
                    }
                    await ContextMenuManager.ShowAnalyzerContextMenuAsync(this.m_Context, analyzerTreeNode, analyzerTreeNode?.Source, screenLocation);
                }
            });
        }

        #endregion

        #region Public Events

        public event EventHandler ClosePanel;

        #endregion

        #region Internal Properties

        internal TreeView TreeView
        {
            get { return this.treeView1; }
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            this.treeView1.CollapseAll();
            this.treeView1.Nodes.Clear();
        }

        public void Activate()
        {
        }

        public async Task SelectOrPopulateAsync(K2SpyTreeNode node)
        {
            foreach (AnalyzerTreeNode root in this.treeView1.Nodes.OfType<AnalyzerTreeNode>())
            {
                if (root.Source == node)
                {
                    this.treeView1.Focus();
                    this.treeView1.SelectedNode = root;
                    return;
                }
            }
            AnalyzerTreeNode newNode = await AnalyzerTreeNode.CreateAsync(node);
            newNode.Expand();
            this.treeView1.Nodes.Add(newNode);
            this.treeView1.Focus();
            this.treeView1.SelectedNode = newNode;
        }

        public void Initialize(K2SpyContext k2SpyContext)
        {
            this.m_Context = k2SpyContext;
            this.treeView1.ImageList = k2SpyContext.TreeView.ImageList;
            //this.treeView1.ImageList = new ImageList();
            //foreach (string key in k2SpyContext.TreeView.ImageList.Images.Keys)
            //{
            //    this.treeView1.ImageList.Images.Add(key, k2SpyContext.TreeView.ImageList.Images[key]);
            //}
            //// add "Used by" and "Using" images
            //this.treeView1.ImageList.Images.Add("Analyze", Properties.Resources.Search16);
        }

        #endregion

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.contextMenuStrip.Font = base.Font;
        }

        private void paneHeader_CloseClicked(object sender, EventArgs e)
        {
            this.Clear();
            this.ClosePanel?.Invoke(this, EventArgs.Empty);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.Try(() =>
            {
                AnalyzerTreeNode node = this.treeView1.GetNodeAtLocation(e.Location) as AnalyzerTreeNode;
                if (node != null && node.Source != null)
                {
                    if (this.m_BeforeExpandCancelDictionary.ContainsKey(node))
                        this.m_BeforeExpandCancelDictionary[node] = true;
                    if (this.m_BeforeCollapseCancelDictionary.ContainsKey(node))
                        this.m_BeforeCollapseCancelDictionary[node] = true;

                    this.m_Context.TreeView.SelectNode(node.Source);
                }
                else
                {
                    string asd = "";
                }
            });
        }

        private Dictionary<TreeNode, bool> m_BeforeExpandCancelDictionary = new Dictionary<TreeNode, bool>();
        private async void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (e.Node is AnalyzerTreeNode)
                {
                    if (!this.m_BeforeExpandCancelDictionary.ContainsKey(e.Node))
                    {
                        this.m_BeforeExpandCancelDictionary.Add(e.Node, false);
                        e.Cancel = true;
                        await Task.Delay(150);
                        e.Node.Expand();
                        this.m_BeforeExpandCancelDictionary.Remove(e.Node);
                    }
                    else
                    {
                        e.Cancel = this.m_BeforeExpandCancelDictionary[e.Node];
                    }
                }
            });
        }

        private Dictionary<TreeNode, bool> m_BeforeCollapseCancelDictionary = new Dictionary<TreeNode, bool>();
        private async void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (!this.m_BeforeCollapseCancelDictionary.ContainsKey(e.Node))
                {
                    this.m_BeforeCollapseCancelDictionary.Add(e.Node, false);
                    e.Cancel = true;
                    await Task.Delay(150);
                    e.Node.Collapse(true);
                    this.m_BeforeCollapseCancelDictionary.Remove(e.Node);
                }
                else
                {
                    e.Cancel = this.m_BeforeCollapseCancelDictionary[e.Node];
                }
            });
        }

        private async void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (e.Node is IAnalyzeTreeNode)
                    await ((IAnalyzeTreeNode)e.Node).AfterExpandAsync(this.m_Context);
            });
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            this.Try(() =>
            {
                (e.Node as IAnalyzeTreeNode)?.AfterCollapse(this.m_Context);

                e.Node.DescendantsWhere(key => (key as IAnalysisCompletedTreeNode)?.AnalysisCompleted == false).ToList().ForEach(key => key.Collapse());
            });
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            this.Try(() =>
            {
                TreeNode node = this.treeView1.SelectedNode;
                if (node != null)
                {
                    if (e.KeyData == Keys.Delete)
                    {
                        if (node.Parent == null)
                        {
                            node.Collapse(false);
                            node.Remove();
                        }
                    }
                    else if (e.KeyData == Keys.Space)
                    {
                        // try to select the node in the left tree view
                        if (node is AnalyzerTreeNode analyzerTreeNode)
                        {
                            this.m_Context.TreeView.SelectNode(analyzerTreeNode.Source);
                        }
                    }
                }
            });
        }

        private void tsmiGoToItem_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                AnalyzerTreeNode node = this.treeView1.SelectedNode as AnalyzerTreeNode;
                if (node != null)
                {
                    this.m_Context.TreeView.SelectNode(node.Source);
                }
            });
        }

        private void tsmiRemove_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Parent == null)
                {
                    this.treeView1.SelectedNode.Collapse(false);
                    this.treeView1.SelectedNode.Remove();
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.treeView1.SelectedNode.Nodes.Add("test");
            });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            this.TryAsync(async () =>
            {
                this.treeView1.SelectedNode.Collapse(false);
                await Task.Delay(1000);
                this.treeView1.SelectedNode.Nodes.Clear();
            });
        }
    }
}
