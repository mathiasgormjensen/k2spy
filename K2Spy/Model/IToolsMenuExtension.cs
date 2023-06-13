using System.Threading;
using System.Windows.Forms;

namespace K2Spy.Model
{
    public interface IOptionsTitleExtension : IOptionsExtension
    {
        string OptionsTitle { get; }
    }

    public interface IGeneralOptionsExtension : IOptionsExtension
    {
    }

    public interface IOptionsExtension : IExtension
    {
        System.Windows.Forms.Control CreateOptionsControl(K2SpyContext k2SpyContext);
        void CommitOptions(Control optionsControl);
        bool IsOptionsControlDirty(Control optionsControl);
    }

    internal interface IOptionsPriorityExtension : IOptionsExtension
    {
        int OptionsPriority { get; }
    }

    public interface IToolsMenuExtension : IExtension
    {
        ToolStripItem[] CreateToolsMenuItems(K2SpyContext k2SpyContext);
    }
}
