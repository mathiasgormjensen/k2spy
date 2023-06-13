using K2Spy.ExtensionMethods;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    [System.Diagnostics.DebuggerDisplay("PersistantLocalStorageAsyncCache<{typeof(TKey).FullName,nq}>, Count={Count}")]
    public class PersistantLocalStorageAsyncCache<TKey>
    {
        #region Private Fields

        private Func<TKey, Task<FileNameAndTimestamp>> m_FileNameAndTimestampFactory;
        private Func<TKey, Task<string>> m_AsyncValueFactory;
        private Dictionary<TKey, string> m_KeyPathDictionary = new Dictionary<TKey, string>();
        private string m_Directory;

        #endregion

        #region Constructors

        public PersistantLocalStorageAsyncCache(string directoryName, Func<TKey, Task<FileNameAndTimestamp>> fileNameAndTimestampFactory, Func<TKey, Task<string>> asyncValueFactory)
        {
            this.m_FileNameAndTimestampFactory = fileNameAndTimestampFactory;
            this.m_AsyncValueFactory = asyncValueFactory;

            this.m_Directory = System.IO.Path.Combine(Configuration.Directory, "PersistantLocalStorageAsyncCache", directoryName);
            System.IO.Directory.CreateDirectory(this.m_Directory);
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get { return this.m_KeyPathDictionary.Count; }
        }

        #endregion

        #region Public Methods

        public TKey[] GetKeys()
        {
            return this.m_KeyPathDictionary.Keys.ToArray();
        }

        public virtual void Clear(bool clearDiskCache = false)
        {
            this.m_KeyPathDictionary.Clear();
            if (clearDiskCache)
                new System.IO.DirectoryInfo(this.m_Directory).DeleteContents();
        }

        public virtual bool Evict(TKey key)
        {
            return this.m_KeyPathDictionary.Remove(key);
        }

        public int Evict(Func<TKey, bool> predicate)
        {
            int count = 0;
            foreach (TKey key in this.m_KeyPathDictionary.Keys.ToArray())
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

        public virtual async Task<string> GetAsync(TKey key)
        {
            string path;
            FileNameAndTimestamp fileNameAndTimestamp = await this.m_FileNameAndTimestampFactory(key);
            if (!this.m_KeyPathDictionary.TryGetValue(key, out path))
            {
                path = System.IO.Path.Combine(this.m_Directory, fileNameAndTimestamp.FileName);
                this.m_KeyPathDictionary.Add(key, path);
            }

            bool exists = System.IO.File.Exists(path);
            bool isUpToDate = false;
            if (exists)
            {
                DateTime lastWriteTimeUtc = System.IO.File.GetLastWriteTimeUtc(path);
                isUpToDate = fileNameAndTimestamp.TimestampUtc == lastWriteTimeUtc;
            }

            if (!exists || !isUpToDate)
            {
                // OK, we need to update the cache!
                string data = await this.m_AsyncValueFactory(key);
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        System.IO.File.WriteAllText(path, data, Encoding.UTF8);
                        System.IO.File.SetLastWriteTimeUtc(path, fileNameAndTimestamp.TimestampUtc);
                    }
                    catch when (i < 5)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
            return System.IO.File.ReadAllText(path, Encoding.UTF8);
        }

        //public void Cache(TKey key, string value, bool update = true)
        //{
        //    if (!this.m_Dictionary.TryAdd(key, value))
        //    {
        //        if (update)
        //        {
        //            string currentValue;
        //            if (this.m_Dictionary.TryGetValue(key, out currentValue))
        //            {
        //                if (!this.m_Dictionary.TryUpdate(key, value, currentValue))
        //                {
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Private Methods

        private void UpdateLocalCache(FileNameAndTimestamp fileNameAndTimestamp, string data)
        {
            string path = System.IO.Path.Combine(this.m_Directory, fileNameAndTimestamp.FileName);
            System.IO.File.WriteAllText(path, data, Encoding.UTF8);
            System.IO.File.SetLastWriteTimeUtc(path, fileNameAndTimestamp.TimestampUtc);
        }

        private bool LoadFromLocalCacheIfUpToDate(FileNameAndTimestamp fileNameAndTimestamp, out string data)
        {
            data = null;
            string path = System.IO.Path.Combine(this.m_Directory, fileNameAndTimestamp.FileName);
            if (System.IO.File.Exists(path))
            {
                DateTime lastWriteTimeUtc = System.IO.File.GetLastWriteTimeUtc(path);
                if (fileNameAndTimestamp.TimestampUtc == lastWriteTimeUtc)
                {
                    data = System.IO.File.ReadAllText(path, Encoding.UTF8);
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}