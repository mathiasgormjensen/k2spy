using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    internal partial class ManageConnections : BaseForm
    {
        #region Private Fields

        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Constructors

        public ManageConnections(K2SpyContext k2SpyContext = null)
        {
            this.m_K2SpyContext = k2SpyContext;
            InitializeComponent();
        }

        #endregion

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Connection[] items = Connections.Default.Items?.OrderBy(key => key.FullDisplayName).ToArray();
            if (items != null)
                this.lbConnections.Items.AddRange(items);
        }

        #endregion

        private void lbConnections_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void lbConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnConnect.Enabled =
                this.btnRemove.Enabled =
                this.btnEdit.Enabled = this.lbConnections.SelectedItem != null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (EditConnection dlg = new EditConnection())
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Connection connection = new Connection(dlg.BuildConnectionString());
                        Connections.Default.Add(connection);
                        this.lbConnections.Items.Add(connection);
                    }
                }
            });
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                Connection item = (Connection)this.lbConnections.SelectedItem;
                using (EditConnection dlg = new EditConnection(item.ConnectionString))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        item.ConnectionString = dlg.BuildConnectionString();
                        this.lbConnections.Invalidate();
                        this.lbConnections.Refresh();
                        this.lbConnections.Update();
                        this.lbConnections.Hide();
                        this.lbConnections.Show();
                        string displayMember = this.lbConnections.DisplayMember;
                        this.lbConnections.DisplayMember = "";
                        this.lbConnections.DisplayMember = displayMember;
                        // System.Windows.Forms.MessageBox.Show(item.ShortDisplayName);
                        Connections.Default.QueueSave();
                    }
                }
            });
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                Connection item = (Connection)this.lbConnections.SelectedItem;
                if (MessageBox.Show(this, $"Are you sure you want to remove {item.ShortDisplayName}?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    this.lbConnections.Items.Remove(item);
                    Connections.Default.Remove(item);
                }
            });

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                Connection item = (Connection)this.lbConnections.SelectedItem;
                Connections.Default.Items.ToList().ForEach(key => key.Selected = key == item);
                await this.m_K2SpyContext.ConnectAsync(item.ConnectionString);
                base.Close();
            });
        }
    }
}