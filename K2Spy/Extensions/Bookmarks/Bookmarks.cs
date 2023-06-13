using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2Spy.Extensions.Bookmarks
{
    internal class Bookmarks
    {
        #region Private Fields

        private List<string> m_Bookmarks = new List<string>();
        private System.Threading.CancellationTokenSource m_CancellationTokenSource;
        private K2SpyContext m_K2SpyContext;

        #endregion

        #region Public Methods

        public string[] GetAllBookmarks()
        {
            return this.m_Bookmarks.ToArray();
        }

        public bool IsBookmarked(string path)
        {
            return this.m_Bookmarks.Contains(path, StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string path)
        {
            this.m_Bookmarks.Add(path);
            this.QueueSave();
        }

        public void Remove(string path)
        {
            this.m_Bookmarks.RemoveAll(key => key.Equals(path, StringComparison.OrdinalIgnoreCase));
            this.QueueSave();
        }

        public void Load(K2SpyContext k2SpyContext)
        {
            this.m_K2SpyContext = k2SpyContext;
            string path = System.IO.Path.Combine(Configuration.Directory, "bookmarks.xml");
            this.m_Bookmarks.Clear();
            try
            {
                this.m_Bookmarks.AddRange(Xml.Deserialize<string[]>(path));
            }
            catch { }
        }

        #endregion

        protected async void QueueSave()
        {
            this.m_CancellationTokenSource?.Cancel();
            System.Threading.CancellationToken cancellationToken = (this.m_CancellationTokenSource = new System.Threading.CancellationTokenSource()).Token;
            await Task.Delay(500);
            if (!cancellationToken.IsCancellationRequested)
            {
                this.Save(this.m_K2SpyContext);
            }
        }

        protected void Save(K2SpyContext context)
        {
            string path = System.IO.Path.Combine(Configuration.Directory, "bookmarks.xml");
            Xml.Serialize<string[]>(this.m_Bookmarks.ToArray(), path);
        }
    }
}
