using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ToolStripItemCollectionExtensionMethods
    {
        public static void ApplyDefaultFont(this System.Windows.Forms.ToolStripItemCollection that, bool recursive = true, bool preserveStyle = true)
        {
            that.UpdateFont(Fonts.DefaultFont, recursive, preserveStyle);
        }

        public static void UpdateFont(this System.Windows.Forms.ToolStripItemCollection that, Font font, bool recursive = true, bool preserveStyle = true)
        {
            foreach (System.Windows.Forms.ToolStripItem item in that)
            {
                item.UpdateFont(font, recursive, preserveStyle);
            }
        }
    }
}
