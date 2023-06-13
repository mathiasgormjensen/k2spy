namespace K2Spy
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.outerSplitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.treeViewImages = new System.Windows.Forms.ImageList(this.components);
            this.paneHeader1 = new K2Spy.PaneHeader();
            this.scDefinitionAnalysis = new System.Windows.Forms.SplitContainer();
            this.scSearchDefinition = new System.Windows.Forms.SplitContainer();
            this.tsTop = new System.Windows.Forms.ToolStrip();
            this.tsbtnSearch = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.tssepConnections = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiManageConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWelcome = new System.Windows.Forms.ToolStripMenuItem();
            this.licensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiKnownIssues = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.loadingOverlay1 = new K2Spy.WorkingOverlay();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.progressBarLabel1 = new K2Spy.ProgressBarLabel();
            ((System.ComponentModel.ISupportInitialize)(this.outerSplitContainer)).BeginInit();
            this.outerSplitContainer.Panel1.SuspendLayout();
            this.outerSplitContainer.Panel2.SuspendLayout();
            this.outerSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDefinitionAnalysis)).BeginInit();
            this.scDefinitionAnalysis.Panel1.SuspendLayout();
            this.scDefinitionAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSearchDefinition)).BeginInit();
            this.scSearchDefinition.SuspendLayout();
            this.tsTop.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // outerSplitContainer
            // 
            this.outerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerSplitContainer.Location = new System.Drawing.Point(0, 49);
            this.outerSplitContainer.Name = "outerSplitContainer";
            // 
            // outerSplitContainer.Panel1
            // 
            this.outerSplitContainer.Panel1.Controls.Add(this.treeView);
            this.outerSplitContainer.Panel1.Controls.Add(this.paneHeader1);
            this.outerSplitContainer.Panel1MinSize = 250;
            // 
            // outerSplitContainer.Panel2
            // 
            this.outerSplitContainer.Panel2.Controls.Add(this.scDefinitionAnalysis);
            this.outerSplitContainer.Panel2MinSize = 350;
            this.outerSplitContainer.Size = new System.Drawing.Size(739, 426);
            this.outerSplitContainer.SplitterDistance = 250;
            this.outerSplitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeViewImages;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(250, 401);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // treeViewImages
            // 
            this.treeViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImages.ImageStream")));
            this.treeViewImages.TransparentColor = System.Drawing.Color.Transparent;
            this.treeViewImages.Images.SetKeyName(0, "Default");
            this.treeViewImages.Images.SetKeyName(1, "FolderClosed");
            this.treeViewImages.Images.SetKeyName(2, "RootItem");
            this.treeViewImages.Images.SetKeyName(3, "Form");
            this.treeViewImages.Images.SetKeyName(4, "ServiceInstance");
            this.treeViewImages.Images.SetKeyName(5, "Workflow");
            this.treeViewImages.Images.SetKeyName(6, "View");
            // 
            // paneHeader1
            // 
            this.paneHeader1.AutoSize = true;
            this.paneHeader1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader1.Closable = false;
            this.paneHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader1.Location = new System.Drawing.Point(0, 0);
            this.paneHeader1.Name = "paneHeader1";
            this.paneHeader1.Size = new System.Drawing.Size(250, 25);
            this.paneHeader1.TabIndex = 1;
            this.paneHeader1.TabStop = false;
            this.paneHeader1.Text = "Categories and components";
            this.paneHeader1.Visible = false;
            // 
            // scDefinitionAnalysis
            // 
            this.scDefinitionAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDefinitionAnalysis.Location = new System.Drawing.Point(0, 0);
            this.scDefinitionAnalysis.Name = "scDefinitionAnalysis";
            this.scDefinitionAnalysis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scDefinitionAnalysis.Panel1
            // 
            this.scDefinitionAnalysis.Panel1.Controls.Add(this.scSearchDefinition);
            this.scDefinitionAnalysis.Panel1MinSize = 255;
            this.scDefinitionAnalysis.Panel2MinSize = 130;
            this.scDefinitionAnalysis.Size = new System.Drawing.Size(485, 426);
            this.scDefinitionAnalysis.SplitterDistance = 289;
            this.scDefinitionAnalysis.TabIndex = 1;
            // 
            // scSearchDefinition
            // 
            this.scSearchDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSearchDefinition.Location = new System.Drawing.Point(0, 0);
            this.scSearchDefinition.Name = "scSearchDefinition";
            this.scSearchDefinition.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.scSearchDefinition.Panel1MinSize = 155;
            this.scSearchDefinition.Panel2MinSize = 80;
            this.scSearchDefinition.Size = new System.Drawing.Size(485, 289);
            this.scSearchDefinition.SplitterDistance = 156;
            this.scSearchDefinition.TabIndex = 0;
            // 
            // tsTop
            // 
            this.tsTop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSearch,
            this.tsbtnCollapseAll});
            this.tsTop.Location = new System.Drawing.Point(0, 24);
            this.tsTop.Name = "tsTop";
            this.tsTop.Size = new System.Drawing.Size(739, 25);
            this.tsTop.TabIndex = 1;
            this.tsTop.Text = "toolStrip1";
            // 
            // tsbtnSearch
            // 
            this.tsbtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSearch.Image = global::K2Spy.Properties.Resources.VBSearch_16x;
            this.tsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSearch.Name = "tsbtnSearch";
            this.tsbtnSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbtnSearch.Text = "Search";
            this.tsbtnSearch.Click += new System.EventHandler(this.tsbtnSearch_Click);
            // 
            // tsbtnCollapseAll
            // 
            this.tsbtnCollapseAll.Image = global::K2Spy.Properties.Resources.CollapseAll_16x;
            this.tsbtnCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCollapseAll.Name = "tsbtnCollapseAll";
            this.tsbtnCollapseAll.Size = new System.Drawing.Size(86, 22);
            this.tsbtnCollapseAll.Text = "Collapse all";
            this.tsbtnCollapseAll.Click += new System.EventHandler(this.tsbtnCollapseAll_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiView,
            this.tsmiTools,
            this.tsmiWindow,
            this.tsmiHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(739, 24);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "menuStrip";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiConnections,
            this.tsmiClose});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "&File";
            // 
            // tsmiConnections
            // 
            this.tsmiConnections.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssepConnections,
            this.tsmiManageConnections,
            this.tsmiDisconnect});
            this.tsmiConnections.Name = "tsmiConnections";
            this.tsmiConnections.Size = new System.Drawing.Size(139, 22);
            this.tsmiConnections.Text = "&Connections";
            // 
            // tssepConnections
            // 
            this.tssepConnections.Name = "tssepConnections";
            this.tssepConnections.Size = new System.Drawing.Size(179, 6);
            // 
            // tsmiManageConnections
            // 
            this.tsmiManageConnections.Name = "tsmiManageConnections";
            this.tsmiManageConnections.Size = new System.Drawing.Size(182, 22);
            this.tsmiManageConnections.Text = "Manage connections";
            this.tsmiManageConnections.Click += new System.EventHandler(this.tsmiManageConnections_Click);
            // 
            // tsmiDisconnect
            // 
            this.tsmiDisconnect.Name = "tsmiDisconnect";
            this.tsmiDisconnect.Size = new System.Drawing.Size(182, 22);
            this.tsmiDisconnect.Text = "&Disconnect";
            this.tsmiDisconnect.Click += new System.EventHandler(this.tsmiDisconnect_Click_1);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(139, 22);
            this.tsmiClose.Text = "&Close";
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // tsmiView
            // 
            this.tsmiView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSearch,
            this.tsmiCollapseAll,
            this.toolStripMenuItem1,
            this.tsmiOptions});
            this.tsmiView.Name = "tsmiView";
            this.tsmiView.Size = new System.Drawing.Size(44, 20);
            this.tsmiView.Text = "&View";
            // 
            // tsmiSearch
            // 
            this.tsmiSearch.Image = global::K2Spy.Properties.Resources.VBSearch_16x;
            this.tsmiSearch.Name = "tsmiSearch";
            this.tsmiSearch.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.tsmiSearch.Size = new System.Drawing.Size(179, 22);
            this.tsmiSearch.Text = "&Search";
            this.tsmiSearch.Click += new System.EventHandler(this.tsmiSearch_Click);
            // 
            // tsmiCollapseAll
            // 
            this.tsmiCollapseAll.Image = global::K2Spy.Properties.Resources.CollapseAll_16x;
            this.tsmiCollapseAll.Name = "tsmiCollapseAll";
            this.tsmiCollapseAll.Size = new System.Drawing.Size(179, 22);
            this.tsmiCollapseAll.Text = "&Collapse all";
            this.tsmiCollapseAll.Click += new System.EventHandler(this.tsmiCollapseAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(176, 6);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.Image = global::K2Spy.Properties.Resources.Settings16;
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(179, 22);
            this.tsmiOptions.Text = "&Options";
            this.tsmiOptions.Click += new System.EventHandler(this.tsmiOptions_Click);
            // 
            // tsmiTools
            // 
            this.tsmiTools.Name = "tsmiTools";
            this.tsmiTools.Size = new System.Drawing.Size(45, 20);
            this.tsmiTools.Text = "&Tools";
            // 
            // tsmiWindow
            // 
            this.tsmiWindow.Name = "tsmiWindow";
            this.tsmiWindow.Size = new System.Drawing.Size(63, 20);
            this.tsmiWindow.Text = "&Window";
            this.tsmiWindow.Visible = false;
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiWelcome,
            this.licensesToolStripMenuItem,
            this.tsmiCheckForUpdates,
            this.tsmiKnownIssues,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(43, 20);
            this.tsmiHelp.Text = "&Help";
            // 
            // tsmiWelcome
            // 
            this.tsmiWelcome.Name = "tsmiWelcome";
            this.tsmiWelcome.Size = new System.Drawing.Size(168, 22);
            this.tsmiWelcome.Text = "&Welcome";
            this.tsmiWelcome.Click += new System.EventHandler(this.tsmiWelcome_Click);
            // 
            // licensesToolStripMenuItem
            // 
            this.licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
            this.licensesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.licensesToolStripMenuItem.Text = "&Licenses";
            this.licensesToolStripMenuItem.Click += new System.EventHandler(this.licensesToolStripMenuItem_Click);
            // 
            // tsmiCheckForUpdates
            // 
            this.tsmiCheckForUpdates.Enabled = false;
            this.tsmiCheckForUpdates.Name = "tsmiCheckForUpdates";
            this.tsmiCheckForUpdates.Size = new System.Drawing.Size(168, 22);
            this.tsmiCheckForUpdates.Text = "&Check for updates";
            // 
            // tsmiKnownIssues
            // 
            this.tsmiKnownIssues.Enabled = false;
            this.tsmiKnownIssues.Name = "tsmiKnownIssues";
            this.tsmiKnownIssues.Size = new System.Drawing.Size(168, 22);
            this.tsmiKnownIssues.Text = "&Known issues";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(168, 22);
            this.tsmiAbout.Text = "&About";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // loadingOverlay1
            // 
            this.loadingOverlay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingOverlay1.BackColor = System.Drawing.Color.White;
            this.loadingOverlay1.DockOnShow = false;
            this.loadingOverlay1.Location = new System.Drawing.Point(0, 0);
            this.loadingOverlay1.Name = "loadingOverlay1";
            this.loadingOverlay1.Size = new System.Drawing.Size(739, 495);
            this.loadingOverlay1.Status = "Initializing...";
            this.loadingOverlay1.TabIndex = 6;
            this.loadingOverlay1.Visible = false;
            // 
            // pnlStatus
            // 
            this.pnlStatus.AutoSize = true;
            this.pnlStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlStatus.Controls.Add(this.progressBarLabel1);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 475);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(739, 20);
            this.pnlStatus.TabIndex = 8;
            // 
            // progressBarLabel1
            // 
            this.progressBarLabel1.AutoSize = true;
            this.progressBarLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.progressBarLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarLabel1.Location = new System.Drawing.Point(0, 0);
            this.progressBarLabel1.Name = "progressBarLabel1";
            this.progressBarLabel1.ProgressBarMarqueeAnimationSpeed = 30;
            this.progressBarLabel1.Size = new System.Drawing.Size(739, 20);
            this.progressBarLabel1.TabIndex = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 495);
            this.Controls.Add(this.outerSplitContainer);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.tsTop);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.loadingOverlay1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "K2Spy";
            this.outerSplitContainer.Panel1.ResumeLayout(false);
            this.outerSplitContainer.Panel1.PerformLayout();
            this.outerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outerSplitContainer)).EndInit();
            this.outerSplitContainer.ResumeLayout(false);
            this.scDefinitionAnalysis.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scDefinitionAnalysis)).EndInit();
            this.scDefinitionAnalysis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSearchDefinition)).EndInit();
            this.scSearchDefinition.ResumeLayout(false);
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer outerSplitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList treeViewImages;
        private System.Windows.Forms.SplitContainer scSearchDefinition;
        private System.Windows.Forms.ToolStrip tsTop;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearch;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiWindow;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripButton tsbtnSearch;
        private System.Windows.Forms.ToolStripMenuItem licensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ToolStripMenuItem tsmiConnections;
        private System.Windows.Forms.ToolStripSeparator tssepConnections;
        private System.Windows.Forms.ToolStripMenuItem tsmiManageConnections;
        private System.Windows.Forms.ToolStripMenuItem tsmiWelcome;
        private System.Windows.Forms.SplitContainer scDefinitionAnalysis;
        private PaneHeader paneHeader1;
        private WorkingOverlay loadingOverlay1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCollapseAll;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckForUpdates;
        private ProgressBarLabel progressBarLabel1;
        private System.Windows.Forms.ToolStripMenuItem tsmiKnownIssues;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisconnect;
        private System.Windows.Forms.ToolStripButton tsbtnCollapseAll;
    }
}

