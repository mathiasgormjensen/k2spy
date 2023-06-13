using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.PreloadDefinitions
{
    internal class PreloadDefinitionsExtension : Model.IInternalExtension, Model.IPreloadExtension
    {
        public int Priority => 0;

        public string DisplayName => "Preload definitions";

        public int PreloadPriority => -1;

        public string PreloaderDescription => "Preloading definitions...";

        public async Task PerformPreloadAsync(K2SpyContext k2SpyContext, ReportProgressDelegate reportProgress, CancellationToken cancellationToken)
        {
            K2SpyTreeNode[] candidates = k2SpyContext.TreeView.Nodes.OfType<K2SpyTreeNode>()
                .SelectMany(key => key.DescendantsOfType<K2SpyTreeNode>())
                .Select(async key => await key.GetActAsOrSelfAsync())
                .Select(key => key.Result)
                //.Where(key => key is SmartObjectInfoTreeNode || key is FormInfoTreeNode || key is ViewInfoTreeNode || key is ServiceInstanceTreeNode || key is ProcessSetTreeNode)
                //.Where(key => !(key is IActAsTreeNode) && !(key is K2SpyTreeNodeClone) & !(key is SmartPropertyTreeNode) && !(key is SmartMethodTreeNode))
                .Distinct()
                .ToArray();
            for (int i = 0; i < candidates.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                K2SpyTreeNode node = candidates[i];
                int percentage = (i * 100) / candidates.Length;
                reportProgress($"Processing {node.FullPath}...", percentage);
                Task<Definition> task = node.GetFormattedDefinitionAsync() ?? node.GetRawDefinitionAsync();
                if (task != null)
                    await task;
            }
        }
    }
}
