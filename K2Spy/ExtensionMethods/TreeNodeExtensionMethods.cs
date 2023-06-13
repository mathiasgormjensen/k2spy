using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class TreeNodeExtensionMethods
    {
        /// <summary>
        /// Fixes the issue where left arrow doesn't work because the node is both empty and expanded
        /// </summary>
        /// <param name="that"></param>
        public static void CollapseIfEmptyAndExpanded(this System.Windows.Forms.TreeNode that)
        {
            if (that != null)
            {
                if (that.IsExpanded && that.Nodes.Count == 0)
                {
                    using (TreeViewUpdateContext.CreateIfNecessary(that))
                    {
                        that.Collapse(true);
                        that.Nodes.Add("");
                        that.Collapse(true);
                        that.Nodes.Clear();
                    }
                }
            }
        }

        public static async Task InitializeChildrenAsync(this System.Windows.Forms.TreeNode that)
        {
            if (that is IInitializeChildren initializeChildren) 
            {
                await initializeChildren.InitializeChildrenAsync();
            }
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> GetDescendants(this System.Windows.Forms.TreeNode that)
        {
            return that.Nodes.GetDescendants();
            //foreach (System.Windows.Forms.TreeNode child in node.Nodes)
            //{
            //    yield return child;

            //    foreach (System.Windows.Forms.TreeNode childchild in child.GetDescendants())
            //    {
            //        yield return childchild;
            //    }
            //}
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> DescendantsWhere(this System.Windows.Forms.TreeNode that, Func<System.Windows.Forms.TreeNode, bool> predicate)
        {
            return that.Nodes.DescendantsWhere(predicate);
        }

        public static IEnumerable<T> DescendantsOfTypeWhere<T>(this System.Windows.Forms.TreeNode that, Func<T, bool> predicate)
            where T : System.Windows.Forms.TreeNode
        {
            return that.Nodes.DescendantsOfTypeWhere<T>(predicate);
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> Descendants<T>(this System.Windows.Forms.TreeNode that)
            where T : System.Windows.Forms.TreeNode
        {
            return that.Nodes.Descendants();
        }

        public static IEnumerable<T> DescendantsOfType<T>(this System.Windows.Forms.TreeNode that)
            where T : System.Windows.Forms.TreeNode
        {
            return that.DescendantsOfTypeWhere<T>(key => true);
        }

        public static void ImageKeyFrom(this System.Windows.Forms.TreeNode that, K2SpyTreeNode imageKeySource)
        {
            EventHandler imageKeyChangedHandler = (sender, e) =>
            {
                if (that is K2SpyTreeNode k2SpyTreeNode)
                {
                    k2SpyTreeNode.ImageKey = imageKeySource.ImageKey;
                }
                else
                {
                    that.ImageKey = imageKeySource.ImageKey;
                    that.SelectedImageKey = imageKeySource.SelectedImageKey;
                }
            };
            imageKeySource.ImageKeyChanged += imageKeyChangedHandler;
            imageKeyChangedHandler(imageKeySource, EventArgs.Empty);
        }

        public static void SetImageKey(this System.Windows.Forms.TreeNode that, string imageKey)
        {
            that.ImageKey = imageKey;
            that.SelectedImageKey = imageKey;
        }

        public static TTreeNode WithImageKey<TTreeNode>(this TTreeNode that, string imageKey)
            where TTreeNode : System.Windows.Forms.TreeNode
        {
            that.ImageKey = imageKey;
            that.SelectedImageKey = imageKey;
            return that;
        }
    }
}
