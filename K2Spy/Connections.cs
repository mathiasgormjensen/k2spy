using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class Connections
    {
        #region Private Fields

        private List<Connection> m_Items = new List<Connection>();
        private System.Threading.CancellationTokenSource m_QueueSaveCancellationTokenSource;
        private static Connections m_Default;

        #endregion

        #region Public Methods

        public static Connections Default
        {
            get { return Connections.m_Default ?? (Connections.m_Default = Connections.Load()); }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlElement]
        public Connection[] Items
        {
            get { return this.m_Items.ToArray(); }
            set
            {
                this.m_Items.Clear();
                if (value != null)
                    this.m_Items.AddRange(value);
            }
        }

        #endregion

        #region Public Methods

        public async void QueueSave()
        {
            this.m_QueueSaveCancellationTokenSource?.Cancel();
            System.Threading.CancellationToken cancellationToken = (this.m_QueueSaveCancellationTokenSource = new System.Threading.CancellationTokenSource()).Token;
            await Task.Delay(500);
            if (!cancellationToken.IsCancellationRequested)
            {
                this.Save();
            }
        }

        public void Add(Connection connection)
        {
            this.Remove(connection);
            this.m_Items.Add(connection);
            this.QueueSave();
        }

        public void Remove(Connection connection)
        {
            while (this.m_Items.Remove(connection)) ;
            this.QueueSave();
        }

        public void Clear()
        {
            this.m_Items.Clear();
            this.QueueSave();
        }

        #endregion

        #region Protected Methods

        protected static Connections Load()
        {
            string path = Connections.GetPath();
            try
            {
                return Xml.Deserialize<Connections>(path);
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Failed to load connections: {ex}");
                Connections connections = new Connections();
#if false
                connections.Items = new Connection[]
                {
                    new Connection("localhost", "Host=localhost;Port=5555;Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;CachePassword=True"),
                    new Connection("k2.denallix.com", "Host=k2.denallix.com;Port=5555;Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;CachePassword=True"),
                    new Connection("dev-workflow.cbs.dk", "Host=dev-workflow.cbs.dk;Port=5555;Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;CachePassword=True"),
                };
#endif
                return connections;
            }
        }

        protected void Save()
        {
            string path = Connections.GetPath();
            Xml.Serialize(this, path);
        }

        #endregion

        #region Private Methods

        private static string GetPath()
        {
            string path = System.IO.Path.Combine(Configuration.Directory, "connections.xml");
            return path;
        }

        #endregion
    }
}