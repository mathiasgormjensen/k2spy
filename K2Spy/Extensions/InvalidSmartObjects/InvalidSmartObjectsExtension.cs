using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.InvalidSmartObjects
{
    internal class InvalidSmartObjectsExtension : Model.IToolsMenuExtension
    {
        public string DisplayName => "Invalid SmartObjects";

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext k2SpyContext)
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem("Show invalid SmartObjects", Properties.Resources.StatusInvalidOutline_16x, (sender, e) =>
                {
                    k2SpyContext.MainForm.Try(() =>
                    {
                        InvalidSmartObjects dialog = new InvalidSmartObjects(k2SpyContext);
                        dialog.Show();
                    });
                })
            };
        }
    }
}
