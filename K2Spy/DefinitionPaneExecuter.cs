using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class DefinitionPaneExecuter
    {
        #region Private Fields

        private bool m_Active = false;
        private Action m_Action;

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the specified action immediately if the definition pane is active, otherwise executes it once the definition pane is active
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteIfOrWhenActive(Action action)
        {
            if (this.m_Active)
            {
                action();
                this.m_Action = null;
            }
            else
            {
                this.m_Action = action;
            }
        }

        public void OnActivateDefinitionPane()
        {
            this.m_Active = true;
            this.m_Action?.Invoke();
            this.m_Action = null;
        }

        public void OnDeactivateDefinitionPane()
        {
            this.m_Active = false;
        }

        public void OnOpenDefinitionPane()
        {
        }

        public void OnCloseDefinitionPane()
        {
            this.m_Active = false;
        }

        #endregion
    }
}