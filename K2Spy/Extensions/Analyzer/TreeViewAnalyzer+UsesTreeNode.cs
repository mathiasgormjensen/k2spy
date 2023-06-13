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
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private class UsesTreeNode : TreeNode, IAnalyzeTreeNode, IAnalysisCompletedTreeNode
        {
            #region Private Fields

            private System.Threading.CancellationTokenSource m_CancellationTokenSource;

            #endregion

            public UsesTreeNode(K2SpyTreeNode source)
                : base("Uses")
            {
                this.Source = source;
                this.SetImageKey("Analyze");
                source.Evicted += async (sender, e) =>
                {
                    if (this.AnalysisCompleted || base.IsExpanded)
                    {
                        base.Collapse();
                        base.Nodes.Clear();
                        base.Nodes.Add(new SearchingTreeNode());
                        this.AnalysisCompleted = false;
                    }
                };
                base.Nodes.Add(new SearchingTreeNode());
            }

            public static async Task<bool> IsSupportedAsync(K2SpyTreeNode node)
            {
                node = await node.GetActAsOrSelfAsync();
                return node is FormInfoTreeNode || node is ViewInfoTreeNode || node is SmartObjectInfoTreeNode || node is WorkflowTreeNode || node is SmartMethodTreeNode;
            }

            public K2SpyTreeNode Source { get; private set; }

            public bool AnalysisCompleted { get; set; } = false;

            public async Task AfterExpandAsync(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
                await AsyncExpander.ExpandAsync(this, k2SpyContext, this.m_CancellationTokenSource = new System.Threading.CancellationTokenSource(), this.PopulateChildNodes);
            }

            public void AfterCollapse(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
            }

            private async Task PopulateChildNodes(K2SpyContext k2SpyContext, System.Threading.CancellationTokenSource cancellationTokenSource, ReportProgressDelegate progressReporter, Func<TreeNode, Task> addNode)
            {
                K2SpyTreeNode source = await this.Source.GetActAsOrSelfAsync();

                UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, source);
                if (usesInformation != null)
                {
                    AsyncTreeNodePredicate considerChildrenPredicate = async node =>
                    {
                        if (node is ServiceTypesRootTreeNode || node is ServiceTypeTreeNode)
                            return true;
                        else if (node is ControlsRootTreeNode)
                            return true;
                        return await TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync(node);
                    };
                    AsyncTreeNodePredicate considerNodePredicate = async node =>
                    {
                        // don't consider the current node
                        if ((node as K2SpyTreeNode)?.GetActAsOrSelfAsync().Equals(this.Source.GetActAsOrSelfAsync()) == true)
                            return false;
                        if (node is ControlTypeInfoTreeNode)
                            return true;
                        if (node is K2SpyTreeNodeClone)
                            return true;
                        if (node is ServiceInstanceTreeNode)
                        {
                            return true;
                        }
                        return await TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync(node);
                    };
                    AsyncTreeNodePredicate isMatchAsyncPredicate = async node =>
                    {
                        if (node is K2SpyTreeNodeClone k2SpyTreeNodeClone)
                        {
                            node = await k2SpyTreeNodeClone.GetActAsAsync();
                        }
                        if (node is ControlTypeInfoTreeNode controlTypeInfoTreeNode)
                        {
                            if (Properties.Settings.Default.PopulateControlsInUsesAnalysis)
                            {
                                SourceCode.Forms.Management.ControlTypeInfo controlTypeInfo = await controlTypeInfoTreeNode.GetSourceAsync();
                                return usesInformation.ControlNames?.Contains(controlTypeInfo.Name) == true;
                            }
                        }
                        else if (node is CategoryDataTreeNode categoryDataTreeNode)
                        {
                            return usesInformation.Guids?.Contains(categoryDataTreeNode.DataAsGuid) == true || usesInformation.ProcessFullNames?.Contains(categoryDataTreeNode.Data) == true;
                        }
                        else if (node is ServiceInstanceTreeNode serviceInstanceTreeNode)
                        {
                            return usesInformation.Guids?.Contains(serviceInstanceTreeNode.ServiceInstanceGuid) == true;
                        }
                        else if (node is SmartMethodTreeNode smartMethodTreeNode)
                        {
                            if (Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInUsesAnalysis)
                            {
                                SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo = await smartMethodTreeNode.GetSourceAsync();
                                return usesInformation.SmartMethods?.Contains(new UsedSmartMethodInformation(smartMethodInfo.SmartObjectInfo.Guid, smartMethodInfo.Name)) == true;
                            }
                        }
                        else if (node is SmartPropertyTreeNode smartPropertyTreeNode)
                        {
                            if (Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInUsesAnalysis)
                            {
                                SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo = await smartPropertyTreeNode.GetSourceAsync();
                                return usesInformation.SmartProperties?.Contains(new UsedSmartPropertyInformation(smartPropertyInfo.SmartObjectInfo.Guid, smartPropertyInfo.Name)) == true;
                            }
                        }
                        return false;
                    };
                    // usesInformation.ControlNames = 
                    TreeViewSearchSettings treeViewSearchSettings = new TreeViewSearchSettings(considerChildrenPredicate, considerNodePredicate, isMatchAsyncPredicate);
                    await K2SpyTreeViewSearcher.PerformSearchAsync(k2SpyContext, treeViewSearchSettings, async match => await addNode(await AnalyzerTreeNode.CreateAsync(match)), progressReporter, cancellationTokenSource.Token);
                }
            }
        }
    }
}