using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    internal partial class EditHyperlink : BaseForm
    {
        public EditHyperlink()
        {
            InitializeComponent();
        }

        public string HyperlinkAsString
        {
            get { return new HyperlinkObject() { Display = this.txtDisplayName.Text, Link = this.txtLink.Text }.Serialize(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.txtDisplayName.ResetText();
                    this.txtLink.ResetText();
                }
                else
                {
                    HyperlinkObject hyperlink = HyperlinkObject.Deserialize(value);
                    this.txtDisplayName.Text = hyperlink.Display;
                    this.txtLink.Text = hyperlink.Link;
                }
            }
        }
    }
}
