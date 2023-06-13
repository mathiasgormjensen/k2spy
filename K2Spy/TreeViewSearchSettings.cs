using System;
using System.Reflection;
using System.Threading.Tasks;

namespace K2Spy
{
    public class TreeViewSearchSettings
    {
        public TreeViewSearchSettings(AsyncTreeNodePredicate considerChildrenAsyncPredicate, AsyncTreeNodePredicate considerNodeAsyncPredicate, AsyncTreeNodePredicate isMatchAsyncPredicate, params K2SpyTreeNode[] ignores)
            : this(null, considerChildrenAsyncPredicate, considerNodeAsyncPredicate, isMatchAsyncPredicate, ignores)
        {
        }

        public TreeViewSearchSettings(System.Windows.Forms.TreeNode scopeRoot, AsyncTreeNodePredicate considerChildrenAsyncPredicate, AsyncTreeNodePredicate considerNodeAsyncPredicate, AsyncTreeNodePredicate isMatchAsyncPredicate, params K2SpyTreeNode[] ignores)
        {
            this.ScopeRoot = scopeRoot;
            this.ConsiderChildrenAsyncPredicate = considerChildrenAsyncPredicate;
            this.ConsiderNodeAsyncPredicate = considerNodeAsyncPredicate;
            this.IsMatchAsyncPredicate = isMatchAsyncPredicate;
            this.Ignores = ignores;
        }

        public System.Windows.Forms.TreeNode ScopeRoot { get; private set; }

        public AsyncTreeNodePredicate ConsiderChildrenAsyncPredicate { get; private set; }
        
        public AsyncTreeNodePredicate ConsiderNodeAsyncPredicate { get; private set; }
        
        public AsyncTreeNodePredicate IsMatchAsyncPredicate { get; private set; }

        public K2SpyTreeNode[] Ignores { get; private set; }
    }
}
