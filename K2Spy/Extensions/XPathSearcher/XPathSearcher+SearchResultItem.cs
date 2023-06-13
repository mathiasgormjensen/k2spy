using K2Spy.ExtensionMethods;
using System.Windows.Forms;
using System.Xml;

namespace K2Spy.Extensions.XPathSearcher
{
    partial class XPathSearcher
    {
        private class SearchResultItem
        {
            private int m_ImageIndex = -1;

            private class XmlLineInfo : IXmlLineInfo
            {
                public XmlLineInfo(int lineNumber, int linePosition)
                {
                    this.LineNumber = lineNumber;
                    this.LinePosition = linePosition;
                }

                public int LineNumber { get; private set; }

                public int LinePosition { get; private set; }

                public bool HasLineInfo()
                {
                    return true;
                }
            }

            public SearchResultItem(K2SpyTreeNode node, object value, ImageList imageList)
            {
                this.K2SpyTreeNode = node;
                //this.Value = value;
                //this.XPathItem = value as System.Xml.XPath.XPathItem;
                //this.LineInfo = this.XPathItem as System.Xml.IXmlLineInfo;

                this.NameColumn = node.Text;

                if (value is System.Xml.XPath.XPathNavigator navigator)
                {
                    this.ResultColumn = $"{navigator.NodeType}: {navigator.OuterXml}";
                }
                else
                {
                    this.ResultColumn = value?.ToString();
                }
                if (this.ResultColumn?.Length > 150)
                    this.ResultColumn = this.ResultColumn.Substring(0, 150) + " (truncated...)";

                if (value is IXmlLineInfo lineInfo)
                {
                    this.LineInfo = new XmlLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
                    this.PositionColumn = lineInfo.LineNumber + ", " + lineInfo.LinePosition;
                }

                this.PathColumn = node.FullPath;

                //if (!string.IsNullOrEmpty(node.ImageKey))
                //    this.m_ImageIndex = imageList.Images.IndexOfKey(node.ImageKey);
            }

            public string NameColumn { get; private set; }

            public string ResultColumn { get; private set; }

            public string PositionColumn { get; private set; }

            public string PathColumn { get; private set; }

            public System.Xml.IXmlLineInfo LineInfo { get; private set; }

            public K2SpyTreeNode K2SpyTreeNode { get; private set; }

            public ListViewItem CreateListViewItem()
            {
                ListViewItem item = new ListViewItem(new string[] { this.NameColumn, this.ResultColumn, this.PositionColumn, this.PathColumn }/*, this.m_ImageIndex*/) { Tag = this };
                item.ImageIndexFrom(this.K2SpyTreeNode);
                return item;
            }
        }
    }
}
