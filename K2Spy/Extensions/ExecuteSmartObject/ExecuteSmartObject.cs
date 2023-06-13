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
    internal partial class ExecuteSmartObject : BaseForm
    {
        #region Private Fields

        private Guid m_InitialSmartObjectGuid;
        private K2SpyContext m_Context;
        private ExecuteSmartObjectData m_ExecuteSmartObjectData;
        private string m_ExecuteSmartObjectDataPath;
        private string m_InitialMethodName;

        #endregion

        #region Constructors

        public ExecuteSmartObject(K2SpyContext context, Guid smartObjectGuid, string methodName = null)
        {
            InitializeComponent();
            this.m_Context = context;
            this.m_InitialSmartObjectGuid = smartObjectGuid;
            this.m_InitialMethodName = methodName;

            this.ResetInputTable(true);
            this.ResetOutputTable(true);

            this.toolStripStatusLabel.Text = "";
        }

        #endregion

        #region Protected Properties

        protected SmartMethodBase SelectedSmartMethod
        {
            get { return this.comboMethod.SelectedItem as SmartMethodBase; }
        }

        protected SmartObject SelectedSmartObject { get; private set; }

        #endregion

        protected override async void OnCreateControl()
        {
            base.OnCreateControl();

            await this.TryAsync(async () =>
            {
                HotKeyManager.AddHotKey(this, () => { this.tsbtnExecute.PerformClick(); }, Keys.Enter, true);
                HotKeyManager.AddHotKey(this, () => { this.tsbtnExecute.PerformClick(); }, Keys.F5);

                await this.LoadSmartObjectAsync(this.m_InitialSmartObjectGuid, this.m_InitialMethodName);
            });
        }

        protected async Task<SmartObject> LoadSmartObjectAsync(Guid smartObjectGuid, string methodName = null)
        {
            SmartObject smartObject = await this.m_Context.Cache.SmartObjectCache.GetAsync(smartObjectGuid);
            this.LoadSmartObject(smartObject, methodName);
            return smartObject;
        }

        protected void LoadSmartObject(SmartObject smartObject, string methodName = null)
        {
            this.SelectedSmartObject = smartObject;
            this.comboMethod.Items.Clear();
            this.comboMethod.DisplayMember = "Metadata.DisplayName";
            this.tsbtnExecute.Enabled = false;
            this.comboMethod.Items.AddRange(smartObject.AllMethods.OrderBy(key => key.Metadata.DisplayName).ToArray());
            this.lblSmartObjectDisplayName.Text = smartObject.Metadata.DisplayName;

            this.ResetInputTable();
            this.comboMethod.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(methodName))
            {
                SmartMethodBase smartMethodBase = smartObject.AllMethods.FirstOrDefault(key => key.Name == methodName);
                if (smartMethodBase != null)
                    this.comboMethod.SelectedItem = smartMethodBase;
            }

            base.Text = $"Execute SmartObject - {smartObject.Metadata.DisplayName}";
        }

        protected void ResetInputTable(bool initializeColumns = false)
        {
            this.tableInput.Controls.Clear();
            this.tableInput.RowStyles.Clear();
            this.tableInput.RowCount = 0;

            if (initializeColumns)
            {
                this.tableInput.ColumnCount = 4;
                this.tableInput.ColumnStyles.Clear();
                this.tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                this.tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                this.tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                this.tableInput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

        protected void ResetOutputTable(bool initializeColumns = false)
        {
            this.tableOutput.Controls.Clear();
            this.tableOutput.RowStyles.Clear();
            this.tableOutput.RowCount = 0;

            if (initializeColumns)
            {
                this.tableOutput.ColumnCount = 3;
                this.tableOutput.ColumnStyles.Clear();
                this.tableOutput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                this.tableOutput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                this.tableOutput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

        protected void ResetOutputDataTable()
        {
            (this.dataGridView.DataSource as IDisposable)?.Dispose();
            this.dataGridView.DataSource = null;
        }

    
        private void AddOutputProperty(SmartProperty property)
        {
            int row = this.tableOutput.RowCount;
            this.tableOutput.RowCount++;
            this.tableOutput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Label label = new Label();
            label.AutoSize = true;
            label.Dock = DockStyle.Top;
            label.Text = property.Metadata.DisplayName;
            label.Text += $" ({property.Type}):";
            label.Padding = new Padding(0, 6, 0, 0);
            this.tableOutput.Controls.Add(label, 0, row);

            Control edit = null;
            AutoResizeTextBox textBox = new AutoResizeTextBox();
            edit = textBox;
            textBox.Dock = DockStyle.Top;
            textBox.Text = property.Value;
            if (edit is TextBox)
                ((TextBox)edit).ReadOnly = true;
            else
                edit.Enabled = false;
            this.tableOutput.Controls.Add(edit, 1, row);

            if (property is SmartFileProperty || property is SmartImageProperty)
            {
                Button downloadButton = this.CreateSaveFileOrImageButton(property);
                this.tableOutput.Controls.Add(downloadButton, 2, row);
            }
            else
            {
                this.tableOutput.SetColumnSpan(edit, 2);
            }
        }

        private Button CreateSaveFileOrImageButton(SmartProperty property)
        {
            Button downloadButton = new Button();
            downloadButton.Text = "Save";
            downloadButton.UseVisualStyleBackColor = true;
            downloadButton.AutoSize = true;
            downloadButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            if (string.IsNullOrEmpty(property.Value) || "scnull".Equals(property.Value, StringComparison.OrdinalIgnoreCase))
            {
                downloadButton.Enabled = false;
            }
            else
            {
                downloadButton.Click += (sender, e) =>
                {
                    this.Try(() =>
                    {
                        using (SaveFileDialog dlg = new SaveFileDialog())
                        {
                            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                            document.LoadXml(property.Value);
                            string name = document.DocumentElement.SelectSingleNode("name").InnerText;
                            dlg.FileName = name;
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                string content = document.DocumentElement.SelectSingleNode("content").InnerText;
                                byte[] data = Convert.FromBase64String(content);
                                System.IO.File.WriteAllBytes(dlg.FileName, data);
                            }
                        }
                    });
                };
            }
            return downloadButton;
        }

        private void comboMethod_SelectedValueChanged(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                using (new LayoutSuspender(this.tableInput))
                {
                    using (new LayoutSuspender(this.tableOutput))
                    {
                        this.m_PropertyUpdaters.Clear();
                        this.m_PropertyClearers.Clear();
                        this.m_InputPropertyCreators.Clear();
                        SmartMethodBase method = this.SelectedSmartMethod;
                        this.tsbtnExecute.Enabled = method != null;

                        this.ResetInputTable();
                        this.ResetOutputTable();
                        if (method != null)
                        {
                            List<SmartProperty> requiredProperties = new List<SmartProperty>();
                            requiredProperties.AddRange(method.Parameters.OfType<SmartProperty>());
                            requiredProperties.AddRange(method.RequiredProperties.OfType<SmartProperty>());

                            foreach (SmartProperty property in requiredProperties.Distinct().OrderBy(key => key.Metadata.DisplayName))
                            {
                                this.AddInputProperty(property, true);
                            }
                            foreach (SmartProperty property in method.InputProperties.OfType<SmartProperty>().Except(requiredProperties).Distinct().OrderBy(key => key.Metadata.DisplayName))
                            {
                                this.AddInputProperty(property, false);
                            }
                        }
                    }
                }
            });
        }

        private List<Func<InputProperty>> m_InputPropertyCreators = new List<Func<InputProperty>>();
        private List<Action<SmartMethodBase>> m_PropertyUpdaters = new List<Action<SmartMethodBase>>();
        private List<Action> m_PropertyClearers = new List<Action>();
        private bool m_PopulateInputProperties = false;
        private void AddInputProperty(SmartProperty property, bool required)
        {
            int row = this.tableInput.RowCount;
            this.tableInput.RowCount++;
            this.tableInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Label label = new Label();
            label.AutoSize = true;
            label.Dock = DockStyle.Top;
            label.Text = property.Metadata.DisplayName;
            label.Padding = new Padding(0, 2, 0, 0);
            label.Margin = new Padding(3, 2, 3, 0);
            if (required)
            {
                label.Text = "* " + label.Text;
                label.Font = new Font(base.Font, FontStyle.Bold);
            }
            label.Text += $" ({property.Type}):";
            this.tableInput.Controls.Add(label, 0, row);

            string defaultValue = "";
            if (this.m_PopulateInputProperties)
                defaultValue = this.m_ExecuteSmartObjectData.InputProperties?.FirstOrDefault(key => key.Name == property.Name)?.Value;

            Control editorControl = null;
            Control secondaryEditControl = null;
            Func<string> getValue = () =>
            {
                return editorControl.Text;
            };
            Action resetValue = () =>
            {
                editorControl.ResetText();
            };
            if (property.Type == PropertyType.DateTime)
            {
                TableLayoutPanel table = new TableLayoutPanel();
                table.RowStyles.Clear();
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.RowCount = 1;
                table.ColumnStyles.Clear();
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                table.Dock = DockStyle.Top;
                table.AutoSize = true;
                table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                table.Padding = new Padding(0);
                table.Margin = new Padding(0);

                DateTimePicker datePicker = new DateTimePicker();
                datePicker.Margin = new Padding(0);
                datePicker.ShowCheckBox = true;
                datePicker.Checked = false;
                datePicker.Format = DateTimePickerFormat.Short;
                datePicker.Dock = DockStyle.Top;

                DateTimePicker timePicker = new DateTimePicker();
                timePicker.Margin = new Padding(0);
                timePicker.ShowCheckBox = true;
                timePicker.Checked = false;
                timePicker.ShowUpDown = true;
                timePicker.Format = DateTimePickerFormat.Time;
                timePicker.Dock = DockStyle.Top;

                if (!string.IsNullOrEmpty(defaultValue) && DateTime.TryParse(defaultValue, out DateTime date))
                {
                    datePicker.Checked = true;
                    datePicker.Value = date;
                    timePicker.Checked = true;
                    timePicker.Value = date;
                }

                table.Controls.Add(datePicker, 0, 0);
                table.Controls.Add(timePicker, 1, 0);

                resetValue = () =>
                {
                    datePicker.Checked = false;
                    datePicker.ResetText();
                    timePicker.Checked = false;
                    timePicker.ResetText();
                };
                getValue = () =>
                {
                    if (datePicker.Checked || timePicker.Checked)
                    {
                        DateTime result = DateTime.Now;
                        if (datePicker.Checked)
                            result = datePicker.Value;
                        result = result.Subtract(result.TimeOfDay);
                        if (timePicker.Checked)
                            result = result.Add(timePicker.Value.TimeOfDay);

                        return result.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    return null;
                };
                editorControl = table;
            }
            else if (property.Type == PropertyType.Date)
            {
                DateTimePicker datePicker = new DateTimePicker();
                datePicker.Margin = new Padding(0);
                datePicker.ShowCheckBox = true;
                datePicker.Checked = false;
                datePicker.Format = DateTimePickerFormat.Short;
                datePicker.Dock = DockStyle.Top;

                if (!string.IsNullOrEmpty(defaultValue) && DateTime.TryParse(defaultValue, out DateTime date))
                {
                    datePicker.Checked = true;
                    datePicker.Value = date;
                }

                resetValue = () =>
                {
                    datePicker.Checked = false;
                    datePicker.ResetText();
                };
                getValue = () =>
                {
                    if (datePicker.Checked)
                    {
                        DateTime result = DateTime.Now;
                        if (datePicker.Checked)
                            result = datePicker.Value;
                        result = result.Subtract(result.TimeOfDay);

                        return result.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    return null;
                };
                editorControl = datePicker;
            }
            else if (property.Type == PropertyType.File)
            {
                AutoResizeTextBox textBox = new AutoResizeTextBox();
                textBox.Text = defaultValue;
                editorControl = textBox;

                secondaryEditControl = new Button();
                secondaryEditControl.Text = "...";
                ((Button)secondaryEditControl).Padding = new Padding(0);
                ((Button)secondaryEditControl).Margin = new Padding(0);
                ((Button)secondaryEditControl).AutoSize = true;
                ((Button)secondaryEditControl).AutoSizeMode = AutoSizeMode.GrowAndShrink;
                ((Button)secondaryEditControl).UseVisualStyleBackColor = true;
                ((Button)secondaryEditControl).Dock = DockStyle.Top;
                secondaryEditControl.Click += (sender, e) =>
                {
                    this.Try(() =>
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            if (System.IO.Path.IsPathRooted(textBox.Tag as string))
                                dlg.FileName = textBox.Tag as string;
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                textBox.Tag = dlg.FileName;
                                textBox.Text = FileObject.FromFile(dlg.FileName).Serialize();
                            }
                        }
                    });
                };
                getValue = () =>
                {
                    return textBox.Text;
                };
                resetValue = () =>
                {
                    textBox.ResetText();
                    textBox.Tag = null;
                };
            }
            else if (property.Type == PropertyType.Image)
            {
                AutoResizeTextBox textBox = new AutoResizeTextBox();
                textBox.Text = defaultValue;
                editorControl = textBox;

                secondaryEditControl = new Button();
                secondaryEditControl.Text = "...";
                ((Button)secondaryEditControl).Padding = new Padding(0);
                ((Button)secondaryEditControl).Margin = new Padding(0);
                ((Button)secondaryEditControl).AutoSize = true;
                ((Button)secondaryEditControl).AutoSizeMode = AutoSizeMode.GrowAndShrink;
                ((Button)secondaryEditControl).UseVisualStyleBackColor = true;
                ((Button)secondaryEditControl).Dock = DockStyle.Top;
                secondaryEditControl.Click += (sender, e) =>
                {
                    this.Try(() =>
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            if (System.IO.Path.IsPathRooted(textBox.Tag as string))
                                dlg.FileName = textBox.Tag as string;
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                textBox.Text = ImageObject.FromFile(dlg.FileName).Serialize();
                                textBox.Tag = dlg.FileName;
                            }
                        }
                    });
                };
                getValue = () =>
                {
                    return textBox.Text;
                };
                resetValue = () =>
                {
                    textBox.ResetText();
                    textBox.Tag = null;
                };
            }
            else if (property.Type == PropertyType.HyperLink)
            {
                AutoResizeTextBox textBox = new AutoResizeTextBox();
                textBox.Text = defaultValue;
                editorControl = textBox;

                secondaryEditControl = new Button();
                secondaryEditControl.Text = "Edit";
                ((Button)secondaryEditControl).Padding = new Padding(0);
                ((Button)secondaryEditControl).Margin = new Padding(0);
                ((Button)secondaryEditControl).AutoSize = true;
                ((Button)secondaryEditControl).AutoSizeMode = AutoSizeMode.GrowAndShrink;
                ((Button)secondaryEditControl).UseVisualStyleBackColor = true;
                ((Button)secondaryEditControl).Dock = DockStyle.Fill;
                ((Button)secondaryEditControl).Click += (sender, e) =>
                {
                    this.Try(() =>
                    {
                        using (EditHyperlink dlg = new EditHyperlink())
                        {
                            dlg.HyperlinkAsString = textBox.Text;
                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                textBox.Text = dlg.HyperlinkAsString;
                                // textBox.Tag = FileObject.FromFile(dlg.FileName).Serialize();
                            }
                        }
                    });
                };
                getValue = () =>
                {
                    return textBox.Tag as string;
                };
                resetValue = () =>
                {
                    textBox.ResetText();
                    textBox.Tag = null;
                };
            }
            else
            {
                // treat as textbox
                AutoResizeTextBox textBox = new AutoResizeTextBox();
                textBox.Text = defaultValue;
                editorControl = textBox;

                if (property.Type == PropertyType.Text)
                {
                    secondaryEditControl = new Button();
                    secondaryEditControl.Text = "Edit";
                    ((Button)secondaryEditControl).Padding = new Padding(0);
                    ((Button)secondaryEditControl).Margin = new Padding(0);
                    ((Button)secondaryEditControl).AutoSize = true;
                    ((Button)secondaryEditControl).AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    ((Button)secondaryEditControl).UseVisualStyleBackColor = true;
                    ((Button)secondaryEditControl).Dock = DockStyle.Fill;
                    ((Button)secondaryEditControl).Click += (sender, e) =>
                    {
                        this.Try(() =>
                        {
                            using (EditText dlg = new EditText(textBox))
                            {
                                dlg.ShowDialog(this);
                            }
                        });
                    };
                }
            }
            this.m_PropertyClearers.Add(resetValue);
            editorControl.Margin = new Padding(3, 1, 3, 0);
            editorControl.Dock = DockStyle.Top;
            this.tableInput.Controls.Add(editorControl, 1, row);
            if (secondaryEditControl == null)
            {
                this.tableInput.SetColumnSpan(editorControl, 2);
            }
            else
            {
                secondaryEditControl.Margin = new Padding(3, 1, 3, 0);
                this.tableInput.Controls.Add(secondaryEditControl, 2, row);
            }


            CheckBox checkBox = new CheckBox();
            checkBox.AutoSize = true;
            checkBox.Text = "Null";
            checkBox.Margin = new Padding(1, 1, 1, 0);
            checkBox.Padding = new Padding(0, 4, 0, 0);
            checkBox.CheckedChanged += (sender, e) =>
            {
                this.Try(() =>
                {
                    editorControl.Enabled = !checkBox.Checked;
                    label.Enabled = editorControl.Enabled;
                    if (secondaryEditControl != null)
                        secondaryEditControl.Enabled = editorControl.Enabled;
                });
            };
            this.m_PropertyClearers.Add(() => checkBox.Checked = false);
            this.tableInput.Controls.Add(checkBox, 3, row);

            this.m_PropertyUpdaters.Add(method =>
            {
                SmartProperty match = method.Parameters.OfType<SmartParameter>().FirstOrDefault(key => key.Name == property.Name) ?? method.InputProperties.OfType<SmartProperty>().FirstOrDefault(key => key.Name == property.Name);
                if (match == null)
                    throw new Exception($"The property {property.Name} was not found");
                if (checkBox.Checked)
                {
                    if (match is SmartFileProperty)
                        match.Value = FileObject.Scnull;
                    else if (match is SmartImageProperty)
                        match.Value = ImageObject.Scnull;
                    else if (match is SmartHyperlinkProperty)
                        match.Value = HyperlinkObject.Scnull;
                    else
                        match.Value = "scnull";
                }
                else
                {
                    string value = getValue();
                    match.Value = getValue();
                }
            });
            this.m_InputPropertyCreators.Add(() =>
            {
                return new InputProperty()
                {
                    Name = property.Name,
                    DisplayName = property.Metadata.DisplayName,
                    Null = checkBox.Checked,
                    Value = getValue()
                };
            });
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                this.m_PropertyClearers.ForEach(key => key());
            });
        }

        private async void tsbtnExecute_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                using (this.loadingOverlay1.ShowThenHide("Executing SmartObject..."))
                {
                    this.ResetOutputTable();
                    SmartObjectClientServer smartObjectClientServer = await this.m_Context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SmartObjectClientServer>();
                    SmartObject smartObject = this.SelectedSmartObject.Clone();
                    SmartMethodBase method = smartObject.AllMethods.First(key => key.Name == this.SelectedSmartMethod.Name);
                    smartObject.MethodToExecute = method.Name;
                    this.m_PropertyUpdaters.ForEach(key => key(method));
                    if (this.SelectedSmartMethod.Type == MethodType.list)
                    {
                        this.dataGridView.Visible = true;
                        this.tableOutput.Visible = false;
                        DateTime begin = DateTime.Now;
                        DataTable dataTable = await Task.Run<DataTable>(() => smartObjectClientServer.ExecuteListDataTable(smartObject));
                        TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                        this.toolStripStatusLabel.Text = $"Found {dataTable.Rows.Count} records in " + timeSpan.ToString();
                        this.dataGridView.DataSource = dataTable;
                    }
                    else
                    {
                        this.dataGridView.Visible = false;
                        this.tableOutput.Visible = true;
                        using (new LayoutSuspender(this.tableOutput))
                        {
                            DateTime begin = DateTime.Now;
                            SmartObject smartObject2 = await Task.Run<SmartObject>(() => smartObjectClientServer.ExecuteScalar(smartObject));
                            TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                            this.toolStripStatusLabel.Text = "Execution time: " + timeSpan.ToString();
                            foreach (SmartProperty property in method.ReturnProperties.OfType<SmartProperty>().OrderBy(key => key.Metadata.DisplayName))
                            {
                                this.AddOutputProperty(property);
                            }
                        }
                    }
                }
            });
        }

        private void ExecuteSmartObject_Load(object sender, EventArgs e)
        {

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.tsbtnExecute.PerformClick();
        }

        private void tssbtnSave_ButtonClick(object sender, EventArgs e)
        {
            this.tsmiSave.PerformClick();
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            this.Try(() =>
            {
                if (this.m_ExecuteSmartObjectData == null)
                {
                    this.tsmiSaveAs.PerformClick();
                }
                else
                {
                    this.m_ExecuteSmartObjectData.SmartObjectDisplayName = this.SelectedSmartObject.Metadata.DisplayName;
                    this.m_ExecuteSmartObjectData.SmartObjectGuid = this.SelectedSmartObject.Guid;
                    this.m_ExecuteSmartObjectData.SmartObjectSystemName = this.SelectedSmartObject.Name;

                    this.m_ExecuteSmartObjectData.MethodDisplayName = this.SelectedSmartMethod.Metadata.DisplayName;
                    this.m_ExecuteSmartObjectData.MethodSystemName = this.SelectedSmartMethod.Name;

                    this.m_ExecuteSmartObjectData.InputProperties = this.m_InputPropertyCreators.Select(key => key()).ToArray();

                    Xml.Serialize(this.m_ExecuteSmartObjectData, this.m_ExecuteSmartObjectDataPath);
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
                        this.m_ExecuteSmartObjectData = new ExecuteSmartObjectData();
                        this.m_ExecuteSmartObjectDataPath = dlg.FileName;
                        this.tsmiSave.PerformClick();
                    }
                }
            });
        }

        private async void tsbtnOpen_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Filter = "XML|*.xml";
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        ExecuteSmartObjectData executeSmartObjectData = Xml.Deserialize<ExecuteSmartObjectData>(dlg.FileName);
                        this.m_ExecuteSmartObjectData = executeSmartObjectData;
                        this.m_ExecuteSmartObjectDataPath = dlg.FileName;

                        SmartObject smartObject = await this.LoadSmartObjectAsync(executeSmartObjectData.SmartObjectGuid);
                        this.m_PopulateInputProperties = true;
                        this.comboMethod.SelectedItem = null;
                        this.comboMethod.SelectedItem = smartObject.AllMethods.OfType<SmartMethodBase>().Single(key => key.Name == executeSmartObjectData.MethodSystemName);
                        this.m_PopulateInputProperties = false;
                    }
                }
            });
        }

        private async void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            await this.TryAsync(async () =>
            {
                await this.m_Context.Cache.RefreshSmartObjectAsync(this.SelectedSmartObject.Guid);
            });
        }
    }
}
