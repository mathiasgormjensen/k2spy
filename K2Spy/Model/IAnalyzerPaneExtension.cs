using System;
using System.Windows.Forms;

namespace K2Spy.Model
{
    internal interface IAnalyzerPaneExtension : IExtension
    {
        event EventHandler CloseAnalyzerPane;
        Control CreateAnalyzerPaneControl(K2SpyContext k2SpyContext);
        void OnAnalyzerPaneActivated(K2SpyContext k2SpyContext);
    }
}
