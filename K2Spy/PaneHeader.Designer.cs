namespace K2Spy
{
    partial class PaneHeader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneHeader));
            this.tsTop = new System.Windows.Forms.ToolStrip();
            this.tslblSearchHeader = new System.Windows.Forms.ToolStripLabel();
            this.tsbtnCloseSearchPanel = new System.Windows.Forms.ToolStripButton();
            this.tsTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsTop
            // 
            this.tsTop.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tsTop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblSearchHeader,
            this.tsbtnCloseSearchPanel});
            this.tsTop.Location = new System.Drawing.Point(0, 0);
            this.tsTop.Name = "tsTop";
            this.tsTop.Size = new System.Drawing.Size(381, 25);
            this.tsTop.TabIndex = 1;
            this.tsTop.Text = "toolStrip1";
            // 
            // tslblSearchHeader
            // 
            this.tslblSearchHeader.Name = "tslblSearchHeader";
            this.tslblSearchHeader.Size = new System.Drawing.Size(44, 22);
            this.tslblSearchHeader.Text = "Header";
            // 
            // tsbtnCloseSearchPanel
            // 
            this.tsbtnCloseSearchPanel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbtnCloseSearchPanel.AutoToolTip = false;
            this.tsbtnCloseSearchPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnCloseSearchPanel.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCloseSearchPanel.Image")));
            this.tsbtnCloseSearchPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCloseSearchPanel.Name = "tsbtnCloseSearchPanel";
            this.tsbtnCloseSearchPanel.Size = new System.Drawing.Size(23, 22);
            this.tsbtnCloseSearchPanel.Text = "x";
            this.tsbtnCloseSearchPanel.ToolTipText = "Close";
            this.tsbtnCloseSearchPanel.Click += new System.EventHandler(this.tsbtnCloseSearchPanel_Click);
            // 
            // PaneHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tsTop);
            this.Name = "PaneHeader";
            this.Size = new System.Drawing.Size(381, 150);
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsTop;
        private System.Windows.Forms.ToolStripLabel tslblSearchHeader;
        private System.Windows.Forms.ToolStripButton tsbtnCloseSearchPanel;
    }
}
