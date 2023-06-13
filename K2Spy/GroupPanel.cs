using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class GroupPanel : System.Windows.Forms.Panel
    {
        public GroupPanel()
        {
            //base.Padding = new System.Windows.Forms.Padding(1);
            base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            base.Padding = new System.Windows.Forms.Padding(1);
            base.BackColor = Color.White;
            base.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object Padding { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object BorderStyle { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object BackColor { get; private set; }
    }
}
