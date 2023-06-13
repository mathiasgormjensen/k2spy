using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.ExecuteSmartObject
{
    internal class ExecuteSmartObjectExtension : Model.ITreeViewContextMenuExtension, Model.IInternalExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension, Model.ICommandBarExtension, Model.IAnalyzerContextMenuMenuPriorityExtension, Model.ISearcherContextMenuPriorityExtension, Model.ITreeViewContextMenuPriorityExtension
    {
        public string DisplayName => "Execute SmartObject";

        public int AnalyzerContextMenuPriority => 1;

        public int SearcherContextMenuPriority => 1;

        public int TreeViewContextMenuPriority => 1;

        public ToolStripItem[] CreateAnalyzerContextMenuItems(K2SpyContext k2SpyContext, TreeNode analyzerNode, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateCommandBarItems(K2SpyContext context)
        {
            return new ToolStripItem[]
                {
                new ToolStripMenuItem("ADO Query", Properties.Resources.QueryView_16x, (sender, e) =>
                {
                    context.MainForm.Try(() =>
                    {
                        AdoQuery dlg = new AdoQuery(context);
                        dlg.Show();
                    });
                })
            };
        }

        public ToolStripItem[] CreateSearcherContextMenuItems(K2SpyContext k2SpyContext, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            return this.CreateTreeViewContextMenuItems(k2SpyContext, treeNode, actingAsOrSelfTreeNode);
        }

        public ToolStripItem[] CreateTreeViewContextMenuItems(K2SpyContext context, K2SpyTreeNode treeNode, K2SpyTreeNode actingAsOrSelfTreeNode)
        {
            Guid smartObjectGuid = Guid.Empty;
            string methodName = null;
            string listMethodName = null;
            if (actingAsOrSelfTreeNode is SmartObjectInfoTreeNode smartObjectInfoTreeNode)
            {
                smartObjectGuid = smartObjectInfoTreeNode.SmartObjectGuid;
            }
            else if (actingAsOrSelfTreeNode is SmartMethodTreeNode smartMethodTreeNode)
            {
                smartObjectGuid = smartMethodTreeNode.GetSource().SmartObjectInfo.Guid;
                methodName = smartMethodTreeNode.GetSource().Name;
                if (smartMethodTreeNode.GetSource().Type == SourceCode.SmartObjects.Management.MethodInfoType.list)
                    listMethodName = methodName;
            }

            if (smartObjectGuid != Guid.Empty)
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Execute SmartObject", Properties.Resources.SelectObject_16x, (sender, e) =>
                    {
                        context.MainForm.Try(() =>
                        {
                            ExecuteSmartObject dlg = new ExecuteSmartObject(context, smartObjectGuid,methodName);
                            dlg.Show();
                        });
                    }),
                    new ToolStripMenuItem("ADO Query", Properties.Resources.QueryView_16x, (sender, e) =>
                    {
                        context.MainForm.Try(() =>
                        {
                            AdoQuery dlg = new AdoQuery(context, smartObjectGuid, listMethodName);
                            dlg.Show();
                        });
                    })
                };
            }
            return null;
        }
    }
}