using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class DoubleBufferedListView : System.Windows.Forms.ListView
    {
        public DoubleBufferedListView()
        {
            base.DoubleBuffered = true;
        }
    }
}
