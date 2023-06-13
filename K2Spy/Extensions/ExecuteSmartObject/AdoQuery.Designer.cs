namespace K2Spy.Extensions.ExecuteSmartObject
{
    partial class AdoQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdoQuery));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tssbtnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnExecute = new System.Windows.Forms.ToolStripButton();
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.groupPanel1 = new K2Spy.GroupPanel();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.scInputResult = new System.Windows.Forms.SplitContainer();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.paneHeader1 = new K2Spy.PaneHeader();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.paneHeader2 = new K2Spy.PaneHeader();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.loadingOverlay1 = new K2Spy.WorkingOverlay();
            this.toolStrip.SuspendLayout();
            this.paddedPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.pnlInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scInputResult)).BeginInit();
            this.scInputResult.Panel1.SuspendLayout();
            this.scInputResult.Panel2.SuspendLayout();
            this.scInputResult.SuspendLayout();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbtnSave,
            this.tsbtnOpen,
            this.toolStripSeparator1,
            this.tsbtnExecute});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(513, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tssbtnSave
            // 
            this.tssbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tssbtnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiSaveAs});
            this.tssbtnSave.Image = global::K2Spy.Properties.Resources.Save16;
            this.tssbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbtnSave.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.tssbtnSave.Name = "tssbtnSave";
            this.tssbtnSave.Size = new System.Drawing.Size(32, 22);
            this.tssbtnSave.Text = "Save";
            this.tssbtnSave.ButtonClick += new System.EventHandler(this.tssbtnSave_ButtonClick);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSave.Size = new System.Drawing.Size(136, 22);
            this.tsmiSave.Text = "&Save";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.Size = new System.Drawing.Size(136, 22);
            this.tsmiSaveAs.Text = "Save &as...";
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = global::K2Spy.Properties.Resources.OpenForm16;
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbtnOpen.Text = "Open";
            this.tsbtnOpen.Click += new System.EventHandler(this.tsbtnOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnExecute
            // 
            this.tsbtnExecute.Image = global::K2Spy.Properties.Resources.Run_blue_16x;
            this.tsbtnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnExecute.Name = "tsbtnExecute";
            this.tsbtnExecute.Size = new System.Drawing.Size(87, 22);
            this.tsbtnExecute.Text = "Execute (F5)";
            this.tsbtnExecute.Click += new System.EventHandler(this.tsbtnExecute_Click);
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.Controls.Add(this.groupPanel1);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 25);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(513, 476);
            this.paddedPanel1.TabIndex = 1;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.pnlInner);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 5);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(503, 466);
            this.groupPanel1.TabIndex = 0;
            // 
            // pnlInner
            // 
            this.pnlInner.Controls.Add(this.scInputResult);
            this.pnlInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInner.Location = new System.Drawing.Point(1, 1);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(499, 462);
            this.pnlInner.TabIndex = 5;
            // 
            // scInputResult
            // 
            this.scInputResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scInputResult.Location = new System.Drawing.Point(0, 0);
            this.scInputResult.Name = "scInputResult";
            this.scInputResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scInputResult.Panel1
            // 
            this.scInputResult.Panel1.Controls.Add(this.pnlInput);
            this.scInputResult.Panel1.Controls.Add(this.paneHeader1);
            this.scInputResult.Panel1MinSize = 100;
            // 
            // scInputResult.Panel2
            // 
            this.scInputResult.Panel2.Controls.Add(this.dataGridView);
            this.scInputResult.Panel2.Controls.Add(this.paneHeader2);
            this.scInputResult.Panel2MinSize = 100;
            this.scInputResult.Size = new System.Drawing.Size(499, 462);
            this.scInputResult.SplitterDistance = 150;
            this.scInputResult.TabIndex = 1;
            // 
            // pnlInput
            // 
            this.pnlInput.AutoScroll = true;
            this.pnlInput.Controls.Add(this.txtQuery);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInput.Location = new System.Drawing.Point(0, 25);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(499, 125);
            this.pnlInput.TabIndex = 1;
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(499, 125);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuppressCtrlEnter);
            this.txtQuery.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SuppressCtrlEnter);
            // 
            // paneHeader1
            // 
            this.paneHeader1.AutoSize = true;
            this.paneHeader1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader1.Closable = false;
            this.paneHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader1.Location = new System.Drawing.Point(0, 0);
            this.paneHeader1.Name = "paneHeader1";
            this.paneHeader1.Size = new System.Drawing.Size(499, 25);
            this.paneHeader1.TabIndex = 0;
            this.paneHeader1.TabStop = false;
            this.paneHeader1.Text = "ADO Query";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 25);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(499, 283);
            this.dataGridView.TabIndex = 1;
            // 
            // paneHeader2
            // 
            this.paneHeader2.AutoSize = true;
            this.paneHeader2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader2.Closable = false;
            this.paneHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader2.Location = new System.Drawing.Point(0, 0);
            this.paneHeader2.Name = "paneHeader2";
            this.paneHeader2.Size = new System.Drawing.Size(499, 25);
            this.paneHeader2.TabIndex = 0;
            this.paneHeader2.TabStop = false;
            this.paneHeader2.Text = "Result";
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 501);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(513, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel.Text = "(Status)";
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
            this.loadingOverlay1.Size = new System.Drawing.Size(513, 523);
            this.loadingOverlay1.Status = "Loading...";
            this.loadingOverlay1.TabIndex = 3;
            this.loadingOverlay1.Visible = false;
            // 
            // AdoQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 523);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.loadingOverlay1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdoQuery";
            this.Text = "ADO Query";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.paddedPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.pnlInner.ResumeLayout(false);
            this.scInputResult.Panel1.ResumeLayout(false);
            this.scInputResult.Panel1.PerformLayout();
            this.scInputResult.Panel2.ResumeLayout(false);
            this.scInputResult.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scInputResult)).EndInit();
            this.scInputResult.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSplitButton tssbtnSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAs;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnExecute;
        private PaddedPanel paddedPanel1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Panel pnlInner;
        private System.Windows.Forms.SplitContainer scInputResult;
        private System.Windows.Forms.Panel pnlInput;
        private PaneHeader paneHeader1;
        private System.Windows.Forms.DataGridView dataGridView;
        private PaneHeader paneHeader2;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        public System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private WorkingOverlay loadingOverlay1;
    }
}