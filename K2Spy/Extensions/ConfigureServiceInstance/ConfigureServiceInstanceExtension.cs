using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ConfigureServiceInstance
{
    [Model.IgnoreExtension]
    internal class ConfigureServiceInstanceExtension : Model.ITreeViewContextMenuExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension
    {
        #region Public Properties

        public string DisplayName => "Configure service instance";

        #endregion

        #region Public Methods

        public ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            if (actingAsOrSelfTreeNode is ServiceInstanceTreeNode)
            {
                return new ToolStripItem[] 
                {
                    new ToolStripMenuItem("Configure service instance", null, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async () =>
                        {
                            // TODO implement this
                            System.Windows.Forms.MessageBox.Show("Not yet implemented");
                            // throw new NotImplementedException();
                        });
                    })
                };
            }
            return null;
        }

        #endregion
    }
}
