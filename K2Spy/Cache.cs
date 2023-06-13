using K2Spy.ExtensionMethods;
using SourceCode.Forms.Management;
using SourceCode.SmartObjects.Management;
using SourceCode.SmartObjects.Services.Management;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
                using (new StopWatch("Preloading category stuff"))
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
                    action(categoryServer.GetCategoryManager(1, true, true).RootCategory);
                    ((Action)delegate ()
                    {
                        // clean K2s cache
                        System.Threading.Thread.Sleep(200);
                        categoryServer.GetCategoryExplorer(1, true, "", false);
                    }).BeginInvoke(null, null);
                    return list.ToDictionary(key => key.Id, key => key).ToArray();
                }
            });

            // style profiles
            this.StyleProfileInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.StyleProfileInfo>(() =>
            {
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                return formsManager.GetStyleProfiles().StyleProfiles.ToDictionary(key => key.Guid, key => key).ToArray();
            });
            this.StyleProfileCache = new InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.StyleProfile>(async styleProfileGuid =>
            {
                SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await this.StyleProfileInfoCache.GetAsync(styleProfileGuid);
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                string jsonDefinition = formsManager.GetStyleProfileDefinition(styleProfileGuid);
                SourceCode.Forms.Authoring.StyleProfile styleProfile = new SourceCode.Forms.Authoring.StyleProfile();
                styleProfile.FromJson(jsonDefinition);
                return styleProfile;
            });
            this.StyleProfileRawXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "StyleProfiles-raw",
                async styleProfileGuid =>
                {
                    SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await this.StyleProfileInfoCache.GetAsync(styleProfileGuid);
                    return new FileNameAndTimestamp(styleProfileGuid, styleProfileInfo.ModifiedDate);
                },
                async styleProfileGuid =>
                {
                    SourceCode.Forms.Authoring.StyleProfile styleProfile = await this.StyleProfileCache.GetAsync(styleProfileGuid);
                    return styleProfile.ToXml();
                });
            this.StyleProfileFormattedXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "StyleProfiles-formatted",
                async styleProfileGuid =>
                {
                    SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await this.StyleProfileInfoCache.GetAsync(styleProfileGuid);
                    return new FileNameAndTimestamp(styleProfileGuid, styleProfileInfo.ModifiedDate);
                },
                async styleProfileGuid =>
                {
                    SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await this.StyleProfileInfoCache.GetAsync(styleProfileGuid);
                    string xmlDefinition = await this.StyleProfileRawXmlDefinitionCache.GetAsync(styleProfileGuid);
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.LoadXml(xmlDefinition);
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                    builder.AppendLine($"Created by:     {styleProfileInfo.CreatedBy}");
                    builder.AppendLine($"Created on:     {styleProfileInfo.CreatedDate}");
                    builder.AppendLine($"Modified by:    {styleProfileInfo.ModifiedBy}");
                    builder.AppendLine($"Modified on:    {styleProfileInfo.ModifiedDate}");
                    if (styleProfileInfo.IsCheckedOut)
                        builder.AppendLine($"Checked out by: {styleProfileInfo.CheckedOutBy}");

                    builder.AppendLine($"Category:       {styleProfileInfo.CategoryPath}");

                    System.Xml.XmlComment comment = document.CreateComment(builder.ToString());
                    document.InsertBefore(comment, document.DocumentElement);
                    return Xml.PrettyPrint(document);
                });

            // forms
            this.FormInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.FormInfo>(() =>
            {
                SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                return formsManager.GetForms().Forms.ToDictionary(key => key.Guid, key => key).ToArray();
                // return formsManager.GetForm(formGuid);
            }, async (formGuid) =>
            {
                await this.FormRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(formGuid));
            });

            this.FormRawXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Forms-raw",
                async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    return new FileNameAndTimestamp(formGuid, formInfo.ModifiedDate);
                },
                async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    string xmlDefinition = formsManager.GetFormDefinition(formGuid);
                    return xmlDefinition;
                });
            this.FormFormattedXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Forms-formatted",
                async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    return new FileNameAndTimestamp(formGuid, formInfo.ModifiedDate);
                },
                async formGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.FormInfo formInfo = await this.FormInfoCache.GetAsync(formGuid);
                    string xmlDefinition = await this.FormRawXmlDefinitionCache.GetAsync(formGuid);
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
            this.ViewRawXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Views-raw",
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
                    return xmlDefinition;
                });
            this.ViewFormattedXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "Views-formatted",
                async viewGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.ViewInfo viewInfo = await this.ViewInfoCache.GetAsync(viewGuid);
                    return new FileNameAndTimestamp(viewGuid, viewInfo.ModifiedDate);
                }, async viewGuid =>
                {
                    SourceCode.Forms.Management.FormsManager formsManager = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Forms.Management.FormsManager>();
                    SourceCode.Forms.Management.ViewInfo viewInfo = await this.ViewInfoCache.GetAsync(viewGuid);
                    string xmlDefinition = await this.ViewRawXmlDefinitionCache.GetAsync(viewGuid);
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

            this.ControlTypeDefinitionCache = new InmemoryAsyncCache<string, string>(
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
            this.SmartObjectRawXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "SmartObjects-raw",
                async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    return new FileNameAndTimestamp(smartObjectGuid, smartObjectInfo.ModifiedDate);
                }, async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    string xmlDefinition = smartObjectManagementServer.GetSmartObjectDefinition(false, smartObjectGuid);
                    return xmlDefinition;
                });
            this.SmartObjectFormattedXmlDefinitionCache = new PersistantLocalStorageAsyncCache<Guid>(
                "SmartObjects-formatted",
                async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    return new FileNameAndTimestamp(smartObjectGuid, smartObjectInfo.ModifiedDate);
                }, async smartObjectGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await this.SmartObjectInfoCache.GetAsync(smartObjectGuid);
                    string xmlDefinition = await this.SmartObjectRawXmlDefinitionCache.GetAsync(smartObjectGuid);
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
                string xmlDefinition = await this.SmartObjectRawXmlDefinitionCache.GetAsync(smartObjectGuid);

                SourceCode.SmartObjects.Authoring.SmartObjectDefinition smartObjectDefinition = new SourceCode.SmartObjects.Authoring.SmartObjectDefinition();
                smartObjectDefinition.FromXml(xmlDefinition);
                return smartObjectDefinition;
            });
            this.SmartObjectCache = new InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Client.SmartObject>(smartObjectGuid =>
            {
                SourceCode.SmartObjects.Client.SmartObjectClientServer smartObjectClientServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Client.SmartObjectClientServer>();
                return smartObjectClientServer.GetSmartObject(smartObjectGuid);
            });
            this.SmartObjectInfoCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Management.SmartObjectInfo>(() =>
            {
                using (new StopWatch("Preload SmartObjectInfo"))
                {
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    return smartObjectManagementServer.GetSmartObjects().SmartObjects.ToDictionary(key => key.Guid, key => key).ToArray();
                }
            });

            // workflows
            this.ProcessInfoCache = new PreloadedInmemoryAsyncCache<string, SourceCode.WebDesigner.Management.ProcessInfo>(() =>
            {
                SourceCode.Designer.Client.K2DesignerManagementClient k2DesignerManagementClient = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Designer.Client.K2DesignerManagementClient>();
                IEnumerable<SourceCode.WebDesigner.Management.ProcessInfo> values = k2DesignerManagementClient.FindProcessesByName("%");
                return values.ToArray().ToDictionary(key => key.Name, key => key).ToArray();
            });
            this.WorkflowRawKprxXmlDefinitionCache = new PersistantLocalStorageAsyncCache<string>(
                "Workflows-raw",
                async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    SourceCode.Workflow.Management.ProcessSet processSet = await this.ProcessSetCache.GetAsync(processFullName);
                    string fileName = processFullName;
                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                        fileName = fileName.Replace(c.ToString(), "_");
                    return new FileNameAndTimestamp(fileName, processSet.ModifiedDate);
                },
                async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    byte[] data = workflowManagementServer.GetProcessKprx(processFullName);
                    string xmlDefinition = System.Text.Encoding.UTF8.GetString(data);
                    xmlDefinition = Xml.RemoveUtf8Bom(xmlDefinition);
                    return xmlDefinition;
                });

            this.WorkflowFormattedKprxXmlDefinitionCache = new PersistantLocalStorageAsyncCache<string>(
                "Workflows-formatted",
                async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    SourceCode.Workflow.Management.ProcessSet processSet = await this.ProcessSetCache.GetAsync(processFullName);
                    string fileName = processFullName;
                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                        fileName = fileName.Replace(c.ToString(), "_");
                    return new FileNameAndTimestamp(fileName, processSet.ModifiedDate);
                },
                async processFullName =>
                {
                    SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                    string xmlDefinition = await this.WorkflowRawKprxXmlDefinitionCache.GetAsync(processFullName);

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
            this.ServiceInstanceCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.ServiceInstance>(
                () =>
                {
                    using (new StopWatch("Preloading service instances"))
                    {
                        SourceCode.SmartObjects.Authoring.ServiceInstance[] serviceInstances = this.ServiceTypeCache.GetAllValuesAsync().GetAwaiter().GetResult().SelectMany(key => key.ServiceInstances.ToArray()).ToArray();
                        return serviceInstances.ToDictionary(key => key.Guid, key => key).ToArray();
                    }
                    //string xmlDefinition = await this.ServiceInstanceFormattedXmlDefinitionCache.GetAsync(serviceInstanceGuid);
                    //SourceCode.SmartObjects.Authoring.ServiceInstance result = SourceCode.SmartObjects.Authoring.ServiceInstance.Create(xmlDefinition);
                    //return result;
                });
            this.ServiceInstanceRawXmlDefinitionCache = new InmemoryAsyncCache<Guid, string>(
                async serviceInstanceGuid =>
                {
                    SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                    //string allXml = null;
                    //using (new StopWatch("GetFullServiceExplorer"))
                    //{
                    //    allXml = smartObjectManagementServer.GetServiceExplorer();
                    //}
                    string xmlDefinition = smartObjectManagementServer.GetServiceInstance(serviceInstanceGuid);

                    return xmlDefinition;
                });
            this.ServiceInstanceFormattedXmlDefinitionCache = new InmemoryAsyncCache<Guid, string>(
                async serviceInstanceGuid =>
                {
                    string xmlDefinition = await this.ServiceInstanceRawXmlDefinitionCache.GetAsync(serviceInstanceGuid);

                    return Xml.PrettyPrint(xmlDefinition);
                });

            // service
            this.ServiceTypeCache = new PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.Service>(() =>
            {
                SourceCode.SmartObjects.Management.SmartObjectManagementServer smartObjectManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.SmartObjects.Management.SmartObjectManagementServer>();
                string xml = smartObjectManagementServer.GetServiceExplorer(SourceCode.SmartObjects.Management.ServiceExplorerLevel.ServiceObject);
                SourceCode.SmartObjects.Authoring.ServiceExplorer serviceExplorer = SourceCode.SmartObjects.Authoring.ServiceExplorer.Create(xml);
                return serviceExplorer.Services.OfType<SourceCode.SmartObjects.Authoring.Service>().ToDictionary(key => key.Guid, key => key).ToArray();
            });
            //this.ServiceTypeCache.Evicted += async (sender, e) =>
            //{
            //    this.ServiceTypeInfoCache.Evict(e.Key);
            //    this.ServiceTypeXmlDefinitionCache.Evict(e.Key);
            //};

            this.ServiceTypeInfoCache = new PreloadedInmemoryAsyncCache<Guid, ServiceTypeInfo>(() =>
            {
                return this.ServiceTypeXmlDefinitionCache.GetAll().ToDictionary(key => key.Key, key => ServiceTypeInfo.Create(key.Value)).ToArray();
            });
            //async serviceTypeGuid =>
            //{
            //    string xml = await this.ServiceTypeXmlDefinitionCache.GetAsync(serviceTypeGuid);
            //    return ServiceTypeInfo.Create(xml);
            //});

            this.ServiceTypeXmlDefinitionCache = new InmemoryAsyncCache<Guid, string>(async serviceGuid =>
            {
                SourceCode.SmartObjects.Authoring.Service service = await this.ServiceTypeCache.GetAsync(serviceGuid);
                string xml = service.ToXml();
                return Xml.PrettyPrint(xml);
            });

            // process set
            this.ProcessSetCache = new PreloadedInmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessSet>(() =>
            {
                SourceCode.Workflow.Management.WorkflowManagementServer workflowManagementServer = this.ConnectionFactory.GetOrCreateBaseAPIConnection<SourceCode.Workflow.Management.WorkflowManagementServer>();
                SourceCode.Workflow.Management.ProcessSets processSets = workflowManagementServer.GetProcSets();
                return processSets.OfType<SourceCode.Workflow.Management.ProcessSet>().ToDictionary(key => key.FullName, key => key).ToArray();
            });
        }

        #endregion

        #region Public Events

        public event AsyncEventHandler<CacheEventArgs<Guid>> FormEvicted;

        public event AsyncEventHandler<CacheEventArgs<Guid>> FormCached;

        public event AsyncEventHandler<CacheEventArgs<Guid>> FormRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> StyleProfileEvicted;

        public event AsyncEventHandler<CacheEventArgs<Guid>> StyleProfileRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ViewEvicted;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ViewRefreshed;

        public event AsyncEventHandler<CacheEventArgs<string>> ControlEvicted;

        public event AsyncEventHandler<CacheEventArgs<string>> ControlRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> SmartObjectEvicted;

        //public event AsyncEventHandler<CacheEventArgs<Guid>> BeforeSmartObjectRefreshed;

        //public event AsyncEventHandler<CacheEventArgs<Guid>> AfterSmartObjectRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> SmartObjectRefreshed;

        public event AsyncEventHandler<CacheEventArgs<int>> CategoryEvicted;

        public event AsyncEventHandler<CacheEventArgs<int>> CategoryRefreshed;

        public event AsyncEventHandler<CacheEventArgs<string>> WorkflowEvicted;

        public event AsyncEventHandler<CacheEventArgs<string>> WorkflowRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ServiceTypeEvicted;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ServiceTypeRefreshed;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ServiceInstanceEvicted;

        public event AsyncEventHandler<CacheEventArgs<Guid>> ServiceInstanceRefreshed;

        #endregion

        #region Public Properties

        public InmemoryAsyncCache<CategoryDataCacheKey, SourceCode.Categories.Client.CategoryData> CategoryDataCache { get; private set; }

        public PreloadedInmemoryAsyncCache<int, SourceCode.Categories.Client.Category> CategoryCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.StyleProfileInfo> StyleProfileInfoCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.Forms.Authoring.StyleProfile> StyleProfileCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> StyleProfileRawXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> StyleProfileFormattedXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.FormInfo> FormInfoCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> FormRawXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> FormFormattedXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.Forms.Management.ViewInfo> ViewInfoCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> ViewRawXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> ViewFormattedXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<string, SourceCode.Forms.Management.ControlTypeInfo> ControlTypeInfoCache { get; private set; }

        public InmemoryAsyncCache<string, string> ControlTypeDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.SmartObjectDefinition> SmartObjectDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, SourceCode.SmartObjects.Client.SmartObject> SmartObjectCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Management.SmartObjectInfo> SmartObjectInfoCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> SmartObjectRawXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<Guid> SmartObjectFormattedXmlDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, string> ServiceInstanceFormattedXmlDefinitionCache { get; private set; }

        public InmemoryAsyncCache<Guid, string> ServiceInstanceRawXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.ServiceInstance> ServiceInstanceCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, ServiceTypeInfo> ServiceTypeInfoCache { get; private set; }

        public PreloadedInmemoryAsyncCache<Guid, SourceCode.SmartObjects.Authoring.Service> ServiceTypeCache { get; private set; }

        public InmemoryAsyncCache<Guid, string> ServiceTypeXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<string> WorkflowRawKprxXmlDefinitionCache { get; private set; }

        public PersistantLocalStorageAsyncCache<string> WorkflowFormattedKprxXmlDefinitionCache { get; private set; }

        public PreloadedInmemoryAsyncCache<string, SourceCode.WebDesigner.Management.ProcessInfo> ProcessInfoCache { get; private set; }

        public PreloadedInmemoryAsyncCache<string, SourceCode.Workflow.Management.ProcessSet> ProcessSetCache { get; private set; }

        public ConnectionFactory ConnectionFactory { get; private set; }

        #endregion

        #region Public Methods

        public void ClearDiskCache()
        {
            this.FormRawXmlDefinitionCache.Clear(true);
            this.FormFormattedXmlDefinitionCache.Clear(true);
            this.ViewRawXmlDefinitionCache.Clear(true);
            this.ViewFormattedXmlDefinitionCache.Clear(true);
            this.SmartObjectRawXmlDefinitionCache.Clear(true);
            this.SmartObjectFormattedXmlDefinitionCache.Clear(true);
            this.WorkflowRawKprxXmlDefinitionCache.Clear(true);
            this.WorkflowFormattedKprxXmlDefinitionCache.Clear(true);
            this.StyleProfileRawXmlDefinitionCache.Clear(true);
            this.StyleProfileFormattedXmlDefinitionCache.Clear(true);
        }

        public async Task ClearAsync()
        {
            this.CategoryCache.Clear();
            this.CategoryDataCache.Clear();
            this.ConnectionFactory.Clear();
            this.FormInfoCache.Clear();
            this.FormRawXmlDefinitionCache.Clear();
            this.FormFormattedXmlDefinitionCache.Clear();
            this.ProcessInfoCache.Clear();
            this.ProcessSetCache.Clear();
            this.ServiceTypeCache.Clear();
            this.ServiceInstanceCache.Clear();
            this.ControlTypeInfoCache.Clear();
            this.ControlTypeDefinitionCache.Clear();
            this.ServiceInstanceFormattedXmlDefinitionCache.Clear();
            this.SmartObjectCache.Clear();
            this.SmartObjectDefinitionCache.Clear();
            this.SmartObjectInfoCache.Clear();
            this.SmartObjectRawXmlDefinitionCache.Clear();
            this.SmartObjectFormattedXmlDefinitionCache.Clear();
            this.ViewInfoCache.Clear();
            this.ViewRawXmlDefinitionCache.Clear();
            this.ViewFormattedXmlDefinitionCache.Clear();
            this.WorkflowRawKprxXmlDefinitionCache.Clear();
            this.StyleProfileFormattedXmlDefinitionCache.Clear();
            this.StyleProfileRawXmlDefinitionCache.Clear();

#if DEBUG
            // TODO make sure that everything has been cleared

#endif
        }

        /// <summary>
        /// Evicts the category from the cache
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task EvictCategoryAsync(int categoryId, bool evictData, bool recursive)
        {
            this.CategoryCache.Evict(categoryId);
            if (evictData || recursive)
                this.CategoryDataCache.Evict(key => key.CategoryId == categoryId);
            if (recursive)
            {
                //this.ConnectionFactory.Remove<SourceCode.Categories.Client.CategoryServer>();
                KeyValuePair<int, SourceCode.Categories.Client.Category>[] children = (await this.CategoryCache.GetAllAsync(false)).Where(key => key.Value.ParentCatId == categoryId).ToArray();
                foreach (int childCategoryId in children.Select(key => key.Key))
                    await this.EvictCategoryAsync(childCategoryId, true, true);
            }
            await this.CategoryEvicted.InvokeAsync(this, new CacheEventArgs<int>(categoryId));
        }

        /// <summary>
        /// Evicts the category from the cache and raises the CategoryRefreshed-event
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task RefreshCategoryAsync(int categoryId, bool recursive)
        {
            await this.EvictCategoryAsync(categoryId, recursive, recursive);
            await this.CategoryCache.GetAsync(categoryId);
            await this.CategoryRefreshed.InvokeAsync(this, new CacheEventArgs<int>(categoryId));
        }

        public async Task EvictWorkflowAsync(string processFullName)
        {
            // this.ProcessFolderCache.Evict(processFullName);
            this.ProcessInfoCache.Evict(processFullName);
            this.ProcessSetCache.Evict(processFullName);
            this.WorkflowRawKprxXmlDefinitionCache.Evict(processFullName);
            await this.WorkflowEvicted.InvokeAsync(this, new CacheEventArgs<string>(processFullName));
        }

        public async Task RefreshWorkflowAsync(string processFullName, bool evict = true)
        {
            if (evict)
                await this.EvictWorkflowAsync(processFullName);
            await this.WorkflowRefreshed.InvokeAsync(this, new CacheEventArgs<string>(processFullName));
        }

        public async Task EvictFormAsync(Guid formGuid)
        {
            this.FormInfoCache.Evict(formGuid);
            this.FormRawXmlDefinitionCache.Evict(formGuid);
            this.FormFormattedXmlDefinitionCache.Evict(formGuid);
            await this.FormEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(formGuid));
        }

        public async Task RefreshFormAsync(Guid formGuid, bool evict = true)
        {
            if (evict)
                await this.EvictFormAsync(formGuid);
            await this.FormRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(formGuid));
        }

        public async Task EvictStyleProfileAsync(Guid styleProfileGuid)
        {
            this.StyleProfileInfoCache.Evict(styleProfileGuid);
            this.StyleProfileFormattedXmlDefinitionCache.Evict(styleProfileGuid);
            this.StyleProfileRawXmlDefinitionCache.Evict(styleProfileGuid);
            await this.StyleProfileEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(styleProfileGuid));
        }

        public async Task RefreshStyleProfileAsync(Guid styleProfileGuid, bool evict = true)
        {
            if (evict)
                await this.EvictStyleProfileAsync(styleProfileGuid);
            await this.StyleProfileRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(styleProfileGuid));
        }

        public async Task ClearSmartObjectsAsync()
        {
            this.SmartObjectCache.Clear();
            this.SmartObjectDefinitionCache.Clear();
            this.SmartObjectInfoCache.Clear();
            this.SmartObjectRawXmlDefinitionCache.Clear();
            this.SmartObjectFormattedXmlDefinitionCache.Clear();
        }

        public async Task EvictSmartObjectAsync(Guid smartObjectGuid)
        {
            this.SmartObjectCache.Evict(smartObjectGuid);
            this.SmartObjectDefinitionCache.Evict(smartObjectGuid);
            this.SmartObjectInfoCache.Evict(smartObjectGuid);
            this.SmartObjectRawXmlDefinitionCache.Evict(smartObjectGuid);
            this.SmartObjectFormattedXmlDefinitionCache.Evict(smartObjectGuid);
            await this.SmartObjectEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(smartObjectGuid));
        }

        public async Task RefreshSmartObjectAsync(Guid smartObjectGuid, bool evict = true)
        {
            if (evict)
            await this.EvictSmartObjectAsync(smartObjectGuid);
            CacheEventArgs<Guid> eventArgs = new CacheEventArgs<Guid>(smartObjectGuid);
            //await this.BeforeSmartObjectRefreshed.InvokeAsync(this, eventArgs);
            await this.SmartObjectRefreshed.InvokeAsync(this, eventArgs);
            //await this.AfterSmartObjectRefreshed.InvokeAsync(this, eventArgs);
        }

        public async Task EvictViewAsync(Guid viewGuid)
        {
            this.ViewInfoCache.Evict(viewGuid);
            this.ViewRawXmlDefinitionCache.Evict(viewGuid);
            this.ViewFormattedXmlDefinitionCache.Evict(viewGuid);
            await this.ViewEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(viewGuid));
        }

        public async Task RefreshViewAsync(Guid viewGuid, bool evict = true)
        {
            if (evict)
                await this.EvictViewAsync(viewGuid);
            await this.ViewRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(viewGuid));
        }

        public async Task ClearViewsAsync()
        {
            this.ViewInfoCache.Clear();
            this.ViewRawXmlDefinitionCache.Clear();
            this.ViewFormattedXmlDefinitionCache.Clear();
        }

        public async Task EvictControlAsync(string controlFullName)
        {
            this.ControlTypeInfoCache.Evict(controlFullName);
            await this.ControlEvicted.InvokeAsync(this, new CacheEventArgs<string>(controlFullName));
        }

        public async Task RefreshControlAsync(string controlFullName, bool evict = true)
        {
            if (evict)
                await this.EvictControlAsync(controlFullName);
            await this.ControlRefreshed.InvokeAsync(this, new CacheEventArgs<string>(controlFullName));
        }

        public async Task ClearControlsAsync()
        {
            this.ControlTypeDefinitionCache.Clear();
            this.ControlTypeInfoCache.Clear();
        }

        public async Task ClearFormsAsync()
        {
            this.FormFormattedXmlDefinitionCache.Clear();
            this.FormInfoCache.Clear();
            this.FormRawXmlDefinitionCache.Clear();
        }

        public async Task ClearServiceTypesAsync()
        {
            this.ServiceTypeCache.Clear();
            this.ServiceTypeInfoCache.Clear();
            this.ServiceTypeXmlDefinitionCache.Clear();
        }

        public async Task ClearWorkflowsAsync()
        {
            this.WorkflowFormattedKprxXmlDefinitionCache.Clear();
            this.WorkflowRawKprxXmlDefinitionCache.Clear();
            this.ProcessInfoCache.Clear();
            this.ProcessSetCache.Clear();
        }

        public async Task EvictServiceTypeAsync(Guid serviceTypeGuid)
        {
            this.ServiceTypeCache.Evict(serviceTypeGuid);
            this.ServiceTypeInfoCache.Evict(serviceTypeGuid);
            this.ServiceTypeXmlDefinitionCache.Evict(serviceTypeGuid);
            await this.ServiceTypeEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(serviceTypeGuid));
        }

        public async Task RefreshServiceTypeAsync(Guid serviceTypeGuid, bool evict = true)
        {
            if (evict)
                await this.EvictServiceTypeAsync(serviceTypeGuid);
            await this.ServiceTypeRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(serviceTypeGuid));
        }

        public async Task EvictServiceInstanceAsync(Guid serviceInstanceGuid)
        {
            this.ServiceInstanceCache.Evict(serviceInstanceGuid);
            this.ServiceInstanceFormattedXmlDefinitionCache.Evict(serviceInstanceGuid);
            this.ServiceInstanceRawXmlDefinitionCache.Evict(serviceInstanceGuid);
            await this.ServiceInstanceEvicted.InvokeAsync(this, new CacheEventArgs<Guid>(serviceInstanceGuid));
        }

        public async Task RefreshServiceInstanceAsync(Guid serviceInstanceGuid, bool evict = true)
        {
            if (evict)
                await this.EvictServiceInstanceAsync(serviceInstanceGuid);
            await this.ServiceInstanceRefreshed.InvokeAsync(this, new CacheEventArgs<Guid>(serviceInstanceGuid));
        }

        #endregion
    }
}