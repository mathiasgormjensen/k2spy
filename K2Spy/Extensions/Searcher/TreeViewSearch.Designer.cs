namespace K2Spy.Extensions.Searcher
{
    partial class TreeViewSearch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeViewSearch));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tstxtSearch = new K2Spy.ToolStripSpringTextBox();
            this.tsbtnStopSearch = new System.Windows.Forms.ToolStripButton();
            this.tslblSearchType = new System.Windows.Forms.ToolStripLabel();
            this.tsddbtnSearchType = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiSearchTreeNodes = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchAllDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchFormDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchViewDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchSmartObjectDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchWorkflowDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSearchServiceInstanceDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnScope = new System.Windows.Forms.ToolStripButton();
            this.paneHeader = new K2Spy.PaneHeader();
            this.listView1 = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.categoryScope1 = new K2Spy.CategoryScope();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.CanOverflow = false;
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstxtSearch,
            this.tsbtnStopSearch,
            this.tslblSearchType,
            this.tsddbtnSearchType,
            this.tsbtnScope});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(506, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip2";
            // 
            // tstxtSearch
            // 
            this.tstxtSearch.Name = "tstxtSearch";
            this.tstxtSearch.Size = new System.Drawing.Size(236, 25);
            this.tstxtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tstxtSearch_KeyPress);
            this.tstxtSearch.TextChanged += new System.EventHandler(this.tstxtSearch_TextChanged);
            // 
            // tsbtnStopSearch
            // 
            this.tsbtnStopSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnStopSearch.Enabled = false;
            this.tsbtnStopSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnStopSearch.Image")));
            this.tsbtnStopSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStopSearch.Name = "tsbtnStopSearch";
            this.tsbtnStopSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbtnStopSearch.Text = "Stop search";
            this.tsbtnStopSearch.Click += new System.EventHandler(this.tsbtnStopSearch_Click);
            // 
            // tslblSearchType
            // 
            this.tslblSearchType.Name = "tslblSearchType";
            this.tslblSearchType.Size = new System.Drawing.Size(59, 22);
            this.tslblSearchType.Text = "Search for";
            // 
            // tsddbtnSearchType
            // 
            this.tsddbtnSearchType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSearchTreeNodes,
            this.tsmiSearchAllDefinitions,
            this.tsmiSearchFormDefinitions,
            this.tsmiSearchViewDefinitions,
            this.tsmiSearchSmartObjectDefinitions,
            this.tsmiSearchWorkflowDefinitions,
            this.tsmiSearchServiceInstanceDefinitions});
            this.tsddbtnSearchType.Image = global::K2Spy.Properties.Resources.TreeView_16x;
            this.tsddbtnSearchType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnSearchType.Name = "tsddbtnSearchType";
            this.tsddbtnSearchType.Size = new System.Drawing.Size(91, 22);
            this.tsddbtnSearchType.Text = "Tree nodes";
            // 
            // tsmiSearchTreeNodes
            // 
            this.tsmiSearchTreeNodes.Image = global::K2Spy.Properties.Resources.TreeView_16x;
            this.tsmiSearchTreeNodes.Name = "tsmiSearchTreeNodes";
            this.tsmiSearchTreeNodes.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchTreeNodes.Text = "Tree nodes";
            // 
            // tsmiSearchAllDefinitions
            // 
            this.tsmiSearchAllDefinitions.Image = global::K2Spy.Properties.Resources.XMLDocumentTypeDefinitionFile_16x;
            this.tsmiSearchAllDefinitions.Name = "tsmiSearchAllDefinitions";
            this.tsmiSearchAllDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchAllDefinitions.Text = "All definitions";
            // 
            // tsmiSearchFormDefinitions
            // 
            this.tsmiSearchFormDefinitions.Image = global::K2Spy.Properties.Resources.Form16;
            this.tsmiSearchFormDefinitions.Name = "tsmiSearchFormDefinitions";
            this.tsmiSearchFormDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchFormDefinitions.Text = "Form definitions";
            // 
            // tsmiSearchViewDefinitions
            // 
            this.tsmiSearchViewDefinitions.Image = global::K2Spy.Properties.Resources.View16;
            this.tsmiSearchViewDefinitions.Name = "tsmiSearchViewDefinitions";
            this.tsmiSearchViewDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchViewDefinitions.Text = "View definitions";
            // 
            // tsmiSearchSmartObjectDefinitions
            // 
            this.tsmiSearchSmartObjectDefinitions.Image = global::K2Spy.Properties.Resources.SmartObject16;
            this.tsmiSearchSmartObjectDefinitions.Name = "tsmiSearchSmartObjectDefinitions";
            this.tsmiSearchSmartObjectDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchSmartObjectDefinitions.Text = "SmartObject definitions";
            // 
            // tsmiSearchWorkflowDefinitions
            // 
            this.tsmiSearchWorkflowDefinitions.Image = global::K2Spy.Properties.Resources.Workflow16;
            this.tsmiSearchWorkflowDefinitions.Name = "tsmiSearchWorkflowDefinitions";
            this.tsmiSearchWorkflowDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchWorkflowDefinitions.Text = "Workflow definitions";
            // 
            // tsmiSearchServiceInstanceDefinitions
            // 
            this.tsmiSearchServiceInstanceDefinitions.Image = global::K2Spy.Properties.Resources.ServiceInstance16;
            this.tsmiSearchServiceInstanceDefinitions.Name = "tsmiSearchServiceInstanceDefinitions";
            this.tsmiSearchServiceInstanceDefinitions.Size = new System.Drawing.Size(214, 22);
            this.tsmiSearchServiceInstanceDefinitions.Text = "Service instance definitions";
            // 
            // tsbtnScope
            // 
            this.tsbtnScope.Image = global::K2Spy.Properties.Resources.FolderClosed16;
            this.tsbtnScope.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnScope.Name = "tsbtnScope";
            this.tsbtnScope.Size = new System.Drawing.Size(58, 22);
            this.tsbtnScope.Text = "Scope";
            this.tsbtnScope.Click += new System.EventHandler(this.tsbtnScope_Click);
            // 
            // paneHeader
            // 
            this.paneHeader.AutoSize = true;
            this.paneHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader.Location = new System.Drawing.Point(0, 0);
            this.paneHeader.Name = "paneHeader";
            this.paneHeader.Size = new System.Drawing.Size(506, 25);
            this.paneHeader.TabIndex = 0;
            this.paneHeader.Text = "Search";
            this.paneHeader.CloseClicked += new System.EventHandler(this.paneHeader_CloseClicked);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmPath});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 82);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(506, 386);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 150;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 300;
            // 
            // categoryScope1
            // 
            this.categoryScope1.AutoSize = true;
            this.categoryScope1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.categoryScope1.Dock = System.Windows.Forms.DockStyle.Top;
            this.categoryScope1.HideScopeCheckBox = true;
            this.categoryScope1.Location = new System.Drawing.Point(0, 50);
            this.categoryScope1.Name = "categoryScope1";
            this.categoryScope1.Size = new System.Drawing.Size(506, 29);
            this.categoryScope1.TabIndex = 1;
            this.categoryScope1.Visible = false;
            this.categoryScope1.CategoryPathChanged += new System.EventHandler(this.categoryScope1_CategoryPathChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(0, 79);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(506, 3);
            this.progressBar1.TabIndex = 4;
            // 
            // TreeViewSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.categoryScope1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.paneHeader);
            this.Name = "TreeViewSearch";
            this.Size = new System.Drawing.Size(506, 468);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private ToolStripSpringTextBox tstxtSearch;
        private System.Windows.Forms.ToolStripButton tsbtnStopSearch;
        private System.Windows.Forms.ToolStripLabel tslblSearchType;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnSearchType;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchTreeNodes;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchAllDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchFormDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchViewDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchSmartObjectDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchWorkflowDefinitions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSearchServiceInstanceDefinitions;
        private PaneHeader paneHeader;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmPath;
        private CategoryScope categoryScope1;
        private System.Windows.Forms.ToolStripButton tsbtnScope;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
