using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public partial class SelectCategory : BaseForm
    {
        public SelectCategory(K2SpyContext k2SpyContext)
        {
            InitializeComponent();

            this.treeView1.ImageList = k2SpyContext.TreeView.ImageList;
            CategoryRootTreeNode root = k2SpyContext.TreeView.Nodes.OfType<CategoryRootTreeNode>().Single();
            this.treeView1.Nodes.Add(this.Clone(root));
            this.treeView1.Nodes[0].Expand();
        }

        [DefaultValue(false)]
        public bool AllowSelectRoot { get; set; } = false;

        [DefaultValue("")]
        public string SelectedCategory
        {
            get { return this.treeView1.SelectedNode?.FullPath; }
            set { this.treeView1.SelectedNode = this.treeView1.GetDescendants().FirstOrDefault(key => key.FullPath == value); }
        }

        private TreeNode Clone(TreeNode source)
        {
            TreeNode[] children = source.Nodes.OfType<CategoryTreeNode>().Select(key => this.Clone(key)).ToArray();
            TreeNode clone = new TreeNode(source.Text, children).With(key => key.SelectedImageKey = key.ImageKey = source.ImageKey);
            return clone;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.AllowSelectRoot)
                this.btnOK.Enabled = this.treeView1.SelectedNode != null;
            else
                this.btnOK.Enabled = this.treeView1.SelectedNode?.Parent != null;
        }
    }
}
