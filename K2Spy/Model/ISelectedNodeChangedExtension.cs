using System.Windows.Forms;

namespace K2Spy.Model
{
    internal interface ISelectedNodeChangedExtension : IExtension
    {
        void SelectedNodeChanged(TreeNode treeNode);
    }
}
