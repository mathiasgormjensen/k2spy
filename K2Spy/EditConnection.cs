using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    public partial class EditConnection : BaseForm
    {
        public EditConnection(string connectionString = "")
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                base.Text = "Edit connection";
                SCConnectionStringBuilder sCConnectionStringBuilder = new SCConnectionStringBuilder(connectionString);
                this.txtServerName.Text = sCConnectionStringBuilder.Host;
                this.txtServerPort.Text = sCConnectionStringBuilder.Port.ToString();
                this.chkIntegratedSecurity.Checked = sCConnectionStringBuilder.Integrated;
                this.txtUserName.Text = sCConnectionStringBuilder.UserID;
                this.txtUserPassword.Text = sCConnectionStringBuilder.Password;
                this.txtWindowsDomain.Text = sCConnectionStringBuilder.WindowsDomain;
                this.txtLabel.Text = sCConnectionStringBuilder.SecurityLabelName;
            }
            else
            {
                base.Text = "Add connection";
            }
        }

        public string BuildConnectionString()
        {
            return this.BuildConnectionStringBuilder().ConnectionString;
        }

        public SCConnectionStringBuilder BuildConnectionStringBuilder()
        {
            SCConnectionStringBuilder builder = new SCConnectionStringBuilder();
            builder.Host = this.txtServerName.Text;
            builder.Port = uint.Parse(this.txtServerPort.Text);
            builder.Integrated = this.chkIntegratedSecurity.Checked;
            if (!builder.Integrated)
            {
                builder.UserID = this.txtUserName.Text;
                builder.Password = this.txtUserPassword.Text;
                builder.WindowsDomain = this.txtWindowsDomain.Text;
                builder.SecurityLabelName = this.txtLabel.Text;
            }
            builder.IsPrimaryLogin = true;
            return builder;
        }

        private void chkIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            this.lblUserName.Enabled =
                this.lblUserPassword.Enabled =
                this.lblWindowsDomain.Enabled =
                this.lblLabel.Enabled =
                this.txtUserName.Enabled =
                this.txtUserPassword.Enabled =
                this.txtWindowsDomain.Enabled =
                this.txtLabel.Enabled = !this.chkIntegratedSecurity.Checked;
        }
    }
}
