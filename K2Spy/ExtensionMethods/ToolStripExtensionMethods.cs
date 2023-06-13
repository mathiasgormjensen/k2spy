using System.Drawing;

namespace K2Spy.ExtensionMethods
{
    public static class ToolStripExtensionMethods
    {
        public static void ApplyDefaultFont(this System.Windows.Forms.ToolStrip that, bool recursive = true, bool preserveStyle = true)
        {
            that.UpdateFont(Fonts.DefaultFont, recursive, preserveStyle);
        }

        public static void UpdateFont(this System.Windows.Forms.ToolStrip that, Font font, bool recursive = true, bool preserveStyle = true)
        {
            that.Font = new Font(font.FontFamily, font.Size, preserveStyle ? that.Font.Style : font.Style);
            if (recursive)
            {
                that.Items.UpdateFont(font, recursive, preserveStyle);
            }
        }
    }
}
