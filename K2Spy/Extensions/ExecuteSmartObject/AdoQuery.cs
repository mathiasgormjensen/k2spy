using ScintillaNET.Demo.Utils;
using SourceCode.SmartObjects.Authoring;
using SourceCode.SmartObjects.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    internal partial class AdoQuery : BaseForm
    {
        private SmartObjectDefinition m_SmartObjectDefinition;
        private Guid? m_InitialSmartObjectGuid;
        private K2SpyContext m_Context;
        private string m_InitialMethodName;

        public AdoQuery(K2SpyContext context, Guid? smartObjectGuid = null, string methodName = null)
        {
            InitializeComponent();
            this.m_Context = context;
            this.m_InitialSmartObjectGuid = smartObjectGuid;
            this.m_InitialMethodName = methodName;

            this.toolStripStatusLabel.Text = "";
        }

        protected async override void OnCreateControl()
        {
            base.OnCreateControl();

            await this.TryAsync(async () =>
            {
                HotKeyManager.AddHotKey(this, () => { this.tsbtnExecute.PerformClick(); }, Keys.Enter, true);
                HotKeyManager.AddHotKey(this, () => { this.tsbtnExecute.PerformClick(); }, Keys.F5);

                if (this.m_InitialSmartObjectGuid.HasValue)
                {
                    string name = (await this.m_Context.Cache.SmartObjectInfoCache.GetAsync(this.m_InitialSmartObjectGuid.Value)).Name;
                    this.txtQuery.Text = @"SELECT *
FROM " + name;
                    if (!string.IsNullOrEmpty(this.m_InitialMethodName))
                        this.txtQuery.Text += "." + this.m_InitialMethodName;
                }
            });
        }

        private async void tsbtnExecute_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                (this.dataGridView.DataSource as IDisposable)?.Dispose();
                this.dataGridView.DataSource = null;
                using (this.loadingOverlay1.ShowThenHide("Executing SmartObjects..."))
                {
                    SmartObjectClientServer smartObjectClientServer = await this.m_Context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SmartObjectClientServer>();
                    this.dataGridView.Visible = true;
                    DateTime begin = DateTime.Now;
                    DataTable dataTable = smartObjectClientServer.ExecuteSQLQueryDataTable(this.txtQuery.Text);
                    TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                    this.toolStripStatusLabel.Text = "Execution time: " + timeSpan.ToString();
                    this.dataGridView.DataSource = dataTable;
                }
            });
        }

        private void SuppressCtrlEnter(object sender, KeyEventArgs e)
        {
            this.Try(() =>
            {
                if (e.Control && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            });
        }

        private AdoQueryData m_AdoQueryData;
        private string m_AdoQueryDataPath;
        private void tsmiSave_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                if (this.m_AdoQueryData == null)
                {
                    this.tsmiSaveAs.PerformClick();
                }
                else
                {
                    this.m_AdoQueryData.Query = this.txtQuery.Lines;
                    Xml.Serialize(this.m_AdoQueryData, this.m_AdoQueryDataPath);
                }
            });
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.AddExtension = true;
                    dlg.Filter = "XML|*.xml";
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        this.m_AdoQueryData = new AdoQueryData();
                        this.m_AdoQueryDataPath = dlg.FileName;
                        this.tsmiSave.PerformClick();
                    }
                }
            });
        }

        private void tsbtnOpen_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Filter = "XML|*.xml";
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        AdoQueryData executeSmartObjectData = Xml.Deserialize<AdoQueryData>(dlg.FileName);
                        this.m_AdoQueryData = executeSmartObjectData;
                        this.m_AdoQueryDataPath = dlg.FileName;

                        this.txtQuery.Lines = executeSmartObjectData.Query;
                    }
                }
            });
        }

        private void tssbtnSave_ButtonClick(object sender, EventArgs e)
        {
            this.tsmiSave.PerformClick();
        }
    }
}