using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExtractCategory
{
    internal class ExtractCategoryExtension : Model.ITreeViewContextMenuExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension, Model.IGeneralOptionsExtension, Model.IOptionsPriorityExtension
    {
        #region Public Properties

        public string DisplayName => "Extract category";

        public int OptionsPriority => 100;

        #endregion

        #region Public Methods

        public Control CreateOptionsControl(K2SpyContext k2SpyContext)
        {
            CheckBox checkBox = new CheckBox()
            {
                Text = "Extract formatted definitions (may result in issues if imported due to xml:space=preserve)",
                Checked = Properties.Settings.Default.ExtractFormattedDefinitions,
                AutoSize = true,
                Dock = DockStyle.Top,
                AutoEllipsis = true
            };
            return checkBox;
        }

        public void CommitOptions(Control optionsControl)
        {
            Properties.Settings.Default.ExtractFormattedDefinitions = ((CheckBox)optionsControl).Checked;
        }

        public bool IsOptionsControlDirty(Control optionsControl)
        {
            return ((CheckBox)optionsControl).Checked != Properties.Settings.Default.ExtractFormattedDefinitions;
        }

        public ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            if (actingAsOrSelfTreeNode is CategoryTreeNode && !(actingAsOrSelfTreeNode is CategoryRootTreeNode))
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Extract category", null, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async ()=>
                        {
                            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                            {
                                dlg.Description = "Select the folder to extract the category to";
                                if (dlg.ShowDialog(context.MainForm) == DialogResult.OK)
                                {
                                    await Task.Run(() =>
                                    {
                                        Func<string, string> toValidFileName = name =>
                                        {
                                            name = name?.Trim();
                                            if (!string.IsNullOrWhiteSpace(name))
                                            {
                                                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                                                    name = name.Replace(c.ToString(), "_");
                                            }
                                            if (string.IsNullOrWhiteSpace(name))
                                                return "unknown";
                                            return name;
                                        };
                                        Action<CategoryTreeNode, string> extractCategory = null;
                                        DateTime begin = DateTime.Now;
                                        extractCategory = async (node, path) =>
                                        {
                                            System.IO.Directory.CreateDirectory(path);
                                            foreach (TreeNode child in node.Nodes)
                                            {
                                                context.QueueSetStatus($"Extracting {node.FullPath}");
                                                string newPath = System.IO.Path.Combine(path, toValidFileName(child.Text));
                                                if (child is CategoryTreeNode)
                                                {
                                                    extractCategory((CategoryTreeNode)child, newPath);
                                                }
                                                else if (child is K2SpyTreeNode)
                                                {
                                                    K2SpyTreeNode actAsOrSelf = await ((K2SpyTreeNode)child).GetActAsOrSelfAsync();
                                                    Task<Definition> task = null;
                                                    if (Properties.Settings.Default.ExtractFormattedDefinitions)
                                                        task = actAsOrSelf.GetFormattedDefinitionAsync() ?? actAsOrSelf.GetRawDefinitionAsync();
                                                    else
                                                        task = actAsOrSelf.GetRawDefinitionAsync() ?? actAsOrSelf.GetFormattedDefinitionAsync();
                                                    if (task != null)
                                                    {
                                                        Definition definition = task.GetAwaiter().GetResult();
                                                        if (definition != null && !string.IsNullOrEmpty(definition.Text))
                                                        {
                                                            if (definition.Type == DefinitionType.XML)
                                                                newPath += ".xml";
                                                            else if (definition.Type == DefinitionType.JSON)
                                                                newPath += ".json";
                                                            System.IO.File.WriteAllText(newPath, definition.Text, Encoding.UTF8);
                                                        }
                                                    }
                                                }
                                            }
                                        };
                                        extractCategory((CategoryTreeNode)actingAsOrSelfTreeNode, dlg.SelectedPath);
                                        TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                                        context.QueueSetStatus($"Extraction completed in {timeSpan}", true);
                                        System.Diagnostics.Process.Start(dlg.SelectedPath);
                                    });
                                }
                            }
                        },() => context.QueueSetStatus(""));
                    })
                };
            }
            return null;
        }

        #endregion
    }
}
