using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class LayoutSuspender : IDisposable
    {
        #region Private Fields

        private System.Windows.Forms.Control m_Control;

        #endregion

        #region Constructors

        public LayoutSuspender(System.Windows.Forms.Control control)
        {
            this.m_Control = control;
            this.m_Control?.SuspendLayout();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this.m_Control?.ResumeLayout();
        }

        #endregion
    }
}