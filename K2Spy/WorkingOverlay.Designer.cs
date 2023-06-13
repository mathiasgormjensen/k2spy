namespace K2Spy
{
    partial class WorkingOverlay
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
            this.tableLoadingOverlay = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoadingStatus = new System.Windows.Forms.Label();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.tableLoadingOverlay.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLoadingOverlay
            // 
            this.tableLoadingOverlay.ColumnCount = 1;
            this.tableLoadingOverlay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.Controls.Add(this.lblLoadingStatus, 0, 0);
            this.tableLoadingOverlay.Controls.Add(this.pbLoading, 0, 1);
            this.tableLoadingOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLoadingOverlay.Location = new System.Drawing.Point(0, 0);
            this.tableLoadingOverlay.Name = "tableLoadingOverlay";
            this.tableLoadingOverlay.RowCount = 2;
            this.tableLoadingOverlay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoadingOverlay.Size = new System.Drawing.Size(379, 200);
            this.tableLoadingOverlay.TabIndex = 13;
            // 
            // lblLoadingStatus
            // 
            this.lblLoadingStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblLoadingStatus.AutoSize = true;
            this.lblLoadingStatus.Location = new System.Drawing.Point(162, 87);
            this.lblLoadingStatus.Name = "lblLoadingStatus";
            this.lblLoadingStatus.Size = new System.Drawing.Size(54, 13);
            this.lblLoadingStatus.TabIndex = 0;
            this.lblLoadingStatus.Text = "Loading...";
            // 
            // pbLoading
            // 
            this.pbLoading.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbLoading.Location = new System.Drawing.Point(139, 103);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(100, 23);
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbLoading.TabIndex = 1;
            // 
            // LoadingOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLoadingOverlay);
            this.Name = "LoadingOverlay";
            this.Size = new System.Drawing.Size(379, 200);
            this.tableLoadingOverlay.ResumeLayout(false);
            this.tableLoadingOverlay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLoadingOverlay;
        private System.Windows.Forms.Label lblLoadingStatus;
        private System.Windows.Forms.ProgressBar pbLoading;
    }
}
