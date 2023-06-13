using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class TreeNodeCollectionExtensionMethods
    {
        public static void AddIfNotNull(this System.Windows.Forms.TreeNodeCollection that, System.Windows.Forms.TreeNode node)
        {
            if (node != null)
                that.Add(node);
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> GetDescendants(this System.Windows.Forms.TreeNodeCollection that)
        {
            foreach (System.Windows.Forms.TreeNode child in that)
            {
                yield return child;

                foreach (System.Windows.Forms.TreeNode childchild in child.GetDescendants())
                {
                    yield return childchild;
                }
            }
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> DescendantsWhere(this System.Windows.Forms.TreeNodeCollection that, Func<System.Windows.Forms.TreeNode, bool> predicate)
        {
            IEnumerable<System.Windows.Forms.TreeNode> list = that.Descendants();
            return list.Where(predicate).AsEnumerable();
        }

        public static IEnumerable<System.Windows.Forms.TreeNode> Descendants(this System.Windows.Forms.TreeNodeCollection that)
        {
            List<System.Windows.Forms.TreeNode> list = new List<System.Windows.Forms.TreeNode>();
            Action<System.Windows.Forms.TreeNodeCollection> action = null;
            action = nodes =>
            {
                if (nodes != null)
                {
                    foreach (System.Windows.Forms.TreeNode node in nodes)
                    {
                        action(node.Nodes);
                        list.Add(node);
                    }
                }
            };
            action(that);
            return list.AsEnumerable();
        }

        public static async Task<IEnumerable<T>> DescendantsOfTypeWhereAsync<T>(this System.Windows.Forms.TreeNodeCollection that, Func<T, Task<bool>> predicate)
        {
            List<T> list = new List<T>();
            Func<System.Windows.Forms.TreeNodeCollection,Task> action = null;
            action = async nodes =>
            {
                if (nodes != null)
                {
                    foreach (System.Windows.Forms.TreeNode node in nodes)
                    {
                        await action(node.Nodes);
                        if (node is T)
                        {
                            if (await predicate((T)(object)node))
                                list.Add((T)(object)node);
                        }
                    }
                }
            };
            await action(that);
            return list.AsEnumerable();
        }

        public static IEnumerable<T> DescendantsOfTypeWhere<T>(this System.Windows.Forms.TreeNodeCollection that, Func<T, bool> predicate)
        {
#if true
            return that.DescendantsOfTypeWhereAsync<T>(key => Task.FromResult(predicate(key))).GetAwaiter().GetResult();
#else
            List<T> list = new List<T>();
            Action<System.Windows.Forms.TreeNodeCollection> action = null;
            action = nodes =>
            {
                if (nodes != null)
                {
                    foreach (System.Windows.Forms.TreeNode node in nodes)
                    {
                        action(node.Nodes);
                        if (node is T)
                        {
                            if (predicate((T)(object)node))
                                list.Add((T)(object)node);
                        }
                    }
                }
            };
            action(that);
            return list.AsEnumerable();
#endif
        }

        public static IEnumerable<T> DescendantsOfType<T>(this System.Windows.Forms.TreeNodeCollection that)
            //where T : System.Windows.Forms.TreeNode
        {
            return that.DescendantsOfTypeWhere<T>(key => true);
        }
    }
}
