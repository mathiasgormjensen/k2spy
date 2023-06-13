using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private delegate Task ExpandAsync(K2SpyContext context, System.Threading.CancellationTokenSource cancellationTokenSource, ReportProgressDelegate progressReporter, Func<TreeNode, Task> addNode);
    }
}
