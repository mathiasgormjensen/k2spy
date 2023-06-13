using K2Spy.ExtensionMethods;
using SourceCode.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.CacheOptions
{
    public partial class CacheOptionsControl : UserControl
    {
        #region Private Fields

        private K2SpyContext m_K2SpyContext;
        private System.Threading.CancellationTokenSource m_CancellationTokenSource;

        #endregion

        #region Constructors

        private CacheOptionsControl()
        {
            InitializeComponent();

            base.Disposed += (sender, e) => this.m_CancellationTokenSource?.Cancel();
        }

        public CacheOptionsControl(K2SpyContext k2SpyContext)
            : this()
        {
            this.m_K2SpyContext = k2SpyContext;
        }

        #endregion

        #region Public Properties

        public bool IsDirty
        {
            get { return Properties.Settings.Default.KeepXPathDocumentsInMemory != this.chkKeepXPathDocumentsInMemory.Checked; }
        }

        #endregion

        #region Public Methods

        public void Commit()
        {
            Properties.Settings.Default.KeepXPathDocumentsInMemory = this.chkKeepXPathDocumentsInMemory.Checked;
            if (!Properties.Settings.Default.KeepXPathDocumentsInMemory)
            {
                K2SpyTreeNodeExtensionMethods.ClearCache();
                GC.Collect();
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.chkKeepXPathDocumentsInMemory.Checked = Properties.Settings.Default.KeepXPathDocumentsInMemory;

            base.Dock = DockStyle.Top;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.AutoSize = true;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            this.chkKeepXPathDocumentsInMemory.AutoEllipsis = false;
            this.chkKeepXPathDocumentsInMemory.AutoEllipsis = true;
        }

        #endregion

        #region Private Methods

        private void lnkCacheInformation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Try(() =>
            {
                string path = Configuration.Directory;
                System.Diagnostics.Process.Start(path);
            });
        }

        private async void btnClearCache_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (MessageBox.Show(this, @"Clearing the cache may cause certain aspects of the application to perform slower than usual, until the cache is restored.
Do you want to continue?", "Clear cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnStopPreload.PerformClick();
                    await Task.Run(() =>
                    {
                        this.m_K2SpyContext.Cache.ClearDiskCache();
                        Extensions.ExtensionsManager.GetExtensions<Model.IClearDiskCacheExtension>().ToList().ForEach(key => key.ClearDiskCache());
                    });
                }
            });
        }

        private async void btnClearInmemoryCache_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                await this.m_K2SpyContext.Cache.ClearAsync();
            });
        }

        private void btnClearConnections_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.m_K2SpyContext.ConnectionFactory.Clear();
            });
        }

        private void btnGC_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                GC.Collect();
                GC.Collect(0, GCCollectionMode.Forced, true);
            });
        }

        #endregion

        private async void btnPreload_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                if (MessageBox.Show(this, "Preloading the cache may take a few minutes depending on network speed and the amount data that needs to be cached.", "Preload", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    DateTime begin = DateTime.Now;
                    object x = new object();
                    this.m_CancellationTokenSource?.Cancel();
                    System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
                    this.m_CancellationTokenSource = cancellationTokenSource;
                    Model.IPreloadExtension[] preloadExtensions = ExtensionsManager.GetExtensions<Model.IPreloadExtension>(false)
                        .OrderByDescending(key => key.PreloadPriority)
                        .ToArray();
                    this.pbOverall.Maximum = preloadExtensions.Length * 100;
                    this.lblOverallProgress.Text = "";
                    this.lblSpecificProgress.Text = "";
                    this.pbOverall.Value = 0;
                    this.pbSpecific.Value = 0;
                    try
                    {
                        for (int i = 0; i < preloadExtensions.Length; i++)
                        {
                            if (cancellationTokenSource.IsCancellationRequested || this.m_CancellationTokenSource.IsCancellationRequested)
                                break;

                            Model.IPreloadExtension preloadExtension = preloadExtensions[i];

                            this.m_K2SpyContext.ActionQueue.Queue(() =>
                            {
                                if (!cancellationTokenSource.IsCancellationRequested)
                                {
                                    this.pbOverall.Visible = true;
                                    this.pbSpecific.Visible = true;
                                    this.lblOverallProgress.Visible = true;
                                    this.lblSpecificProgress.Visible = true;
                                    this.btnStopPreload.Enabled = true;

                                    if (!string.IsNullOrEmpty(preloadExtension.PreloaderDescription))
                                        this.lblOverallProgress.Text = preloadExtension.PreloaderDescription;
                                    else
                                        this.lblOverallProgress.Text = $"Preloading {preloadExtension.DisplayName}...";
                                    this.lblSpecificProgress.Text = "";
                                    this.pbSpecific.Value = 0;
                                }
                            });
                            int overallOffset = i * 100;
                            await Task.Run(() => preloadExtension.PerformPreloadAsync(this.m_K2SpyContext, (a, b) =>
                            {
                                this.m_K2SpyContext.ActionQueue.QueueOnce(x, () =>
                                {
                                    if (!cancellationTokenSource.IsCancellationRequested)
                                    {
                                        this.pbOverall.Value = overallOffset + b;
                                        this.lblSpecificProgress.Text = a;
                                        this.pbSpecific.Value = b;
                                    }
                                });
                            }, cancellationTokenSource.Token));
                        }
                    }
                    finally
                    {
                        this.pbOverall.Visible = false;
                        this.pbSpecific.Visible = false;
                        this.lblOverallProgress.Visible = false;
                        this.lblSpecificProgress.Visible = false;
                        this.btnStopPreload.Enabled = false;
                        GC.Collect();
                        GC.Collect(0, GCCollectionMode.Forced, true);
                    }
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        TimeSpan elapsedTime = DateTime.Now.Subtract(begin);
                        elapsedTime = elapsedTime;
                        MessageBox.Show(this, $"Preload completed in {elapsedTime}", "Preload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            });
        }

        private void btnStopPreload_Click(object sender, EventArgs e)
        {
            this.m_CancellationTokenSource?.Cancel();
        }
    }
}
