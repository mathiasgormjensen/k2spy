namespace K2Spy.Extensions.ExecuteSmartObject
{
    partial class EditHyperlink
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
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupPanel1 = new K2Spy.GroupPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.flowButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.flowButtons.Location = new System.Drawing.Point(0, 70);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(448, 29);
            this.flowButtons.TabIndex = 1;
            this.flowButtons.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(370, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(289, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(448, 70);
            this.panel1.TabIndex = 0;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 5);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(438, 60);
            this.groupPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblDisplayName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLink, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDisplayName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLink, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(434, 56);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(3, 0);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblDisplayName.Size = new System.Drawing.Size(78, 19);
            this.lblDisplayName.TabIndex = 0;
            this.lblDisplayName.Text = "Display name:";
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(3, 28);
            this.lblLink.Name = "lblLink";
            this.lblLink.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblLink.Size = new System.Drawing.Size(31, 19);
            this.lblLink.TabIndex = 1;
            this.lblLink.Text = "Link:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDisplayName.Location = new System.Drawing.Point(87, 3);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(344, 22);
            this.txtDisplayName.TabIndex = 2;
            // 
            // txtLink
            // 
            this.txtLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLink.Location = new System.Drawing.Point(87, 31);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(344, 22);
            this.txtLink.TabIndex = 3;
            // 
            // EditHyperlink
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(448, 99);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowButtons);
            this.MaximizeBox = false;
            this.Name = "EditHyperlink";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Hyperlink";
            this.flowButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label lblLink;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.TextBox txtLink;
    }
}