using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class DirectoryInfoExtensionMethods
    {
        public static void DeleteContents(this System.IO.DirectoryInfo that)
        {
            that.GetFiles().ToList().ForEach(key => key.Delete());
            that.GetDirectories().ToList().ForEach(key => key.Delete(true));
        }
    }
}
