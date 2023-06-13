using K2Spy.ExtensionMethods;
using SourceCode.Forms.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.CheckOutManagement
{
    internal class CheckInCheckOutManagerExtension : /*Model.IInitializedExtension, */Model.ITreeViewContextMenuExtension, Model.ISearcherContextMenuExtension, Model.IAnalyzerContextMenuExtension
    {
        #region Private Fields

        private List<K2SpyTreeNode> m_Nodes = new List<K2SpyTreeNode>();

        #endregion

        #region Public Properties

        public string DisplayName => "Check-in/check-out manager";

        #endregion

        #region Public Methods

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
            if (treeNode.ImageKey == "FormCheckedOut" || treeNode.ImageKey == "ViewCheckedOut" || treeNode.ImageKey == "StyleProfileCheckedOut")
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Check in", Properties.Resources.CheckIn16, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async () => 
                        {
                            SourceCode.Forms.Management.FormsManager formsManager = await context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>();
                            if (actingAsOrSelfTreeNode is ViewInfoTreeNode viewInfoTreeNode)
                            {
                                SourceCode.Forms.Management.ViewInfo viewInfo = await context.Cache.ViewInfoCache.GetAsync(viewInfoTreeNode.ViewGuid);
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(viewInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.CheckInView(viewInfoTreeNode.ViewGuid);

                                await context.Cache.RefreshViewAsync(viewInfo.Guid);
                            }
                            else if (actingAsOrSelfTreeNode is FormInfoTreeNode formInfoTreeNode)
                            {
                                SourceCode.Forms.Management.FormInfo formInfo = await context.Cache.FormInfoCache.GetAsync(formInfoTreeNode.FormGuid);
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(formInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.CheckInForm(formInfoTreeNode.FormGuid);

                                await context.Cache.RefreshFormAsync(formInfo.Guid);
                            }
                            else if (actingAsOrSelfTreeNode is StyleProfileInfoTreeNode styleProfileInfoTreeNode)
                            {
                                SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await styleProfileInfoTreeNode.GetSourceAsync();
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(styleProfileInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.CheckInStyleProfile(styleProfileInfo.Guid);

                                await context.Cache.RefreshStyleProfileAsync(styleProfileInfo.Guid);
                            }
                        });
                    }),
                    new ToolStripMenuItem("Cancel check out", Properties.Resources.CancelCheckOut16, async (sender, e) =>
                    {
                        await context.MainForm.TryAsync(async () =>
                        {
                            SourceCode.Forms.Management.FormsManager formsManager = await context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>();
                            if (actingAsOrSelfTreeNode is ViewInfoTreeNode viewInfoTreeNode)
                            {
                                SourceCode.Forms.Management.ViewInfo viewInfo = await context.Cache.ViewInfoCache.GetAsync(viewInfoTreeNode.ViewGuid);
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(viewInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.UndoViewCheckOut(viewInfoTreeNode.ViewGuid);

                                await context.Cache.RefreshViewAsync(viewInfo.Guid);
                            }
                            else if (actingAsOrSelfTreeNode is FormInfoTreeNode formInfoTreeNode)
                            {
                                SourceCode.Forms.Management.FormInfo formInfo = await context.Cache.FormInfoCache.GetAsync(formInfoTreeNode.FormGuid);
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(formInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.UndoFormCheckOut(formInfoTreeNode.FormGuid);

                                await context.Cache.RefreshFormAsync(formInfo.Guid);
                            }
                            else if (actingAsOrSelfTreeNode is StyleProfileInfoTreeNode styleProfileInfoTreeNode)
                            {
                                SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await styleProfileInfoTreeNode.GetSourceAsync();
                                SourceCode.Forms.Management.FormsManager formsManagerWithImpersonation = await context.Cache.ConnectionFactory.CreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>(styleProfileInfo.CheckedOutBy);
                                using (formsManagerWithImpersonation.Connection)
                                    formsManagerWithImpersonation.UndoStyleProfileCheckOut(styleProfileInfo.Guid);

                                await context.Cache.RefreshStyleProfileAsync(styleProfileInfo.Guid);
                            }
                        });
                    })
                };
            }
            else if (treeNode.ImageKey == "WorkflowLocked")
            {
                return new ToolStripItem[]
                {
                    new ToolStripMenuItem("Unlock", Properties.Resources.Unlock_16x, async (sender,e) =>
                    {
                        await context.MainForm.TryAsync(async () =>
                        {
                            WorkflowTreeNode processSetTreeNode = (WorkflowTreeNode)actingAsOrSelfTreeNode;
                            SourceCode.WebDesigner.Management.ProcessInfo processInfo = await context.Cache.ProcessInfoCache.GetAsync(processSetTreeNode.ProcessFullName);
                            SourceCode.Designer.Client.K2DesignerManagementClient k2DesignerManagementClient = await context.Cache.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.Designer.Client.K2DesignerManagementClient>();
                            k2DesignerManagementClient.ExecuteFrameworkMethod(SourceCode.Designer.Client.FrameworkMethodInfoConstants.Hosts.SmartForms,
                                SourceCode.Designer.Client.FrameworkMethodInfoConstants.Id.Process,
                                SourceCode.Designer.Client.FrameworkMethodInfoConstants.ClassId.Processdataservice,
                                SourceCode.Designer.Client.FrameworkMethodInfoConstants.Method.UnlockProcess,
                                "",
                                processInfo.Id,
                                processInfo.LockedBy);
                            await context.Cache.RefreshWorkflowAsync(processSetTreeNode.ProcessFullName);
                        });
                    })
                };
            }
            else
            {
                // this node is not checked out, determine if we should show a check-out button
                FormInfoTreeNode formInfoTreeNode = actingAsOrSelfTreeNode as FormInfoTreeNode;
                ViewInfoTreeNode viewInfoTreeNode = actingAsOrSelfTreeNode as ViewInfoTreeNode;
                StyleProfileInfoTreeNode styleProfileInfoTreeNode = actingAsOrSelfTreeNode as StyleProfileInfoTreeNode;
                if (formInfoTreeNode != null || viewInfoTreeNode != null || styleProfileInfoTreeNode != null)
                {
                    return new ToolStripItem[]
                    {
                        new ToolStripMenuItem("Check out", Properties.Resources.CheckOut16, async (sender, e) =>
                        {
                            await context.MainForm.TryAsync(async () =>
                            {
                                SourceCode.Forms.Management.FormsManager formsManager =  context.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                                if (formInfoTreeNode != null)
                                {
                                    SourceCode.Forms.Management.FormInfo formInfo = await context.Cache.FormInfoCache.GetAsync(formInfoTreeNode.FormGuid);
                                    formsManager.CheckOutForm(formInfoTreeNode.FormGuid);
                                    await context.Cache.RefreshFormAsync(formInfo.Guid);
                                }
                                else if (viewInfoTreeNode != null)
                                {
                                    SourceCode.Forms.Management.ViewInfo viewInfo = await context.Cache.ViewInfoCache.GetAsync(viewInfoTreeNode.ViewGuid);
                                    formsManager.CheckOutView(viewInfoTreeNode.ViewGuid);
                                    await context.Cache.RefreshViewAsync(viewInfo.Guid);
                                }
                                else if (styleProfileInfoTreeNode != null)
                                {
                                    SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await styleProfileInfoTreeNode.GetSourceAsync();
                                    formsManager.CheckOutStyleProfile(styleProfileInfo.Guid);
                                    await context.Cache.RefreshStyleProfileAsync(styleProfileInfo.Guid);
                                }
                            });
                        })
                    };
                }
            }
            return null;
        }

        #endregion
    }
}