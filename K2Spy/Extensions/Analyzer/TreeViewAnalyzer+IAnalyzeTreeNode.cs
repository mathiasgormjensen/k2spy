using System.Threading.Tasks;

namespace K2Spy.Extensions.Analyzer
{
    partial class TreeViewAnalyzer
    {
        private interface IAnalyzeTreeNode
        {
            void AfterCollapse(K2SpyContext k2SpyContext);
            Task AfterExpandAsync(K2SpyContext k2SpyContext);
        }
    }
}
