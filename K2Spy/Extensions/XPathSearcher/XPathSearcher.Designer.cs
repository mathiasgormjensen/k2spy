namespace K2Spy.Extensions.XPathSearcher
{
    partial class XPathSearcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XPathSearcher));
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.categoryScope1 = new K2Spy.CategoryScope();
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.groupPanel1 = new K2Spy.GroupPanel();
            this.progressBarLabel1 = new K2Spy.ProgressBarLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.paddedPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtXPath
            // 
            this.txtXPath.AcceptsReturn = true;
            this.txtXPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXPath.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXPath.Location = new System.Drawing.Point(3, 3);
            this.txtXPath.Multiline = true;
            this.txtXPath.Name = "txtXPath";
            this.tableLayoutPanel1.SetRowSpan(this.txtXPath, 2);
            this.txtXPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtXPath.Size = new System.Drawing.Size(552, 99);
            this.txtXPath.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.Image = global::K2Spy.Properties.Resources.Run_blue_16x;
            this.btnSearch.Location = new System.Drawing.Point(561, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 70);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "&Search (F5)";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmResult,
            this.clmPosition,
            this.clmPath});
            this.tableLayoutPanel1.SetColumnSpan(this.listView1, 2);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 137);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(646, 284);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 130;
            // 
            // clmResult
            // 
            this.clmResult.Text = "Result";
            this.clmResult.Width = 200;
            // 
            // clmPosition
            // 
            this.clmPosition.Text = "Position";
            this.clmPosition.Width = 70;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 200;
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = true;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.Enabled = false;
            this.btnStop.Image = global::K2Spy.Properties.Resources.Stop_16x;
            this.btnStop.Location = new System.Drawing.Point(561, 79);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(88, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop (ESC)";
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtXPath, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnStop, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.categoryScope1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(652, 424);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // categoryScope1
            // 
            this.categoryScope1.AutoSize = true;
            this.categoryScope1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.categoryScope1, 2);
            this.categoryScope1.Dock = System.Windows.Forms.DockStyle.Top;
            this.categoryScope1.Location = new System.Drawing.Point(0, 105);
            this.categoryScope1.Margin = new System.Windows.Forms.Padding(0);
            this.categoryScope1.Name = "categoryScope1";
            this.categoryScope1.Size = new System.Drawing.Size(652, 29);
            this.categoryScope1.TabIndex = 5;
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.CollapseBottom = true;
            this.paddedPanel1.Controls.Add(this.groupPanel1);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 0);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(666, 433);
            this.paddedPanel1.TabIndex = 0;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(5, 5);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(656, 428);
            this.groupPanel1.TabIndex = 0;
            // 
            // progressBarLabel1
            // 
            this.progressBarLabel1.AutoSize = true;
            this.progressBarLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.progressBarLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarLabel1.Location = new System.Drawing.Point(0, 433);
            this.progressBarLabel1.Name = "progressBarLabel1";
            this.progressBarLabel1.Size = new System.Drawing.Size(666, 20);
            this.progressBarLabel1.TabIndex = 6;
            // 
            // XPathSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 453);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.progressBarLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "XPathSearcher";
            this.Text = "XPath Search";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.paddedPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmResult;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ColumnHeader clmPath;
        private System.Windows.Forms.ColumnHeader clmPosition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private PaddedPanel paddedPanel1;
        private GroupPanel groupPanel1;
        private ProgressBarLabel progressBarLabel1;
        private CategoryScope categoryScope1;
    }
}