namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
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
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiGoToItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.paneHeader = new K2Spy.PaneHeader();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(575, 390);
            this.treeView1.TabIndex = 2;
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGoToItem,
            this.tsmiRemove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(131, 48);
            // 
            // tsmiGoToItem
            // 
            this.tsmiGoToItem.Name = "tsmiGoToItem";
            this.tsmiGoToItem.Size = new System.Drawing.Size(130, 22);
            this.tsmiGoToItem.Text = "&Go to item";
            this.tsmiGoToItem.Click += new System.EventHandler(this.tsmiGoToItem_Click);
            // 
            // tsmiRemove
            // 
            this.tsmiRemove.Image = global::K2Spy.Properties.Resources.ClearIcon16;
            this.tsmiRemove.Name = "tsmiRemove";
            this.tsmiRemove.Size = new System.Drawing.Size(130, 22);
            this.tsmiRemove.Text = "&Remove";
            this.tsmiRemove.Click += new System.EventHandler(this.tsmiRemove_Click);
            // 
            // paneHeader
            // 
            this.paneHeader.AutoSize = true;
            this.paneHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader.Location = new System.Drawing.Point(0, 0);
            this.paneHeader.Name = "paneHeader";
            this.paneHeader.Size = new System.Drawing.Size(575, 25);
            this.paneHeader.TabIndex = 0;
            this.paneHeader.Text = "Analyze";
            this.paneHeader.CloseClicked += new System.EventHandler(this.paneHeader_CloseClicked);
            // 
            // TreeViewAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.paneHeader);
            this.Name = "TreeViewAnalyzer";
            this.Size = new System.Drawing.Size(575, 415);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PaneHeader paneHeader;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiGoToItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemove;
    }
}
