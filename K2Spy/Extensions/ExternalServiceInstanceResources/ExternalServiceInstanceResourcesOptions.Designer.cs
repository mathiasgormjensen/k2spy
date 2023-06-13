namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    partial class ExternalServiceInstanceResourcesOptions
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
            this.lblProperties = new System.Windows.Forms.Label();
            this.txtProperties = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblProperties, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtProperties, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblProperties
            // 
            this.lblProperties.AutoSize = true;
            this.lblProperties.Location = new System.Drawing.Point(3, 0);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Size = new System.Drawing.Size(57, 13);
            this.lblProperties.TabIndex = 0;
            this.lblProperties.Text = "Properties:";
            // 
            // txtProperties
            // 
            this.txtProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProperties.Location = new System.Drawing.Point(3, 16);
            this.txtProperties.Multiline = true;
            this.txtProperties.Name = "txtProperties";
            this.txtProperties.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProperties.Size = new System.Drawing.Size(144, 105);
            this.txtProperties.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Enabled = false;
            this.lblDescription.Location = new System.Drawing.Point(3, 124);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(107, 26);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "(separate compound properties using ; or |)";
            // 
            // ExternalServiceInstanceResourcesOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExternalServiceInstanceResourcesOptions";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblProperties;
        private System.Windows.Forms.TextBox txtProperties;
        private System.Windows.Forms.Label lblDescription;
    }
}
