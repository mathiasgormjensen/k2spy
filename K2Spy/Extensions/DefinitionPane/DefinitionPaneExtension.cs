using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.DefinitionPane
{
    internal class DefinitionPaneExtension : Model.IDefinitionPaneExtension, Model.IInternalExtension, Model.IDelayedSelectedNodeChangedExtension, Model.IExtensionPriority, Model.IInitializedExtension
    {
        #region Private Fields

        private DefinitionPaneControl m_DefinitionPaneControl;
        private K2SpyTreeNode m_LastSelectedTreeNode;
        private DefinitionPaneExecuter m_Executor = new DefinitionPaneExecuter();

        #endregion

        #region Public Events

        public event EventHandler DefinitionPaneTitleChanged;
        public event EventHandler OpenDefinitionPane;
        public event EventHandler CloseDefinitionPane;

        #endregion

        #region Public Properties

        public string DefinitionPaneTitle { get; private set; } = "Definition";

        public string DisplayName => "Definition Pane";

        public int Priority => 1000;

        public bool InitialDefinitionPaneVisibility => true;

        public bool CanCloseDefinitionPane => false;

        #endregion

        #region Public Methods

        public void LoadData(string title, string data, TextFormat format, bool focus = false)
        {
            this.SetTitle(title);
            this.m_DefinitionPaneControl.LoadData(data, format, focus);
        }

        public Control CreateDefinitionPaneControl(K2SpyContext k2SpyContext)
        {
            this.m_DefinitionPaneControl = new DefinitionPaneControl();
            return this.m_DefinitionPaneControl;
        }

        public void GoTo(int line, int position, bool focus = true)
        {
            this.m_DefinitionPaneControl.GoTo(line, position, focus);
        }

        internal void Refresh()
        {
            this.SelectedNodeChangedDelayed(this.m_Context, this.m_TreeNode, this.m_ActingAsOrSelfTreeNode, null);
        }

        private K2SpyContext m_Context;
        private K2SpyTreeNode m_TreeNode;
        private K2SpyTreeNode m_ActingAsOrSelfTreeNode;
        public async void SelectedNodeChangedDelayed(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, CancellationTokenSource cancellationTokenSource)
        {
            Serilog.Log.Debug("DefinitionPane: " + DateTime.Now.ToString());
            if (treeNode != null)
            {
                this.m_Context = context;
                this.m_TreeNode = treeNode;
                this.m_ActingAsOrSelfTreeNode = actingAsOrSelfTreeNode;

                if (this.m_LastSelectedTreeNode != null)
                {
                    this.m_LastSelectedTreeNode.Refreshed -= this.LastSelectedTreeNode_Refreshed;
                }

                this.m_LastSelectedTreeNode = actingAsOrSelfTreeNode;
                if (this.m_LastSelectedTreeNode != null)
                    this.m_LastSelectedTreeNode.Refreshed += this.LastSelectedTreeNode_Refreshed;

                this.SetTitle(treeNode.Text);
                this.m_Executor.ExecuteIfOrWhenActive(async () =>
                {
                    await this.m_DefinitionPaneControl.SelectedNodeChangedDelayed(context, treeNode, actingAsOrSelfTreeNode, cancellationTokenSource);
                });
            }
        }

        public void OnActivateDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_Executor.OnActivateDefinitionPane();
        }

        public void OnDeactivateDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_Executor.OnDeactivateDefinitionPane();
        }

        public void OnOpenDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_Executor.OnOpenDefinitionPane();
        }

        public void OnCloseDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_Executor.OnCloseDefinitionPane();
        }

        public async Task InitializedAsync(K2SpyContext k2SpyContext)
        {
            k2SpyContext.Disconnecting += async (sender, e) =>
            {
                this.m_DefinitionPaneControl?.Reset();
                this.SetTitle("Definition");
            };
        }

        #endregion

        #region Protected Methods

        protected void SetTitle(string title)
        {
            if (this.DefinitionPaneTitle?.Equals(title) != true)
            {
                this.DefinitionPaneTitle = title;
                this.DefinitionPaneTitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Private Methods

        private async Task LastSelectedTreeNode_Refreshed(object sender, EventArgs e)
        {
            this.SelectedNodeChangedDelayed(null, this.m_LastSelectedTreeNode, this.m_LastSelectedTreeNode, new CancellationTokenSource());
        }

        #endregion
    }
}