// #define POPULATE_TREE_IN_PARALLEL
using System;
using System.Windows.Forms;

namespace K2Spy
{
    public class DisableControlContext : IDisposable
    {
        #region Private Fields

        private Control m_Control;
        private ToolStripItem m_ToolStripItem;
        private bool m_State;
        private WaitCursorContext m_UseWaitCursorContext;

        #endregion

        #region Constructors

        public DisableControlContext(ToolStripItem toolStripItem, bool useWaitCursorContext = true)
        {
            this.m_ToolStripItem = toolStripItem;
            this.m_State = toolStripItem.Enabled;
            this.m_ToolStripItem.Enabled = false;
            if (useWaitCursorContext)
                this.m_UseWaitCursorContext = new WaitCursorContext(toolStripItem.GetCurrentParent()?.FindForm());
            Application.DoEvents();
        }

        public DisableControlContext(Control control, bool useWaitCursorContext = true)
        {
            this.m_Control = control;
            this.m_State = control.Enabled;
            this.m_Control.Enabled = false;
            this.m_UseWaitCursorContext = new WaitCursorContext(control.FindForm());
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this.m_UseWaitCursorContext?.Dispose();
            if (this.m_Control != null)
                this.m_Control.Enabled = this.m_State;
            if (this.m_ToolStripItem != null)
                this.m_ToolStripItem.Enabled = this.m_State;
            Application.DoEvents();
        }

        #endregion
    }
}