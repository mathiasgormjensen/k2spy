using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class ListViewUpdateContext : IDisposable
    {
        #region Private Fields

        private System.Windows.Forms.ListView m_ListView;

        #endregion

        #region Constructors

        public ListViewUpdateContext(System.Windows.Forms.ListView listView)
        {
            this.m_ListView = listView;
            this.m_ListView.BeginUpdate();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this.m_ListView.EndUpdate();
        }

        #endregion
    }
}