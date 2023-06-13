using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.Upload
{
    internal class UploadExtension : Model.IToolsMenuExtension
    {
        public string DisplayName => "Upload";

        public ToolStripItem[] CreateToolsMenuItems(K2SpyContext k2SpyContext)
        {
#if StyleProfile
            string menuItemTitle = "Upload form, view or style profile...";
            string dialogTitle = "Select form, view or style profile to upload";
#else
            string menuItemTitle = "Upload form or view...";
            string dialogTitle = "Select form or view to upload";
#endif
            return new ToolStripItem[]
            {
                new ToolStripMenuItem(menuItemTitle, Properties.Resources.UploadFile_16x, async (sender, e) =>
                {
                    await k2SpyContext.MainForm.TryAsync(async() =>
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            dlg.Title = dialogTitle;
                            if (dlg.ShowDialog(k2SpyContext.MainForm) == DialogResult.OK)
                            {
                                k2SpyContext.QueueSetStatus("Uploading file...");
                                DateTime begin = DateTime.Now;
                                string definition = System.IO.File.ReadAllText(dlg.FileName, Encoding.UTF8);
                                definition = Xml.RemoveUtf8Bom(definition);
                                SourceCode.Forms.Management.FormsManager formsManager = await k2SpyContext.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.Forms.Management.FormsManager>();
                                formsManager.Deploy(definition, false);
                                TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                                k2SpyContext.QueueSetStatus($"Uploaded file in {timeSpan}", true);
                            }
                        }
                    }, () => k2SpyContext.QueueSetStatus(""));
                }),
                new ToolStripMenuItem("Upload SmartObject...", Properties.Resources.UploadPackage_16x, async (sender, e) =>
                {
                    await k2SpyContext.MainForm.TryAsync(async() =>
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            dlg.Title = "Select SmartObject to upload";
                            if (dlg.ShowDialog(k2SpyContext.MainForm) == DialogResult.OK)
                            {
                                SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = await k2SpyContext.ConnectionFactory.GetOrCreateBaseAPIConnectionAsync<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                                k2SpyContext.QueueSetStatus("Uploading SmartObject...");
                                DateTime begin = DateTime.Now;
                                string definition = System.IO.File.ReadAllText(dlg.FileName, Encoding.UTF8);
                                definition = Xml.RemoveUtf8Bom(definition);
                                smartObjectManagementServer.DeploySmartObject(definition);
                                TimeSpan timeSpan = DateTime.Now.Subtract(begin);
                                k2SpyContext.QueueSetStatus($"Uploaded SmartObject in {timeSpan}", true);
                            }
                        }
                    }, () => k2SpyContext.QueueSetStatus(""));
                })
            };
        }
    }
}