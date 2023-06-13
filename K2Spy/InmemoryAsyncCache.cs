using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    [System.Diagnostics.DebuggerDisplay("InmemoryAsyncCache<{typeof(TKey).FullName,nq}, {typeof(TValue).FullName,nq}>, Count={Count}")]
    public class InmemoryAsyncCache<TKey, TValue>
    {
        #region Private Fields

        private Func<TKey, Task<TValue>> m_AsyncValueFactory;
        private Func<TKey, TValue> m_ValueFactory;
        private System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> m_Dictionary = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>();

        #endregion

        #region Constructors

        public InmemoryAsyncCache(Func<TKey, Task<TValue>> asyncValueFactory)
        {
            this.m_AsyncValueFactory = asyncValueFactory;
        }

        public InmemoryAsyncCache(Func<TKey, TValue> valueFactory)
        {
            this.m_ValueFactory = valueFactory;
        }

        #endregion

        #region Public Events

        //public event EventHandler<EvictedEventArgs<TKey>> Evicted;

        #endregion

        #region Public Properties

        public int Count
        {
            get { return this.m_Dictionary.Count; }
        }

        #endregion

        #region Public Methods

        public KeyValuePair<TKey,TValue>[] GetAll()
        {
            return this.m_Dictionary.ToArray();
        }

        public TKey[] GetAllKeys()
        {
            return this.m_Dictionary.Keys.ToArray();
        }

        public TValue[] GetAllValues()
        {
            return this.m_Dictionary.Values.ToArray();
        }

        public void Clear()
        {
            this.m_Dictionary.Clear();
        }

        public bool Evict(TKey key)
        {
            TValue ignore;
            bool result = this.m_Dictionary.TryRemove(key, out ignore);
            //this.Evicted?.Invoke(this, new K2Spy.EvictedEventArgs<TKey>(key));
            return result;
        }

        public int Evict(Func<TKey, bool> predicate)
        {
            int count = 0;
            foreach (TKey key in this.m_Dictionary.Keys.ToArray())
            {
                if (predicate(key))
                {
                    if (this.Evict(key))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            TValue value;
            if (this.m_Dictionary.TryGetValue(key, out value))
                return value;

            return await Task.Run(() =>
            {
                return this.m_Dictionary.GetOrAdd(key, ignore =>
                {
                    if (this.m_ValueFactory != null)
                    {
                        return this.m_ValueFactory(key);
                    }
                    else if (this.m_AsyncValueFactory != null)
                    {
                        Task<TValue> task = this.m_AsyncValueFactory(key);
                        if (task != null)
                        {
                            if (task.Status == TaskStatus.Faulted)
                            {

                            }
                            return task.GetAwaiter().GetResult();
                        }
                        else
                        {
                        }
                    }
                    return default(TValue);
                });
            });
        }

        public void Cache(TKey key, TValue value, bool update)
        {
            if (!this.m_Dictionary.TryAdd(key, value))
            {
                if (update)
                {
                    TValue currentValue;
                    if (this.m_Dictionary.TryGetValue(key, out currentValue))
                    {
                        if (!this.m_Dictionary.TryUpdate(key, value, currentValue))
                        {
                        }
                    }
                }
            }
        }

        #endregion
    }
}
