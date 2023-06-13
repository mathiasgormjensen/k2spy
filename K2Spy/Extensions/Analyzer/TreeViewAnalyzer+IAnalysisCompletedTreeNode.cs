using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private interface IAnalysisCompletedTreeNode
        {
            bool AnalysisCompleted { get; set; }
        }
    }
}
