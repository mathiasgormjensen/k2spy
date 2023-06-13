using System;
using System.Windows.Forms;

namespace K2Spy.Model
{
    public interface IDefinitionPaneExtension : IExtension
    {
        event EventHandler DefinitionPaneTitleChanged;
        event EventHandler OpenDefinitionPane;
        event EventHandler CloseDefinitionPane;

        bool InitialDefinitionPaneVisibility { get; }
        bool CanCloseDefinitionPane { get; }
        string DefinitionPaneTitle { get; }

        Control CreateDefinitionPaneControl(K2SpyContext k2SpyContext);

        void OnOpenDefinitionPane(K2SpyContext k2SpyContext);
        void OnCloseDefinitionPane(K2SpyContext k2SpyContext);
        void OnActivateDefinitionPane(K2SpyContext k2SpyContext);
        void OnDeactivateDefinitionPane(K2SpyContext k2SpyContext);
    }
}
