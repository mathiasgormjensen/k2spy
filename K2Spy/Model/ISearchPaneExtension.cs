using System;
using System.Windows.Forms;

namespace K2Spy.Model
{
    internal interface ISearchPaneExtension : IExtension
    {
        event EventHandler OpenSearchPane;
        event EventHandler CloseSearchPane;
        Control CreateSearchPaneControl(K2SpyContext k2SpyContext);
        void ActivateSearchPane(K2SpyContext k2SpyContext);
    }
}
