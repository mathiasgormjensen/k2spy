using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class Disposer : IDisposable
    {
        private Action m_Dispose;
        public Disposer(Action dispose)
        {
            this.m_Dispose = dispose;
        }

        public void Dispose()
        {
            this.m_Dispose?.Invoke();
        }
    }
}