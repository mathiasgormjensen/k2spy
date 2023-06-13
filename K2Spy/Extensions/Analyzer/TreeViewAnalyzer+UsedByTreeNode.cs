using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;
using K2Spy.Model;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private class UsedByTreeNode : TreeNode, IAnalyzeTreeNode, IAnalysisCompletedTreeNode
        {
            private System.Threading.CancellationTokenSource m_CancellationTokenSource;

            public UsedByTreeNode(K2SpyTreeNode source)
                : base("Used By")
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

            public K2SpyTreeNode Source { get; private set; }

            public bool AnalysisCompleted { get; set; } = false;

            public static async Task<bool> IsSupportedAsync(K2SpyTreeNode node)
            {
                node = await node.GetActAsOrSelfAsync();
                return node is FormInfoTreeNode || node is ViewInfoTreeNode || node is SmartObjectInfoTreeNode || node is WorkflowTreeNode || node is ControlTypeInfoTreeNode
                     || node is SmartMethodTreeNode
                      || node is SmartPropertyTreeNode
                      || node is StyleProfileInfoTreeNode;
            }

            public async Task AfterExpandAsync(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
                await AsyncExpander.ExpandAsync(this, k2SpyContext, this.m_CancellationTokenSource = new System.Threading.CancellationTokenSource(), this.PopulateChildNodes);
            }

            public void AfterCollapse(K2SpyContext k2SpyContext)
            {
                this.m_CancellationTokenSource?.Cancel();
            }

            #region Private Methods

            private AsyncTreeNodePredicate BuildGuidPredicate(K2SpyContext k2SpyContext, Guid guid)
            {
                return async node =>
                {
                    UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node as K2SpyTreeNode);
                    return usesInformation?.Guids.Contains(guid) == true;
                };
            }

            private AsyncTreeNodePredicate BuildSmartMethodPredicate(K2SpyContext k2SpyContext, Guid smartObjectGuid, string methodName)
            {
                return async node =>
                {
                    UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node as K2SpyTreeNode);
                    if (usesInformation?.SmartMethods?.Any(key => key.SmartObjectGuid == smartObjectGuid && key.MethodName == methodName) == true)
                    {
                        return true;
                    }
                    return false;
                };
            }

            private AsyncTreeNodePredicate BuildSmartPropertyPredicate(K2SpyContext k2SpyContext, Guid smartObjectGuid, string propertyName)
            {
                return async node =>
                {
                    UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node as K2SpyTreeNode);
                    if (usesInformation?.SmartProperties?.Any(key => key.SmartObjectGuid == smartObjectGuid && key.PropertyName == propertyName) == true)
                    {
                        return true;
                    }
                    return false;
                };
            }

            private AsyncTreeNodePredicate BuildProcessFullNamePredicate(K2SpyContext k2SpyContext, string processFullName)
            {
                return async node =>
                {
                    if (node is K2SpyTreeNode node2)
                    {
                        node2 = await node2.GetActAsOrSelfAsync();
                        UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node as K2SpyTreeNode);
                        if (usesInformation?.ProcessFullNames.Contains(processFullName) == true)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }

            private AsyncTreeNodePredicate BuildControlNamePredicate(K2SpyContext k2SpyContext, string controlName)
            {
                return async node =>
                {
                    UsesInformation usesInformation = await UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node as K2SpyTreeNode);
                    if (usesInformation?.ControlNames?.Contains(controlName) == true)
                    {
                        return true;
                    }
                    return false;
                };
            }

            private async Task PopulateChildNodes(K2SpyContext context, System.Threading.CancellationTokenSource cancellationTokenSource, ReportProgressDelegate progressReporter, Func<TreeNode, Task> addNode)
            {
                TreeViewSearchSettings treeViewSearchSettings = null;
                K2SpyTreeNode source = await this.Source.GetActAsOrSelfAsync();
                AsyncTreeNodePredicate isMatch = async node => false;
                if (source is FormInfoTreeNode formInfoTreeNode)
                {
                    Guid guid = formInfoTreeNode.FormGuid;
                    isMatch = this.BuildGuidPredicate(context, guid);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                else if (source is ViewInfoTreeNode viewInfoTreeNode)
                {
                    Guid guid = viewInfoTreeNode.ViewGuid;
                    isMatch = this.BuildGuidPredicate(context, guid);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                else if (source is StyleProfileInfoTreeNode styleProfileInfoTreeNode)
                {
                    Guid guid = styleProfileInfoTreeNode.CacheKey;
                    isMatch = this.BuildGuidPredicate(context, guid);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormNodeAsync, isMatch, this.Source);
                }
                else if (source is SmartObjectInfoTreeNode smartObjectInfoTreeNode)
                {
                    Guid guid = smartObjectInfoTreeNode.SmartObjectGuid;
                    isMatch = this.BuildGuidPredicate(context, guid);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                else if (source is ControlTypeInfoTreeNode controlTypeInfoTreeNode)
                {
                    SourceCode.Forms.Management.ControlTypeInfo controlTypeInfo = await controlTypeInfoTreeNode.GetSourceAsync();
                    string controlTypeName = controlTypeInfo.Name;
                    isMatch = this.BuildControlNamePredicate(context, controlTypeName);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormOrViewNodeAsync, isMatch, this.Source);
                }
                else if (source is WorkflowTreeNode processSetTreeNode)
                {
                    SourceCode.Workflow.Management.ProcessSet processSet = await processSetTreeNode.GetSourceAsync();
                    string fullName = processSet.FullName;
                    isMatch = this.BuildProcessFullNamePredicate(context, fullName);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeOrCategoryDataTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                else if (source is SmartMethodTreeNode smartMethodTreeNode)
                {
                    SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo = await smartMethodTreeNode.GetSourceAsync();
                    isMatch = this.BuildSmartMethodPredicate(context, smartMethodInfo.SmartObjectInfo.Guid, smartMethodInfo.Name);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                else if (source is SmartPropertyTreeNode smartPropertyTreeNode)
                {
                    SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo = await smartPropertyTreeNode.GetSourceAsync();
                    isMatch = this.BuildSmartPropertyPredicate(context, smartPropertyInfo.SmartObjectInfo.Guid, smartPropertyInfo.Name);
                    treeViewSearchSettings = new TreeViewSearchSettings(TreeNodePredicates.IsCategoryTreeNodeAsync, TreeNodePredicates.IsCategoryDataFormViewOrWorkflowNodeAsync, isMatch, this.Source);
                }
                await K2SpyTreeViewSearcher.PerformSearchAsync(context, treeViewSearchSettings, async match => await addNode(await AnalyzerTreeNode.CreateAsync(match)), progressReporter.Invoke, cancellationTokenSource.Token);
            }

            #endregion
        }
    }
}