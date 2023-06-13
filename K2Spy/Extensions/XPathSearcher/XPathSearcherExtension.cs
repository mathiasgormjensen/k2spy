using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace K2Spy.Extensions.XPathSearcher
{
    //[Model.IgnoreExtension]
    internal class XPathSearcherExtension : Model.IToolsMenuExtension, Model.ICommandBarExtension
    {
        #region Public Properties

        public string DisplayName => "XPath searcher";

        #endregion

        #region Public Methods

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext k2SpyContext)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("Show XPath searcher", Properties.Resources.XMLIntelliSenseDescendant_16x, (sender, e) =>
                {
                    XPathSearcher dlg = new XPathSearcher(this, k2SpyContext);
                    dlg.Show();
                })
            };
        }

        public ToolStripItem[] CreateCommandBarItems(K2SpyContext k2SpyContext)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("Show XPath searcher", Properties.Resources.XMLIntelliSenseDescendant_16x, (sender, e) =>
                {
                    k2SpyContext.MainForm.Try(() =>
                    {
                        XPathSearcher dlg = new XPathSearcher(this, k2SpyContext);
                        dlg.Show();
                    });
                }).With(key => key.DisplayStyle = ToolStripItemDisplayStyle.Image)
            };
        }

        #endregion
    }
}
