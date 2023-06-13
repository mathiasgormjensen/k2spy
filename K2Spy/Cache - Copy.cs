using K2Spy.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class Cache
    {
        #region Constructors

        public Cache(ConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
            this.CategoryDataCache = new InmemoryAsyncCache<CategoryDataCacheKey, SourceCode.Categories.Client.CategoryData>(async key =>
            {
                SourceCode.Categories.Client.Category category = await this.CategoryCache.GetAsync(key.CategoryId);
                return category.DataList.OfType<SourceCode.Categories.Client.CategoryData>().First(item => item.Data == key.Data);
            });
            this.CategoryCache = new PreloadedInmemoryAsyncCache<int, SourceCode.Categories.Client.Category>(() =>
            {
                SourceCode.Categories.Client.CategoryServer categoryServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Categories.Client.CategoryServer>();
                List<SourceCode.Categories.Client.Category> list = new List<SourceCode.Categories.Client.Category>();
                Action<SourceCode.Categories.Client.Category> action = null;
                action = c =>
                {
                    list.Add(c);
                    foreach (SourceCode.Categories.Client.Category child in c.GetChildCategories())
                    {
                        action(child);
                    }
                };

                action(categoryServer.GetCategoryManager(1, true).RootCategory);
                return list.ToDictionary(key => key.Id, key => key).ToArray();
            });

            this.FormInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.FormInfo>(() =>
            {
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                return formsManager.GetForms().Forms.ToDictionary(key => key.Guid, key => key).ToArray();
                // return formsManager.GetForm(formGuid);
            });
            this.FormCache = new InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.Form>(async formGuid =>
            {
                string xmlDefinition = await this.FormXmlDefinitionCache.GetAsync(formGuid);
                return new SourceCode.Forms.Authoring.Form(xmlDefinition);
            });

            this.FormXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Forms",
                async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    return new FileNameAndTimestamp(formGuid, formInfo.ModifiedDate);
                }, async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    string xmlDefinition = formsManager.GetFormDefinition(formGuid);
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.LoadXml(xmlDefinition);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                    builder.AppendLine($"Created by:     {formInfo.CreatedBy}");
                    builder.AppendLine($"Created on:     {formInfo.CreatedDate}");
                    builder.AppendLine($"Modified by:    {formInfo.ModifiedBy}");
                    builder.AppendLine($"Modified on:    {formInfo.ModifiedDate}");
                    if (formInfo.IsCheckedOut)
                        builder.AppendLine($"Checked out by: {formInfo.CheckedOutBy}");

                    builder.AppendLine($"Category:       {formInfo.CategoryPath}");

                    System.Xml.XmlComment comment = document.CreateComment(builder.ToString());
                    document.InsertBefore(comment, document.DocumentElement);
                    return Xml.PrettyPrint(document);
                });

            // views
            this.ViewInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.ViewInfo>(() =>
            {
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                return formsManager.GetViews().Views.ToDictionary(key => key.Guid, key => key).ToArray();
            });
            this.ViewCache = new InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.View>(async viewGuid =>
            {
                string xmlDefinition = await this.ViewXmlDefinitionCache.GetAsync(viewGuid);
                return new SourceCode.Forms.Authoring.View(xmlDefinition);
            });
            this.ViewXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Views",
                async viewGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.ViewInfo viewInfo = await this.ViewInfoCache.GetAsync(viewGuid);
                    return new FileNameAndTimestamp(viewGuid, viewInfo.ModifiedDate);
                }, async viewGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.ViewInfo viewInfo = await this.ViewInfoCache.GetAsync(viewGuid);
                    string xmlDefinition = formsManager.GetViewDefinition(viewGuid);
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.LoadXml(xmlDefinition);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                    builder.AppendLine($"Created by:     {viewInfo.CreatedBy}");
                    builder.AppendLine($"Created on:     {viewInfo.CreatedDate}");
                    builder.AppendLine($"Modified by:    {viewInfo.ModifiedBy}");
                    builder.AppendLine($"Modified on:    {viewInfo.ModifiedDate}");
                    if (viewInfo.IsCheckedOut)
                        builder.AppendLine($"Checked out by: {viewInfo.CheckedOutBy}");

                    builder.AppendLine($"Category:       {viewInfo.CategoryPath}");

                    System.Xml.XmlComment comment = document.CreateComment(builder.ToString());
                    document.InsertBefore(comment, document.DocumentElement);
                    return Xml.PrettyPrint(document);
                });

            // controls
            this.ControlTypeInfoCache = new PreloadedInmemoryAsyncCache<string, SourceCode.Forms.Management.ControlTypeInfo>(() =>
            {
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                return formsManager.GetControlTypes().ControlTypes.ToDictionary(key => key.FullName, key => key).ToArray();
            });

            this.ControlTypeDefinitionCache = new TemporaryLocalStorageAsyncCache<string>(
                "Controls",
                async controlFullName =>
                {
                    SourceCode.Forms.Management.ControlTypeInfo controlTypeInfo = await this.ControlTypeInfoCache.GetAsync(controlFullName);
                    string name = controlTypeInfo.Name;
                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                        name = name.Replace(c, '_');
                    return name;
                }, 
                async controlFullName =>
                {
                    SourceCode.Forms.Management.ControlTypeInfo controlTypeInfo = await this.ControlTypeInfoCache.GetAsync(controlFullName);
                    System.Xml.XmlDocument xmlDocument = controlTypeInfo.ToXml();
                    return Xml.PrettyPrint(xmlDocument);
                });

            //this.ViewInfoCache = new SimpleAsyncCache<Guid, SourceCode.Forms.Management.ViewInfo>(viewGuid =>
            //{
            //    SourceCode.Forms.Management.FormsManager formsManager = Session.Current.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
            //    return formsManager.GetView(viewGuid);
            //});

            // SmartObjects
            this.SmartObjectXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "SmartObjects",
                async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    return new FileNameAndTimestamp(smartObjectGuid, smartObjectInfo.ModifiedDate);
                }, async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    string xmlDefinition = smartObjectManagementServer.GetSmartObjectDefinition(false, smartObjectGuid);
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.LoadXml(xmlDefinition);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                    builder.AppendLine($"Created by:     {smartObjectInfo.CreatedBy}");
                    builder.AppendLine($"Created on:     {smartObjectInfo.CreateDate}");
                    builder.AppendLine($"Modified by:    {smartObjectInfo.ModifiedBy}");
                    builder.AppendLine($"Modified on:    {smartObjectInfo.ModifiedDate}");
                    builder.AppendLine($"Category:       {smartObjectInfo.CategoryPath}");

                    System.Xml.XmlComment comment = document.CreateComment(builder.ToString());
                    document.InsertBefore(comment, document.DocumentElement);

                    return Xml.PrettyPrint(document);
                });
            this.SmartObjectDefinitionCache = new InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.SmartObjectDefinition>(async smartObjectGuid =>
            {
                string xmlDefinition = await this.SmartObjectXmlDefinitionCache.GetAsync(smartObjectGuid);

                SourceCode.SmartObjects.Authoring.SmartObjectDefinition smartObjectDefinition = new SourceCode.SmartObjects.Authoring.SmartObjectDefinition();
                smartObjectDefinition.FromXml(xmlDefinition);
                return smartObjectDefinition;
            });
            this.SmartObjectClientCache = new InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Client.SmartObject>(smartObjectGuid =>
            {
                SourceCode.SmartObjects.Client.SmartObjectClientServer smartObjectClientServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Client.SmartObjectClientServer>();
                return smartObjectClientServer.GetSmartObject(smartObjectGuid);
            });
            this.SmartObjectInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Management.SmartObjectInfo>(() =>
            {
                SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                return smartObjectManagementServer.GetSmartObjects().SmartObjects.ToDictionary(key => key.Guid, key => key).ToArray();
            });

            // workflows
            this.ProcessInfoCache = new InmemoryAsyncCache<string, SourceCode.WebDesigner.Management.ProcessInfo>(processFullName =>
            {
                SourceCode.Designer.Client.K2DesignerManagementClient k2DesignerManagementClient = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Designer.Client.K2DesignerManagementClient>();
                IEnumerable<SourceCode.WebDesigner.Management.ProcessInfo> values = k2DesignerManagementClient.FindProcessesByName(processFullName);
                return values.SingleOrDefault();
            });
            this.WorkflowKprxXmlDefinitionCache = new PersistantLocalStorageAsyncCache<string>(
                "Workflows",
                async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    SourceCode.Workflow.Management.ProcessSet processSet = await this.ProcessSetCache.GetAsync(processFullName);
                    return new FileNameAndTimestamp(processFullName.Replace("\\", "_"), processSet.ModifiedDate);
                }, async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    byte[] data = workflowManagementServer.GetProcessKprx(processFullName);
                    string xmlDefinition = System.Text.Encoding.UTF8.GetString(data);
                    xmlDefinition = Xml.RemoveUtf8Bom(xmlDefinition);

                    SourceCode.Workflow.Management.ProcessSet processSet = await this.ProcessSetCache.GetAsync(processFullName);
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.LoadXml(xmlDefinition);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                    builder.AppendLine($"Created by:     {processSet.CreatedBy}");
                    builder.AppendLine($"Created on:     {processSet.CreatedDate}");
                    builder.AppendLine($"Modified by:    {processSet.ModifiedBy}");
                    builder.AppendLine($"Modified on:    {processSet.ModifiedDate}");
                    SourceCode.WebDesigner.Management.ProcessInfo processInfo = await this.ProcessInfoCache.GetAsync(processFullName);
                    if (processInfo?.IsLocked == true)
                        builder.AppendLine($"Locked by:      {processInfo.LockedBy}");

                    System.Xml.XmlComment comment = document.CreateComment(builder.ToString());
                    document.InsertBefore(comment, document.DocumentElement);

                    return Xml.PrettyPrint(document);
                });

            // service instances
            this.ServiceInstanceXmlDefinitionCache = new TemporaryLocalStorageAsyncCache<Guid>(
                "ServiceInstances",
                async serviceInstanceGuid =>
                {
                    return serviceInstanceGuid.ToString("N");
                },
                async serviceInstanceGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    string xmlDefinition = smartObjectManagementServer.GetServiceInstance(serviceInstanceGuid);

                    return Xml.PrettyPrint(xmlDefinition);
                });
            this.ServiceInstanceCache = new InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.ServiceInstance>(async serviceInstanceGuid =>
            {
                string xmlDefinition = await this.ServiceInstanceXmlDefinitionCache.GetAsync(serviceInstanceGuid);
                SourceCode.SmartObjects.Authoring.ServiceInstance result = SourceCode.SmartObjects.Authoring.ServiceInstance.Create(xmlDefinition);
                return result;
            });

            // service
            this.ServiceCache = new InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.Service>(serviceGuid =>
            {
                SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                string xml = smartObjectManagementServer.GetServiceExplorer(serviceGuid);
                SourceCode.SmartObjects.Authoring.ServiceExplorer serviceExplorer = SourceCode.SmartObjects.Authoring.ServiceExplorer.Create(xml);
                return serviceExplorer.Services.OfType<SourceCode.SmartObjects.Authoring.Service>().Single();
            });

            // process folder
            this.ProcessFolderCache = new InmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessFolder>(folderName =>
            {
                SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                return workflowManagementServer.GetProcessFolders().OfType<SourceCode.Workflow.Management.ProcessFolder>().First(key => key.FolderName.Equals(folderName, StringComparison.OrdinalIgnoreCase));
            });

            // process set
            this.ProcessSetCache = new InmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessSet>(processFullName =>
            {
                SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                return workflowManagementServer.GetProcSet(processFullName);
            });

            System.Xml.XmlDocument emptyDocument = new System.Xml.XmlDocument();
            this.TreeNodeXPathDocumentCache = new InmemoryAsyncCache<K2SpyTreeNode, System.Xml.XPath.XPathDocument>(async node =>
            {
                node.Refreshed += (sender, e) => this.TreeNodeXPathDocumentCache.Evict(node);
                node.Disposed += (sender, e) => this.TreeNodeXPathDocumentCache.Evict(node);
                Task<Definition> task = node.GetDefinitionAsync();
                Definition definition = null;
                if (task != null)
                    definition = await task;
                if (definition?.Type == DefinitionType.XML)
                {
                    return definition.FormattedDefinition.AsXPathDocument();
                }
                return null;
            });
        }

        #endregion

        #region Public Events

        public event EventHandler<EvictFromCacheEventArgs<Guid>> FormEvicted;

        public event EventHandler<EvictFromCacheEventArgs<Guid>> ViewEvicted;

        public event EventHandler<EvictFromCacheEventArgs<string>> ControlEvicted;

        public event EventHandler<EvictFromCacheEventArgs<Guid>> SmartObjectEvictedd;

        public event EventHandler<EvictFromCacheEventArgs<int>> CategoryEvicted;

        public event EventHandler<EvictFromCacheEventArgs<string>> WorkflowEvicted;

        #endregion

        #region Public Properties

        public InmemoryAsyncCache<CategoryDataCacheKey, SourceCode.Categories.Client.CategoryData> CategoryDataCache { get; private set; }

        public PreloadedInmemoryAsyncCache<int, SourceCode.Categories.Client.Category> CategoryCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.FormInfo> FormInfoCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.Form> FormCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> FormXmlDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.View> ViewCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.ViewInfo> ViewInfoCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> ViewXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<string, SourceCode.Forms.Management.ControlTypeInfo> ControlTypeInfoCache { get; private set; }

        public TemporaryLocalStorageAsyncCache<string> ControlTypeDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.SmartObjectDefinition> SmartObjectDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Client.SmartObject> SmartObjectClientCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Management.SmartObjectInfo> SmartObjectInfoCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> SmartObjectXmlDefinitionCache { get; private set; }

        public TemporaryLocalStorageAsyncCache<Guid> ServiceInstanceXmlDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.ServiceInstance> ServiceInstanceCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.Service> ServiceCache { get; private set; }

        public PersistantLocalStorageAsyncCache<string> WorkflowKprxXmlDefinitionCache { get; private set; }

        public InmemoryAsyncCache<string, SourceCode.WebDesigner.Management.ProcessInfo> ProcessInfoCache { get; private set; }

        public InmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessFolder> ProcessFolderCache { get; private set; }

        public InmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessSet> ProcessSetCache { get; private set; }

        public InmemoryAsyncCache<K2SpyTreeNode, System.Xml.XPath.XPathDocument> TreeNodeXPathDocumentCache { get; private set; }

        public ConnectionFactory ConnectionFactory { get; private set; }

        #endregion

        #region Public Methods

        public async Task ClearAsync()
        {
            this.TreeNodeXPathDocumentCache.Clear();
            this.CategoryCache.Clear();
            this.CategoryDataCache.Clear();
            this.ConnectionFactory.Clear();
            this.FormCache.Clear();
            this.FormInfoCache.Clear();
            this.FormXmlDefinitionCache.Clear();
            this.ProcessFolderCache.Clear();
            this.ProcessInfoCache.Clear();
            this.ProcessSetCache.Clear();
            this.ServiceCache.Clear();
            this.ServiceInstanceCache.Clear();
            this.ControlTypeInfoCache.Clear();
            await this.ControlTypeDefinitionCache.ClearAsync();
            await this.ServiceInstanceXmlDefinitionCache.ClearAsync();
            this.SmartObjectClientCache.Clear();
            this.SmartObjectDefinitionCache.Clear();
            this.SmartObjectInfoCache.Clear();
            this.SmartObjectXmlDefinitionCache.Clear();
            this.ViewCache.Clear();
            this.ViewInfoCache.Clear();
            this.ViewXmlDefinitionCache.Clear();
            this.WorkflowKprxXmlDefinitionCache.Clear();
        }

        public void EvictCategory(int categoryId)
        {
            this.CategoryCache.Evict(categoryId);
            this.CategoryDataCache.Evict(key => key.CategoryId == categoryId);
            this.ConnectionFactory.Remove<SourceCode.Categories.Client.CategoryServer>();
            this.CategoryEvicted?.Invoke(this, new EvictFromCacheEventArgs<int>(categoryId));
        }

        public void EvictWorkflow(string processFullName)
        {
            this.ProcessFolderCache.Evict(processFullName);
            this.ProcessInfoCache.Evict(processFullName);
            this.ProcessSetCache.Evict(processFullName);
            this.WorkflowKprxXmlDefinitionCache.Evict(processFullName);
            this.WorkflowEvicted?.Invoke(this, new EvictFromCacheEventArgs<string>(processFullName));
        }

        public void EvictForm(Guid formGuid)
        {
            this.FormCache.Evict(formGuid);
            this.FormInfoCache.Evict(formGuid);
            this.FormXmlDefinitionCache.Evict(formGuid);
            this.FormEvicted?.Invoke(this, new EvictFromCacheEventArgs<Guid>(formGuid));
        }

        public void EvictSmartObject(Guid smartObjectGuid)
        {
            this.SmartObjectClientCache.Evict(smartObjectGuid);
            this.SmartObjectDefinitionCache.Evict(smartObjectGuid);
            this.SmartObjectInfoCache.Evict(smartObjectGuid);
            this.SmartObjectXmlDefinitionCache.Evict(smartObjectGuid);
            this.SmartObjectEvictedd?.Invoke(this, new EvictFromCacheEventArgs<Guid>(smartObjectGuid));
        }

        public void EvictView(Guid viewGuid)
        {
            this.ViewCache.Evict(viewGuid);
            this.ViewInfoCache.Evict(viewGuid);
            this.ViewXmlDefinitionCache.Evict(viewGuid);
            this.ViewEvicted?.Invoke(this, new EvictFromCacheEventArgs<Guid>(viewGuid));
        }

        public void EvictControl(string controlFullName)
        {
            this.ControlTypeInfoCache.Evict(controlFullName);
            this.ControlEvicted?.Invoke(this, new EvictFromCacheEventArgs<string>(controlFullName));
        }

        #endregion
    }
}