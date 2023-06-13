using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ToolStripItemExtensionMethods
    {
        public static void ApplyDefaultFont(this System.Windows.Forms.ToolStripItem that, bool recursive = true, bool preserveStyle = true)
        {
            that.UpdateFont(Fonts.DefaultFont, recursive, preserveStyle);
        }

        public static void UpdateFont(this System.Windows.Forms.ToolStripItem that, Font font, bool recursive = true, bool preserveStyle = true)
        {
            that.Font = new Font(font.FontFamily, font.Size, preserveStyle ? that.Font.Style : font.Style);
            if (recursive)
            {
                if (that is System.Windows.Forms.ToolStripDropDownItem)
                {
                    ((System.Windows.Forms.ToolStripDropDownItem)that).DropDownItems.UpdateFont(font, recursive, preserveStyle);
                }
            }
        }
    }
}
