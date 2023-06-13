using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;
using System.Windows.Forms.VisualStyles;

namespace K2Spy
{
    public partial class WorkingOverlay : UserControl
    {
        #region Private Fields

        private bool m_Hidden = true;
        private bool m_Large = true;
        private int m_Depth;

        #endregion

        #region Constructors

        public WorkingOverlay()
        {
            InitializeComponent();
            base.Visible = true;
        }

        #endregion

        #region Public Properties

        [DefaultValue(true)]
        public bool DockOnShow { get; set; } = true;

        private bool m_WhiteBackColor = true;
        private Color? m_LastNonWhiteBackColor = null;

        [DefaultValue(true)]
        public bool WhiteBackColor
        {
            get { return this.m_WhiteBackColor; }
            set
            {
                this.m_WhiteBackColor = value;
                if (value)
                {
                    this.m_LastNonWhiteBackColor = base.BackColor;
                    base.BackColor = Color.White;
                }
                else if (base.BackColor == Color.White)
                {
                    if (this.m_LastNonWhiteBackColor.HasValue)
                        base.BackColor = this.m_LastNonWhiteBackColor.Value;
                    else
                        base.ResetBackColor();
                }
            }
        }

        [DefaultValue(true)]
        public bool Large
        {
            get { return this.m_Large; }
            set
            {
                this.m_Large = value;
                Font baseFont = base.Font;

                if (value)
                    this.lblLoadingStatus.Font = new Font(baseFont.FontFamily, 11.25F);
                else
                    this.lblLoadingStatus.Font = baseFont;
            }
        }

        public string Status
        {
            get { return this.lblLoadingStatus.Text; }
            set { this.lblLoadingStatus.Text = value; }
        }

        #endregion

        #region Public Methods

        public void Hide(bool force = false)
        {
            this.HideInternal(force);
        }

        public void Show(bool delayVisibility = true)
        {
            this.ShowInternal(delayVisibility);
        }

        public void Show(string status, bool delayVisibility = true)
        {
            this.Status = status;
            this.Show(delayVisibility);
        }

        public IDisposable ShowThenHide(bool delayVisibility = true)
        {
            this.ShowInternal(delayVisibility);
            return new Disposer(() => this.HideInternal(false));
        }

        public IDisposable ShowThenHide(string status, bool delayVisibility = true)
        {
            this.Show(status, delayVisibility);
            return new Disposer(() => this.HideInternal(false));
        }

        #endregion

        #region Internal Methods

        internal void ResetVisibilityDepth()
        {
            this.m_Depth = 0;
        }

        #endregion

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (this.WhiteBackColor)
                this.WhiteBackColor = true;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.Large = this.Large;
        }

        private void HideInternal(bool force = false)
        {
            this.m_Depth = Math.Max(0, this.m_Depth - 1);
            if (force)
                this.m_Depth = 0;

            if (this.m_Depth == 0)
            {
                this.m_Hidden = true;
                this.InvokeIfRequired(() =>
                {
                    base.Visible = false;
                    this.pbLoading.Style = ProgressBarStyle.Blocks;
                    this.lblLoadingStatus.Text = "";
                });
            }
        }

        private async void ShowInternal(bool delayVisibility = true)
        {
            this.m_Depth++;

            bool hidden = this.m_Hidden;
            this.m_Hidden = false;
            if (delayVisibility == false)
            {
                // do nothing
            }
            else if (hidden)
            {
                await Task.Delay(100);
            }

            this.InvokeIfRequired(() =>
            {
                if (!this.m_Hidden)
                {
                    this.pbLoading.Style = ProgressBarStyle.Marquee;
                    if (this.DockOnShow)
                        base.Dock = DockStyle.Fill;
                    base.Visible = true;
                    base.BringToFront();
                }
            });
        }
    }
}