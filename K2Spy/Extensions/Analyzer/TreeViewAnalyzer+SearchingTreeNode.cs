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
        private class SearchingTreeNode : TreeNode
        {
            public SearchingTreeNode()
            {
                this.SetImageKey("Loading");
                this.Reset();
            }

            public void SetPercentage(int value)
            {
                if (value == -1)
                {
                    this.Reset();
                }
                else
                {
                    base.Text = "Searching (" + value + "%)...";
                }
            }

            public void Reset()
            {
                base.Text = "Searching...";
            }
        }
    }
}
