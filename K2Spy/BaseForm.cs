using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public class BaseForm : System.Windows.Forms.Form
    {
        public BaseForm()
        {
            this.ApplyDefaultFont();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new System.Drawing.Size MinimumSize { get; private set; }

        //public new void ShowDialog(IWin32Window owner)
        //{
        //}

        //public new void ShowDialog()
        //{
        //}

        //public new void Show(IWin32Window owner)
        //{
        //}

        //public new void Show()
        //{
        //}

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime && !base.DesignMode)
            {
                if (base.MinimumSize.IsEmpty)
                    base.MinimumSize = base.Size;
            }
        }
    }
}
