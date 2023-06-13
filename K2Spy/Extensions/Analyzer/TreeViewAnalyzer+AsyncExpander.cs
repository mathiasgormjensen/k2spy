using K2Spy.ExtensionMethods;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private class AsyncExpander
        {
            internal async static Task ExpandAsync<T>(T source, K2SpyContext k2SpyContext, System.Threading.CancellationTokenSource cancellationTokenSource, ExpandAsync callback)
                where T : TreeNode, IAnalysisCompletedTreeNode
            {
                object setPercentageActionKey = new object();
                if (!source.AnalysisCompleted)
                {
                    SearchingTreeNode searchingTreeNode = null;
                    using (TreeViewUpdateContext.CreateIfNecessary(source))
                    {
                        if (!(source.Nodes.OfType<TreeNode>().FirstOrDefault() is SearchingTreeNode))
                            source.Nodes.Insert(0, new SearchingTreeNode());
                        searchingTreeNode = (SearchingTreeNode)source.Nodes[0];

                        while (source.Nodes.Count > 1)
                            source.Nodes[1].Remove();
                    }

                    ReportProgressDelegate reportProgcess = (status, percentage) =>
                    {
                        k2SpyContext.QueueSetStatus(status, percentage == 0);
                        k2SpyContext.ActionQueue.QueueOnce(setPercentageActionKey, () =>
                        {
                            searchingTreeNode.SetPercentage(percentage);
                        });
                    };
                    await callback.Invoke(k2SpyContext, cancellationTokenSource, reportProgcess, async newNode =>
                    {
                        //System.Threading.Thread.Sleep(250);
                        k2SpyContext.ActionQueue.Queue(() =>
                        {
                            if (!cancellationTokenSource.IsCancellationRequested)
                            {
                                using (new LayoutSuspender(source.TreeView))
                                {
                                    List<TreeNode> list = source.Nodes.OfType<TreeNode>().Where(key => !(key is SearchingTreeNode)).ToList();
                                    list.Add(newNode);
                                    int offset = source.Nodes.OfType<TreeNode>().TakeWhile(key => !(key is AnalyzerTreeNode)).Count();
                                    list = list.OrderBy(key => key.Text).ToList();
                                    int index = list.IndexOf(newNode) + offset;
                                    source.Nodes.Insert(index, newNode);
                                }
                            }
                        });
                    });

                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        //source.AnalysisCompleted = true;
                        k2SpyContext.ActionQueue.Queue(() =>
                        {
                            if (!cancellationTokenSource.IsCancellationRequested)
                            {
                                SearchingTreeNode[] nodesToRemove = source.Nodes.OfType<SearchingTreeNode>().ToArray();
                                nodesToRemove.ToList().ForEach(key => key.Remove());
                                source.CollapseIfEmptyAndExpanded();
                            }
                        });
                    }
                }
            }
        }
    }
}
