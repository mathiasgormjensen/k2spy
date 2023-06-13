using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace K2Spy
{
    public class ListViewSortableVirtualList<T>
    {
        #region Private Fields

        private List<T> m_InnerList = new List<T>();
        private Func<T[], int, SortOrder, T[]> m_Sorter;
        private K2SpyContext m_K2SpyContext;
        private ListView m_ListView;

        #endregion

        #region Constructors

        //public ListViewSortableVirtualList(ListView listView, Func<T[], int, SortOrder, T[]> sorter)
        //    : this(null, listView, sorter, null)
        //{
        //}

        public ListViewSortableVirtualList(K2SpyContext k2SpyContext, ListView listView, Func<T[], int, SortOrder, T[]> sorter, Func<T, ListViewItem> itemFactory)
        {
            this.m_K2SpyContext = k2SpyContext;
            this.m_ListView = listView;
            this.m_Sorter = sorter;
            if (itemFactory != null)
            {
                listView.VirtualMode = true;
                listView.RetrieveVirtualItem += (sender, e) =>
                {
                    T item = this[e.ItemIndex];
                    e.Item = itemFactory(item);
                };
            }
            listView.ColumnClick += (sender, e) =>
            {
                listView.Try(() =>
                {
                    if (this.SortColumn == e.Column)
                    {
                        // toggle sort order of current sort column
                        switch (this.SortOrder)
                        {
                            case SortOrder.Ascending:
                                this.SortOrder = SortOrder.Descending;
                                break;
                            case SortOrder.Descending:
                                this.SortOrder = SortOrder.None;
                                break;
                            case SortOrder.None:
                                this.SortOrder = SortOrder.Ascending;
                                break;
                        }
                    }
                    else
                    {
                        // change current sort column
                        this.SortColumn = e.Column;
                        if (this.SortOrder == SortOrder.None)
                            this.SortOrder = SortOrder.Ascending;
                    }
                    listView.SetSortIcon(this.SortColumn, this.SortOrder);

                    this.Sort();

                    using (new ListViewUpdateContext(listView))
                        listView.Refresh();
                });
            };

        }

        #endregion

        #region Public Properties

        public T this[int index]
        {
            get { return this.SortedNodes[index]; }
        }

        public int Length { get; private set; }

        public T[] SortedNodes { get; private set; }

        public T[] SelectedItems
        {
            get
            {
                int[] indices = this.m_ListView.SelectedIndices.OfType<int>().ToArray();
                return indices.Select(key => this[key]).ToArray();
            }
        }

        public T SelectedItem
        {
            get { return this.SelectedItems.FirstOrDefault(); }
        }

        #endregion

        #region Protected Properties

        protected int SortColumn { get; private set; }

        protected SortOrder SortOrder { get; set; } = SortOrder.None;

        #endregion

        #region Public Methods

        public void Clear()
        {
            this.m_InnerList.Clear();
            this.Length = 0;
            this.QueueUpdateVirtualListSize();
        }

        public void Add(T node)
        {
            this.m_InnerList.Add(node);
            this.Length = this.m_InnerList.Count;
            this.Sort();
            this.QueueUpdateVirtualListSize();
        }

        #endregion

        private readonly object m_QueueUpdateVirtualListSizeKey = new object();
        protected void QueueUpdateVirtualListSize()
        {
            this.m_K2SpyContext?.ActionQueue.QueueOnce(this.m_QueueUpdateVirtualListSizeKey, () =>
            {
                this.m_ListView.VirtualListSize = this.Length;
            });
        }

        protected void Sort()
        {
            if (this.SortOrder == SortOrder.None)
            {
                this.SortedNodes = this.m_InnerList.ToArray();
            }
            else
            {
                this.SortedNodes = this.m_Sorter.Invoke(this.m_InnerList.ToArray(), this.SortColumn, this.SortOrder);
            }
        }
    }
}
