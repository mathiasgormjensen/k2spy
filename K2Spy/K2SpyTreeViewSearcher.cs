using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public static class K2SpyTreeViewSearcher
    {
        #region Public Methods

        [Obsolete("", true)]
        public static async Task PerformSearchAsync(K2SpyContext k2SpyContext, TreeViewSearchSettings searchSettings, Action<K2SpyTreeNode> matchFoundCallback, ReportProgressDelegate reportProgress, System.Threading.CancellationToken cancellationToken)
        {
            DateTime begin = DateTime.Now;
            TreeNode[] searchCandidates = await Task.Run(() =>
            {
                reportProgress($"Locating candidates...", 0);
                TreeNodeCollection nodes = searchSettings.ScopeRoot != null ? searchSettings.ScopeRoot.Nodes : k2SpyContext.TreeView.Nodes;
                return K2SpyTreeViewSearcher.GetSearchCandidatesAsync(k2SpyContext.TreeView, nodes, searchSettings.ConsiderChildrenAsyncPredicate, searchSettings.ConsiderNodeAsyncPredicate, searchSettings.Ignores, cancellationToken);
            });

            int count = 0;
#if false
            await Task.Run(async () =>
            {
                foreach (TreeNode node in searchCandidates)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    count++;
                    int percentage = Math.Min(100, Math.Max(0, (count * 100) / searchCandidates.Length));
                    reportProgress($"Searching {node.FullPath} ({percentage}%)...", percentage);
                    bool isMatch = await searchSettings.IsMatchAsyncPredicate(node);

                    if (isMatch)
                    {
                        matchFoundCallback((K2SpyTreeNode)node);
                    }
                }
            });
#else
            await Task.Run(() =>
            {
                foreach (TreeNode node in searchCandidates)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    count++;
                    int percentage = Math.Min(100, Math.Max(0, (count * 100) / searchCandidates.Length));
                    reportProgress($"Searching {node.FullPath} ({percentage}%)...", percentage);
                    bool isMatch = searchSettings.IsMatchAsyncPredicate(node).GetAwaiter().GetResult();

                    if (isMatch)
                    {
                        matchFoundCallback((K2SpyTreeNode)node);
                    }
                }
            });
#endif

            reportProgress("", 0);
            if (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                reportProgress($"Search completed in {timeSpan}", 0);
            }
        }

        public static async Task PerformSearchAsync(K2SpyContext k2SpyContext, TreeViewSearchSettings searchSettings, Func<K2SpyTreeNode, Task> matchFoundCallback, ReportProgressDelegate reportProgress, System.Threading.CancellationToken cancellationToken)
        {
            DateTime begin = DateTime.Now;
            reportProgress($"Locating candidates...", 0);
            TreeNodeCollection nodes = searchSettings.ScopeRoot != null ? searchSettings.ScopeRoot.Nodes : k2SpyContext.TreeView.Nodes;
            Console.WriteLine("Before locate candidates");
            TreeNode[] searchCandidates = await K2SpyTreeViewSearcher.GetSearchCandidatesAsync(k2SpyContext.TreeView, nodes, searchSettings.ConsiderChildrenAsyncPredicate, searchSettings.ConsiderNodeAsyncPredicate, searchSettings.Ignores, cancellationToken);
            Console.WriteLine("Before after candidates");
            int count = 0;
#if false
            await Task.Run(async () =>
            {
                foreach (TreeNode node in searchCandidates)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    count++;
                    int percentage = Math.Min(100, Math.Max(0, (count * 100) / searchCandidates.Length));
                    reportProgress($"Searching {node.FullPath} ({percentage}%)...", percentage);
                    bool isMatch = await searchSettings.IsMatchAsyncPredicate(node);

                    if (isMatch)
                    {
                        matchFoundCallback((K2SpyTreeNode)node);
                    }
                }
            });
#else
#if true
            Console.WriteLine("Before search candidates");
            await Task.Run(async () =>
            {
                foreach (TreeNode node in searchCandidates)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    count++;
                    int percentage = Math.Min(100, Math.Max(0, (count * 100) / searchCandidates.Length));
                    reportProgress($"Searching {node.FullPath} ({percentage}%)...", percentage);
                    bool isMatch = await searchSettings.IsMatchAsyncPredicate(node);

                    if (isMatch)
                    {
                        await matchFoundCallback((K2SpyTreeNode)node);
                    }
                }
            });
            Console.WriteLine("After search candidates");

#else
            await Task.Run(() =>
            {
                foreach (TreeNode node in searchCandidates)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    count++;
                    int percentage = Math.Min(100, Math.Max(0, (count * 100) / searchCandidates.Length));
                    reportProgress($"Searching {node.FullPath} ({percentage}%)...", percentage);
                    bool isMatch = searchSettings.IsMatchAsyncPredicate(node).GetAwaiter().GetResult();

                    if (isMatch)
                    {
                        matchFoundCallback((K2SpyTreeNode)node);
                    }
                }
            });
#endif
#endif

            reportProgress("", 0);
            if (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                reportProgress($"Search completed in {timeSpan}", 0);
            }
        }

        #endregion

        #region Private Methods

        private static async Task<K2SpyTreeNode[]> GetSearchCandidatesAsync(TreeView treeView, TreeNodeCollection nodes, AsyncTreeNodePredicate considerChildrenPredicate, AsyncTreeNodePredicate considerNodePredicate, K2SpyTreeNode[] ignores, System.Threading.CancellationToken cancellationToken)
        {
            List<TreeNode> list = new List<TreeNode>();
            Func<TreeNodeCollection, Task> processNodesAsync = null;
            processNodesAsync = async collection =>
            {
                TreeNode[] treeNodeArray = collection.OfType<TreeNode>().ToArray();
                foreach (TreeNode node in treeNodeArray)
                {
                    if (ignores?.Contains(node) == true)
                        continue;
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    if (await considerChildrenPredicate(node))
                    {
                        await node.InitializeChildrenAsync();
                        await processNodesAsync(node.Nodes);
                    }
                    if (await considerNodePredicate(node))
                        list.Add(node);
                }
            };
            using (TreeViewUpdateContext.CreateIfNecessary(treeView))
                await processNodesAsync(nodes);
            return list.OfType<K2SpyTreeNode>().ToArray();
        }

        #endregion
    }
}