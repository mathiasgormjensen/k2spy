using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.RefreshServiceInstance
{
    internal class RefreshServiceInstanceExtension : Model.ITreeViewContextMenuExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension
    {
        #region Public Properties

        public string DisplayName => "Refresh service instance";

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
                    new ToolStripMenuItem("Refresh service instance", null, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async () =>
                        {
                            SourceCode.SmartObjects.Services.Management.ServiceManagementServer serviceManagementServer = (await context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.SmartObjects.Services.Management.ServiceManagementServer>());
                            context.QueueSetStatus("Refreshing service instance");
                            DateTime begin = DateTime.Now;
                            await Task.Run(() =>
                            {
                                serviceManagementServer.RefreshServiceInstance(((ServiceInstanceTreeNode)actingAsOrSelfTreeNode).ServiceInstanceGuid);
                            });
                            TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                            context.QueueSetStatus($"Refreshed service instance in {timeSpan}", true);
                        }, () => context.QueueSetStatus(""));
                    })
                };
            }
            return null;
        }

        #endregion
    }
}
