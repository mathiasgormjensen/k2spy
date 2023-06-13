using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Analyzer
{
    public partial class AnalyzerOptionsControl : UserControl
    {
        public AnalyzerOptionsControl()
        {
            InitializeComponent();

            this.chkPopulateControlsInUsesAnalysis.Checked = Properties.Settings.Default.PopulateControlsInUsesAnalysis;
            this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Checked = Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInUsesAnalysis;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.Dock = DockStyle.Top;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.AutoSize = true;
        }

        public void Commit()
        {
            Properties.Settings.Default.PopulateControlsInUsesAnalysis = this.chkPopulateControlsInUsesAnalysis.Checked;
            Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInUsesAnalysis = this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Checked;
        }

        public bool IsDirty
        {
            get
            {
                return this.chkPopulateControlsInUsesAnalysis.Checked != Properties.Settings.Default.PopulateControlsInUsesAnalysis ||
                this.chkPopulateSmartMethodsAndPropertiesInUsesAnalysis.Checked != Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInUsesAnalysis;
            }
        }
    }
}
