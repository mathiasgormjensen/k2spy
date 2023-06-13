using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class CacheEventArgs<T> : EventArgs
    {
        public CacheEventArgs(T cacheKey)
        {
            this.CacheKey = cacheKey;
        }

        public T CacheKey { get; private set; }
    }
}
