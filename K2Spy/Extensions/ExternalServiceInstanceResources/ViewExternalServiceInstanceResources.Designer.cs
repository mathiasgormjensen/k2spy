namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    partial class ViewExternalServiceInstanceResources
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewExternalServiceInstanceResources));
            this.paddedPanel1 = new K2Spy.PaddedPanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmResource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBarLabel1 = new K2Spy.ProgressBarLabel();
            this.paddedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // paddedPanel1
            // 
            this.paddedPanel1.Controls.Add(this.listView1);
            this.paddedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paddedPanel1.Location = new System.Drawing.Point(0, 0);
            this.paddedPanel1.Name = "paddedPanel1";
            this.paddedPanel1.Size = new System.Drawing.Size(800, 430);
            this.paddedPanel1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmResource,
            this.clmPath});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(5, 5);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(790, 420);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 150;
            // 
            // clmResource
            // 
            this.clmResource.Text = "Resource";
            this.clmResource.Width = 300;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 300;
            // 
            // progressBarLabel1
            // 
            this.progressBarLabel1.AutoSize = true;
            this.progressBarLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.progressBarLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarLabel1.Location = new System.Drawing.Point(0, 430);
            this.progressBarLabel1.Name = "progressBarLabel1";
            this.progressBarLabel1.Size = new System.Drawing.Size(800, 20);
            this.progressBarLabel1.TabIndex = 1;
            // 
            // ViewExternalServiceInstanceResources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.paddedPanel1);
            this.Controls.Add(this.progressBarLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewExternalServiceInstanceResources";
            this.Text = "External service instance resources";
            this.paddedPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PaddedPanel paddedPanel1;
        private System.Windows.Forms.ListView listView1;
        private ProgressBarLabel progressBarLabel1;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmResource;
        private System.Windows.Forms.ColumnHeader clmPath;
    }
}