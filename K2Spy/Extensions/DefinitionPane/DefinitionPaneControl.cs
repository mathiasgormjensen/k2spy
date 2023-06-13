using K2Spy.ExtensionMethods;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.DefinitionPane
{
    internal partial class DefinitionPaneControl : UserControl
    {
        #region Private Fields

        private K2SpyContext m_Context;
        private K2SpyTreeNode m_TreeNode;
        private K2SpyTreeNode m_ActingAsOrSelfTreeNode;

        #endregion

        #region Constructors

        public DefinitionPaneControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task SelectedNodeChangedDelayed(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode, CancellationTokenSource cancellationTokenSource)
        {
            this.m_Context = context;
            this.m_TreeNode = treeNode;
            this.m_ActingAsOrSelfTreeNode = actingAsOrSelfTreeNode;

            await Task.Delay(1);
            this.ShowLoading();
            await Task.Run(async () =>
            {
                try
                {
                    string text = null;
                    TextFormat format = TextFormat.XML;

                    Task<Definition> task = (actingAsOrSelfTreeNode ?? treeNode)?.GetFormattedDefinitionAsync();
                    if (task != null)
                    {
                        Definition definition = await task;
                        text = definition.Text;
                        if (definition.Type == DefinitionType.JSON)
                            format = TextFormat.JSON;
                        else if (definition.Type == DefinitionType.XML)
                            format = TextFormat.XML;
                        else
                            format = TextFormat.Text;
                    }
                    if (cancellationTokenSource?.IsCancellationRequested != true)
                    {
                        this.InvokeIfRequired(() =>
                        {
                            if (string.IsNullOrEmpty(text))
                            {
                                text = $"(no definition details for {treeNode.Text})";
                                format = TextFormat.Text;
                            }
                            this.LoadData(text, format);
                            this.HideLoading();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Serilog.Log.Warning($"Failed to load definition of {treeNode.FullPath}: {ex}");
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        this.InvokeIfRequired(() =>
                        {
                            this.LoadData(ex.ToString(), TextFormat.Text);
                            this.HideLoading();
                        });
                    }
                }
            });
        }

        public void Reset()
        {
            this.LoadData("", TextFormat.Text);
        }

        public void LoadFile(string file, TextFormat format, bool focus = false)
        {
            string data = System.IO.File.ReadAllText(file);
            this.LoadData(data, format, focus);
        }

        public void LoadData(string data, TextFormat format, bool focus = false)
        {
            this.InvokeIfRequired(() =>
            {
                this.scintilla1.ReadOnly = false;
                switch (format)
                {
                    case TextFormat.Text:
                        this.InitTextSyntaxColoring();
                        break;
                    case TextFormat.XML:
                        this.InitXmlSyntaxColoring();
                        break;
                    case TextFormat.JSON:
                        this.InitJsonSyntaxColoring();
                        break;
                    case TextFormat.CSharp:
                        this.InitCppSyntaxColoring();
                        break;
                    default:
                        break;
                }
                this.scintilla1.Text = data;
                this.scintilla1.ReadOnly = true;
                if (focus)
                    this.scintilla1.Focus();
            });
        }

        public void ShowLoading()
        {
            this.loadingOverlay1.Show("Loading definition...");
        }

        public void HideLoading()
        {
            this.loadingOverlay1.Hide(true);
        }

        public void GoTo(int line, int position, bool focus = true)
        {
#if false
            position = this.scintilla1.Lines[line].Position + position;
            this.scintilla1.SetSelection(position, position + 10);
            this.scintilla1.ScrollCaret();
#else
            this.scintilla1.Lines[line].Goto();
            this.scintilla1.GotoPosition(this.scintilla1.CurrentPosition + position);
            if (focus)
                this.scintilla1.Focus();
#endif
        }

#endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ScintillaNET.Scintilla scintilla = this.scintilla1;// = new ScintillaNET.Scintilla();
            // this.pnlScintilla.Controls.Add(scintilla);
            scintilla.Dock = DockStyle.Fill;

            // INITIAL VIEW CONFIG
            this.scintilla1.WrapMode = WrapMode.None;
            this.scintilla1.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors();

            // INIT HOTKEYS
            InitHotkeys();
            this.InitTextSyntaxColoring();
        }

#region PASTE


        private void InitColors()
        {
            this.scintilla1.SetSelectionBackColor(true, IntToColor(0x70B7F6));
            this.scintilla1.CaretLineVisible = true;
            // this.scintilla1.CaretLineBackColor = IntToColor(0xFFD000);
            this.scintilla1.CaretLineBackColor = IntToColor(0xFFEA99);
            // this.scintilla1.CaretLineBackColorAlpha = 100;
        }

        private void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this.scintilla1, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(this.scintilla1, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            this.scintilla1.ClearCmdKey(Keys.Control | Keys.F);
            this.scintilla1.ClearCmdKey(Keys.Control | Keys.R);
            this.scintilla1.ClearCmdKey(Keys.Control | Keys.H);
            this.scintilla1.ClearCmdKey(Keys.Control | Keys.L);
            this.scintilla1.ClearCmdKey(Keys.Control | Keys.U);
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

            this.scintilla1.WrapMode = WrapMode.Word;

            this.scintilla1.Lexer = Lexer.Null;

            this.InitNumberMarginAndFolding(false, false);
        }

        private void InitJsonSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            this.scintilla1.WrapMode = WrapMode.None;

            this.scintilla1.Lexer = Lexer.Cpp;

            this.scintilla1.Styles[1].ForeColor(0x008000);
            this.scintilla1.Styles[2].ForeColor(0x008000);
            this.scintilla1.Styles[3].ForeColor(0x008000);
            this.scintilla1.Styles[15].ForeColor(0x008000);
            this.scintilla1.Styles[17].ForeColor(0x008000);
            this.scintilla1.Styles[18].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Json.BlockComment].ForeColor = IntToColor(0x8000);
            this.scintilla1.Styles[Style.Json.LineComment].ForeColor = IntToColor(0x8000);
            // this.scintilla1.Styles[Style.Json.BlockComment].ForeColor = IntToColor(0x008000);

            this.scintilla1.Styles[Style.Json.Default].ForeColor = IntToColor(0x000000);
            //this.scintilla1.Styles[Style.Json.Default].BackColor = IntToColor(0xFFFFFF);

            this.scintilla1.Styles[Style.Json.Number].ForeColor = IntToColor(0xFF8000);
            //this.scintilla1.Styles[Style.Json.Number].BackColor = IntToColor(0xFFFFFF);

            this.scintilla1.Styles[6].ForeColor = IntToColor(0x800000);
            //this.scintilla1.Styles[6].BackColor = IntToColor(0xFFFFFF);

            this.scintilla1.Styles[7].ForeColor = IntToColor(0x808080);
            //this.scintilla1.Styles[7].BackColor = IntToColor(0xFFFFFF);

            this.scintilla1.Styles[5].ForeColor = IntToColor(0x0000FF);
            //this.scintilla1.Styles[5].BackColor = IntToColor(0xFFFFFF);
            this.scintilla1.Styles[5].Bold = true;

            this.scintilla1.Styles[10].ForeColor = IntToColor(0x8000FF);
            //this.scintilla1.Styles[10].BackColor = IntToColor(0xFFFFFF);
            this.scintilla1.Styles[10].Bold = true;


            this.InitNumberMarginAndFolding(true, true);
        }

        private void InitXmlSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            this.scintilla1.WrapMode = WrapMode.None;

            this.scintilla1.Lexer = Lexer.Xml;

            this.scintilla1.Styles[Style.Xml.XmlStart].ForeColor = IntToColor(0xFF0000);
            this.scintilla1.Styles[Style.Xml.XmlEnd].ForeColor = IntToColor(0xFF0000);
            this.scintilla1.Styles[Style.Xml.Default].ForeColor = IntToColor(0x000000);
            this.scintilla1.Styles[Style.Xml.Default].Bold = true;
            this.scintilla1.Styles[Style.Xml.Comment].ForeColor = IntToColor(0x008000);
            this.scintilla1.Styles[Style.Xml.Number].ForeColor = IntToColor(0xFF0000);
            this.scintilla1.Styles[Style.Xml.DoubleString].ForeColor = IntToColor(0x8000FF);
            this.scintilla1.Styles[Style.Xml.DoubleString].Bold = true;
            this.scintilla1.Styles[Style.Xml.SingleString].ForeColor = IntToColor(0x8000FF);
            this.scintilla1.Styles[Style.Xml.SingleString].Bold = true;
            this.scintilla1.Styles[Style.Xml.Tag].ForeColor = IntToColor(0x0000FF);
            this.scintilla1.Styles[Style.Xml.TagEnd].ForeColor = IntToColor(0x0000FF);
            this.scintilla1.Styles[Style.Xml.TagUnknown].ForeColor = IntToColor(0x0000FF);
            this.scintilla1.Styles[Style.Xml.Attribute].ForeColor = IntToColor(0xFF0000);
            this.scintilla1.Styles[Style.Xml.AttributeUnknown].ForeColor = IntToColor(0xFF0000);
            this.scintilla1.Styles[Style.Xml.CData].ForeColor = IntToColor(0xFF8000);
            this.scintilla1.Styles[Style.Xml.Entity].ForeColor = IntToColor(0x000000);

            this.InitNumberMarginAndFolding(true, true);
        }

        private void InitCppSyntaxColoring()
        {
            this.ResetSyntaxColoring();

            this.scintilla1.WrapMode = WrapMode.None;

            this.scintilla1.Lexer = Lexer.Cpp;

            this.scintilla1.Styles[Style.Cpp.Preprocessor].ForeColor(0x8AAFEE);
            this.scintilla1.Styles[Style.Cpp.Word].ForeColor(0x0000FF);
            this.scintilla1.Styles[Style.Cpp.Word2].ForeColor(0x8000FF);
            this.scintilla1.Styles[Style.Cpp.Number].ForeColor(0xFF8000);
            this.scintilla1.Styles[Style.Cpp.String].ForeColor(0x808080);
            this.scintilla1.Styles[Style.Cpp.Character].ForeColor(0x808080);
            this.scintilla1.Styles[Style.Cpp.Operator].ForeColor(0x000080).Bold();
            this.scintilla1.Styles[Style.Cpp.Regex].Bold();
            this.scintilla1.Styles[Style.Cpp.Comment].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Cpp.CommentLine].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Cpp.CommentLineDoc].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Cpp.CommentDocKeyword].ForeColor(0x008000).Bold();
            this.scintilla1.Styles[Style.Cpp.CommentDocKeywordError].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Cpp.PreprocessorComment].ForeColor(0x008000);
            this.scintilla1.Styles[Style.Cpp.PreprocessorCommentDoc].ForeColor(0x008000);

            this.scintilla1.SetKeywords(0, "async await class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            this.scintilla1.SetKeywords(1, "void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File System Windows Forms ScintillaNET");

            this.InitNumberMarginAndFolding(true, true);
        }

        private void ResetSyntaxColoring()
        {
            // Configure the default style
            this.scintilla1.StyleResetDefault();
            this.scintilla1.Styles[Style.Default].Font = "Consolas";
            this.scintilla1.Styles[Style.Default].Size = 10;
            this.scintilla1.Styles[Style.Default].ForeColor = IntToColor(0x212121);
            this.scintilla1.Styles[Style.Default].BackColor = IntToColor(0xFFFFE1);
            this.scintilla1.StyleClearAll();

            this.scintilla1.SetKeywords(0, "");
            this.scintilla1.SetKeywords(1, "");
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
            this.scintilla1.Styles[Style.LineNumber].ForeColor = Color.Black;// IntToColor(BACK_COLOR);
            this.scintilla1.Styles[Style.LineNumber].BackColor = IntToColor(0xFFFFE1);
            //TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(FORE_COLOR);
            this.scintilla1.Styles[Style.IndentGuide].ForeColor = IntToColor(0xC2C3C9);

            var nums = this.scintilla1.Margins[NUMBER_MARGIN];
            nums.Width = visible ? 30 : 0;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;
        }

        private void InitCodeFolding(bool enabled = true)
        {
            this.scintilla1.SetFoldMarginColor(false, IntToColor(0xFFFFE1));
            this.scintilla1.SetFoldMarginColor(true, IntToColor(0xFFFFE1));
            this.scintilla1.SetFoldMarginHighlightColor(true, IntToColor(0xFFFFE1));

            string enabledAsString = enabled ? "1" : "0";
            // Enable code folding
            this.scintilla1.SetProperty("fold", enabledAsString);
            this.scintilla1.SetProperty("fold.compact", enabledAsString);
            this.scintilla1.SetProperty("fold.html", enabledAsString);

            // Configure a margin to display folding symbols
            this.scintilla1.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            this.scintilla1.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            this.scintilla1.Margins[FOLDING_MARGIN].Sensitive = true;
            this.scintilla1.Margins[FOLDING_MARGIN].Width = enabled ? 20 : 0;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                this.scintilla1.Markers[i].SetForeColor(Color.White);// IntToColor(0xC2C3C9)); // IntToColor(BACK_COLOR)); // styles for [+] and [-]
                this.scintilla1.Markers[i].SetBackColor(IntToColor(0xC2C3C9)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            this.scintilla1.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            this.scintilla1.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            this.scintilla1.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            this.scintilla1.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.scintilla1.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            this.scintilla1.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.scintilla1.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            this.scintilla1.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

#endregion


#region Quick Search Bar

        bool SearchIsOpen = false;

        private void OpenSearch()
        {

            SearchManager.SearchBox = this.TxtSearch;
            SearchManager.TextArea = this.scintilla1;

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
            if (e.KeyCode == Keys.Enter)
                SearchManager.Find(true, false);
        }

#endregion


#region Utils

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

#endregion

#endregion
    }
}