using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private class ServiceInstanceSmartObjectsTreeNode : TreeNode, IAnalyzeTreeNode, IAnalysisCompletedTreeNode
        {
            private System.Threading.CancellationTokenSource m_CancellationTokenSource;

            public ServiceInstanceSmartObjectsTreeNode(K2SpyTreeNode source)
            {
                base.Text = "SmartObjects";
                this.SetImageKey("Analyze");
                this.Source = (ServiceInstanceTreeNode)source;
                base.Nodes.Add(new SearchingTreeNode());
            }

            public bool AnalysisCompleted { get; set; } = false;

            public static bool IsSupported(K2SpyTreeNode source)
            {
                return source is ServiceInstanceTreeNode;
            }

            protected ServiceInstanceTreeNode Source { get; private set; }


            public void AfterCollapse(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
            }

            public async Task AfterExpandAsync(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
                await AsyncExpander.ExpandAsync(this, k2SpyContext, this.m_CancellationTokenSource = new System.Threading.CancellationTokenSource(), this.PopulateChildNodes);
                
            }

            private async Task PopulateChildNodes(K2SpyContext k2SpyContext, System.Threading.CancellationTokenSource cancellationTokenSource, ReportProgressDelegate progressReporter, Func<TreeNode, Task> addNode)
            {
                Guid[] guids = await Task.Run(() => k2SpyContext.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>().GetSmartObjects(this.Source.ServiceInstanceGuid).SmartObjectList.OfType<SourceCode.SmartObjects.Management.SmartObjectInfo>().Select(key => key.Guid).ToArray());
                if (guids.Length == 0)
                {
                    this.AnalysisCompleted = true;
                    base.Nodes.Clear();
                }
                else
                {
                    TreeViewSearchSettings treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataSmartObjectNodeAsync, async node =>
                    {
                        if (node is CategoryDataTreeNode)
                        {
                            return guids.Contains(((CategoryDataTreeNode)node).DataAsGuid);
                        }
                        return false;
                    });

                    await K2SpyTreeViewSearcher.PerformSearchAsync(k2SpyContext, treeViewSearchSettings, async match => await addNode(await AnalyzerTreeNode.CreateAsync(match)), progressReporter.Invoke, cancellationTokenSource.Token);
                }
            }
        }
    }
}
