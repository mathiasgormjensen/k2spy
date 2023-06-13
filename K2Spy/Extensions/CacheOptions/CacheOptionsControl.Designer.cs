namespace K2Spy.Extensions.CacheOptions
{
    partial class CacheOptionsControl
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
            this.flowCache = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearDiskCache = new System.Windows.Forms.Button();
            this.btnClearInmemoryCache = new System.Windows.Forms.Button();
            this.btnClearConnections = new System.Windows.Forms.Button();
            this.btnGC = new System.Windows.Forms.Button();
            this.lnkCacheInformation = new System.Windows.Forms.LinkLabel();
            this.chkKeepXPathDocumentsInMemory = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblOverallProgress = new System.Windows.Forms.Label();
            this.lblSpecificProgress = new System.Windows.Forms.Label();
            this.pbOverall = new System.Windows.Forms.ProgressBar();
            this.pbSpecific = new System.Windows.Forms.ProgressBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreload = new System.Windows.Forms.Button();
            this.btnStopPreload = new System.Windows.Forms.Button();
            this.flowCache.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowCache
            // 
            this.flowCache.AutoSize = true;
            this.flowCache.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowCache.Controls.Add(this.btnClearDiskCache);
            this.flowCache.Controls.Add(this.btnClearInmemoryCache);
            this.flowCache.Controls.Add(this.btnClearConnections);
            this.flowCache.Controls.Add(this.btnGC);
            this.flowCache.Location = new System.Drawing.Point(0, 23);
            this.flowCache.Margin = new System.Windows.Forms.Padding(0);
            this.flowCache.Name = "flowCache";
            this.flowCache.Size = new System.Drawing.Size(437, 29);
            this.flowCache.TabIndex = 11;
            // 
            // btnClearDiskCache
            // 
            this.btnClearDiskCache.AutoSize = true;
            this.btnClearDiskCache.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearDiskCache.Location = new System.Drawing.Point(3, 3);
            this.btnClearDiskCache.Name = "btnClearDiskCache";
            this.btnClearDiskCache.Size = new System.Drawing.Size(96, 23);
            this.btnClearDiskCache.TabIndex = 0;
            this.btnClearDiskCache.Text = "Clear disk cache";
            this.btnClearDiskCache.UseVisualStyleBackColor = true;
            this.btnClearDiskCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // btnClearInmemoryCache
            // 
            this.btnClearInmemoryCache.AutoSize = true;
            this.btnClearInmemoryCache.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearInmemoryCache.Location = new System.Drawing.Point(105, 3);
            this.btnClearInmemoryCache.Name = "btnClearInmemoryCache";
            this.btnClearInmemoryCache.Size = new System.Drawing.Size(124, 23);
            this.btnClearInmemoryCache.TabIndex = 1;
            this.btnClearInmemoryCache.Text = "Clear &in-memory cache";
            this.btnClearInmemoryCache.UseVisualStyleBackColor = true;
            this.btnClearInmemoryCache.Visible = false;
            this.btnClearInmemoryCache.Click += new System.EventHandler(this.btnClearInmemoryCache_Click);
            // 
            // btnClearConnections
            // 
            this.btnClearConnections.AutoSize = true;
            this.btnClearConnections.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearConnections.Location = new System.Drawing.Point(235, 3);
            this.btnClearConnections.Name = "btnClearConnections";
            this.btnClearConnections.Size = new System.Drawing.Size(102, 23);
            this.btnClearConnections.TabIndex = 2;
            this.btnClearConnections.Text = "Clear connections";
            this.btnClearConnections.UseVisualStyleBackColor = true;
            this.btnClearConnections.Visible = false;
            this.btnClearConnections.Click += new System.EventHandler(this.btnClearConnections_Click);
            // 
            // btnGC
            // 
            this.btnGC.AutoSize = true;
            this.btnGC.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGC.Location = new System.Drawing.Point(343, 3);
            this.btnGC.Name = "btnGC";
            this.btnGC.Size = new System.Drawing.Size(91, 23);
            this.btnGC.TabIndex = 3;
            this.btnGC.Text = "Collect garbage";
            this.btnGC.UseVisualStyleBackColor = true;
            this.btnGC.Visible = false;
            this.btnGC.Click += new System.EventHandler(this.btnGC_Click);
            // 
            // lnkCacheInformation
            // 
            this.lnkCacheInformation.AutoSize = true;
            this.lnkCacheInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkCacheInformation.LinkArea = new System.Windows.Forms.LinkArea(18, 38);
            this.lnkCacheInformation.Location = new System.Drawing.Point(3, 165);
            this.lnkCacheInformation.Name = "lnkCacheInformation";
            this.lnkCacheInformation.Size = new System.Drawing.Size(452, 17);
            this.lnkCacheInformation.TabIndex = 13;
            this.lnkCacheInformation.TabStop = true;
            this.lnkCacheInformation.Text = "Data is cached in %LOCALAPPDATA%\\K2Spy";
            this.lnkCacheInformation.UseCompatibleTextRendering = true;
            this.lnkCacheInformation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCacheInformation_LinkClicked);
            // 
            // chkKeepXPathDocumentsInMemory
            // 
            this.chkKeepXPathDocumentsInMemory.AutoSize = true;
            this.chkKeepXPathDocumentsInMemory.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkKeepXPathDocumentsInMemory.Location = new System.Drawing.Point(3, 3);
            this.chkKeepXPathDocumentsInMemory.Name = "chkKeepXPathDocumentsInMemory";
            this.chkKeepXPathDocumentsInMemory.Size = new System.Drawing.Size(452, 17);
            this.chkKeepXPathDocumentsInMemory.TabIndex = 14;
            this.chkKeepXPathDocumentsInMemory.Text = "Keep XPath documents in memory (increases performance and memory consumption)";
            this.chkKeepXPathDocumentsInMemory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chkKeepXPathDocumentsInMemory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lnkCacheInformation, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.flowCache, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOverallProgress, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblSpecificProgress, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.pbOverall, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.pbSpecific, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(458, 182);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // lblOverallProgress
            // 
            this.lblOverallProgress.AutoEllipsis = true;
            this.lblOverallProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverallProgress.Location = new System.Drawing.Point(3, 81);
            this.lblOverallProgress.Name = "lblOverallProgress";
            this.lblOverallProgress.Size = new System.Drawing.Size(452, 13);
            this.lblOverallProgress.TabIndex = 16;
            this.lblOverallProgress.Text = "Overall progress";
            this.lblOverallProgress.Visible = false;
            // 
            // lblSpecificProgress
            // 
            this.lblSpecificProgress.AutoEllipsis = true;
            this.lblSpecificProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSpecificProgress.Location = new System.Drawing.Point(3, 123);
            this.lblSpecificProgress.Name = "lblSpecificProgress";
            this.lblSpecificProgress.Size = new System.Drawing.Size(452, 13);
            this.lblSpecificProgress.TabIndex = 17;
            this.lblSpecificProgress.Text = "Specific progress";
            this.lblSpecificProgress.Visible = false;
            // 
            // pbOverall
            // 
            this.pbOverall.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbOverall.Location = new System.Drawing.Point(3, 97);
            this.pbOverall.Name = "pbOverall";
            this.pbOverall.Size = new System.Drawing.Size(452, 23);
            this.pbOverall.TabIndex = 18;
            this.pbOverall.Visible = false;
            // 
            // pbSpecific
            // 
            this.pbSpecific.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbSpecific.Location = new System.Drawing.Point(3, 139);
            this.pbSpecific.Name = "pbSpecific";
            this.pbSpecific.Size = new System.Drawing.Size(452, 23);
            this.pbSpecific.TabIndex = 19;
            this.pbSpecific.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnPreload);
            this.flowLayoutPanel1.Controls.Add(this.btnStopPreload);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 52);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(458, 29);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // btnPreload
            // 
            this.btnPreload.AutoSize = true;
            this.btnPreload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPreload.Location = new System.Drawing.Point(3, 3);
            this.btnPreload.Name = "btnPreload";
            this.btnPreload.Size = new System.Drawing.Size(86, 23);
            this.btnPreload.TabIndex = 15;
            this.btnPreload.Text = "&Preload cache";
            this.btnPreload.UseVisualStyleBackColor = true;
            this.btnPreload.Click += new System.EventHandler(this.btnPreload_Click);
            // 
            // btnStopPreload
            // 
            this.btnStopPreload.AutoSize = true;
            this.btnStopPreload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnStopPreload.Enabled = false;
            this.btnStopPreload.Image = global::K2Spy.Properties.Resources.Stop_16x;
            this.btnStopPreload.Location = new System.Drawing.Point(95, 3);
            this.btnStopPreload.Name = "btnStopPreload";
            this.btnStopPreload.Size = new System.Drawing.Size(93, 23);
            this.btnStopPreload.TabIndex = 16;
            this.btnStopPreload.Text = "&Stop preload";
            this.btnStopPreload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStopPreload.UseVisualStyleBackColor = true;
            this.btnStopPreload.Click += new System.EventHandler(this.btnStopPreload_Click);
            // 
            // CacheOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CacheOptionsControl";
            this.Size = new System.Drawing.Size(458, 353);
            this.flowCache.ResumeLayout(false);
            this.flowCache.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowCache;
        private System.Windows.Forms.Button btnClearDiskCache;
        private System.Windows.Forms.Button btnClearInmemoryCache;
        private System.Windows.Forms.Button btnClearConnections;
        private System.Windows.Forms.Button btnGC;
        private System.Windows.Forms.LinkLabel lnkCacheInformation;
        private System.Windows.Forms.CheckBox chkKeepXPathDocumentsInMemory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnPreload;
        private System.Windows.Forms.Label lblOverallProgress;
        private System.Windows.Forms.Label lblSpecificProgress;
        private System.Windows.Forms.ProgressBar pbOverall;
        private System.Windows.Forms.ProgressBar pbSpecific;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnStopPreload;
    }
}
