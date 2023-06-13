using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Bookmarks
{
    internal partial class ManageBookmarks : BaseForm
    {
        #region Private Fields

        private K2SpyContext m_K2SpyContext;
        private string m_SelectedPath;
        private Bookmarks m_Bookmarks;

        #endregion

        #region Constructors

        public ManageBookmarks(K2SpyContext context, Bookmarks bookmarks)
        {
            InitializeComponent();

            this.m_K2SpyContext = context;
            this.m_Bookmarks = bookmarks;
            this.m_SelectedPath = context.TreeView.SelectedNode?.FullPath;
            if (!string.IsNullOrEmpty(this.m_SelectedPath) && !this.m_Bookmarks.IsBookmarked(this.m_SelectedPath))
                this.btnAdd.Enabled = true;
        }

        #endregion

        #region Protected Methods

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.PopulateBookmarks();
        }

        protected void PopulateBookmarks()
        {
            this.lbBookmarks.Items.Clear();
            this.lbBookmarks.Items.AddRange(this.m_Bookmarks.GetAllBookmarks().OrderBy(key => key).ToArray());
        }

        #endregion

        #region Private Methods

        private async void btnGoTo_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                await this.m_K2SpyContext.TreeView.SelectNodeByPathAsync(this.lbBookmarks.SelectedItem as string);
            });
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.m_Bookmarks.Remove(this.lbBookmarks.SelectedItem as string);
                this.lbBookmarks.Items.Remove(this.lbBookmarks.SelectedItem);
            });
        }

        private void lbBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnGoTo.Enabled = this.btnRemove.Enabled = this.lbBookmarks.SelectedItem != null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.m_Bookmarks.Add(this.m_SelectedPath);
                this.PopulateBookmarks();
            });
        }

        private void lbBookmarks_DoubleClick(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                if (this.lbBookmarks.SelectedItem != null)
                {
                    this.btnGoTo.PerformClick();
                }
            });
        }

        #endregion
    }
}
