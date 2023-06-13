namespace K2Spy.Extensions.Analyzer
{
    partial class AnalyzerOptionsControl
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
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis = new System.Windows.Forms.CheckBox();
            this.chkPopulateControlsInUsesAnalysis = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkPopulateSmartMethodsAndPropertiesInUsesAnalysis
            // 
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.AutoEllipsis = true;
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.AutoSize = true;
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Location = new System.Drawing.Point(0, 17);
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Name = "chkPopulateSmartMethodsAndPropertiesInUsesAnalysis";
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Size = new System.Drawing.Size(284, 17);
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.TabIndex = 9;
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Text = "Populate SmartObject methods and properties";
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.UseVisualStyleBackColor = true;
            // 
            // chkPopulateControlsInUsesAnalysis
            // 
            this.chkPopulateControlsInUsesAnalysis.AutoEllipsis = true;
            this.chkPopulateControlsInUsesAnalysis.AutoSize = true;
            this.chkPopulateControlsInUsesAnalysis.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkPopulateControlsInUsesAnalysis.Location = new System.Drawing.Point(0, 0);
            this.chkPopulateControlsInUsesAnalysis.Name = "chkPopulateControlsInUsesAnalysis";
            this.chkPopulateControlsInUsesAnalysis.Size = new System.Drawing.Size(284, 17);
            this.chkPopulateControlsInUsesAnalysis.TabIndex = 8;
            this.chkPopulateControlsInUsesAnalysis.Text = "Populate controls";
            this.chkPopulateControlsInUsesAnalysis.UseVisualStyleBackColor = true;
            // 
            // AnalyzerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis);
            this.Controls.Add(this.chkPopulateControlsInUsesAnalysis);
            this.Name = "AnalyzerOptionsControl";
            this.Size = new System.Drawing.Size(284, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPopulateSmartMethodsAndPropertiesInUsesAnalysis;
        private System.Windows.Forms.CheckBox chkPopulateControlsInUsesAnalysis;
    }
}
