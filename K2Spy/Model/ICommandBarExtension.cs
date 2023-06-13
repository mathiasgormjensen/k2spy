using System.Windows.Forms;

namespace K2Spy.Model
{
    internal interface ICommandBarExtension : IExtension
    {
        // void InitializeCommandBar(K2SpyContext k2SpyContext, ToolStrip toolStrip);
        ToolStripItem[] CreateCommandBarItems(K2SpyContext k2SpyContext);
    }

    internal interface ILeftAlignedCommandBarExtension : ICommandBarExtension
    {
    }
}
