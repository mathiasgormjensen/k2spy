using K2Spy.ExtensionMethods;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExternalServiceInstanceResources
{
    partial class ViewExternalServiceInstanceResources
    {
        private class MatchData
        {
            public MatchData(ServiceInstanceTreeNode source, string resource)
            {
                this.Source = source;
                this.Name = source.Text;
                this.Path = source.FullPath;
                this.Resource = resource;
            }

            public string Name { get; private set; }
            public string Resource { get; private set; }
            public string Path { get; private set; }

            public ServiceInstanceTreeNode Source { get; private set; }

            public ListViewItem CreateListViewItem()
            {
                ListViewItem item = new ListViewItem();
                item.Text = this.Name;
                item.ImageIndexFrom(this.Source);
                item.SubItems.Add(this.Resource);
                item.SubItems.Add(this.Path);
                item.Tag = this.Source;
                return item;
            }
        }
    }
}
