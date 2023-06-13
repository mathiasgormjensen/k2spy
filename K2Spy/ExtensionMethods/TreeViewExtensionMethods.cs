using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.ExtensionMethods
{
    public static class TreeViewExtensionMethods
    {
        public static IEnumerable<System.Windows.Forms.TreeNode> GetDescendants(this System.Windows.Forms.TreeView that)
        {
            return that.Nodes.GetDescendants();
        }

        [Obsolete("", true)]
        public static void SelectNodeByPath(this System.Windows.Forms.TreeView that, string path, bool throwOnNotFound = true)
        {
            that.SelectedNode = that.GetNodeByPath(path, throwOnNotFound);
        }

        public static async Task<System.Windows.Forms.TreeNode> SelectNodeByPathAsync(this System.Windows.Forms.TreeView that, string path, bool throwOnNotFound = true, bool focusTreeView = true)
        {
            TreeNode match = await that.GetNodeByPathAsync(path, throwOnNotFound);
            that.SelectedNode = match;
            if (focusTreeView)
                that.Focus();
            return match;
        }

        [Obsolete("", true)]
        public static System.Windows.Forms.TreeNode GetNodeByPath(this System.Windows.Forms.TreeView that, string path, bool throwOnNotFound = true)
        {
            System.Windows.Forms.TreeNode match = that.Nodes.DescendantsWhere(key => key.FullPath == path).SingleOrDefault();
            if (match == null && throwOnNotFound)
                throw new Exception($"The node with the path {path} was not found");
            return match;
        }

        public static async Task<System.Windows.Forms.TreeNode> GetNodeByPathAsync(this System.Windows.Forms.TreeNodeCollection that, string path, bool throwOnNotFound = true)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            foreach (TreeNode node in that)
            {
                if (node.FullPath == path)
                {
                    return node;
                }
                else if (path.StartsWith(node.FullPath))
                {
                    if (node is IInitializeChildren initializeChildren)
                        await initializeChildren.InitializeChildrenAsync();

                    TreeNode match = await node.Nodes.GetNodeByPathAsync(path, false);
                    if (match != null)
                    {
                        return match;
                    }
                }
            }
            if (throwOnNotFound)
                throw new Exception($"The node with the path {path} was not found");
            return null;
        }

        public static async Task<System.Windows.Forms.TreeNode> GetNodeByPathAsync(this System.Windows.Forms.TreeView that, string path, bool throwOnNotFound = true)
        {
            return await that.Nodes.GetNodeByPathAsync(path, throwOnNotFound);
        }

        public static void EnableSelectNodeOnRightClick(this System.Windows.Forms.TreeView that)
        {
            that.NodeMouseClick += (sender, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    that.SelectNodeAtLocation(e.Location);
                }
            };
        }

        public static System.Windows.Forms.TreeNode GetNodeAtLocation(this System.Windows.Forms.TreeView that, Point location)
        {
            System.Windows.Forms.TreeNode node = that.GetNodeAt(location);
            if (node != null)
            {
                Rectangle bounds = node.Bounds;
                if (that.ImageList != null)
                    bounds = new Rectangle(bounds.X - 20, bounds.Y, bounds.Width + 20, bounds.Height);
                if (bounds.Contains(location))
                {
                    return node;
                }
            }
            return null;
        }

        public static System.Windows.Forms.TreeNode SelectNodeAtLocation(this System.Windows.Forms.TreeView that, Point location)
        {
            System.Windows.Forms.TreeNode node = that.GetNodeAtLocation(location);
            if (node != null)
            {
                that.SelectedNode = node;
                return node;
            }
            return null;
        }

        public static bool SelectNode(this System.Windows.Forms.TreeView that, K2SpyTreeNode node, bool focusTreeView = true)
        {
            Func<System.Windows.Forms.TreeNodeCollection, bool> action = null;
            action = nodes =>
            {
                foreach (System.Windows.Forms.TreeNode child in nodes)
                {
                    if (node == child as K2SpyTreeNode)
                    {
                        that.SelectedNode = child;
                        if (focusTreeView)
                            that.Focus();
                        return true;
                    }
                    if (action(child.Nodes))
                        return true;
                }
                return false;
            };
            return action(that.Nodes);
        }

        [Obsolete("", true)]
        public static TNode GetNodeBelowRootNode<TRoot, TNode>(this System.Windows.Forms.TreeView that, Predicate<TNode> predicate)
            where TRoot : K2SpyTreeNode
            where TNode : K2SpyTreeNode
        {
            TRoot root = that?.Nodes.OfType<TRoot>()?.FirstOrDefault();
            IEnumerable<TNode> nodes = root?.Nodes.OfType<TNode>();
            TNode node = nodes?.FirstOrDefault(predicate.Invoke);
            return node;
        }

        public static async Task<TNode> GetNodeBelowRootNodeAsync<TRoot, TNode>(this System.Windows.Forms.TreeView that, Predicate<TNode> predicate)
            where TRoot : K2SpyTreeNode
            where TNode : K2SpyTreeNode
        {
            TRoot root = that?.Nodes.OfType<TRoot>()?.FirstOrDefault();
            if (root is IInitializeChildren initializeChildren)
                await initializeChildren.InitializeChildrenAsync();

            IEnumerable<TNode> nodes = root?.Nodes.OfType<TNode>();
            TNode node = nodes?.FirstOrDefault(predicate.Invoke);
            return node;
        }
    }
}
