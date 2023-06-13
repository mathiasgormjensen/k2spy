using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    internal partial class CategoryScope : UserControl
    {
        #region Private Fields

        private K2SpyContext m_Context;

        #endregion

        #region Constructors

        public CategoryScope()
        {
            InitializeComponent();
            base.Dock = DockStyle.Top;
            base.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        #endregion

        #region Public Events

        public event EventHandler CategoryPathChanged;

        #endregion

        #region Public Properties

        private bool m_HideScopeCheckBox = false;
        [DefaultValue(false)]
        public bool HideScopeCheckBox
        {
            get { return this.m_HideScopeCheckBox; }
            set
            {
                this.m_HideScopeCheckBox = value;
                this.chkScope.Visible = value == false;
            }
        }

        [DefaultValue(false)]
        public bool Scoped
        {
            get { return this.chkScope.Checked; }
            set { this.chkScope.Checked = value; }
        }

        [DefaultValue("")]
        public string CategoryPath
        {
            get { return this.txtCategoryPath.Text; }
            set
            {
                if (this.txtCategoryPath.Text != value)
                {
                    this.txtCategoryPath.Text = value;
                    this.CategoryPathChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Public Methods

        public void Initialize(K2SpyContext k2SpyContext)
        {
            this.m_Context = k2SpyContext;
        }

        #endregion

        #region Private Methods

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (SelectCategory dlg = new SelectCategory(this.m_Context))
                {
                    dlg.AllowSelectRoot = true;
                    dlg.SelectedCategory = this.CategoryPath;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        this.CategoryPath = dlg.SelectedCategory;
                    }
                }
            });
        }

        private void chkScope_CheckedChanged(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.txtCategoryPath.Enabled = this.btnBrowse.Enabled = this.chkScope.Checked;
                if (this.chkScope.Checked && string.IsNullOrEmpty(this.CategoryPath))
                    this.btnBrowse.PerformClick();
            });
        }

        #endregion
    }
}