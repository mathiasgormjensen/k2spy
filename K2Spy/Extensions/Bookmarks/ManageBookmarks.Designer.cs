namespace K2Spy.Extensions.Bookmarks
{
    partial class ManageBookmarks
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
            this.lbBookmarks = new System.Windows.Forms.ListBox();
            this.tableOuter = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnHiddenClose = new System.Windows.Forms.Button();
            this.tableOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbBookmarks
            // 
            this.lbBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBookmarks.FormattingEnabled = true;
            this.lbBookmarks.Location = new System.Drawing.Point(3, 3);
            this.lbBookmarks.Name = "lbBookmarks";
            this.tableOuter.SetRowSpan(this.lbBookmarks, 4);
            this.lbBookmarks.Size = new System.Drawing.Size(478, 210);
            this.lbBookmarks.TabIndex = 0;
            this.lbBookmarks.SelectedIndexChanged += new System.EventHandler(this.lbBookmarks_SelectedIndexChanged);
            this.lbBookmarks.DoubleClick += new System.EventHandler(this.lbBookmarks_DoubleClick);
            // 
            // tableOuter
            // 
            this.tableOuter.ColumnCount = 2;
            this.tableOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableOuter.Controls.Add(this.lbBookmarks, 0, 0);
            this.tableOuter.Controls.Add(this.btnAdd, 1, 0);
            this.tableOuter.Controls.Add(this.btnGoTo, 1, 1);
            this.tableOuter.Controls.Add(this.btnRemove, 1, 2);
            this.tableOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableOuter.Location = new System.Drawing.Point(0, 0);
            this.tableOuter.Name = "tableOuter";
            this.tableOuter.RowCount = 4;
            this.tableOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableOuter.Size = new System.Drawing.Size(547, 216);
            this.tableOuter.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(487, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnGoTo
            // 
            this.btnGoTo.AutoSize = true;
            this.btnGoTo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGoTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGoTo.Enabled = false;
            this.btnGoTo.Location = new System.Drawing.Point(487, 32);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(57, 23);
            this.btnGoTo.TabIndex = 2;
            this.btnGoTo.Text = "&Go to";
            this.btnGoTo.UseVisualStyleBackColor = true;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.AutoSize = true;
            this.btnRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(487, 61);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(57, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnHiddenClose
            // 
            this.btnHiddenClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHiddenClose.Location = new System.Drawing.Point(234, 15);
            this.btnHiddenClose.Name = "btnHiddenClose";
            this.btnHiddenClose.Size = new System.Drawing.Size(75, 23);
            this.btnHiddenClose.TabIndex = 3;
            this.btnHiddenClose.Text = "&Close";
            this.btnHiddenClose.UseVisualStyleBackColor = true;
            // 
            // ManageBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHiddenClose;
            this.ClientSize = new System.Drawing.Size(547, 216);
            this.Controls.Add(this.tableOuter);
            this.Controls.Add(this.btnHiddenClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageBookmarks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bookmarks";
            this.tableOuter.ResumeLayout(false);
            this.tableOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbBookmarks;
        private System.Windows.Forms.TableLayoutPanel tableOuter;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnHiddenClose;
        private System.Windows.Forms.Button btnAdd;
    }
}