using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K2Spy.Extensions.TreeNodeStateMemory
{
    public class TreeNodeState
    {
        #region Private Fields

        private System.Threading.CancellationTokenSource m_CancellationTokenSource;

        #endregion

        #region Public Properties

        public string SelectedTreeNode { get; set; }

        public bool RememberSelection { get; set; } = true;

        public bool RememberExpandState { get; set; } = true;

        [System.Xml.Serialization.XmlIgnore]
        public List<string> ExpandedTreeNodes { get; private set; } = new List<string>();

        public string[] SerializedExpandedTreeNodes
        {
            get { return this.ExpandedTreeNodes.OrderBy(key => key).Distinct().ToArray(); }
            set
            {
                this.ExpandedTreeNodes.Clear();
                if (value != null)
                    this.ExpandedTreeNodes.AddRange(value);
            }
        }

        #endregion

        #region Public Methods

        public static TreeNodeState Load()
        {
            try
            {
                string path = TreeNodeState.GetStatePath();
                return Xml.Deserialize<TreeNodeState>(path);
            }
            catch (Exception ex)
            {
            }
            return new TreeNodeState();
        }

        public void Save()
        {
            try
            {
                string path = TreeNodeState.GetStatePath();
                Xml.Serialize(this, path);
            }
            catch (Exception ex)
            {
            }
        }

        public async void QueueSave()
        {
            this.m_CancellationTokenSource?.Cancel();
            System.Threading.CancellationToken cancellationToken = (this.m_CancellationTokenSource = new System.Threading.CancellationTokenSource()).Token;
            await Task.Delay(500);
            if (!cancellationToken.IsCancellationRequested)
            {
                this.Save();
            }
        }

        #endregion

        #region Private Methods

        private static string GetStatePath()
        {
            string path = System.IO.Path.Combine(Configuration.Directory, "treestatememory.xml");
            return path;
        }

        #endregion
    }
}