namespace K2Spy.Plugins.DefinitionPane
{
    partial class DefinitionPaneControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefinitionPaneControl));
            this.pnlScintilla = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tableLoadingOverlay = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoadingStatus = new System.Windows.Forms.Label();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.PanelSearch = new System.Windows.Forms.Panel();
            this.BtnNextSearch = new System.Windows.Forms.Button();
            this.BtnPrevSearch = new System.Windows.Forms.Button();
            this.BtnCloseSearch = new System.Windows.Forms.Button();
            this.TxtSearch = new System.Windows.Forms.TextBox();
            this.pnlScintilla.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLoadingOverlay.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlScintilla
            // 
            this.pnlScintilla.BackColor = System.Drawing.Color.Ivory;
            this.pnlScintilla.Controls.Add(this.flowLayoutPanel1);
            this.pnlScintilla.Controls.Add(this.tableLoadingOverlay);
            this.pnlScintilla.Controls.Add(this.PanelSearch);
            this.pnlScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScintilla.Location = new System.Drawing.Point(0, 0);
            this.pnlScintilla.Name = "pnlScintilla";
            this.pnlScintilla.Size = new System.Drawing.Size(783, 507);
            this.pnlScintilla.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 477);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(341, 29);
            this.flowLayoutPanel1.TabIndex = 16;
            this.flowLayoutPanel1.Visible = false;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "XML";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Location = new System.Drawing.Point(47, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Null";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button3.Location = new System.Drawing.Point(91, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "C++";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button4.Location = new System.Drawing.Point(137, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(66, 23);
            this.button4.TabIndex = 16;
            this.button4.Text = "Load XML";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button5.Location = new System.Drawing.Point(209, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(59, 23);
            this.button5.TabIndex = 17;
            this.button5.Text = "Load C#";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button6.Location = new System.Drawing.Point(274, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(64, 23);
            this.button6.TabIndex = 18;
            this.button6.Text = "Load text";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tableLoadingOverlay
            // 
            this.tableLoadingOverlay.ColumnCount = 1;
            this.tableLoadingOverlay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.Controls.Add(this.lblLoadingStatus, 0, 0);
            this.tableLoadingOverlay.Controls.Add(this.pbLoading, 0, 1);
            this.tableLoadingOverlay.Location = new System.Drawing.Point(545, 372);
            this.tableLoadingOverlay.Name = "tableLoadingOverlay";
            this.tableLoadingOverlay.RowCount = 2;
            this.tableLoadingOverlay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.Size = new System.Drawing.Size(238, 135);
            this.tableLoadingOverlay.TabIndex = 12;
            // 
            // lblLoadingStatus
            // 
            this.lblLoadingStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblLoadingStatus.AutoSize = true;
            this.lblLoadingStatus.Location = new System.Drawing.Point(63, 54);
            this.lblLoadingStatus.Name = "lblLoadingStatus";
            this.lblLoadingStatus.Size = new System.Drawing.Size(112, 13);
            this.lblLoadingStatus.TabIndex = 0;
            this.lblLoadingStatus.Text = "Loading definition...";
            // 
            // pbLoading
            // 
            this.pbLoading.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbLoading.Location = new System.Drawing.Point(69, 70);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(100, 23);
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbLoading.TabIndex = 1;
            // 
            // PanelSearch
            // 
            this.PanelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelSearch.BackColor = System.Drawing.Color.White;
            this.PanelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSearch.Controls.Add(this.BtnNextSearch);
            this.PanelSearch.Controls.Add(this.BtnPrevSearch);
            this.PanelSearch.Controls.Add(this.BtnCloseSearch);
            this.PanelSearch.Controls.Add(this.TxtSearch);
            this.PanelSearch.Location = new System.Drawing.Point(491, 3);
            this.PanelSearch.Name = "PanelSearch";
            this.PanelSearch.Size = new System.Drawing.Size(292, 30);
            this.PanelSearch.TabIndex = 11;
            this.PanelSearch.Visible = false;
            // 
            // BtnNextSearch
            // 
            this.BtnNextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNextSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNextSearch.ForeColor = System.Drawing.Color.White;
            this.BtnNextSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnNextSearch.Image")));
            this.BtnNextSearch.Location = new System.Drawing.Point(233, 4);
            this.BtnNextSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnNextSearch.Name = "BtnNextSearch";
            this.BtnNextSearch.Size = new System.Drawing.Size(25, 20);
            this.BtnNextSearch.TabIndex = 9;
            this.BtnNextSearch.Tag = "Find next (Enter)";
            this.BtnNextSearch.UseVisualStyleBackColor = true;
            this.BtnNextSearch.Click += new System.EventHandler(this.BtnNextSearch_Click);
            // 
            // BtnPrevSearch
            // 
            this.BtnPrevSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPrevSearch.ForeColor = System.Drawing.Color.White;
            this.BtnPrevSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnPrevSearch.Image")));
            this.BtnPrevSearch.Location = new System.Drawing.Point(205, 4);
            this.BtnPrevSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnPrevSearch.Name = "BtnPrevSearch";
            this.BtnPrevSearch.Size = new System.Drawing.Size(25, 20);
            this.BtnPrevSearch.TabIndex = 8;
            this.BtnPrevSearch.Tag = "Find previous (Shift+Enter)";
            this.BtnPrevSearch.UseVisualStyleBackColor = true;
            this.BtnPrevSearch.Click += new System.EventHandler(this.BtnPrevSearch_Click);
            // 
            // BtnCloseSearch
            // 
            this.BtnCloseSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCloseSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCloseSearch.ForeColor = System.Drawing.Color.White;
            this.BtnCloseSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnCloseSearch.Image")));
            this.BtnCloseSearch.Location = new System.Drawing.Point(261, 4);
            this.BtnCloseSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCloseSearch.Name = "BtnCloseSearch";
            this.BtnCloseSearch.Size = new System.Drawing.Size(25, 20);
            this.BtnCloseSearch.TabIndex = 7;
            this.BtnCloseSearch.Tag = "Close (Esc)";
            this.BtnCloseSearch.UseVisualStyleBackColor = true;
            this.BtnCloseSearch.Click += new System.EventHandler(this.BtnClearSearch_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSearch.Location = new System.Drawing.Point(10, 6);
            this.TxtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(189, 16);
            this.TxtSearch.TabIndex = 6;
            this.TxtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // DefinitionPaneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScintilla);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DefinitionPaneControl";
            this.Size = new System.Drawing.Size(783, 507);
            this.pnlScintilla.ResumeLayout(false);
            this.pnlScintilla.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLoadingOverlay.ResumeLayout(false);
            this.tableLoadingOverlay.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            this.PanelSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlScintilla;
        private System.Windows.Forms.Panel PanelSearch;
        private System.Windows.Forms.Button BtnNextSearch;
        private System.Windows.Forms.Button BtnPrevSearch;
        private System.Windows.Forms.Button BtnCloseSearch;
        private System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.TableLayoutPanel tableLoadingOverlay;
        private System.Windows.Forms.Label lblLoadingStatus;
        private System.Windows.Forms.ProgressBar pbLoading;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}

