namespace K2Spy
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.chkPreserveInvalidCategoryEntriesInTreeView = new System.Windows.Forms.CheckBox();
            this.chkPopulateSmartMethodsAndPropertiesInTreeView = new System.Windows.Forms.CheckBox();
            this.chkPopulateServiceObjectsInTreeView = new System.Windows.Forms.CheckBox();
            this.chkPopulateControlsInTreeView = new System.Windows.Forms.CheckBox();
            this.headerTreeView = new K2Spy.PaneHeader();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.flowButtons.SuspendLayout();
            this.paddedPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowButtons
            // 
            this.flowButtons.AutoSize = true;
            this.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowButtons.Controls.Add(this.btnCancel);
            this.flowButtons.Controls.Add(this.btnOK);
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowButtons.Location = new System.Drawing.Point(0, 292);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(434, 29);
            this.flowButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(356, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(275, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.Controls.Add(this.tabControl);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 0);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(434, 292);
            this.paddedPanel1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpGeneral);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(5, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(424, 282);
            this.tabControl.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.AutoScroll = true;
            this.tpGeneral.Controls.Add(this.chkPreserveInvalidCategoryEntriesInTreeView);
            this.tpGeneral.Controls.Add(this.chkPopulateSmartMethodsAndPropertiesInTreeView);
            this.tpGeneral.Controls.Add(this.chkPopulateServiceObjectsInTreeView);
            this.tpGeneral.Controls.Add(this.chkPopulateControlsInTreeView);
            this.tpGeneral.Controls.Add(this.headerTreeView);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(416, 256);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // chkPreserveInvalidCategoryEntriesInTreeView
            // 
            this.chkPreserveInvalidCategoryEntriesInTreeView.AutoSize = true;
            this.chkPreserveInvalidCategoryEntriesInTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPreserveInvalidCategoryEntriesInTreeView.Location = new System.Drawing.Point(3, 79);
            this.chkPreserveInvalidCategoryEntriesInTreeView.Name = "chkPreserveInvalidCategoryEntriesInTreeView";
            this.chkPreserveInvalidCategoryEntriesInTreeView.Size = new System.Drawing.Size(410, 17);
            this.chkPreserveInvalidCategoryEntriesInTreeView.TabIndex = 4;
            this.chkPreserveInvalidCategoryEntriesInTreeView.Text = "Preserve invalid category entries";
            this.toolTip.SetToolTip(this.chkPreserveInvalidCategoryEntriesInTreeView, resources.GetString("chkPreserveInvalidCategoryEntriesInTreeView.ToolTip"));
            this.chkPreserveInvalidCategoryEntriesInTreeView.UseVisualStyleBackColor = true;
            // 
            // chkPopulateSmartMethodsAndPropertiesInTreeView
            // 
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.AutoSize = true;
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Location = new System.Drawing.Point(3, 62);
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Name = "chkPopulateSmartMethodsAndPropertiesInTreeView";
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Size = new System.Drawing.Size(410, 17);
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.TabIndex = 3;
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Text = "Populate SmartObject methods and properties";
            this.toolTip.SetToolTip(this.chkPopulateSmartMethodsAndPropertiesInTreeView, "Specifies whether SmartObject methods and properties should be populated in the t" +
        "reeview, and hence whether they should be searchable");
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.UseVisualStyleBackColor = true;
            // 
            // chkPopulateServiceObjectsInTreeView
            // 
            this.chkPopulateServiceObjectsInTreeView.AutoSize = true;
            this.chkPopulateServiceObjectsInTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPopulateServiceObjectsInTreeView.Location = new System.Drawing.Point(3, 45);
            this.chkPopulateServiceObjectsInTreeView.Name = "chkPopulateServiceObjectsInTreeView";
            this.chkPopulateServiceObjectsInTreeView.Size = new System.Drawing.Size(410, 17);
            this.chkPopulateServiceObjectsInTreeView.TabIndex = 2;
            this.chkPopulateServiceObjectsInTreeView.Text = "Populate service objects";
            this.chkPopulateServiceObjectsInTreeView.UseVisualStyleBackColor = true;
            // 
            // chkPopulateControlsInTreeView
            // 
            this.chkPopulateControlsInTreeView.AutoSize = true;
            this.chkPopulateControlsInTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPopulateControlsInTreeView.Location = new System.Drawing.Point(3, 28);
            this.chkPopulateControlsInTreeView.Name = "chkPopulateControlsInTreeView";
            this.chkPopulateControlsInTreeView.Size = new System.Drawing.Size(410, 17);
            this.chkPopulateControlsInTreeView.TabIndex = 1;
            this.chkPopulateControlsInTreeView.Text = "Populate controls";
            this.chkPopulateControlsInTreeView.UseVisualStyleBackColor = true;
            // 
            // headerTreeView
            // 
            this.headerTreeView.AutoSize = true;
            this.headerTreeView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.headerTreeView.Closable = false;
            this.headerTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerTreeView.Location = new System.Drawing.Point(3, 3);
            this.headerTreeView.Name = "headerTreeView";
            this.headerTreeView.Size = new System.Drawing.Size(410, 25);
            this.headerTreeView.TabIndex = 0;
            this.headerTreeView.TabStop = false;
            this.headerTreeView.Text = "TreeView";
            // 
            // Options
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 321);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.flowButtons);
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.flowButtons.ResumeLayout(false);
            this.paddedPanel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private PaddedPanel paddedPanel1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpGeneral;
        private PaneHeader headerTreeView;
        private System.Windows.Forms.CheckBox chkPopulateControlsInTreeView;
        private System.Windows.Forms.CheckBox chkPopulateSmartMethodsAndPropertiesInTreeView;
        private System.Windows.Forms.CheckBox chkPopulateServiceObjectsInTreeView;
        private System.Windows.Forms.CheckBox chkPreserveInvalidCategoryEntriesInTreeView;
        private System.Windows.Forms.ToolTip toolTip;
    }
}