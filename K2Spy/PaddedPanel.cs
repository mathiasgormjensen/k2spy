using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class PaddedPanel : System.Windows.Forms.Panel
    {
        private bool m_CollapseBottom;

        public PaddedPanel()
        {
            this.UpdatePadding();
            base.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object Padding { get; private set; }

        [DefaultValue(false)]
        public bool CollapseBottom
        {
            get { return this.m_CollapseBottom; }
            set
            {
                this.m_CollapseBottom = value;
                this.UpdatePadding();
            }
        }

        protected void UpdatePadding()
        {
            if (this.m_CollapseBottom)
            {
                base.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            }
            else
            {
                base.Padding = new System.Windows.Forms.Padding(5);
            }
        }
    }
}