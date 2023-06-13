namespace K2Spy
{
    partial class EditConnection
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
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.groupPanel1 = new K2Spy.GroupPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblServerName = new System.Windows.Forms.Label();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.chkIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserPassword = new System.Windows.Forms.Label();
            this.lblWindowsDomain = new System.Windows.Forms.Label();
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserPassword = new System.Windows.Forms.TextBox();
            this.txtWindowsDomain = new System.Windows.Forms.TextBox();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.flowButtons.SuspendLayout();
            this.paddedPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
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
            this.flowButtons.Location = new System.Drawing.Point(0, 206);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(465, 29);
            this.flowButtons.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(387, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(306, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.Controls.Add(this.groupPanel1);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 0);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(465, 206);
            this.paddedPanel1.TabIndex = 0;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tableLayoutPanel);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 5);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(455, 196);
            this.groupPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.lblServerName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lblServerPort, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.chkIntegratedSecurity, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lblUserName, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblUserPassword, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.lblWindowsDomain, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.lblLabel, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.txtServerName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.txtServerPort, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.txtUserName, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.txtUserPassword, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.txtWindowsDomain, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.txtLabel, 1, 6);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(451, 191);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(3, 0);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblServerName.Size = new System.Drawing.Size(72, 19);
            this.lblServerName.TabIndex = 3;
            this.lblServerName.Text = "Server name:";
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(3, 28);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblServerPort.Size = new System.Drawing.Size(66, 19);
            this.lblServerPort.TabIndex = 5;
            this.lblServerPort.Text = "Server port:";
            // 
            // chkIntegratedSecurity
            // 
            this.chkIntegratedSecurity.AutoSize = true;
            this.chkIntegratedSecurity.Checked = true;
            this.chkIntegratedSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel.SetColumnSpan(this.chkIntegratedSecurity, 2);
            this.chkIntegratedSecurity.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkIntegratedSecurity.Location = new System.Drawing.Point(3, 59);
            this.chkIntegratedSecurity.Name = "chkIntegratedSecurity";
            this.chkIntegratedSecurity.Size = new System.Drawing.Size(445, 17);
            this.chkIntegratedSecurity.TabIndex = 7;
            this.chkIntegratedSecurity.Text = "Integrated security";
            this.chkIntegratedSecurity.UseVisualStyleBackColor = true;
            this.chkIntegratedSecurity.CheckedChanged += new System.EventHandler(this.chkIntegratedSecurity_CheckedChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Enabled = false;
            this.lblUserName.Location = new System.Drawing.Point(3, 79);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblUserName.Size = new System.Drawing.Size(64, 19);
            this.lblUserName.TabIndex = 8;
            this.lblUserName.Text = "User name:";
            // 
            // lblUserPassword
            // 
            this.lblUserPassword.AutoSize = true;
            this.lblUserPassword.Enabled = false;
            this.lblUserPassword.Location = new System.Drawing.Point(3, 107);
            this.lblUserPassword.Name = "lblUserPassword";
            this.lblUserPassword.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblUserPassword.Size = new System.Drawing.Size(86, 19);
            this.lblUserPassword.TabIndex = 10;
            this.lblUserPassword.Text = "User password:";
            // 
            // lblWindowsDomain
            // 
            this.lblWindowsDomain.AutoSize = true;
            this.lblWindowsDomain.Enabled = false;
            this.lblWindowsDomain.Location = new System.Drawing.Point(3, 135);
            this.lblWindowsDomain.Name = "lblWindowsDomain";
            this.lblWindowsDomain.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblWindowsDomain.Size = new System.Drawing.Size(101, 19);
            this.lblWindowsDomain.TabIndex = 12;
            this.lblWindowsDomain.Text = "Windows domain:";
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Enabled = false;
            this.lblLabel.Location = new System.Drawing.Point(3, 163);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblLabel.Size = new System.Drawing.Size(37, 19);
            this.lblLabel.TabIndex = 14;
            this.lblLabel.Text = "Label:";
            // 
            // txtServerName
            // 
            this.txtServerName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtServerName.Location = new System.Drawing.Point(110, 3);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(338, 22);
            this.txtServerName.TabIndex = 4;
            this.txtServerName.Text = "localhost";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtServerPort.Location = new System.Drawing.Point(110, 31);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(338, 22);
            this.txtServerPort.TabIndex = 6;
            this.txtServerPort.Text = "5555";
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(110, 82);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(338, 22);
            this.txtUserName.TabIndex = 9;
            this.txtUserName.Text = "Denallix\\Administrator";
            // 
            // txtUserPassword
            // 
            this.txtUserPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUserPassword.Enabled = false;
            this.txtUserPassword.Location = new System.Drawing.Point(110, 110);
            this.txtUserPassword.Name = "txtUserPassword";
            this.txtUserPassword.Size = new System.Drawing.Size(338, 22);
            this.txtUserPassword.TabIndex = 11;
            this.txtUserPassword.Text = "K2pass!";
            this.txtUserPassword.UseSystemPasswordChar = true;
            // 
            // txtWindowsDomain
            // 
            this.txtWindowsDomain.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWindowsDomain.Enabled = false;
            this.txtWindowsDomain.Location = new System.Drawing.Point(110, 138);
            this.txtWindowsDomain.Name = "txtWindowsDomain";
            this.txtWindowsDomain.Size = new System.Drawing.Size(338, 22);
            this.txtWindowsDomain.TabIndex = 13;
            // 
            // txtLabel
            // 
            this.txtLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLabel.Enabled = false;
            this.txtLabel.Location = new System.Drawing.Point(110, 166);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(338, 22);
            this.txtLabel.TabIndex = 15;
            // 
            // EditConnection
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(465, 235);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.flowButtons);
            this.Name = "EditConnection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit connection";
            this.flowButtons.ResumeLayout(false);
            this.paddedPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private PaddedPanel paddedPanel1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.CheckBox chkIntegratedSecurity;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserPassword;
        private System.Windows.Forms.Label lblWindowsDomain;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserPassword;
        private System.Windows.Forms.TextBox txtWindowsDomain;
        private System.Windows.Forms.TextBox txtLabel;
    }
}