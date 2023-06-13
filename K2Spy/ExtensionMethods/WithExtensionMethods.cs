using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class WithExtensionMethods
    {
        public static T With<T>(this T that, Action<T> action)
        {
            action(that);
            return that;
        }
    }
}
