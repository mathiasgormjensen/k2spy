using K2Spy.ExtensionMethods;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    //public delegate K2SpyContext GetK2SpyContext();

    public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs eventArgs);
    
    public delegate Task AsyncEventHandler(object sender, EventArgs eventArgs);

    public class K2SpyContext
    {
        #region Private Fields

        private Action<string, bool> m_SetStatusCallback;

        #endregion

        #region Constructors

        public K2SpyContext(ConnectionFactory connectionFactory, Cache session, MainForm mainForm, TreeView treeView, InvokeOnUIThreadActionQueue actionQueue, Action<string, bool> setStatusCallback)
        {
            this.ConnectionFactory = connectionFactory;
            this.Cache = session;
            this.MainForm = mainForm;
            this.TreeView = treeView;
            this.ActionQueue = actionQueue;
            this.m_SetStatusCallback = setStatusCallback;
        }

        #endregion

        #region Public Events

        public event AsyncEventHandler Disconnecting;
        public event AsyncEventHandler Disconnected;
        public event AsyncEventHandler Connecting;
        public event AsyncEventHandler Connected;

        #endregion

        #region Public Properties

        public ConnectionFactory ConnectionFactory { get; private set; }

        public Cache Cache { get; private set; }

        public MainForm MainForm { get; private set; }

        public TreeView TreeView { get; private set; }

        public InvokeOnUIThreadActionQueue ActionQueue { get; private set; }

        #endregion

        #region Public Methods

        public async Task DisconnectAsync()
        {
            await this.Disconnecting.InvokeAsync(this, EventArgs.Empty);

            // do we need to do something?

            await this.Cache.ClearAsync();

            await this.Disconnected.InvokeAsync(this, EventArgs.Empty);
        }

        public async Task ConnectAsync(string connectionString = "Host=localhost;Port=5555;Integrated=True;IsPrimaryLogin=True;Authenticate=True;EncryptedPassword=False;CachePassword=True")
        {
            await this.DisconnectAsync();

            this.ConnectionFactory.SetConnectionString(connectionString, true);

            await this.Connecting.InvokeAsync(this, EventArgs.Empty);

            await this.Connected.InvokeAsync(this, EventArgs.Empty);
        }

        //public void SetStatus(string status, bool? progressBarVisibility, int ignore)
        //{
        //    this.SetStatus(status, progressBarVisibility, false);
        //}

        //public void SetStatus(string status, bool? progressBarVisibility = null, bool expirationInMiliseconds = false)
        //{
        //    this.ActionQueue.Queue(() =>
        //    {
        //        this.m_SetStatusCallback?.Invoke(status, progressBarVisibility, expirationInMiliseconds);
        //    });
        //}

        public void QueueSetStatus(string status, bool hideProgressBar = false)//, bool expirationInMiliseconds = false)
        {
            this.m_SetStatusCallback?.Invoke(status, hideProgressBar);
        }

        #endregion
    }
}