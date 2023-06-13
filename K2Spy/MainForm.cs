// #define POPULATE_TREE_IN_PARALLEL
using K2Spy.ExtensionMethods;
using ScintillaNET;
using ScintillaNET.Demo.Utils;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace K2Spy
{
    public partial class MainForm : BaseForm
    {
        #region Private Fields

        private System.Threading.CancellationTokenSource m_AfterSelectCancellationTokenSource;
        private InvokeOnUIThreadActionQueue m_ActionQueue;
        private ConnectionFactory m_ConnectionFactory;
        private Cache m_Cache;
        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            this.CloseSearchPane();
            this.CloseAnalyzerPane();

            if (Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView == false)
            {
                this.loadingOverlay1.Show(false);
                this.loadingOverlay1.ResetVisibilityDepth();
            }

            this.treeView.SetDoubleBuffered(true);
            base.DoubleBuffered = true;

            //base.MinimumSize = base.Size;

            this.m_ActionQueue = new InvokeOnUIThreadActionQueue(this);

            this.progressBarLabel1.Reset();

            this.treeView.ImageList.Images.Clear();

            this.treeView.ImageList.Images.Add("Default", Properties.Resources.API16);
            this.treeView.ImageList.Images.Add("Category", Properties.Resources.FolderClosed16);
            this.treeView.ImageList.Images.Add("RootItem", Properties.Resources.RootItem16);
            this.treeView.ImageList.Images.Add("Form", Properties.Resources.Form16);
            this.treeView.ImageList.Images.Add("FormCheckedOut", Properties.Resources.Form_CheckOut16);
            this.treeView.ImageList.Images.Add("View", Properties.Resources.View16);
            this.treeView.ImageList.Images.Add("ViewCheckedOut", Properties.Resources.View_CheckOut16);
            this.treeView.ImageList.Images.Add("Forms", Properties.Resources.forms);
            this.treeView.ImageList.Images.Add("Views", Properties.Resources.views);
            this.treeView.ImageList.Images.Add("SmartObject", Properties.Resources.SmartObject16);
            this.treeView.ImageList.Images.Add("ServiceInstance", Properties.Resources.ServiceInstance16);
            this.treeView.ImageList.Images.Add("Transparent", Properties.Resources.transparent_16);
            this.treeView.ImageList.Images.Add("Loading", Properties.Resources.Loading_16x);
            this.treeView.ImageList.Images.Add("Workflows", Properties.Resources.workflowserver);
            this.treeView.ImageList.Images.Add("Workflow", Properties.Resources.Workflow16);
            this.treeView.ImageList.Images.Add("WorkflowLocked", Properties.Resources.ProcessDefinition_Locked16);
            this.treeView.ImageList.Images.Add("ServiceObject", Properties.Resources.servicetype);
            this.treeView.ImageList.Images.Add("ServiceObjects", Properties.Resources.servicetypesroot);
            this.treeView.ImageList.Images.Add("SmartObjects", Properties.Resources.smartobjectsroot);
            this.treeView.ImageList.Images.Add("ProcessFolder", Properties.Resources.processfolder);
            this.treeView.ImageList.Images.Add("Analyze", Properties.Resources.VBSearch_16x);
            this.treeView.ImageList.Images.Add("Properties", Properties.Resources.Properties16);
            this.treeView.ImageList.Images.Add("Method", Properties.Resources.Method16);
            this.treeView.ImageList.Images.Add("Control", Properties.Resources.Controls16);
            this.treeView.ImageList.Images.Add("Controls", Properties.Resources.ControlsRoot);
            this.treeView.ImageList.Images.Add("Error", Properties.Resources.Error16);
            this.treeView.ImageList.Images.Add("StyleProfile", Properties.Resources.StyleProfile16);
            this.treeView.ImageList.Images.Add("StyleProfileCheckedOut", Properties.Resources.StyleProfile_CheckOut16);
            this.treeView.ImageList.Images.Add("StyleProfiles", Properties.Resources.StyleProfileRoot);

            this.m_ConnectionFactory = new ConnectionFactory();
            this.m_Cache = new Cache(this.m_ConnectionFactory);
        }

        #endregion

        #region Internal Methods

        internal void ShowAnalyzerPane()
        {
            this.scDefinitionAnalysis.Panel2Collapsed = false;
            Extensions.ExtensionsManager.GetExtension<Model.IAnalyzerPaneExtension>().OnAnalyzerPaneActivated(this.GetK2SpyContext());
        }

        internal void CloseAnalyzerPane()
        {
            this.scDefinitionAnalysis.Panel2Collapsed = true;
        }

        internal void ShowSearchPane()
        {
            this.scSearchDefinition.Panel1Collapsed = false;
            Extensions.ExtensionsManager.GetExtension<Model.ISearchPaneExtension>().ActivateSearchPane(this.GetK2SpyContext());
        }

        internal void CloseSearchPane()
        {
            this.scSearchDefinition.Panel1Collapsed = true;
        }

        internal async Task DisconnectAsync()
        {
            using (this.ShowLoadingOverlay("Disconnecting..."))
            {
                await this.GetK2SpyContext().DisconnectAsync();
            }
        }

        internal async Task ConnectAsync(Connection connection)
        {
            using (this.ShowLoadingOverlay("Connecting..."))
            {
                Connections.Default.Items.ToList().ForEach(key => key.Selected = key == connection);
                Connections.Default.QueueSave();
                await this.GetK2SpyContext().ConnectAsync(connection.ConnectionString);
                await this.PopulateConnectionsAsync(false);
            }
        }

        internal protected async Task PopulateConnectionsAsync(bool performSelect = false)
        {
            while (true)
            {
                ToolStripItem item = this.tsmiConnections.DropDownItems[0];
                if (item == this.tssepConnections)
                    break;
                this.tsmiConnections.DropDownItems.Remove(item);
            }

            foreach (Connection connection in Connections.Default.Items.OrderBy(key => key.ShortDisplayName).Reverse())
            {
                ToolStripMenuItem item = new ToolStripMenuItem(connection.ShortDisplayName);
                item.Checked = connection.Selected;
                item.Click += async (sender, e) =>
                {
                    await this.TryAsync(async () =>
                    {
                        await this.ConnectAsync(connection);
                    });
                };
                this.tsmiConnections.DropDownItems.Insert(0, item);
                if (performSelect && connection.Selected)
                    await this.GetK2SpyContext().ConnectAsync(connection.ConnectionString);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            // clear the nodes to avoid them being disposed and hence slowing down the closing of the form
            this.treeView.Nodes.Clear();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.progressBarLabel1.Initialize(this.GetK2SpyContext());

            this.treeView.EnableSelectNodeOnRightClick();
            this.treeView.OnOpenContextMenuStrip(async (sender, e) =>
            {
                await this.TryAsync(async () =>
                {
                    Point screenLocation = e.MouseScreenLocation;
                    TreeNode selectedNode = this.treeView.SelectedNode;
                    if (e.Context == OpenContextMenuStripContext.Keyboard)
                    {
                        selectedNode.EnsureVisible();
                        Rectangle bounds = selectedNode.Bounds;
                        screenLocation = this.treeView.PointToScreen(bounds.Location);
                        screenLocation.X += bounds.Width / 2;
                        screenLocation.Y += bounds.Height / 2;
                    }
                    await ContextMenuManager.ShowTreeViewContextMenuAsync(this.GetK2SpyContext(), selectedNode as K2SpyTreeNode, screenLocation);
                });
            });
        }

        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.scSearchDefinition.SplitterDistance = 0;
            this.scDefinitionAnalysis.SplitterDistance = this.scDefinitionAnalysis.Height;

            await this.TryAsync(async () =>
            {
                await this.InitializeAsync();

                if (this.treeView.SelectedNode == null)
                    this.tsmiWelcome.PerformClick();
            });
        }

        protected K2SpyContext GetK2SpyContext()
        {
            return this.m_K2SpyContext ?? (this.m_K2SpyContext = new K2SpyContext(this.m_ConnectionFactory, this.m_Cache, this, this.treeView, this.m_ActionQueue, (status, hideProgressBar) =>
            {
                this.progressBarLabel1.QueueSetProgressBarStyle(hideProgressBar ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee);
                this.progressBarLabel1.QueueSetText(status);
            }));
        }

        protected async Task PopulateToolsMenuAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            Model.IToolsMenuExtension[] toolsMenuExtensions = Extensions.ExtensionsManager.GetExtensions<Model.IToolsMenuExtension>().ToArray();
            foreach (Model.IToolsMenuExtension extension in toolsMenuExtensions.OrderBy(key => key.DisplayName))
            {
                ToolStripMenuItem item = new ToolStripMenuItem(extension.DisplayName);
                item.Font = base.Font;
                item.DropDownItems.AddRange(extension.CreateToolsMenuItems(k2SpyContext));
                if (item.DropDownItems.Count > 0)
                    this.tsmiTools.DropDownItems.Add(item);
            }

            this.tsmiTools.Visible = this.tsmiTools.HasDropDownItems;
        }

        protected async Task PopulateCommandBarExtensionsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            Model.ICommandBarExtension[] commandBarExtensions = Extensions.ExtensionsManager.GetExtensions<Model.ICommandBarExtension>().ToArray();
            int leftPosition = 0;
            foreach (Model.ICommandBarExtension extension in commandBarExtensions)
            {
                ToolStripItem[] items = extension.CreateCommandBarItems(k2SpyContext);
                if (items?.Length > 0)
                {
                    List<ToolStripItem> list = new List<ToolStripItem>();
                    list.AddRange(items);
                    if (extension is Model.ILeftAlignedCommandBarExtension)
                    {
                        list.Add(new ToolStripSeparator());

                        foreach (ToolStripItem item in list.Reverse<ToolStripItem>())
                            this.tsTop.Items.Insert(leftPosition, item);
                        leftPosition += list.Count;
                    }
                    else
                    {
                        list.Insert(0, new ToolStripSeparator());

                        this.tsTop.Items.AddRange(list.ToArray());
                    }
                }
            }
            this.tsTop.UpdateFont(this.tsTop.Font);
            this.tsTop.Items.OfType<ToolStripItem>()
                .ToList()
                .ForEach(key =>
                {
                    key.Margin = this.tsbtnSearch.Margin;
                    key.Padding = this.tsbtnSearch.Padding;
                });
            this.tsTop.Items.OfType<ToolStripItem>()
                .Where(key => string.IsNullOrEmpty(key.ToolTipText) && !string.IsNullOrEmpty(key.Text) && !key.AutoToolTip)
                .ToList()
                .ForEach(key => key.AutoToolTip = true);
        }

        protected async Task PopulateSearchPaneAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            Model.ISearchPaneExtension searchPaneExtension = Extensions.ExtensionsManager.GetExtension<Model.ISearchPaneExtension>();
            Control control = searchPaneExtension.CreateSearchPaneControl(k2SpyContext);
            control.Dock = DockStyle.Fill;
            this.scSearchDefinition.Panel1.Controls.Clear();
            this.scSearchDefinition.Panel1.Controls.Add(control);
            this.scSearchDefinition.Panel1Collapsed = true;
            searchPaneExtension.OpenSearchPane += (sender, e) =>
            {
            };
            searchPaneExtension.CloseSearchPane += (sender, e) =>
            {
                this.scSearchDefinition.Panel1Collapsed = true;
            };
        }

        protected async Task PopulateAnalyzerPaneAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            Model.IAnalyzerPaneExtension analyzerPaneExtension = Extensions.ExtensionsManager.GetExtension<Model.IAnalyzerPaneExtension>();
            Control control = analyzerPaneExtension.CreateAnalyzerPaneControl(k2SpyContext);
            control.Dock = DockStyle.Fill;
            this.scDefinitionAnalysis.Panel2.Controls.Clear();
            this.scDefinitionAnalysis.Panel2.Controls.Add(control);
            this.scDefinitionAnalysis.Panel2Collapsed = true;
            analyzerPaneExtension.CloseAnalyzerPane += (sender, e) =>
            {
                this.CloseAnalyzerPane();
            };
        }

        private class TabHeaderButton : Button
        {
            private bool m_CanClose;

            public TabHeaderButton()
            {
                base.Padding = new Padding(0);
                base.Margin = new Padding(0);
                base.AutoSize = true;
                base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                base.TextImageRelation = TextImageRelation.TextBeforeImage;
                base.ImageAlign = System.Drawing.ContentAlignment.TopRight;
                base.FlatAppearance.BorderSize = 0;
                base.FlatStyle = FlatStyle.Flat;
                this.UpdateStyling();
            }

            public Control DefinitionPaneControl { get; set; }

            public Model.IDefinitionPaneExtension Extension { get; set; }

            public bool CanClose
            {
                get { return this.m_CanClose; }
                set
                {
                    if (this.m_CanClose != value)
                    {
                        this.m_CanClose = value;
                        this.UpdateStyling();
                    }
                }
            }

            public event EventHandler CloseClicked;

            protected override void OnMouseEnter(EventArgs e)
            {
                this.m_Hover = true;
                base.OnMouseEnter(e);
                this.UpdateStyling();
            }

            protected Rectangle GetCloseButtonRectangle()
            {
                if (this.m_CanClose)
                {
                    Rectangle clientRectangle = base.ClientRectangle;
                    Rectangle rectangle = new Rectangle(clientRectangle.Width - 15, (base.Height / 2) - 6, 11, 11);
                    return rectangle;
                }
                return Rectangle.Empty;
            }

            //protected override void OnPaint(PaintEventArgs pevent)
            //{
            //    base.OnPaint(pevent);
            //    pevent.Graphics.DrawRectangle(Pens.Red, this.GetCloseButtonRectangle());
            //}

            protected bool OverCloseButton(Point location)
            {
                Rectangle rectangle = this.GetCloseButtonRectangle();
                return rectangle != Rectangle.Empty && rectangle.Contains(location);
            }

            protected override void OnMouseDown(MouseEventArgs mevent)
            {
                base.OnMouseDown(mevent);
                this.m_MouseDown = true;
                this.UpdateStyling();
            }

            protected override void OnMouseUp(MouseEventArgs mevent)
            {
                base.OnMouseUp(mevent);
                this.m_MouseDown = false;
                this.UpdateStyling();
            }

            protected void UpdateStyling()
            {
                base.SuspendLayout();
                FontStyle fontStyle = FontStyle.Regular;
                if (this.Selected)
                {
                    base.ForeColor = Color.White;
                    fontStyle = FontStyle.Bold;
                    base.BackColor = Color.FromArgb(0, 122, 204);
                    base.FlatAppearance.MouseOverBackColor = base.BackColor;
                    base.FlatAppearance.MouseDownBackColor = base.BackColor;
                }
                else
                {
                    if (this.m_Hover)
                        base.ForeColor = Color.White;
                    else
                        base.ForeColor = Color.Black;
                    fontStyle = FontStyle.Regular;
                    base.BackColor = SystemColors.Control;
                    base.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 151, 234);//.ActiveCaption;
                    base.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 122, 204);
                }

                if (this.m_CanClose)
                {
                    if (this.m_Hover)
                    {
                        if (this.m_OverCloseButton)
                        {
                            if (this.m_MouseDown)
                                base.Image = Properties.Resources.Close_white_12x_12x_v5_mousedown;
                            else
                                base.Image = Properties.Resources.Close_white_12x_12x_v5_hover_v2;
                        }
                        else
                        {
                            base.Image = Properties.Resources.Close_white_12x_12x_v5;
                        }
                    }
                    else
                    {
                        base.Image = Properties.Resources.transparent_12;
                    }
                }
                else
                {
                    base.Image = null;
                }

                if (base.Font.Style != fontStyle)
                    base.Font = new Font(base.Font, fontStyle);
                base.ResumeLayout();
            }

            protected override void OnClick(EventArgs e)
            {
                if (this.m_OverCloseButton)
                {
                    this.CloseClicked?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    //base.Parent.Controls.OfType<TabHeaderButton>().ToList().ForEach(key => key.Selected = key == this);
                    base.OnClick(e);
                }
            }

            private bool m_Hover = false;
            private bool m_Selected;
            private bool m_MouseDown = false;
            public bool Selected
            {
                get { return this.m_Selected; }
                set
                {
                    if (this.m_Selected != value)
                    {
                        this.m_Selected = value;
                        this.UpdateStyling();
                    }
                }
            }

            protected override void OnParentFontChanged(EventArgs e)
            {
                base.OnParentFontChanged(e);

                this.Font = new Font(base.Parent.Font, this.Font.Style);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                this.m_Hover = false;
                this.m_OverCloseButton = false;

                this.UpdateStyling();
                base.OnMouseLeave(e);
            }

            private bool m_OverCloseButton = false;
            protected override void OnMouseMove(MouseEventArgs mevent)
            {
                this.m_OverCloseButton = this.OverCloseButton(mevent.Location);
                this.UpdateStyling();
                base.OnMouseMove(mevent);
            }
        }

        protected async Task PopulateDefinitionPaneAsync()
        {
            Serilog.Log.Logger.CurrentMethod();
            Model.IDefinitionPaneExtension[] definitionPaneExtensions = Extensions.ExtensionsManager.GetExtensions<Model.IDefinitionPaneExtension>().ToArray();
            K2SpyContext k2SpyContext = this.GetK2SpyContext();

            TableLayoutPanel tableHeaders = new TableLayoutPanel();
            tableHeaders.RowStyles.Clear();
            tableHeaders.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableHeaders.ColumnStyles.Clear();
            tableHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tableHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableHeaders.RowCount = 1;
            tableHeaders.ColumnCount = 2;
            tableHeaders.AutoSize = true;
            tableHeaders.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableHeaders.Dock = DockStyle.Top;
            tableHeaders.Padding = new Padding(0);
            tableHeaders.Margin = new Padding(0);
            tableHeaders.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            FlowLayoutPanel flowHeaders = new FlowLayoutPanel() { WrapContents = false };
            flowHeaders.Height = 23;
            flowHeaders.Dock = DockStyle.Top;
            flowHeaders.Padding = new Padding(0);
            flowHeaders.Margin = new Padding(0);

            tableHeaders.Controls.Add(flowHeaders, 0, 0);


            Button tabsButton = new Button();
            tabsButton.FlatAppearance.BorderSize = 0;
            tabsButton.FlatStyle = FlatStyle.Flat;
            tabsButton.Font = new System.Drawing.Font("Wingdings 3", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            tabsButton.Text = "q";
            tabsButton.AutoSize = true;
            tabsButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tabsButton.Padding = new Padding(0);
            tabsButton.Margin = new Padding(0);
            tabsButton.Visible = false;

            Action showOrHideTabsButton = () =>
            {
                tabsButton.Visible = flowHeaders.Controls.OfType<TabHeaderButton>().Where(key => key.Visible).Count() > 1;
            };

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Font = base.Font;
            tabsButton.Click += (sender, e) =>
            {
                contextMenuStrip.Items.Clear();
                foreach (TabHeaderButton tab in flowHeaders.Controls.OfType<TabHeaderButton>().Where(key => key.Visible))
                {
                    ToolStripItem item = contextMenuStrip.Items.Add(tab.Text, null, (sender2, e2) => tab.PerformClick());
                    ((ToolStripMenuItem)item).Checked = tab.Selected;
                }
                contextMenuStrip.Show(tabsButton, new Point(0, tabsButton.Height));
            };
            tableHeaders.Controls.Add(tabsButton, 1, 0);

            Panel tabControls = new Panel();
            tabControls.Dock = DockStyle.Fill;
            foreach (Model.IDefinitionPaneExtension extension in definitionPaneExtensions)
            {
                TabHeaderButton header = new TabHeaderButton();
                header.Text = extension.DefinitionPaneTitle;
                header.Extension = extension;
                header.DefinitionPaneControl = extension.CreateDefinitionPaneControl(this.GetK2SpyContext());
                header.DefinitionPaneControl.Visible = true;
                header.CanClose = extension.CanCloseDefinitionPane;
                header.Click += (sender, e) =>
                {
                    flowHeaders.Controls.OfType<TabHeaderButton>().Where(key => key.Selected).ToList().ForEach(key => key.Extension.OnDeactivateDefinitionPane(this.GetK2SpyContext()));
                    flowHeaders.Controls.OfType<TabHeaderButton>().ToList().ForEach(key => key.Selected = key == header);
                    header.Extension.OnActivateDefinitionPane(this.GetK2SpyContext());
                    header.DefinitionPaneControl.Visible = true;
                    header.DefinitionPaneControl.BringToFront();
                };
                EventHandler closeHeader = (sender, e) =>
                {
                    TabHeaderButton[] buttons = flowHeaders.Controls.OfType<TabHeaderButton>().Where(key => key.Visible).ToArray();
                    header.Visible = false;
                    header.Extension.OnCloseDefinitionPane(this.GetK2SpyContext());
                    TabHeaderButton before = buttons.TakeWhile(key => key != header).LastOrDefault();
                    if (before != null)
                    {
                        before.PerformClick();
                    }
                    else
                    {
                        TabHeaderButton after = buttons.SkipWhile(key => key != header).Skip(1).FirstOrDefault();
                        if (after != null)
                        {
                            after.PerformClick();
                        }
                    }
                    showOrHideTabsButton();
                };
                header.CloseClicked += closeHeader;
                flowHeaders.Controls.Add(header);
                extension.DefinitionPaneTitleChanged += (sender, e) =>
                {
                    this.InvokeIfRequired(() => header.Text = extension.DefinitionPaneTitle);
                };
                header.DefinitionPaneControl.Dock = DockStyle.Fill;
                tabControls.Controls.Add(header.DefinitionPaneControl);
                extension.CloseDefinitionPane += closeHeader;
                extension.OpenDefinitionPane += (sender, e) =>
                {
                    if (!header.Visible)
                        header.SendToBack();
                    header.Visible = true;
                    header.PerformClick();
                    showOrHideTabsButton();
                };
                header.Visible = extension.InitialDefinitionPaneVisibility;

            }
            //selectedChanged(tabControl, new TabControlEventArgs(tabControl.TabPages[0], 0, TabControlAction.Selected));
            this.scSearchDefinition.Panel2.Controls.Add(tableHeaders);
            this.scSearchDefinition.Panel2.Controls.Add(tabControls);
            tabControls.BringToFront();

            showOrHideTabsButton();
            flowHeaders.Controls.OfType<TabHeaderButton>().First().PerformClick();
        }

        protected async Task SetInitializationStatusAsync(string status)
        {
            this.loadingOverlay1.Status = status;
            //await Task.Delay(250);
        }

        protected IDisposable ShowLoadingOverlay(string status = "")
        {
            if (Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView == false)
                return this.loadingOverlay1.ShowThenHide(status);
            return null;
        }

        protected async Task InitializeAsync()
        {
            Serilog.Log.Logger.CurrentMethod();
            using (new StopWatch("Inititalize"))
            {
                using (this.ShowLoadingOverlay())
                {
                    K2SpyContext k2SpyContext = this.GetK2SpyContext();
                    k2SpyContext.Disconnecting += this.K2SpyContext_Disconnecting;
                    k2SpyContext.Disconnected += async (sender, e) => this.tsmiWelcome.PerformClick();
                    k2SpyContext.Connecting += this.K2SpyContext_ConnectingAsync;

                    await this.SetInitializationStatusAsync("Loading plugins...");
                    await Extensions.ExtensionsManager.InitializeAsync();

                    using (new StopWatch("Populate misc"))
                    {
                        await this.PopulateSearchPaneAsync(k2SpyContext);
                        await this.PopulateDefinitionPaneAsync();
                        await this.PopulateAnalyzerPaneAsync(k2SpyContext);
                        await this.PopulateToolsMenuAsync(k2SpyContext);
                    }
                    await this.PopulateCommandBarExtensionsAsync(k2SpyContext);

                    await this.InvokeInitializedExtensionsAsync(k2SpyContext);

                    await this.PopulateConnectionsAsync(true);
                }

                if (Connections.Default.Items.Length == 0)
                {
                    // no connections have been configured, let's show the Manage Connections dialog!
                    MessageBox.Show(this, "No connections have been configured, please configure at least one in Manage Connections", base.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tsmiManageConnections.PerformClick();
                }
                //if (Connections.Default.Items.Length > 0 && !Connections.Default.Items.Any(key => key.Selected))
                //{
                //    if (Connections.Default.Items.Length == 1)
                //    {
                //        await this.ConnectAsync(Connections.Default.Items[0]);
                //    }
                //    else
                //    {
                //        MessageBox.Show(this, "Select which connection to use in File > Connections", base.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
            }
        }

        protected async Task<K2SpyTreeNode> CreateSmartObjectsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
#if !POPULATE_TREE_IN_PARALLEL
                await this.SetInitializationStatusAsync("Populating SmartObjects...");
#endif

                SmartObjectsRootTreeNode smartObjectsRootTreeNode = await SmartObjectsRootTreeNode.CreateAsync(k2SpyContext);
                return smartObjectsRootTreeNode;
            }
        }

        protected async Task<K2SpyTreeNode> CreateFormsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
