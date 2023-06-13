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
    [System.ComponentModel.DefaultEvent("CloseClicked")]
    public partial class PaneHeader : UserControl
    {
#region Private Fields

        private bool m_Closable = true;

        #endregion

        #region Constructors

        public PaneHeader()
        {
            InitializeComponent();

            base.Dock = DockStyle.Top;
            base.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.OnFontChanged(EventArgs.Empty);
        }

        #endregion

        #region Public Events

        public event EventHandler CloseClicked;

        #endregion

        #region Public Properties

        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return this.tslblSearchHeader.Text; }
            set { this.tslblSearchHeader.Text = value; }
        }

        [DefaultValue(true)]
        public bool Closable
        {
            get { return this.m_Closable; }
            set
            {
                this.m_Closable = value;
                base.TabStop = value;
                this.tsbtnCloseSearchPanel.Visible = value;
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Closable = this.Closable;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Closable = this.Closable;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            this.tsTop.Font = base.Font;
            this.tslblSearchHeader.Font = base.Font;

            base.OnFontChanged(e);
        }

        private void tsbtnCloseSearchPanel_Click(object sender, EventArgs e)
        {
            this.CloseClicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
