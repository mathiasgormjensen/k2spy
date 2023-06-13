using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2Spy
{
    [Obsolete("", true)]
    public class AsyncActionEventArgs : EventArgs
    {
        #region Private Fields

        private List<Func<Task>> m_Actions = new List<Func<Task>>();

        #endregion

        #region Public Methods

        public void AddAction(Func<Task> action)
        {
            this.m_Actions.Add(action);
        }

        #endregion

        #region Internal Methods

        internal async Task RunAllAsync()
        {
            await Task.WhenAll(this.m_Actions.Select(key => key()).ToArray());
        }

        #endregion
    }
}