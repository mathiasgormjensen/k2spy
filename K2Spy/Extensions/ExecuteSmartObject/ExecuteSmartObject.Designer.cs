namespace K2Spy.Extensions.ExecuteSmartObject
{
    partial class ExecuteSmartObject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteSmartObject));
            this.tableSmartObject = new System.Windows.Forms.TableLayoutPanel();
            this.lblMethod = new System.Windows.Forms.Label();
            this.comboMethod = new System.Windows.Forms.ComboBox();
            this.lblSmartObject = new System.Windows.Forms.Label();
            this.lblSmartObjectDisplayName = new System.Windows.Forms.Label();
            this.scInputResult = new System.Windows.Forms.SplitContainer();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.tableInput = new System.Windows.Forms.TableLayoutPanel();
            this.paneHeader1 = new K2Spy.PaneHeader();
            this.pnlOutput = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tableOutput = new System.Windows.Forms.TableLayoutPanel();
            this.paneHeader2 = new K2Spy.PaneHeader();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssbtnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnExecute = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.groupPanel1 = new K2Spy.GroupPanel();
            this.loadingOverlay1 = new K2Spy.WorkingOverlay();
            this.tableSmartObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scInputResult)).BeginInit();
            this.scInputResult.Panel1.SuspendLayout();
            this.scInputResult.Panel2.SuspendLayout();
            this.scInputResult.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.pnlOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.pnlInner.SuspendLayout();
            this.paddedPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableSmartObject
            // 
            this.tableSmartObject.AutoSize = true;
            this.tableSmartObject.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableSmartObject.ColumnCount = 2;
            this.tableSmartObject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableSmartObject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSmartObject.Controls.Add(this.lblMethod, 0, 2);
            this.tableSmartObject.Controls.Add(this.comboMethod, 1, 2);
            this.tableSmartObject.Controls.Add(this.lblSmartObject, 0, 1);
            this.tableSmartObject.Controls.Add(this.lblSmartObjectDisplayName, 1, 1);
            this.tableSmartObject.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableSmartObject.Location = new System.Drawing.Point(0, 0);
            this.tableSmartObject.Name = "tableSmartObject";
            this.tableSmartObject.RowCount = 3;
            this.tableSmartObject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableSmartObject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSmartObject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSmartObject.Size = new System.Drawing.Size(675, 44);
            this.tableSmartObject.TabIndex = 0;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(3, 16);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.lblMethod.Size = new System.Drawing.Size(51, 20);
            this.lblMethod.TabIndex = 2;
            this.lblMethod.Text = "Method:";
            // 
            // comboMethod
            // 
            this.comboMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMethod.FormattingEnabled = true;
            this.comboMethod.Location = new System.Drawing.Point(82, 20);
            this.comboMethod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.comboMethod.Name = "comboMethod";
            this.comboMethod.Size = new System.Drawing.Size(590, 21);
            this.comboMethod.TabIndex = 3;
            this.comboMethod.SelectedValueChanged += new System.EventHandler(this.comboMethod_SelectedValueChanged);
            // 
            // lblSmartObject
            // 
            this.lblSmartObject.AutoSize = true;
            this.lblSmartObject.Location = new System.Drawing.Point(3, 3);
            this.lblSmartObject.Name = "lblSmartObject";
            this.lblSmartObject.Size = new System.Drawing.Size(73, 13);
            this.lblSmartObject.TabIndex = 0;
            this.lblSmartObject.Text = "SmartObject:";
            // 
            // lblSmartObjectDisplayName
            // 
            this.lblSmartObjectDisplayName.AutoEllipsis = true;
            this.lblSmartObjectDisplayName.AutoSize = true;
            this.lblSmartObjectDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSmartObjectDisplayName.Location = new System.Drawing.Point(82, 3);
            this.lblSmartObjectDisplayName.Name = "lblSmartObjectDisplayName";
            this.lblSmartObjectDisplayName.Size = new System.Drawing.Size(590, 13);
            this.lblSmartObjectDisplayName.TabIndex = 1;
            this.lblSmartObjectDisplayName.Text = "...";
            // 
            // scInputResult
            // 
            this.scInputResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scInputResult.Location = new System.Drawing.Point(0, 44);
            this.scInputResult.Name = "scInputResult";
            this.scInputResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scInputResult.Panel1
            // 
            this.scInputResult.Panel1.Controls.Add(this.pnlInput);
            this.scInputResult.Panel1.Controls.Add(this.paneHeader1);
            // 
            // scInputResult.Panel2
            // 
            this.scInputResult.Panel2.Controls.Add(this.pnlOutput);
            this.scInputResult.Panel2.Controls.Add(this.paneHeader2);
            this.scInputResult.Size = new System.Drawing.Size(675, 418);
            this.scInputResult.SplitterDistance = 200;
            this.scInputResult.TabIndex = 1;
            // 
            // pnlInput
            // 
            this.pnlInput.AutoScroll = true;
            this.pnlInput.Controls.Add(this.tableInput);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInput.Location = new System.Drawing.Point(0, 25);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(675, 175);
            this.pnlInput.TabIndex = 1;
            // 
            // tableInput
            // 
            this.tableInput.AutoSize = true;
            this.tableInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableInput.ColumnCount = 2;
            this.tableInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableInput.Location = new System.Drawing.Point(0, 0);
            this.tableInput.Margin = new System.Windows.Forms.Padding(0);
            this.tableInput.Name = "tableInput";
            this.tableInput.RowCount = 2;
            this.tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInput.Size = new System.Drawing.Size(675, 0);
            this.tableInput.TabIndex = 0;
            // 
            // paneHeader1
            // 
            this.paneHeader1.AutoSize = true;
            this.paneHeader1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader1.Closable = false;
            this.paneHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader1.Location = new System.Drawing.Point(0, 0);
            this.paneHeader1.Name = "paneHeader1";
            this.paneHeader1.Size = new System.Drawing.Size(675, 25);
            this.paneHeader1.TabIndex = 0;
            this.paneHeader1.TabStop = false;
            this.paneHeader1.Text = "Input";
            // 
            // pnlOutput
            // 
            this.pnlOutput.AutoScroll = true;
            this.pnlOutput.Controls.Add(this.dataGridView);
            this.pnlOutput.Controls.Add(this.tableOutput);
            this.pnlOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOutput.Location = new System.Drawing.Point(0, 25);
            this.pnlOutput.Name = "pnlOutput";
            this.pnlOutput.Size = new System.Drawing.Size(675, 189);
            this.pnlOutput.TabIndex = 2;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(675, 189);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.Visible = false;
            // 
            // tableOutput
            // 
            this.tableOutput.AutoSize = true;
            this.tableOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableOutput.ColumnCount = 2;
            this.tableOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableOutput.Location = new System.Drawing.Point(0, 0);
            this.tableOutput.Name = "tableOutput";
            this.tableOutput.RowCount = 2;
            this.tableOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableOutput.Size = new System.Drawing.Size(675, 0);
            this.tableOutput.TabIndex = 1;
            this.tableOutput.Visible = false;
            // 
            // paneHeader2
            // 
            this.paneHeader2.AutoSize = true;
            this.paneHeader2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paneHeader2.Closable = false;
            this.paneHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneHeader2.Location = new System.Drawing.Point(0, 0);
            this.paneHeader2.Name = "paneHeader2";
            this.paneHeader2.Size = new System.Drawing.Size(675, 25);
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
            this.statusStrip.Size = new System.Drawing.Size(689, 22);
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
            // tsbtnExecute
            // 
            this.tsbtnExecute.Image = global::K2Spy.Properties.Resources.Run_blue_16x;
            this.tsbtnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnExecute.Name = "tsbtnExecute";
            this.tsbtnExecute.Size = new System.Drawing.Size(87, 22);
            this.tsbtnExecute.Text = "Execute (F5)";
            this.tsbtnExecute.Click += new System.EventHandler(this.tsbtnExecute_Click);
            // 
            // tsbtnRefresh
            // 
            this.tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefresh.Image = global::K2Spy.Properties.Resources.Refresh_16x;
            this.tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRefresh.Name = "tsbtnRefresh";
            this.tsbtnRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbtnRefresh.Text = "Refresh SmartObject";
            this.tsbtnRefresh.Click += new System.EventHandler(this.tsbtnRefresh_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbtnSave,
            this.tsbtnOpen,
            this.tsbtnRefresh,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.tsbtnExecute});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(689, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::K2Spy.Properties.Resources.ClearIcon16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Clear";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // pnlInner
            // 
            this.pnlInner.AutoScroll = true;
            this.pnlInner.Controls.Add(this.scInputResult);
            this.pnlInner.Controls.Add(this.tableSmartObject);
            this.pnlInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInner.Location = new System.Drawing.Point(1, 1);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(675, 462);
            this.pnlInner.TabIndex = 5;
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.Controls.Add(this.groupPanel1);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 25);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(689, 476);
            this.paddedPanel1.TabIndex = 1;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.pnlInner);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 5);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(679, 466);
            this.groupPanel1.TabIndex = 0;
            // 
            // loadingOverlay1
            // 
            this.loadingOverlay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingOverlay1.BackColor = System.Drawing.Color.White;
            this.loadingOverlay1.DockOnShow = false;
            this.loadingOverlay1.Location = new System.Drawing.Point(0, 0);
            this.loadingOverlay1.Margin = new System.Windows.Forms.Padding(0);
            this.loadingOverlay1.Name = "loadingOverlay1";
            this.loadingOverlay1.Size = new System.Drawing.Size(689, 523);
            this.loadingOverlay1.Status = "Loading...";
            this.loadingOverlay1.TabIndex = 3;
            // 
            // ExecuteSmartObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 523);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.loadingOverlay1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExecuteSmartObject";
            this.Text = "Execute SmartObject";
            this.Load += new System.EventHandler(this.ExecuteSmartObject_Load);
            this.tableSmartObject.ResumeLayout(false);
            this.tableSmartObject.PerformLayout();
            this.scInputResult.Panel1.ResumeLayout(false);
            this.scInputResult.Panel1.PerformLayout();
            this.scInputResult.Panel2.ResumeLayout(false);
            this.scInputResult.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scInputResult)).EndInit();
            this.scInputResult.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlOutput.ResumeLayout(false);
            this.pnlOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlInner.ResumeLayout(false);
            this.pnlInner.PerformLayout();
            this.paddedPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableSmartObject;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox comboMethod;
        private System.Windows.Forms.SplitContainer scInputResult;
        private PaneHeader paneHeader1;
        private PaneHeader paneHeader2;
        private System.Windows.Forms.Label lblSmartObject;
        private System.Windows.Forms.Label lblSmartObjectDisplayName;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TableLayoutPanel tableInput;
        private System.Windows.Forms.TableLayoutPanel tableOutput;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripSplitButton tssbtnSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAs;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnExecute;
        private System.Windows.Forms.ToolStripButton tsbtnRefresh;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel pnlInner;
        private PaddedPanel paddedPanel1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel pnlOutput;
        private WorkingOverlay loadingOverlay1;
    }
}