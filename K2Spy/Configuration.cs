using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public static class Configuration
    {
        public static string Directory
        {
            get
            {
                string path = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K2Spy");
                System.IO.Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}