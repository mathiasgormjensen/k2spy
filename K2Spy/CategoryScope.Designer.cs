namespace K2Spy
{
    partial class CategoryScope
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkScope = new System.Windows.Forms.CheckBox();
            this.txtCategoryPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.chkScope, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCategoryPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(522, 29);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkScope
            // 
            this.chkScope.AutoSize = true;
            this.chkScope.Location = new System.Drawing.Point(3, 3);
            this.chkScope.Name = "chkScope";
            this.chkScope.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.chkScope.Size = new System.Drawing.Size(60, 19);
            this.chkScope.TabIndex = 0;
            this.chkScope.Text = "Scope:";
            this.chkScope.UseVisualStyleBackColor = true;
            this.chkScope.CheckedChanged += new System.EventHandler(this.chkScope_CheckedChanged);
            // 
            // txtCategoryPath
            // 
            this.txtCategoryPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCategoryPath.Enabled = false;
            this.txtCategoryPath.Location = new System.Drawing.Point(69, 3);
            this.txtCategoryPath.Name = "txtCategoryPath";
            this.txtCategoryPath.ReadOnly = true;
            this.txtCategoryPath.Size = new System.Drawing.Size(418, 20);
            this.txtCategoryPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.AutoSize = true;
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(493, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(26, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // CategoryScope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CategoryScope";
            this.Size = new System.Drawing.Size(522, 240);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkScope;
        private System.Windows.Forms.TextBox txtCategoryPath;
        private System.Windows.Forms.Button btnBrowse;
    }
}
