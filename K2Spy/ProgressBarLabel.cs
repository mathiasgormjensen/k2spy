using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.VisualStyles;
using SourceCode.Framework;

namespace K2Spy
{
    public partial class ProgressBarLabel : UserControl
    {
        private static readonly int m_TimeoutInSeconds = 5;
        private DateTime m_LastUpdate = DateTime.Now;
        private readonly object m_QueueSetProgressBarStyleActionKey = new object();
        private readonly object m_QueueSetProgressBarValueActionKey = new object();
        private readonly object m_QueueSetTextActionKey = new object();
        private K2SpyContext m_Context;

        public ProgressBarLabel()
        {
            InitializeComponent();

            base.Dock = DockStyle.Bottom;
            base.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        }

        [DefaultValue(ProgressBarStyle.Blocks)]
        public ProgressBarStyle ProgressBarStyle
        {
            get { return this.progressBar.Style; }
            set { this.progressBar.Style = value; }
        }

        [DefaultValue(100)]
        public int ProgressBarMarqueeAnimationSpeed
        {
            get { return this.progressBar.MarqueeAnimationSpeed; }
            set { this.progressBar.MarqueeAnimationSpeed = value; }
        }

        [DefaultValue(0)]
        public int ProgressBarValue
        {
            get { return this.progressBar.Value; }
            set { this.progressBar.Value = value; }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return this.label.Text; }
            set
            {
                if (this.label.Text != value)
                {
                    this.label.Text = value;
                }
                else
                {

                }
            }
        }

        public void Initialize(K2SpyContext k2SpyContext)
        {
            this.m_Context = k2SpyContext;
        }

        public void QueueReset()
        {
            //this.m_Context.ActionQueue.Remove(this.m_QueueSetProgressBarStyleActionKey);
            //this.m_Context.ActionQueue.Remove(this.m_QueueSetProgressBarValueActionKey);
            //this.m_Context.ActionQueue.Remove(this.m_QueueSetTextActionKey);
            this.QueueOnce(new object(), this.Reset);
            // this.m_Context.ActionQueue.Queue(this.Reset);
        }

        public void Reset()
        {
            this.expirationTimer.Stop();
            this.Text = "";
            this.ProgressBarStyle = ProgressBarStyle.Blocks;
            this.ProgressBarValue = 0;
        }

        public void QueueSetProgressBarStyle(ProgressBarStyle progressBarStyle)
        {
            this.QueueOnce(this.m_QueueSetProgressBarStyleActionKey, () => this.ProgressBarStyle = progressBarStyle);
        }

        public void QueueSetProgressBarValue(int value)
        {
            this.QueueOnce(this.m_QueueSetProgressBarValueActionKey, () => this.ProgressBarValue = value);
        }

        public void QueueUpdate(string status, int percentage)
        {
            this.QueueSetText(status);
            this.QueueSetProgressBarValue(percentage);
        }

        
        public void QueueSetText(string text)
        {
            this.QueueOnce(this.m_QueueSetTextActionKey, () => this.Text = text);
        }

        protected void QueueOnce(object actionKey, Action action)
        {
            Action startExpirationAndAction = () =>
            {
                this.expirationTimer.Enabled = true;
                action();
            };
            this.m_LastUpdate = DateTime.Now;
            if (this.m_Context == null)
                startExpirationAndAction();
            else
                this.m_Context?.ActionQueue.QueueOnce(actionKey, startExpirationAndAction);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            this.label.Height = this.label.GetPreferredSize(Size.Empty).Height;
        }

        private void expirationTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > this.m_LastUpdate.AddSeconds(ProgressBarLabel.m_TimeoutInSeconds))
            {
                this.Reset();
            }
        }
    }
}
