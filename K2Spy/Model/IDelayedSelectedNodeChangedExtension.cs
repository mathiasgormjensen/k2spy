namespace K2Spy.Model
{
    public interface IDelayedSelectedNodeChangedExtension : IExtension
    {
        void SelectedNodeChangedDelayed(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, System.Threading.CancellationTokenSource cancellationTokenSource);
    }
}
