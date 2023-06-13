using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class ScintillaStyleExtensionMethods
    {
        public static ScintillaNET.Style ForeColor(this ScintillaNET.Style that, Color color)
        {
            that.ForeColor = color;
            return that;
        }

        public static ScintillaNET.Style ForeColor(this ScintillaNET.Style that, int rgb)
        {
            return that.ForeColor(ScintillaStyleExtensionMethods.IntToColor(rgb));
        }

        public static ScintillaNET.Style ForeColor(this ScintillaNET.Style that, string hex)
        {
            return that.ForeColor(ScintillaStyleExtensionMethods.HexToColor(hex));
        }

        public static ScintillaNET.Style BackColor(this ScintillaNET.Style that, Color color)
        {
            that.BackColor = color;
            return that;
        }

        public static ScintillaNET.Style BackColor(this ScintillaNET.Style that, int rgb)
        {
            return that.BackColor(ScintillaStyleExtensionMethods.IntToColor(rgb));
        }

        public static ScintillaNET.Style BackColor(this ScintillaNET.Style that, string hex)
        {
            return that.BackColor(ScintillaStyleExtensionMethods.HexToColor(hex));
        }

        public static ScintillaNET.Style Bold(this ScintillaNET.Style that, bool state = true)
        {
            that.Bold = state;
            return that;
        }

        private static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private static Color HexToColor(string hex)
        {
            int rgb = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return ScintillaStyleExtensionMethods.IntToColor(rgb);
        }
    }
}
