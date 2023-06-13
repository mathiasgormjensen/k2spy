using K2Spy.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public partial class Options : BaseForm
    {
        #region Private Fields

        private List<Action> m_CommitActions = new List<Action>();
        private List<Func<bool>> m_IsOptionsControlDirtyFunction = new List<Func<bool>>();
        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Constructors

        public Options(K2SpyContext k2SpyContext)
        {
            this.m_K2SpyContext = k2SpyContext;
            InitializeComponent();
        }

        #endregion

        #region Protected Properties

        protected bool IsDirty
        {
            get
            {
                if (this.chkPopulateControlsInTreeView.Checked != Properties.Settings.Default.PopulateControlsInTreeView ||
                    this.chkPopulateServiceObjectsInTreeView.Checked != Properties.Settings.Default.PopulateServiceObjectsInTreeView ||
                    this.chkPopulateSmartMethodsAndPropertiesInTreeView.Checked != Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInTreeView ||
                    this.chkPreserveInvalidCategoryEntriesInTreeView.Checked != Properties.Settings.Default.PreserveInvalidCategoryEntriesInTreeView)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.chkPopulateControlsInTreeView.Checked = Properties.Settings.Default.PopulateControlsInTreeView;
            this.chkPopulateServiceObjectsInTreeView.Checked = Properties.Settings.Default.PopulateServiceObjectsInTreeView;
            this.chkPopulateSmartMethodsAndPropertiesInTreeView.Checked = Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInTreeView;
            this.chkPreserveInvalidCategoryEntriesInTreeView.Checked = Properties.Settings.Default.PreserveInvalidCategoryEntriesInTreeView;

            foreach (Model.IOptionsExtension optionsExtension in Extensions.ExtensionsManager.GetExtensions<Model.IOptionsExtension>().OrderByDescending(key => (key as IOptionsPriorityExtension)?.OptionsPriority ?? 0))
            {
                Control control;
                string title = ((optionsExtension as IOptionsTitleExtension)?.OptionsTitle) ?? optionsExtension.DisplayName;
                if (optionsExtension is IGeneralOptionsExtension generalOptionsExtension)
                {
                    PaneHeader paneHeader = new PaneHeader();
                    paneHeader.Closable = false;
                    paneHeader.Text = title;
                    this.tpGeneral.Controls.Add(paneHeader);
                    this.tpGeneral.Controls.SetChildIndex(paneHeader, 0);
                    control = generalOptionsExtension.CreateOptionsControl(this.m_K2SpyContext);
                    control.Dock = DockStyle.Top;
                    this.tpGeneral.Controls.Add(control);
                    this.tpGeneral.Controls.SetChildIndex(control, 0);
                }
                else
                {
                    TabPage tabPage = new TabPage();
                    tabPage.Padding = new Padding(3);
                    tabPage.Text = title;
                    tabPage.UseVisualStyleBackColor = true;
                    this.tabControl.TabPages.Add(tabPage);
                    control = optionsExtension.CreateOptionsControl(this.m_K2SpyContext);
                    control.Dock = DockStyle.Top;
                    tabPage.AutoScroll = true;
                    tabPage.Controls.Add(control);
                }
                this.m_CommitActions.Add(() => optionsExtension.CommitOptions(control));
                this.m_IsOptionsControlDirtyFunction.Add(() => optionsExtension.IsOptionsControlDirty(control));
            }
        }

        #endregion

        #region Private Methods

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                bool isDirty = this.IsDirty;
                if (isDirty || this.m_IsOptionsControlDirtyFunction.Any(key => key.Invoke() == true))
                {
                    this.m_CommitActions.ForEach(key => key.Invoke());

                    Properties.Settings.Default.PopulateControlsInTreeView = this.chkPopulateControlsInTreeView.Checked;
                    Properties.Settings.Default.PopulateServiceObjectsInTreeView = this.chkPopulateServiceObjectsInTreeView.Checked;
                    Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInTreeView = this.chkPopulateSmartMethodsAndPropertiesInTreeView.Checked;
                    Properties.Settings.Default.PreserveInvalidCategoryEntriesInTreeView = this.chkPreserveInvalidCategoryEntriesInTreeView.Checked;
                    Properties.Settings.Default.Save();

                    System.Windows.Forms.MessageBox.Show(this, "Some settings may not be applied until K2Spy is restarted or the connection to K2 is reset", base.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        #endregion
    }
}