#if !POPULATE_TREE_IN_PARALLEL
                    await this.SetInitializationStatusAsync("Populating Forms...");
#endif

                FormsRootTreeNode formsRootTreeNode = await FormsRootTreeNode.CreateAsync(k2SpyContext);
                return formsRootTreeNode;
            }
        }

        protected async Task<K2SpyTreeNode> CreateViewsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
#if !POPULATE_TREE_IN_PARALLEL
                    await this.SetInitializationStatusAsync("Populating Views...");
#endif

                ViewsRootTreeNode viewsRootTreeNode = await ViewsRootTreeNode.CreateAsync(k2SpyContext);
                return viewsRootTreeNode;
            }
        }

        protected async Task<K2SpyTreeNode> CreateControlsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
                if (Properties.Settings.Default.PopulateControlsInTreeView)
                {
#if !POPULATE_TREE_IN_PARALLEL
                        await this.SetInitializationStatusAsync("Populating Controls...");
#endif

                    ControlsRootTreeNode controlsRootTreeNode = await ControlsRootTreeNode.CreateAsync(k2SpyContext);
                    return controlsRootTreeNode;
                }
            }
            return null;
        }

        protected async Task<K2SpyTreeNode> CreateServiceObjectsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
                if (Properties.Settings.Default.PopulateServiceObjectsInTreeView)
                {
#if !POPULATE_TREE_IN_PARALLEL
                        await this.SetInitializationStatusAsync("Populating service objects...");
#endif

                    ServiceTypesRootTreeNode serviceObjectsRootTreeNode = await ServiceTypesRootTreeNode.CreateAsync(k2SpyContext);
                    return serviceObjectsRootTreeNode;
                }
            }
            return null;
        }

        protected async Task<K2SpyTreeNode> CreateStyleProfilesAsync(K2SpyContext k2SpyContext)
        {
#if StyleProfile
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
#if !POPULATE_TREE_IN_PARALLEL
                await this.SetInitializationStatusAsync("Populating style profiles...");
#endif
                StyleProfilesRootTreeNode styleProfilesRootTreeNode = await StyleProfilesRootTreeNode.CreateAsync(k2SpyContext);
                return styleProfilesRootTreeNode;
            }
