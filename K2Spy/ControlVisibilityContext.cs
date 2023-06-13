using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class ControlVisibilityContext : IDisposable
    {
        private bool m_State;
        private System.Windows.Forms.Control m_Control;

        public ControlVisibilityContext(System.Windows.Forms.Control control, bool visible, bool? restoreState=null)
        {
            this.m_Control = control;
            this.m_State = restoreState.HasValue ? restoreState.Value : control.Visible;
            this.m_Control.Visible = visible;
        }

        public void Dispose()
        {
            this.m_Control.Visible = this.m_State;
        }
    }
}
