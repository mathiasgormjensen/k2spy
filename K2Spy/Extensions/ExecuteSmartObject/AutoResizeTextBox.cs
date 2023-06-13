using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    public class AutoResizeTextBox : TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (base.TextLength > 100 || base.Lines.Length > 1)
            {
                base.Multiline = true;
                base.Height = base.PreferredHeight * 5;
                base.ScrollBars = ScrollBars.Both;
            }
            else
            {
                base.ScrollBars = ScrollBars.None;
                base.Multiline = true;
                base.Height = base.PreferredHeight;
            }
        }
    }
}
