using ScintillaNET;
using ScintillaNET.Demo.Utils;
using SourceCode.Hosting.Client.BaseAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Plugins.DefinitionPane
{
    public partial class DefinitionPaneControl : UserControl
    {
        #region Constructors

        public DefinitionPaneControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public ScintillaNET.Scintilla Scintilla { get; private set; }

        #endregion

        #region Public Methods

        public void LoadFile(string file, TextFormat format, bool focus = false)
        {
            string data = System.IO.File.ReadAllText(file);
            this.LoadData(data, format, focus);
        }

        public void LoadData(string data, TextFormat format, bool focus = false)
        {
            this.InvokeIfRequired(() =>
            {
                this.Scintilla.ReadOnly = false;
                switch (format)
                {
                    case TextFormat.Text:
                        this.InitTextSyntaxColoring();
                        break;
                    case TextFormat.XML:
                        this.InitXmlSyntaxColoring();
                        break;
                    case TextFormat.CSharp:
                        this.InitCppSyntaxColoring();
                        break;
                    default:
                        break;
                }
                this.Scintilla.Text = data;
                this.Scintilla.ReadOnly = true;
                if (focus)
                    this.Scintilla.Focus();
            });
        }

        public void ShowLoading()
        {
            this.InvokeIfRequired(() =>
            {
                this.pbLoading.Style = ProgressBarStyle.Marquee;
                this.tableLoadingOverlay.Dock = DockStyle.Fill;
                this.tableLoadingOverlay.Visible = true;
                this.tableLoadingOverlay.BringToFront();
            });
        }

        public void HideLoading()
        {
            this.InvokeIfRequired(() =>
            {
                this.tableLoadingOverlay.Visible = false;
                this.pbLoading.Style = ProgressBarStyle.Blocks;
            });
        }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tableLoadingOverlay.Visible = false;

            ScintillaNET.Scintilla scintilla = this.Scintilla = new ScintillaNET.Scintilla();
            this.pnlScintilla.Controls.Add(scintilla);
            scintilla.Dock = DockStyle.Fill;

            // INITIAL VIEW CONFIG
            Scintilla.WrapMode = WrapMode.None;
            Scintilla.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors();

            // INIT HOTKEYS
            InitHotkeys();

        }

        #region PASTE


        private void InitColors()
        {
            Scintilla.SetSelectionBackColor(true, IntToColor(0x70B7F6));

        }

        private void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this.Scintilla, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(this.Scintilla, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            Scintilla.ClearCmdKey(Keys.Control | Keys.F);
            Scintilla.ClearCmdKey(Keys.Control | Keys.R);
            Scintilla.ClearCmdKey(Keys.Control | Keys.H);
            Scintilla.ClearCmdKey(Keys.Control | Keys.L);
            Scintilla.ClearCmdKey(Keys.Control | Keys.U);
        }

        private void InitNumberMarginAndFolding(bool visibleNumberMargin = true, bool enableCodeFolding = true)
        {
            // NUMBER MARGIN
            InitNumberMargin(visibleNumberMargin);

            // CODE FOLDING MARGIN
            InitCodeFolding(enableCodeFolding);
        }

        private void InitTextSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            Scintilla.WrapMode = WrapMode.Word;

            Scintilla.Lexer = Lexer.Null;

            this.InitNumberMarginAndFolding(false, false);
        }

        private void InitXmlSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            Scintilla.WrapMode = WrapMode.None;

            Scintilla.Lexer = Lexer.Xml;

            Scintilla.Styles[Style.Xml.XmlStart].ForeColor = IntToColor(0xFF0000);
            Scintilla.Styles[Style.Xml.XmlEnd].ForeColor = IntToColor(0xFF0000);
            Scintilla.Styles[Style.Xml.Default].ForeColor = IntToColor(0x000000);
            Scintilla.Styles[Style.Xml.Default].Bold = true;
            Scintilla.Styles[Style.Xml.Comment].ForeColor = IntToColor(0x008000);
            Scintilla.Styles[Style.Xml.Number].ForeColor = IntToColor(0xFF0000);
            Scintilla.Styles[Style.Xml.DoubleString].ForeColor = IntToColor(0x8000FF);
            Scintilla.Styles[Style.Xml.DoubleString].Bold = true;
            Scintilla.Styles[Style.Xml.SingleString].ForeColor = IntToColor(0x8000FF);
            Scintilla.Styles[Style.Xml.SingleString].Bold = true;
            Scintilla.Styles[Style.Xml.Tag].ForeColor = IntToColor(0x0000FF);
            Scintilla.Styles[Style.Xml.TagEnd].ForeColor = IntToColor(0x0000FF);
            Scintilla.Styles[Style.Xml.TagUnknown].ForeColor = IntToColor(0x0000FF);
            Scintilla.Styles[Style.Xml.Attribute].ForeColor = IntToColor(0xFF0000);
            Scintilla.Styles[Style.Xml.AttributeUnknown].ForeColor = IntToColor(0xFF0000);
            Scintilla.Styles[Style.Xml.CData].ForeColor = IntToColor(0xFF8000);
            Scintilla.Styles[Style.Xml.Entity].ForeColor = IntToColor(0x000000);

            this.InitNumberMarginAndFolding(true, true);
        }

        private void InitCppSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            Scintilla.WrapMode = WrapMode.None;

            Scintilla.Lexer = Lexer.Cpp;

            Scintilla.Styles[Style.Cpp.Preprocessor].ForeColor(0x8AAFEE);
            Scintilla.Styles[Style.Cpp.Word].ForeColor(0x0000FF);
            Scintilla.Styles[Style.Cpp.Word2].ForeColor(0x8000FF);
            Scintilla.Styles[Style.Cpp.Number].ForeColor(0xFF8000);
            Scintilla.Styles[Style.Cpp.String].ForeColor(0x808080);
            Scintilla.Styles[Style.Cpp.Character].ForeColor(0x808080);
            Scintilla.Styles[Style.Cpp.Operator].ForeColor(0x000080).Bold();
            Scintilla.Styles[Style.Cpp.Regex].Bold();
            Scintilla.Styles[Style.Cpp.Comment].ForeColor(0x008000);
            Scintilla.Styles[Style.Cpp.CommentLine].ForeColor(0x008000);
            Scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor(0x008000);
            Scintilla.Styles[Style.Cpp.CommentDocKeyword].ForeColor(0x008000).Bold();
            Scintilla.Styles[Style.Cpp.CommentDocKeywordError].ForeColor(0x008000);
            Scintilla.Styles[Style.Cpp.PreprocessorComment].ForeColor(0x008000);
            Scintilla.Styles[Style.Cpp.PreprocessorCommentDoc].ForeColor(0x008000);

            Scintilla.SetKeywords(0, "async await class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            Scintilla.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

            this.InitNumberMarginAndFolding(true, true);
        }

        private void ResetSyntaxColoring()
        {
            // Configure the default style
            Scintilla.StyleResetDefault();
            Scintilla.Styles[Style.Default].Font = "Consolas";
            Scintilla.Styles[Style.Default].Size = 10;
            Scintilla.Styles[Style.Default].ForeColor = IntToColor(0x212121);
            Scintilla.Styles[Style.Default].BackColor = IntToColor(0xFFFFE1);
            Scintilla.StyleClearAll();

            Scintilla.SetKeywords(0, "");
            Scintilla.SetKeywords(1, "");
        }


        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = false;

        private void InitNumberMargin(bool visible = true)
        {
            Scintilla.Styles[Style.LineNumber].ForeColor = Color.Black;// IntToColor(BACK_COLOR);
            Scintilla.Styles[Style.LineNumber].BackColor = IntToColor(0xFFFFE1);
            //TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(FORE_COLOR);
            Scintilla.Styles[Style.IndentGuide].ForeColor = IntToColor(0xC2C3C9);

            var nums = Scintilla.Margins[NUMBER_MARGIN];
            nums.Width = visible ? 30 : 0;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;
        }

        private void InitCodeFolding(bool enabled = true)
        {
            Scintilla.SetFoldMarginColor(false, IntToColor(0xFFFFE1));
            Scintilla.SetFoldMarginColor(true, IntToColor(0xFFFFE1));
            Scintilla.SetFoldMarginHighlightColor(true, IntToColor(0xFFFFE1));

            string enabledAsString = enabled ? "1" : "0";
            // Enable code folding
            Scintilla.SetProperty("fold", enabledAsString);
            Scintilla.SetProperty("fold.compact", enabledAsString);
            Scintilla.SetProperty("fold.html", enabledAsString);

            // Configure a margin to display folding symbols
            Scintilla.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            Scintilla.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            Scintilla.Margins[FOLDING_MARGIN].Sensitive = true;
            Scintilla.Margins[FOLDING_MARGIN].Width = enabled ? 20 : 0;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                Scintilla.Markers[i].SetForeColor(Color.White);// IntToColor(0xC2C3C9)); // IntToColor(BACK_COLOR)); // styles for [+] and [-]
                Scintilla.Markers[i].SetBackColor(IntToColor(0xC2C3C9)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            Scintilla.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            Scintilla.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            Scintilla.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            Scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            Scintilla.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            Scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            Scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            Scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        #endregion


        #region Quick Search Bar

        bool SearchIsOpen = false;

        private void OpenSearch()
        {

            SearchManager.SearchBox = TxtSearch;
            SearchManager.TextArea = Scintilla;

            if (!SearchIsOpen)
            {
                SearchIsOpen = true;
                this.InvokeIfRequired(delegate ()
                {
                    PanelSearch.Visible = true;
                    TxtSearch.Text = SearchManager.LastSearch;
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
            else
            {
                this.InvokeIfRequired(delegate ()
                {
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
        }

        private void CloseSearch()
        {
            if (SearchIsOpen)
            {
                SearchIsOpen = false;
                this.InvokeIfRequired(delegate ()
                {
                    PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            CloseSearch();
        }

        private void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(false, false);
        }
        private void BtnNextSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(true, false);
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true, true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion


        #region Utils

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        [Obsolete("", true)]
        public void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        #endregion

        #endregion

        private async void PopulateDefinition(K2SpyTreeNode node, System.Threading.CancellationTokenSource cancellationTokenSource)
        {
            string text = null;

            await Task.Delay(500);

            Task<Definition> task = node?.GetDefinitionAsync();
            if (task != null)
            {
                Definition definition = await task;
                text = definition.FormattedDefinition;
            }
            if (!cancellationTokenSource.IsCancellationRequested)
                this.InvokeIfRequired(() => this.Scintilla.Text = text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.InitXmlSyntaxColoring();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.InitTextSyntaxColoring();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.InitCppSyntaxColoring();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.LoadFile(@"C:\Users\Administrator\Desktop\loadthis.xml", TextFormat.XML);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.LoadFile(@"C:\github-mgj\private\K2Spy\K2Spy\Plugins\DefinitionPane\DefinitionPaneExtension.cs", TextFormat.CSharp);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.LoadFile(@"C:\Users\Administrator\Desktop\justincase.txt", TextFormat.Text);
        }
    }
}