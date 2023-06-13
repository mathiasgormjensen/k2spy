// #define POPULATE_TREE_IN_PARALLEL
using System;
using System.Windows.Forms;

namespace K2Spy
{
    public class WaitCursorContext : IDisposable
    {
        private Control m_Control;
        private Cursor m_State;

        public WaitCursorContext(Control control)
        {
            this.m_Control = control;
            if (control != null)
            {
                this.m_State = control.Cursor;
                control.Cursor = Cursors.WaitCursor;
            }
        }

        public void Dispose()
        {
            if (this.m_Control != null)
            {
                this.m_Control.Cursor = this.m_State;
            }
        }
    }
}