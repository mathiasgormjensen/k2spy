using K2Spy.ExtensionMethods;
using Newtonsoft.Json.Linq;
using SourceCode.Forms.Utilities;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    [System.Diagnostics.DebuggerDisplay("PreloadedInmemoryAsyncCache<{typeof(TKey).FullName,nq}, {typeof(TValue).FullName,nq}>, Count={Count}")]
    public class PreloadedInmemoryAsyncCache<TKey, TValue>
    {
        #region Private Fields

        private bool m_PreloadPending = true;
        private Func<KeyValuePair<TKey, TValue>[]> m_PreloadFactory;
        private Func<Task<KeyValuePair<TKey, TValue>[]>> m_PreloadFactoryTask;
        private System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> m_Dictionary = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>();
        private System.Collections.Generic.Dictionary<TKey, TValue> m_SecondaryDictionary = new Dictionary<TKey, TValue>();
        private Func<TKey, Task> m_CallbackFactory;

        #endregion

        #region Constructors

        public PreloadedInmemoryAsyncCache(Func<KeyValuePair<TKey, TValue>[]> preloadFactory, Func<TKey, Task> callbackFactory = null)
        {
            this.m_PreloadFactory = preloadFactory;
            this.m_CallbackFactory = callbackFactory;
        }

        //public PreloadedInmemoryAsyncCache(Func<Task<KeyValuePair<TKey, TValue>[]>> preloadFactoryTask)
        //{
        //    this.m_PreloadFactoryTask = preloadFactoryTask;
        //}

        #endregion

        #region Public Events

        //public event AsyncEventHandler<EvictedEventArgs<TKey>> Evicted;

        #endregion

        #region Public Properties

        public int Count
        {
            get { return this.m_Dictionary.Count; }
        }

        #endregion

        #region Public Methods

        public async Task EnsurePreloadedAsync()
        {
            using (new StopWatch("EnsurePreloadedAsync: " + typeof(TValue).Name))
            {
                await this.ConsiderPreloadAsync();
            }
        }

        public void Clear()
        {
            this.m_PreloadPending = true;
            this.m_Dictionary.Clear();
        }

        public bool Evict(TKey key)
        {
            Serilog.Log.Verbose($"PreloadedInmemoryAsyncCache<{typeof(TKey).Name}, {typeof(TValue).Name}>.Evict {key}");
            this.m_PreloadPending = true;
            TValue ignore;
            bool result = this.m_Dictionary.TryRemove(key, out ignore);
            return result;
        }

        public int Evict(Func<TKey, bool> predicate)
        {
            return this.Evict((key, value) => predicate(key));
        }

        public int Evict(Func<TKey, TValue, bool> predicate)
        {
            int count = 0;
            foreach (KeyValuePair<TKey, TValue> pair in this.m_Dictionary.ToArray())
            {
                if (predicate(pair.Key, pair.Value))
                {
                    if (this.Evict(pair.Key))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public async Task<KeyValuePair<TKey, TValue>[]> GetAllAsync(bool considerPreload = true)
        {
            if (considerPreload)
                await this.ConsiderPreloadAsync();
            return this.m_Dictionary.ToArray();
        }

        public async Task<TKey[]> GetAllKeysAsync(bool considerPreload = true)
        {
            if (considerPreload)
                await this.ConsiderPreloadAsync();
            return this.m_Dictionary.Keys.ToArray();
        }

        public async Task<TValue[]> GetAllValuesAsync(bool considerPreload = true)
        {
            if (considerPreload)
                await this.ConsiderPreloadAsync();
            return this.m_Dictionary.Values.ToArray();
        }

        public async Task<TValue> GetAsync(TKey key)
        {
            if (this.m_SecondaryDictionary.TryGetValue(key, out TValue value2))
                return value2;

            await this.ConsiderPreloadAsync();

            TValue value;
            if (this.m_Dictionary.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }

        private void Cache(TKey key, TValue value, bool update, List<Func<Task>> callbacks)
        {
            //this.m_SecondaryDictionary[key] = value;
            //Serilog.Log.Debug($"PreloadedInmemoryAsyncCache<{typeof(TKey).Name}, {typeof(TValue).Name}>.Cache({key}, {value}, {update})");
            Func<TKey,Task> callback = null;
            if (!this.m_Dictionary.TryAdd(key, value))
            {
                //Serilog.Log.Debug("Already cached");
                // already cached
                if (update)
                {
                    TValue currentValue;
                    if (this.m_Dictionary.TryGetValue(key, out currentValue))
                    {
                        if (!this.m_Dictionary.TryUpdate(key, value, currentValue))
                        {
                            //Serilog.Log.Debug("Failed to update cache");
                        }
                        else
                        {
                            //Serilog.Log.Debug("Updated cached");
                        }
                    }
                    else
                    {
                        //Serilog.Log.Debug("The value has disappeared! :S");
                    }
                    callback = this.m_CallbackFactory;
                }
                else
                {
                    //Serilog.Log.Debug("Leaving cached value unchanged");
                }
            }
            else
            {
                Serilog.Log.Verbose($"PreloadedInmemoryAsyncCache<{typeof(TKey).Name}, {typeof(TValue).Name}>: Added {key}:{value} to cache");
                callback = this.m_CallbackFactory;
            }

            if (callback != null)
            {
                callbacks.Add(() => callback(key));
            }
        }

        #endregion

        #region Protected Methods

        private readonly object m_Lock = new object();
        protected async Task ConsiderPreloadAsync()
        {
            if (this.m_PreloadPending)
            {
                if (this.m_PreloadFactoryTask != null)
                {
                    throw new NotImplementedException();
                    await this.m_PreloadFactoryTask();
                }
                else
                {
                    //Serilog.Log.Debug($"Before run {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    List<Func<Task>> callbacks = new List<Func<Task>>();
                    await Task.Run(() =>
                    {
                        //Serilog.Log.Debug($"In run {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                        using (new StopWatch($"Preloading PreloadedInmemoryAsyncCache<{typeof(TKey).Name}, {typeof(TValue).Name}>"))
                        {
                            lock (this.m_Lock)
                            {
                                using (new StopWatch($"Inside lock in PreloadedInmemoryAsyncCache<{typeof(TKey).Name}, {typeof(TValue).Name}>"))
                                {
                                    if (this.m_PreloadPending)
                                    {
                                        if (this.m_PreloadFactory != null)
                                        {
                                            KeyValuePair<TKey, TValue>[] pairs = this.m_PreloadFactory();
                                            if (pairs != null)
                                            {
                                                foreach (KeyValuePair<TKey, TValue> pair in pairs)
                                                {
                                                    this.Cache(pair.Key, pair.Value, false, callbacks);
                                                }
                                            }
                                        }
                                        this.m_PreloadPending = false;
                                    }
                                    else
                                    {
                                        Serilog.Log.Debug("Preload has already been performed");
                                    }
                                }
                            }
                        }
                    });

                    if (false && callbacks.Count > 0)
                    {
                        Serilog.Log.Debug($"Invoking {callbacks.Count} callbacks");
                        foreach (Func<Task> callback in callbacks)
                            await (callback.Invoke() ?? Task.CompletedTask);
                        // await Task.WhenAll(callbacks.ToArray());
                        Serilog.Log.Debug($"Done invoking callbacks");
                    }
                }
            }
        }

        #endregion
    }
}
