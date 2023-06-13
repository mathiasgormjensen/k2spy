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
    internal partial class EditText : BaseForm
    {
        private TextBox m_TextBox;

        public EditText(TextBox textBox)
        {
            this.m_TextBox = textBox;
            InitializeComponent();
            this.textBox.Text = this.m_TextBox.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.m_TextBox.Text = this.textBox.Text;
        }
    }
}
