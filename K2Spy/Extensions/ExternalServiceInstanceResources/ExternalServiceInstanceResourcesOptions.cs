using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    public partial class ExternalServiceInstanceResourcesOptions : UserControl
    {
        private K2SpyContext m_K2SpyContext;

        public ExternalServiceInstanceResourcesOptions(K2SpyContext k2SpyContext)
        {
            InitializeComponent();
            
            this.m_K2SpyContext = k2SpyContext;
            this.txtProperties.Text = Properties.Settings.Default.ExternalServiceInstanceResourcesProperties;
        }

        public void CommitOptions()
        {
            Properties.Settings.Default.ExternalServiceInstanceResourcesProperties = this.txtProperties.Text;
        }

        public bool IsOptionsControlDirty()
        {
            return this.txtProperties.Text != Properties.Settings.Default.ExternalServiceInstanceResourcesProperties;
        }
    }
}
