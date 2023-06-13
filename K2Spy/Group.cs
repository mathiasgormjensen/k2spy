using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace K2Spy
{
    public class Group : Panel
    {
        public Group()
        {
            base.BackColor = Color.White;
            base.Padding = new Padding(1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object Padding { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object BackColor { get; private set; }

        protected override ControlCollection CreateControlsInstance()
        {
            return base.CreateControlsInstance();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, base.ClientSize.Width - 1, base.ClientSize.Height - 1));
        }
    }
}
