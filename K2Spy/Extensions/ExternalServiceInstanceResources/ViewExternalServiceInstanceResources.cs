using K2Spy.ExtensionMethods;
using SourceCode.SmartObjects.Services.Management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    internal partial class ViewExternalServiceInstanceResources : BaseForm
    {
        #region Private Fields

        private K2SpyContext m_K2SpyContext;
        private ListViewSortableVirtualList<MatchData> m_VirtualList;
        private System.Threading.CancellationTokenSource m_CancellationTokenSource;

        #endregion

        #region Constructors

        public ViewExternalServiceInstanceResources(K2SpyContext k2SpyContext)
        {
            InitializeComponent();
            this.m_K2SpyContext = k2SpyContext;
            this.progressBarLabel1.Initialize(k2SpyContext);
            this.listView1.SmallImageList = k2SpyContext.TreeView.ImageList;

            this.m_VirtualList = new ListViewSortableVirtualList<MatchData>(this.m_K2SpyContext, this.listView1, (items, columnIndex, sortOrder) =>
            {
                if (sortOrder != SortOrder.None)
                {
                    if (columnIndex == 0)
                        items = items.OrderBy(key => key.Name).ToArray();
                    else if (columnIndex == 1)
                        items = items.OrderBy(key => key.Resource).ToArray();
                    else if (columnIndex == 2)
                        items = items.OrderBy(key => key.Path).ToArray();

                    if (sortOrder == SortOrder.Descending)
                        items = items.Reverse().ToArray();
                }
                return items;
            }, node => node.CreateListViewItem());
        }

        #endregion

        #region Protected Methods

        protected async override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            string[][] compoundResourceNames = @"Server|Database
sqlconnection
assembly full name
service endpoint url
webservice url
descriptor location".Split(@"
".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
.Select(key => key.Split("|;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
.OrderByDescending(key => key.Length)
.ToArray();

            Dictionary<TreeNode, string> resourceDictionary = new Dictionary<TreeNode, string>();
            ServiceManagementServer serviceManagementServer = (await this.m_K2SpyContext.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<ServiceManagementServer>());
            TreeViewSearchSettings treeViewSearchSettings = new TreeViewSearchSettings(
                async node => node is ServiceTypesRootTreeNode || node is ServiceTypeTreeNode,
                async node => node is ServiceInstanceTreeNode,
                async node =>
                {
                    return await Task.Run(() =>
                    {
                        if (node is ServiceInstanceTreeNode serviceInstanceTreeNode)
                        {
                            string xml = serviceManagementServer.GetExistingServiceInstanceConfiguration(serviceInstanceTreeNode.ServiceInstanceGuid);
                            SettingKeyInfo[] settingKeyInfos = ServiceConfigInfo.Create(xml).ConfigSettings.ToArray();
                            string[] resourceNames = settingKeyInfos?.Select(key => key.Name).ToArray();
                            if (resourceNames != null)
                            {
                                foreach (string[] compoundResourceName in compoundResourceNames)
                                {
                                    Dictionary<string, SettingKeyInfo> dictionary = new Dictionary<string, SettingKeyInfo>();
                                    foreach (string name in compoundResourceName)
                                        dictionary.Add(name, settingKeyInfos.FirstOrDefault(key => key.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

                                    if (!dictionary.ContainsValue(null))
                                    {
                                        // we have a match!
                                        resourceDictionary[node] = String.Join(", ", dictionary.Values.Select(key => $"{key.Name}: {key.Value}"));
                                        return true;
                                    }
                                }
                            }
                        }
                        return false;
                    });
                });
            System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
            this.m_CancellationTokenSource = cancellationTokenSource;
            await K2SpyTreeViewSearcher.PerformSearchAsync(this.m_K2SpyContext, treeViewSearchSettings, async node =>
            {
                this.m_K2SpyContext.ActionQueue.Queue(() =>
                {
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        string resource;
                        resourceDictionary.TryGetValue(node, out resource);
                        this.m_VirtualList.Add(new MatchData((ServiceInstanceTreeNode)node, resource));
                    }
                });
            }, (status, percentage) =>
            {
                this.progressBarLabel1.QueueUpdate(status, percentage);
            }, cancellationTokenSource.Token);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.m_CancellationTokenSource?.Cancel();
        }

        #endregion

        #region Private Methods

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Try(() =>
            {
                ServiceInstanceTreeNode serviceInstanceTreeNode = this.m_VirtualList.SelectedItem?.Source;
                if (serviceInstanceTreeNode != null)
                    this.m_K2SpyContext.TreeView.SelectNode(serviceInstanceTreeNode, true);
            });
        }

        #endregion
    }
}