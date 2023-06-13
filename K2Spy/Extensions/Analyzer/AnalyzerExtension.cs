using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Analyzer
{
    internal class AnalyzerExtension : Model.ITreeViewContextMenuExtension, Model.IAnalyzerPaneExtension, Model.ISearcherContextMenuExtension, Model.IExtensionPriority, Model.IAnalyzerContextMenuExtension, Model.IClearDiskCacheExtension, Model.IGeneralOptionsExtension, Model.IOptionsPriorityExtension, Model.IAnalyzerContextMenuMenuPriorityExtension, Model.ISearcherContextMenuPriorityExtension, Model.ITreeViewContextMenuPriorityExtension, Model.IPreloadExtension
    {
        #region Private Fields

        private TreeViewAnalyzer m_TreeViewAnalyzer;

        #endregion

        #region Public Events

        public event EventHandler CloseAnalyzerPane;

        #endregion

        #region Public Properties

        public string DisplayName => "Analyzer";

        public int Priority => 100;

        public int OptionsPriority => 100;

        public int PreloadPriority => -2;

        public int TreeViewContextMenuPriority => 1000;

        public int SearcherContextMenuPriority => 1000;

        public int AnalyzerContextMenuPriority => 1000;

        public string PreloaderDescription => "Preloading relationship information...";

        #endregion

        #region Public Methods

        public void OnAnalyzerPaneActivated(K2SpyContext k2SpyContext)
        {
            this.m_TreeViewAnalyzer.Activate();
            // throw new NotImplementedException();
        }

        public Control CreateAnalyzerPaneControl(K2SpyContext k2SpyContext)
        {
            this.m_TreeViewAnalyzer?.Dispose();
            this.m_TreeViewAnalyzer = new TreeViewAnalyzer();
            this.m_TreeViewAnalyzer.Initialize(k2SpyContext);
            this.m_TreeViewAnalyzer.ClosePanel += (sender, e) => this.CloseAnalyzerPane?.Invoke(this, EventArgs.Empty);

            k2SpyContext.Disconnecting += async (sender, e) =>
            {
                this.m_TreeViewAnalyzer.Clear();
                this.CloseAnalyzerPane?.Invoke(this, EventArgs.Empty);
            };

            return this.m_TreeViewAnalyzer;
        }


        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateContextMenuItemsAsync(context, treeNode, actingAsOrSelfTreeNode, true, false, false);
        }

        public ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateContextMenuItemsAsync(k2SpyContext, treeNode, actingAsOrSelfTreeNode, analyzerNode?.Parent != null, true, analyzerNode?.Parent == null);
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateContextMenuItemsAsync(k2SpyContext, treeNode, actingAsOrSelfTreeNode, true, false, false);
        }

        public void ClearDiskCache()
        {
            UsesInformationCache.ClearDiskCache();
        }

        public Control CreateOptionsControl(K2SpyContext k2SpyContext)
        {
            return new AnalyzerOptionsControl();
        }

        public void CommitOptions(Control optionsControl)
        {
            ((AnalyzerOptionsControl)optionsControl).Commit();
        }

        public bool IsOptionsControlDirty(Control optionsControl)
        {
            return ((AnalyzerOptionsControl)optionsControl).IsDirty;
        }

        #endregion

        private ToolStripItem[] CreateContextMenuItemsAsync(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, bool createAnalyzeItem, bool createGoToItem, bool createDeleteItem)
        {
            if (actingAsOrSelfTreeNode is FormInfoTreeNode || actingAsOrSelfTreeNode is ViewInfoTreeNode || actingAsOrSelfTreeNode is SmartObjectInfoTreeNode || actingAsOrSelfTreeNode is WorkflowTreeNode || actingAsOrSelfTreeNode is ServiceInstanceTreeNode || actingAsOrSelfTreeNode is ControlTypeInfoTreeNode
                || actingAsOrSelfTreeNode is SmartPropertyTreeNode || actingAsOrSelfTreeNode is SmartMethodTreeNode
                || actingAsOrSelfTreeNode is StyleProfileInfoTreeNode)
            {
                List<ToolStripItem> list = new List<ToolStripItem>();
                if (createAnalyzeItem)
                {
                    list.Add(new ToolStripMenuItem("Analyze", Properties.Resources.VBSearch_16x, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async () =>
                        {
                            ((MainForm)context.MainForm).ShowAnalyzerPane();
                            // select or add the node we need to analyze
                            await this.m_TreeViewAnalyzer.SelectOrPopulateAsync(treeNode);
                        });
                    }));
                }
                if (createGoToItem)
                {
                    list.Add(new ToolStripMenuItem("Open", Properties.Resources.GoToReference_16x, (sender, e) =>
                    {
                        context.MainForm.Try(() => context.TreeView.SelectNode(treeNode));
                    }));
                }
                if (createDeleteItem)
                {
                    list.Add(new ToolStripMenuItem("Remove", Properties.Resources.Remove_color_16x, (sender, e) =>
                    {
                        context.MainForm.Try(() =>
                        {
                            TreeNode node = this.m_TreeViewAnalyzer.TreeView.SelectedNode;
                            if (node != null && node.Parent == null)
                                node.Remove();
                        });
                    }));
                }
                return list.ToArray();
            }
            return null;
        }

        public async Task PerformPreloadAsync(K2SpyContext k2SpyContext, ReportProgressDelegate reportProgress, CancellationToken cancellationToken)
        {
            K2SpyTreeNode[] candidates = k2SpyContext.TreeView.Nodes.OfType<K2SpyTreeNode>()
                .SelectMany(key => key.DescendantsOfType<K2SpyTreeNode>())
                .Select(async key => await key.GetActAsOrSelfAsync())
                .Select(key => key.Result)
                .Where(key => !(key is K2SpyTreeNodeClone))
                .Distinct()
                .ToArray();
            Type[] types = candidates.Select(key => key.GetType()).Distinct().ToArray();
            await Task.Run(() =>
            {
                for (int i = 0; i < candidates.Length; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    K2SpyTreeNode node = candidates[i];
                    int percentage = (i * 100) / candidates.Length;
                    reportProgress($"Processing {node.FullPath}...", percentage);
                    UsesInformationCache.GetUsesInformationAsync(k2SpyContext, node).GetAwaiter().GetResult();
                }
            });
            
        }
    }
}
