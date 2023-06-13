using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ServiceInstanceDetailsPane
{
    [Model.IgnoreExtension]
    internal class ServiceInstanceDetailsPaneExtension : Model.IDefinitionPaneExtension, Model.IInternalExtension, Model.IDelayedSelectedNodeChangedExtension, Model.IToolsMenuExtension
    {
        private Panel m_Panel;
        private TextBox m_TextBox;
        private WorkingOverlay m_LoadingOverlay;
        private DefinitionPaneExecuter m_DefinitionPaneExecuter = new DefinitionPaneExecuter();

        public string DefinitionPaneTitle => this.DisplayName;

        public string DisplayName => "Service instance details";

        public bool InitialDefinitionPaneVisibility => false;

        public bool CanCloseDefinitionPane => true;

        public event EventHandler DefinitionPaneTitleChanged;
        public event EventHandler OpenDefinitionPane;
        public event EventHandler CloseDefinitionPane;

        public Control CreateDefinitionPaneControl(K2SpyContext k2SpyContext)
        {
            this.m_Panel = new Panel();
            this.m_Panel.Dock = DockStyle.Fill;
            this.m_TextBox = new TextBox();
            this.m_TextBox.Multiline = true;
            this.m_TextBox.Dock = DockStyle.Fill;
            this.m_TextBox.ReadOnly = true;

            this.m_LoadingOverlay = new WorkingOverlay();

            this.m_Panel.Controls.Add(this.m_TextBox);
            this.m_Panel.Controls.Add(this.m_LoadingOverlay);

            return this.m_Panel;
        }

        public void OnActivateDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_DefinitionPaneExecuter.OnActivateDefinitionPane();
        }

        public void OnDeactivateDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_DefinitionPaneExecuter.OnDeactivateDefinitionPane();
        }

        public async void SelectedNodeChangedDelayed(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, CancellationTokenSource cancellationTokenSource)
        {
            this.m_DefinitionPaneExecuter.ExecuteIfOrWhenActive(async () =>
            {
                if (treeNode is ServiceInstanceTreeNode serviceInstanceTreeNode)
                {
                    this.m_LoadingOverlay.Show("Loading...");
                    this.m_TextBox.Text = "";
                    SourceCode.SmartObjects.Services.Management.SettingKeyInfo[] settings = await Task.Run(async () =>
                    {
                        SourceCode.SmartObjects.Services.Management.ServiceManagementServer serviceManagementServer = (await context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.SmartObjects.Services.Management.ServiceManagementServer>());
                        string xml = serviceManagementServer.GetExistingServiceInstanceConfiguration(serviceInstanceTreeNode.ServiceInstanceGuid);
                        return SourceCode.SmartObjects.Services.Management.ServiceConfigInfo.Create(xml).ConfigSettings.ToArray();
                    });

                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (SourceCode.SmartObjects.Services.Management.SettingKeyInfo setting in settings.OrderBy(key => key.DisplayName))
                        {
                            builder.AppendLine(setting.DisplayName + ": " + setting.Value);
                        }
                        this.m_LoadingOverlay.Hide(true);
                        this.m_TextBox.Text = builder.ToString();
                    }
                }
                else
                {
                    this.m_TextBox.Text = "(select a service instance tree node to view its configuration)";
                }
            });
        }

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext context)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("Show pane" ,null , (sender,e) =>
                {
                    ToolStripMenuItem that = (ToolStripMenuItem)sender;
                    //that.Checked = !that.Checked;
                    if (true||that.Checked)
                        this.OpenDefinitionPane?.Invoke(this,EventArgs.Empty);
                    else
                        this.CloseDefinitionPane?.Invoke(this,EventArgs.Empty);
                })
            };
        }

        public void OnOpenDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_DefinitionPaneExecuter.OnOpenDefinitionPane();
        }

        public void OnCloseDefinitionPane(K2SpyContext k2SpyContext)
        {
            this.m_DefinitionPaneExecuter.OnCloseDefinitionPane();
        }
    }
}
