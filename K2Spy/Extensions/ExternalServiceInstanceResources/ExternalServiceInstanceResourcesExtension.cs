using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    internal class ExternalServiceInstanceResourcesExtension : Model.IToolsMenuExtension, Model.IOptionsExtension
    {
        public string DisplayName => "External service instance resources";

        public void CommitOptions(Control optionsControl)
        {
            ((ExternalServiceInstanceResourcesOptions)optionsControl).CommitOptions();
        }

        public Control CreateOptionsControl(K2SpyContext k2SpyContext)
        {
            return new ExternalServiceInstanceResourcesOptions(k2SpyContext);
        }

        public bool IsOptionsControlDirty(Control optionsControl)
        {
            return ((ExternalServiceInstanceResourcesOptions)optionsControl).IsOptionsControlDirty();
        }

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext k2SpyContext)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("View external service instance resources", Properties.Resources.ResourceSymbols_16x, (sender, e) =>
                {
                    ViewExternalServiceInstanceResources dlg = new ViewExternalServiceInstanceResources(k2SpyContext);
                    dlg.Show();
                })
            };
        }
    }
}