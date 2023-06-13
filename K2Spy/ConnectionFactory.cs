using SourceCode.Forms.Utilities;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class ConnectionFactory
    {
        #region Private Fields

        private string m_ConnectionString;
        private System.Collections.Concurrent.ConcurrentDictionary<Type, System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI>> m_DictionaryWithImpersonation = new System.Collections.Concurrent.ConcurrentDictionary<Type, System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI>>();

        #endregion

        #region Constructors

        public ConnectionFactory(string connectionString = "Host=localhost;Port=5555;Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;CachePassword=True")
        {
            this.SetConnectionString(connectionString);

            new System.Threading.Thread(this.MonitorThread).Start();
        }

        private void MonitorThread(object state)
        {
            bool running = true;
            System.Windows.Forms.Application.ApplicationExit += (sender, e) => running = false;
            while (running)
            {
                System.Threading.Thread.Sleep(Properties.Settings.Default.ConnectionCachePingFrequencyInMiliseconds);
                try
                {
                    foreach (Type type in this.m_DictionaryWithImpersonation.Keys.ToArray())
                    {
                        foreach (KeyValuePair<string, BaseAPI> pair in this.m_DictionaryWithImpersonation[type].ToArray())
                        {
                            try
                            {
                                TimeSpan timeSpan = pair.Value.Connection.Ping();
                            }
                            catch (Exception ex)
                            {
                                Serilog.Log.Warning($"Failed to ping {pair.Value.GetType()}");
                                this.RemoveInternal(type, pair.Key);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            System.Collections.Concurrent.ConcurrentDictionary<Type, System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI>> dictionary = this.m_DictionaryWithImpersonation;
            this.m_DictionaryWithImpersonation = new System.Collections.Concurrent.ConcurrentDictionary<Type, System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI>>();

            foreach (Type type in dictionary.Keys)
            {
                foreach (BaseAPI connection in dictionary[type].Values)
                {
                    connection.Connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Removes and disposes any cached connections of the specified type
        /// </summary>
        /// <typeparam name="TBaseAPI"></typeparam>
        public void Remove<TBaseAPI>(string impersonateFqn = "")
            where TBaseAPI : BaseAPI
        {
            Type key = typeof(TBaseAPI);

            this.RemoveInternal(key, impersonateFqn);
        }

        private void RemoveInternal(Type type, string impersonateFqn)
        {
            System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI> dictionary;
            if (this.m_DictionaryWithImpersonation.TryGetValue(type, out dictionary))
            {
                if (dictionary.TryRemove(impersonateFqn, out BaseAPI baseAPI))
                {
                    baseAPI.Connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets or creates a connection of the specified type. The connection is cached and may be used by others, and should therefore not be disposed.
        /// </summary>
        /// <typeparam name="TBaseAPI"></typeparam>
        /// <param name="impersonateFqn"></param>
        /// <returns></returns>
        public async Task<TBaseAPI> GetOrCreateBaseAPIConnectionAsync<TBaseAPI>(string impersonateFqn = null)
            where TBaseAPI : BaseAPI
        {
            return await Task.Run(() => this.GetOrCreateBaseAPIConnection<TBaseAPI>(impersonateFqn));
        }

        /// <summary>
        /// Gets or creates a connection of the specified type. The connection is cached and may be used by others, and should therefore not be disposed.
        /// </summary>
        /// <typeparam name="TBaseAPI"></typeparam>
        /// <param name="impersonateFqn"></param>
        /// <returns></returns>
        public TBaseAPI GetOrCreateBaseAPIConnection<TBaseAPI>(string impersonateFqn = null)
            where TBaseAPI : BaseAPI
        {
            System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI> dictionary = null;
            dictionary = this.m_DictionaryWithImpersonation.GetOrAdd(typeof(TBaseAPI), (ignore) => new System.Collections.Concurrent.ConcurrentDictionary<string, BaseAPI>());
            return (TBaseAPI)dictionary.GetOrAdd(impersonateFqn ?? "", type => this.CreateBaseAPIConnection<TBaseAPI>(impersonateFqn));
        }

        /// <summary>
        /// Creates a new connection of the specified type. This connection should be disposed once it is no longer used
        /// </summary>
        /// <typeparam name="TBaseAPI"></typeparam>
        /// <param name="impersonateFqn"></param>
        /// <returns></returns>
        public TBaseAPI CreateBaseAPIConnection<TBaseAPI>(string impersonateFqn = null)
            where TBaseAPI : BaseAPI
        {
            using (new StopWatch($"Creating and connecting {typeof(TBaseAPI).Name}, impersonateFqn: {impersonateFqn}"))
            {
                TBaseAPI result = Activator.CreateInstance<TBaseAPI>();
                result.CreateConnection();
                if (!result.Connection.Open(this.m_ConnectionString))
                    throw new Exception($"Failed to establish a connection to {typeof(TBaseAPI).Name}");
                if (!string.IsNullOrEmpty(impersonateFqn))
                    this.ImpersonateUser(result.Connection, impersonateFqn);
                return result;
            }
        }

        public async Task<TBaseAPI> CreateBaseAPIConnectionAsync<TBaseAPI>(string impersonateFqn = null)
            where TBaseAPI : BaseAPI
        {
            return await Task.Run(() => this.CreateBaseAPIConnection<TBaseAPI>(impersonateFqn));
        }

        #endregion

        #region Internal Methods

        internal void SetConnectionString(string connectionString, bool clear = true)
        {
            this.m_ConnectionString = connectionString;
            if (clear)
                this.Clear();
        }

        #endregion

        #region Private Methods

        private void ImpersonateUser(BaseAPIConnection connection, string impersonateFqn)
        {
            Assembly assembly = typeof(BaseAPIConnection).Assembly;
            MethodInfo method = typeof(BaseAPIConnection).GetMethod("ImpersonateInternal", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            object obj = Enum.Parse(assembly.GetType("SourceCode.Hosting.Client.BaseAPI.ImpersonationType"), "UserRequested", ignoreCase: true);
            object result = method.Invoke(connection, new object[] { impersonateFqn, obj, true });
            if (!true.Equals(result))
                throw new Exception("Failed to imperonsate " + impersonateFqn);
        }

        #endregion
    }
}
