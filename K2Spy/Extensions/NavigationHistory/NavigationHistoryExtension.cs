using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace K2Spy.Extensions.NavigationHistory
{
    internal class NavigationHistoryExtension : Model.IInternalExtension, Model.ILeftAlignedCommandBarExtension, Model.ISelectedNodeChangedExtension, Model.ISelectedNodeResetExtension
    {
        #region Private Fields

        private ToolStripButton m_BackButton;
        private ToolStripButton m_ForwardButton;
        private bool m_HistoryTrackingDisabled;

        private List<string> m_Paths = new List<string>();
        private int m_CurrentIndex = -1;

        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Public Properties

        public string DisplayName => "Navigation history";

        #endregion

        #region Public Methods

        public ToolStripItem[] CreateCommandBarItems(K2SpyContext k2SpyContext)
        {
            this.m_K2SpyContext = k2SpyContext;

            this.m_BackButton = new ToolStripButton();
            this.m_BackButton.Image = Properties.Resources.Backwards_16x;
            this.m_BackButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.m_BackButton.Text = "Back";
            this.m_BackButton.Enabled = false;
            this.m_BackButton.Click += async (sender, e) =>
            {
                await k2SpyContext.MainForm.TryAsync(async () =>
                {
                    await this.BackAsync();
                });
            };

            this.m_ForwardButton = new ToolStripButton();
            this.m_ForwardButton.Image = Properties.Resources.Forward_16x;
            this.m_ForwardButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.m_ForwardButton.Text = "Forward";
            this.m_ForwardButton.Enabled = false;
            this.m_ForwardButton.Click += async (sender, e) =>
            {
                await k2SpyContext.MainForm.TryAsync(async () =>
                {
                    await this.ForwardAsync();
                });
            };

            k2SpyContext.Disconnecting += async (sender, e) =>
            {
                k2SpyContext.MainForm.Try(() =>
                {
                    this.Reset();
                });
            };

            this.Reset();
            
            return new ToolStripItem[] { this.m_BackButton, this.m_ForwardButton };
        }

        protected async Task BackAsync()
        {
            this.m_HistoryTrackingDisabled = true;
            try
            {
                while (this.m_CurrentIndex > 0)
                {
                    this.m_CurrentIndex = this.m_CurrentIndex - 1;
                    if (this.m_CurrentIndex + 1 <= this.m_Paths.Count)
                    {
                        string path = this.m_Paths[this.m_CurrentIndex];
                        TreeNode match = null;
                        if (!string.IsNullOrEmpty(path))
                            match = await this.m_K2SpyContext.TreeView.SelectNodeByPathAsync(path, false, true);
                        if (match != null)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
                this.UpdateButtons();
                this.RemoveNullEntries();
            }
            finally
            {
                this.m_HistoryTrackingDisabled = false;
            }
        }

        protected async Task ForwardAsync()
        {
            this.m_HistoryTrackingDisabled = true;
            try
            {
                while (this.m_CurrentIndex + 1 < this.m_Paths.Count)
                {
                    string path = this.m_Paths[this.m_CurrentIndex + 1];
                    this.m_CurrentIndex++;
                    TreeNode match = null;
                    if (!string.IsNullOrEmpty(path))
                        match = await this.m_K2SpyContext.TreeView.SelectNodeByPathAsync(path, false, true);
                    if (match != null)
                        break;
                }
                this.UpdateButtons();
                this.RemoveNullEntries();
            }
            finally
            {
                this.m_HistoryTrackingDisabled = false;
            }
        }

        protected void Reset()
        {
            this.m_Paths.Clear();
            this.m_CurrentIndex = -1;
            this.UpdateButtons();
        }

        protected void RemoveNullEntries()
        {
            if (this.m_CurrentIndex > 0)
            {
                if (this.m_Paths.Any(key => key == null))
                {
                    int numberOfEmptiesBeforeCurrentIndex = this.m_Paths.Take(this.m_CurrentIndex + 1).Where(key => key == null).Count();
                    this.m_CurrentIndex -= numberOfEmptiesBeforeCurrentIndex;
                    this.m_Paths.RemoveAll(key => key == null);
                }
            }
        }

        protected void Register(TreeNode treeNode)
        {
            if (this.m_HistoryTrackingDisabled)
                return;

            this.RemoveNullEntries();
            if (treeNode == null)
            {
                if (this.m_Paths.Count > 0)
                {
                    // ok, we need to inject an empty entry in the list
                    this.m_Paths.Insert(this.m_CurrentIndex + 1, null);
                    this.m_CurrentIndex++;
                }
                //// don't add anything to the collection, but make sure that both btnBack and btnForward are active if relevant
                //string path = this.m_CurrentIndex >= 0 ? this.m_Paths[this.m_CurrentIndex] : null;
                //if (path != null)
                //{

                //}
            }
            else
            {
                string path = treeNode.FullPath;

                if (this.m_CurrentIndex < 0)
                {
                    // ok, we are dealing with an empty list!
                    this.m_Paths.Add(path);
                    this.m_CurrentIndex = 0;
                    return;
                }


                this.m_Paths.RemoveRange(this.m_CurrentIndex + 1, this.m_Paths.Count - (this.m_CurrentIndex + 1));
                string currentPath = this.m_Paths[this.m_CurrentIndex];
                if (currentPath != path)
                {
                    this.m_Paths.Add(path);
                    this.m_CurrentIndex = Math.Max(this.m_CurrentIndex, 0) + 1;
                }
            }

            this.UpdateButtons();
        }

        protected void UpdateButtons()
        {
            this.m_BackButton.Enabled = this.m_CurrentIndex > 0;
            this.m_ForwardButton.Enabled = this.m_CurrentIndex >= 0 && this.m_CurrentIndex + 1 < this.m_Paths.Count;
        }

        public void SelectedNodeChanged(TreeNode treeNode)
        {
            this.Register(treeNode);
        }

        public void SelectedNodeReset()
        {
            this.SelectedNodeChanged(null);
        }

        #endregion
    }
}