#endif
            return null;
        }

        protected async Task<K2SpyTreeNode> CreateWorkflowServerAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
                if (Properties.Settings.Default.PopulateWorkflowsInTreeView)
                {
#if !POPULATE_TREE_IN_PARALLEL
                        await this.SetInitializationStatusAsync("Populating workflow server...");
#endif
                    using (new StopWatch("Populate workflow server"))
                    {
                        WorkflowsRootTreeNode workflowsRootTreeNode = await WorkflowsRootTreeNode.CreateAsync(k2SpyContext);
                        return workflowsRootTreeNode;
                    }
                }
            }
            return null;
        }

        protected async Task<K2SpyTreeNode> CreateCategoriesAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
#if !POPULATE_TREE_IN_PARALLEL
                    await this.SetInitializationStatusAsync("Populating categories...");
#endif
                CategoryRootTreeNode categoryRootTreeNode = await CategoryRootTreeNode.CreateAsync(k2SpyContext);
                return categoryRootTreeNode;
            }
        }

        protected async Task InvokeInitializedExtensionsAsync(K2SpyContext k2SpyContext)
        {
            Serilog.Log.Logger.CurrentMethod();
            using (StopWatch.CurrentMethod())
            {
                foreach (Model.IInitializedExtension extension in Extensions.ExtensionsManager.GetExtensions<Model.IInitializedExtension>())
                {
                    await this.SetInitializationStatusAsync($"Initializing {extension.DisplayName}...");
                    await extension.InitializedAsync(k2SpyContext);
                }
            }
        }

        #endregion

        #region Private Methods

        private async void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (e.Node != null)
                {
                    Extensions.ExtensionsManager.GetExtensions<Model.ISelectedNodeChangedExtension>().ToList().ForEach(key => key.SelectedNodeChanged(e.Node));

                    this.m_AfterSelectCancellationTokenSource?.Cancel();
                    System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
                    this.m_AfterSelectCancellationTokenSource = cancellationTokenSource;

                    K2SpyContext context = this.GetK2SpyContext();

                    // wait a little to avoid flickering in case the user is moving quickly between nodes
                    await Task.Delay(100);
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        // ok, the selection has not been changed since we started waiting, let's notify the appropriate extensions
                        K2SpyTreeNode node = e.Node as K2SpyTreeNode;
                        K2SpyTreeNode actingAsTreeNode = await node.GetActAsOrSelfAsync();
                        Extensions.ExtensionsManager.Extensions.OfType<Model.IDelayedSelectedNodeChangedExtension>().ToList().ForEach(key => key.SelectedNodeChangedDelayed(context, node, actingAsTreeNode, cancellationTokenSource));
                    }
                }
            });
        }

        private void tsmiSearch_Click(object sender, EventArgs e)
        {
            this.ShowSearchPane();
        }

        private void tsbtnSearch_Click(object sender, EventArgs e)
        {
            this.tsmiSearch.PerformClick();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.ResetSelectedNode();
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Properties.Resources.AboutTxt);
                builder.AppendLine();
                builder.AppendLine("Plugins:");
                foreach (Model.IExtension extension in Extensions.ExtensionsManager.Extensions.OrderBy(key => key.DisplayName))
                    builder.AppendLine($"- {extension.DisplayName}");
                Extensions.ExtensionsManager.GetExtension<Extensions.DefinitionPane.DefinitionPaneExtension>().LoadData("About", builder.ToString(), Extensions.DefinitionPane.TextFormat.Text, false);
            });
        }

        private void licensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.ResetSelectedNode();
                Extensions.ExtensionsManager.GetExtension<Extensions.DefinitionPane.DefinitionPaneExtension>().LoadData("Licenses", Properties.Resources.LicensesTxt, Extensions.DefinitionPane.TextFormat.Text, false);
            });
        }

        private void tsmiWelcome_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.ResetSelectedNode();
                Extensions.ExtensionsManager.GetExtension<Extensions.DefinitionPane.DefinitionPaneExtension>().LoadData("K2Spy", Properties.Resources.WelcomeTxt + @"

---------------------------
Change log
" + Properties.Resources.ChangelogTxt, Extensions.DefinitionPane.TextFormat.Text, false);
            });
        }

        private async void tsmiManageConnections_Click(object sender, EventArgs e)
        {
            using (ManageConnections dlg = new ManageConnections(this.GetK2SpyContext()))
            {
                dlg.ShowDialog(this);
                await this.PopulateConnectionsAsync();
            }
        }

        private async Task K2SpyContext_ConnectingAsync(object sender, EventArgs e)
        {
            base.Text = $"K2Spy - {Connections.Default.Items.FirstOrDefault(key => key.Selected)?.ShortDisplayName}";
            using (new StopWatch("Connecting"))
            {
                using (this.ShowLoadingOverlay())
                {
                    K2SpyContext k2SpyContext = (K2SpyContext)sender;
                    await this.SetInitializationStatusAsync("Populating tree structure...");
                    List<TreeNode> nodes = new List<TreeNode>();
                    using (new StopWatch("Populate tree"))
                    {
#if true && !POPULATE_TREE_IN_PARALLEL
                        // add directly to tree
                        using (TreeViewUpdateContext.CreateIfNecessary(this.treeView))
                        {
                            this.treeView.Nodes.AddIfNotNull(await this.CreateCategoriesAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateFormsAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateViewsAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateStyleProfilesAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateControlsAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateSmartObjectsAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateServiceObjectsAsync(k2SpyContext));
                            this.treeView.Nodes.AddIfNotNull(await this.CreateWorkflowServerAsync(k2SpyContext));
                        }

                        if (Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                        {
                            await Task.WhenAll(this.treeView.Nodes.OfType<IInitializeChildren>().Select(key => key.InitializeChildrenAsync()));
                        }
#elif true && !POPULATE_TREE_IN_PARALLEL
                        // first add to list, then to tree
                        using (new TreeViewUpdateContext(this.treeView))
                        {
                            nodes.Add(await this.CreateCategoriesAsync(k2SpyContext));
                            nodes.Add(await this.CreateFormsAsync(k2SpyContext));
                            nodes.Add(await this.CreateViewsAsync(k2SpyContext));
                            nodes.Add(await this.CreateStyleProfilesAsync(k2SpyContext));
                            nodes.Add(await this.CreateControlsAsync(k2SpyContext));
                            nodes.Add(await this.CreateSmartObjectsAsync(k2SpyContext));
                            nodes.Add(await this.CreateServiceObjectsAsync(k2SpyContext));
                            nodes.Add(await this.CreateWorkflowServerAsync(k2SpyContext));
                        }

                        if (Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                        {
                            await Task.WhenAll(this.treeView.Nodes.OfType<IInitializeChildren>().Select(key => key.InitializeChildrenAsync()));
                        }

                        this.treeView.Nodes.AddRange(nodes.Where(key => key != null).ToArray());
#else
                        // create nodes in parallel and then add them to tree
                        Task<K2SpyTreeNode>[] tasks = new Task<K2SpyTreeNode>[]
                        {
                            this.CreateCategoriesAsync(k2SpyContext).StopWatch("CreateCategoriesAsync"),
                            this.CreateFormsAsync(k2SpyContext).StopWatch("CreateFormsAsync"),
                            this.CreateViewsAsync(k2SpyContext).StopWatch("CreateViewsAsync"),
                            this.CreateStyleProfilesAsync(k2SpyContext).StopWatch("CreateStyleProfilesAsync"),
                            this.CreateControlsAsync(k2SpyContext).StopWatch("CreateControlsAsync"),
                            this.CreateSmartObjectsAsync(k2SpyContext).StopWatch("CreateSmartObjectsAsync"),
                            this.CreateServiceObjectsAsync(k2SpyContext).StopWatch("CreateServiceObjectsAsync"),
                            this.CreateWorkflowServerAsync(k2SpyContext).StopWatch("CreateWorkflowServerAsync")
                        };
                        nodes.AddRange((await Task.WhenAll<K2SpyTreeNode>(tasks)).Where(key => key != null).ToArray());

                        using (new StopWatch("Adding nodes to tree"))
                        {
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<CategoryRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<FormsRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<ViewsRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<StyleProfilesRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<ControlsRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<SmartObjectsRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<ServiceTypesRootTreeNode>().SingleOrDefault());
                            this.treeView.Nodes.AddIfNotNull(nodes.OfType<WorkflowsRootTreeNode>().SingleOrDefault());
                        }

                        if (Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                        {
                            await Task.WhenAll(this.treeView.Nodes.OfType<IInitializeChildren>().Select(key => key.InitializeChildrenAsync().StopWatch("Initialzing " + key.GetType().Name)));
                        }
#endif
                    }

                    if (this.treeView.SelectedNode == null)
                        this.tsmiWelcome.PerformClick();
                }
            }
        }

        private async Task K2SpyContext_Disconnecting(object sender, EventArgs e)
        {
            base.Text = $"K2Spy";

            // we need to clean up the tree view etc
            K2SpyTreeNode[] nodes = this.treeView.Nodes.OfType<K2SpyTreeNode>().ToArray();
            this.treeView.Nodes.Clear();
            nodes.ToList().ForEach(key => key.Dispose());
        }

        private void tsmiCollapseAll_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                TreeNode rootNode = this.treeView.SelectedNode;
                while (rootNode?.Parent != null)
                    rootNode = rootNode.Parent;
                this.treeView.CollapseAll();
                this.ResetSelectedNode();
                this.treeView.SelectedNode = rootNode;
            });
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ResetSelectedNode()
        {
            if (this.treeView.SelectedNode != null)
            {
                this.treeView.SelectedNode = null;
                Extensions.ExtensionsManager.GetExtensions<Model.ISelectedNodeResetExtension>()
                    .ToList()
                    .ForEach(key => key.SelectedNodeReset());

            }
        }

        private void tsmiOptions_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (Options dlg = new Options(this.GetK2SpyContext()))
                    dlg.ShowDialog(this);
            });
        }

        private void ncvbncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            K2SpyTreeNode[] nodes = this.treeView.Nodes.OfType<K2SpyTreeNode>().ToArray();
            this.treeView.Nodes.Clear();
            nodes.ToList().ForEach(key => key.Dispose());
        }

        private async void tsmiDisconnect_Click_1(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                await this.DisconnectAsync();
            });
        }

        private async void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
        }

        #endregion

        private void tsbtnCollapseAll_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.tsmiCollapseAll.PerformClick();
            });
        }

        private async void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            await this.TryAsync(async () => await e.Node.InitializeChildrenAsync());
        }

        private async void cmsTreeView_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Point screenLocation = Cursor.Position;
            Point clientLocation = this.treeView.PointToClient(screenLocation);
            
        }
    }
}