using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;
using System.Collections;

namespace K2Spy
{
    public class InvokeOnUIThreadActionQueue
    {
        #region Private Fields

        private Control m_Owner;
        private Timer m_Timer;
        private Dictionary<object, Action> m_Actions = new Dictionary<object, Action>();

        #endregion

        #region Constructors

        public InvokeOnUIThreadActionQueue(Control owner)
        {
            this.m_Owner = owner;
            this.m_Timer = new Timer();
            this.m_Timer.Interval = Math.Min(1000, Math.Max(100, Properties.Settings.Default.InvokeOnUIThreadActionQueueTickInterval));
            this.m_Timer.Tick += this.timer_Tick;
        }

        #endregion

        #region Public Methods

        public void Remove(object key)
        {
            lock (this.m_Actions)
                this.m_Actions.Remove(key);
        }

        public void QueueOnce(object key, Action action)
        {
            if (key == null)
                key = new object();
            lock (this.m_Actions)
                this.m_Actions[key] = action;
            
            if (!this.m_Timer.Enabled)
            {
                this.m_Owner.InvokeIfRequired(this.m_Timer.Start);
            }
        }

        public void Queue(Action action)
        {
            this.QueueOnce(Guid.NewGuid(), action);
        }

        #endregion

        #region Private Methods

        private void timer_Tick(object sender, EventArgs e)
        {
            Trier.Try(() =>
            {
                //Console.WriteLine($"{DateTime.Now}: Tick!");
                Dictionary<object,Action> dictionary = this.m_Actions;
                Action[] actionsToPerform = null;
                lock (this.m_Actions)
                {
                    this.m_Actions = new Dictionary<object, Action>();
                    actionsToPerform = dictionary.Values.OfType<Action>().ToArray();
                }

                //Console.WriteLine("Total action count: " + this.m_ActionCount + ", this run: " + actionsToPerform.Length);

                if (actionsToPerform.Length == 0 && this.m_Actions.Count == 0)
                    this.m_Timer.Stop();
                // this.m_Owner.InvokeIfRequired(this.m_Timer.Stop);
                if (actionsToPerform.Length > 0)
                {
                    foreach (Action action in actionsToPerform)
                    {
                        action();
                    }
                }
            });
        }

        #endregion
    }
}
