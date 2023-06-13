using K2Spy.ExtensionMethods;
using Serilog;
using SourceCode.Categories.Client;
using SourceCode.Forms.Utilities;
using SourceCode.Workflow.Management;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy
{
    internal class TreeNodeLoadingContext : IDisposable
    {
        private TreeNode m_Node;
        private string m_OriginalText;
        private string m_AssignedText;
        private bool m_Expanded;

        public TreeNodeLoadingContext(TreeNode node, bool clear = true)
        {
            if (node is CategoryRootTreeNode)
            {
            }
            using (TreeViewUpdateContext.CreateIfNecessary(node))
            {
                clear = true;
                this.m_Expanded = node.IsExpanded;
                this.m_Node = node;
                this.m_OriginalText = node.Text;
                //this.m_AssignedText = (node.Text += " (loading...)");
                if (clear)
                {
                    if (node is K2SpyTreeNode node2)
                        node2.ClearAndDisposeChildren();
                    else
                        node.Nodes.Clear();
                }
                node.Nodes.Add(new LoadingTreeNode());
                node.Expand();
                //Application.DoEvents();
            }
        }

        public void Dispose()
        {
            if (this.m_Node is CategoryRootTreeNode)
            {

            }
            if (!this.m_Expanded)
                this.m_Node.Collapse();
            if (this.m_Node.Text == this.m_AssignedText)
                this.m_Node.Text = this.m_OriginalText;
            this.m_Node.Nodes.Clear();
        }
    }

    public interface IActAsTreeNode
    {
        Task<K2SpyTreeNode> GetActAsAsync();
    }

    public class WorkflowsRootTreeNode : K2SpyTreeNode
    {
        private WorkflowsRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "Workflows", "Workflows", true)
        {
        }

        protected override bool RaiseRefreshedEventOnRefresh => true;

        public static async Task<WorkflowsRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            WorkflowsRootTreeNode workflowsRootTreeNode = new WorkflowsRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await workflowsRootTreeNode.InitializeAsync();
            return workflowsRootTreeNode;
        }

        public async override Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearWorkflowsAsync();
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                TreeNode[] nodes = this.DescendantsWhere(key => key is WorkflowTreeNode).ToArray();
                int count = nodes.Length;
                return $@"Workflow count: {count}
" + string.Join("\r\n", nodes.Select(key => "- " + key.Text));
            });
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            base.Nodes.Add(new LoadingTreeNode());


            List<K2SpyTreeNode> list = new List<K2SpyTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.Workflow.Management.ProcessSet[] processSets = await base.GetK2SpyContext().Cache.ProcessSetCache.GetAllValuesAsync();
                foreach (SourceCode.Workflow.Management.ProcessSet processSet in processSets.OrderBy(key => key.FullName))
                {
                    list.Add(await WorkflowTreeNode.CreateAsync(base.GetK2SpyContext(), processSet.FullName));
                }
            }
            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.Clear();
                base.Nodes.AddRange(list.ToArray());
            }
        }
    }

    public class WorkflowTreeNode : K2SpyCacheBackedTreeNode<string, SourceCode.Workflow.Management.ProcessSet>
    {
        private WorkflowTreeNode(K2SpyContext k2SpyContext, string processFullName)
            : base(k2SpyContext, "Workflow", processFullName, k2SpyContext.Cache.ProcessSetCache.GetAsync, true)
        {
            base.AddSearchableValue(processFullName);

            Cache cache = k2SpyContext.Cache;
            base.HandleCacheEventForCacheKey(
                "WorkflowRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "WorkflowEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                });
            // base.AddDisposeAction(() => session.WorkflowEvicted -= eventHandler);
        }

        public string ProcessFullName
        {
            get { return base.CacheKey; }
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictWorkflowAsync(this.ProcessFullName);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshWorkflowAsync(this.ProcessFullName);
            await base.RefreshAsync();
        }

        public static async Task<WorkflowTreeNode> CreateAsync(K2SpyContext k2SpyContext, string processFullName)
        {
            WorkflowTreeNode workflowTreeNode = new WorkflowTreeNode(k2SpyContext, processFullName);
            await workflowTreeNode.InitializeAsync();
            return workflowTreeNode;
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.WorkflowFormattedKprxXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override Task<Definition> GetRawDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.WorkflowRawKprxXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        protected async Task InitializeAsync()
        {
            SourceCode.Workflow.Management.ProcessSet processSet = await base.GetSourceAsync();
            base.Text = processSet.FullName;

            SourceCode.WebDesigner.Management.ProcessInfo processInfo = await base.GetK2SpyContext().Cache.ProcessInfoCache.GetAsync(this.CacheKey);
            if (processInfo?.IsLocked == true)
                this.SetImageKey("WorkflowLocked");
            else
                this.SetImageKey("Workflow");

            // TODO populate client events and other relevant items

            //int processId = processSet.ProcID;
            //Event[] events = base.GetK2SpyContext().ConnectionFactory.GetOrCreateBaseAPIConnection<WorkflowManagementServer>().GetProcessEvents(processId).OfType<Event>().ToArray();
            //List<K2SpyTreeNode> list = new List<K2SpyTreeNode>();
            //foreach (Event @event in events)
            //{
            //    list.Add(new ProcessEventTreeNode(base.GetK2SpyContext(), @event));
            //}
            //base.Nodes.AddRange(list.OrderBy(key => key.Text).ToArray());
        }
    }

    [Obsolete("Not yet supported", true)]
    public class ProcessEventTreeNode : K2SpyCacheBackedTreeNode<string, Event>
    {
        public ProcessEventTreeNode(K2SpyContext k2SpyContext, Event source) :
            base(k2SpyContext, null, "", source.Name, ignore => Task.FromResult(source), false)
        {
        }

        public override async Task<Definition> GetFormattedDefinitionAsync()
        {
            SourceCode.Configuration.ConfigurationManager yy = new SourceCode.Configuration.ConfigurationManager();
            typeof(SourceCode.Configuration.ConfigurationManager).InvokeMember("_productPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.SetField, null, null, new object[] { @"C:\Program Files (x86)\k2\Host Server\Bin" });
            typeof(SourceCode.Configuration.ConfigurationManager).InvokeMember("_configFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.SetField, null, null, new object[] { @"C:\Program Files (x86)\k2\Host Server\Bin\K2HostServer.exe.config" });
            string xml = (await (base.Parent as WorkflowTreeNode).GetRawDefinitionAsync()).Text;
            using (SourceCode.Framework.SerializationInfoProvider p = new SourceCode.Framework.SerializationInfoProvider())
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    object aa = p.Load(stream);
                    aa = aa;
                }
            }
            Event @event = await base.GetSourceAsync();
            return await Definition.CreateFromXmlAsync(() => Xml.SerializeToString(@event));
        }
    }

    public class ServiceTypesRootTreeNode : K2SpyTreeNode
    {
        private ServiceTypesRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "ServiceObjects", "Service Objects", true)
        {
        }

        protected override bool RaiseRefreshedEventOnRefresh => true;

        public static async Task<ServiceTypesRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            ServiceTypesRootTreeNode result = new ServiceTypesRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                int count = base.Nodes.Count;
                return $@"Service count: {count}
" + string.Join("\r\n", base.Nodes.OfType<TreeNode>().Select(key => "- " + key.Text));
            });
        }

        public async override Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearServiceTypesAsync();
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        protected async Task InitializeAsync()
        {
            List<ServiceTypeTreeNode> list = new List<ServiceTypeTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.SmartObjects.Authoring.Service[] services = await base.GetK2SpyContext().Cache.ServiceTypeCache.GetAllValuesAsync();
                foreach (SourceCode.SmartObjects.Authoring.Service service in services.OrderBy(key => key.DisplayName))
                {
                    list.Add(await ServiceTypeTreeNode.CreateAsync(base.GetK2SpyContext(), service.Guid));
                }
            }
            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.AddRange(list.ToArray());
            }
        }
    }

    public class ServiceTypeTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.SmartObjects.Authoring.Service>
    {
        private ServiceTypeTreeNode(K2SpyContext k2SpyContext, Guid guid)
            : base(k2SpyContext, "ServiceObject", guid, k2SpyContext.Cache.ServiceTypeCache.GetAsync, true)
        {
#if true
            base.HandleCacheEventForCacheKey(
                "ServiceTypeRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
#else
            base.HandleCacheEvent<Guid>(
                "ServiceTypeRefreshed",
                async (sender, e) =>
                {
                    if (e.CacheKey == base.CacheKey)
                    {
                        await base.OnRefreshedAsync();
                    }
                });
#endif
            base.HandleCacheEventForCacheKey(
                "ServiceTypeEvicted",
                async (sender, e) =>
                {
                    // await this.InitializeAsync();
                    await base.OnEvictedAsync();
                });
        }

        public static async Task<ServiceTypeTreeNode> CreateAsync(K2SpyContext k2SpyContext, Guid serviceGuid)
        {
            ServiceTypeTreeNode serviceTreeNode = new ServiceTypeTreeNode(k2SpyContext, serviceGuid);
            await serviceTreeNode.InitializeAsync();
            return serviceTreeNode;
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictServiceTypeAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshServiceTypeAsync(base.CacheKey);
            await base.RefreshAsync();
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromXmlAsync(this.GetXmlDefinitionAsync(), true);
        }

        protected async Task<string> GetXmlDefinitionAsync()
        {
            Guid guid = this.CacheKey;
            return await base.GetK2SpyContext().Cache.ServiceTypeXmlDefinitionCache.GetAsync(guid);
        }

        protected async Task InitializeAsync()
        {
#if true

            base.Nodes.Clear();
            base.Nodes.Add(new LoadingTreeNode());

            SourceCode.SmartObjects.Authoring.Service service = await base.GetSourceAsync();
            base.Text = service.Metadata.DisplayName;

            List<ServiceInstanceTreeNode> list = new List<ServiceInstanceTreeNode>();
            foreach (SourceCode.SmartObjects.Authoring.ServiceInstance serviceInstance in service.ServiceInstances.OfType<SourceCode.SmartObjects.Authoring.ServiceInstance>().OrderBy(key => key.Metadata.DisplayName))
            {
                list.Add(await ServiceInstanceTreeNode.CreateAsync(base.GetK2SpyContext(), serviceInstance.Guid));
            }
            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.Clear();
                base.Nodes.AddRange(list.ToArray());
            }
#else
            base.Nodes.Clear();
            SourceCode.SmartObjects.Authoring.Service service = await base.GetSourceAsync();
            base.Text = service.Metadata.DisplayName;

            // cache service instance
            foreach (SourceCode.SmartObjects.Authoring.ServiceInstance serviceInstance in service.ServiceInstances)
            {
                //base.GetK2SpyContext().Cache.ServiceInstanceCache.Cache(serviceInstance.Guid, serviceInstance);
                base.Nodes.Add(await ServiceInstanceTreeNode.CreateAsync(base.GetK2SpyContext(), serviceInstance.Guid));
            }
#endif
        }
    }

    public class ServiceInstanceTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.SmartObjects.Authoring.ServiceInstance>
    {
        private ServiceInstanceTreeNode(K2SpyContext k2SpyContext, Guid guid)
            : base(k2SpyContext, "ServiceInstance", guid, k2SpyContext.Cache.ServiceInstanceCache.GetAsync, true)
        {
            base.AddSearchableValue(guid);

            if (guid == new Guid("cd3804d6-973d-4de4-bf78-8427f6761011"))
            {

            }

            base.HandleCacheEventForCacheKey(
                "ServiceInstanceRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "ServiceInstanceEvicted",
                async (sender, e) =>
                {
                    // await this.InitializeAsync();
                    await base.OnEvictedAsync();
                });
        }

        public Guid ServiceInstanceGuid
        {
            get { return base.CacheKey; }
        }

        public static async Task<ServiceInstanceTreeNode> CreateAsync(K2SpyContext k2SpyContext, Guid guid)
        {
            ServiceInstanceTreeNode serviceInstanceTreeNode = new ServiceInstanceTreeNode(k2SpyContext, guid);
            await serviceInstanceTreeNode.InitializeAsync();
            return serviceInstanceTreeNode;
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictServiceInstanceAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshServiceInstanceAsync(base.CacheKey);
            await base.RefreshAsync();
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.ServiceInstanceFormattedXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override Task<Definition> GetRawDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.ServiceInstanceRawXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            SourceCode.SmartObjects.Authoring.ServiceInstance serviceInstance = await base.GetSourceAsync();
            base.Text = serviceInstance.Metadata.DisplayName;
        }

        protected async Task<string> GetDefinitionXmlAsync()
        {
            Guid guid = this.ServiceInstanceGuid;
            return await base.GetK2SpyContext().Cache.ServiceInstanceFormattedXmlDefinitionCache.GetAsync(guid);
        }
    }

    public class SmartObjectsRootTreeNode : K2SpyTreeNode
    {
        private SmartObjectsRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "SmartObjects", "SmartObjects", true)
        {
        }
        
        protected override bool RaiseRefreshedEventOnRefresh => true;

        public static async Task<SmartObjectsRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            SmartObjectsRootTreeNode result = new SmartObjectsRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync();
            return result;
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearSmartObjectsAsync();
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                int count = base.Nodes.Count;
                return $@"SmartObject count: {count}
" + string.Join("\r\n", base.Nodes.OfType<TreeNode>().Select(key => "- " + key.Text));
            });
        }

        protected async Task InitializeAsync()
        {
            List<SmartObjectInfoTreeNode> list = new List<SmartObjectInfoTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.SmartObjects.Management.SmartObjectInfo[] smartObjects = await base.GetK2SpyContext().Cache.SmartObjectInfoCache.GetAllValuesAsync();
                foreach (SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo in smartObjects.OrderBy(key => key.Metadata.DisplayName))
                {
                    list.Add(await SmartObjectInfoTreeNode.CreateAsync(base.GetK2SpyContext(), smartObjectInfo.Guid));
                }
            }
            base.Nodes.AddRange(list.ToArray());
        }
    }

    public class SmartPropertyTreeNode : K2SpyCacheBackedTreeNode<string, SourceCode.SmartObjects.Management.SmartPropertyInfo>
    {
        private SmartPropertyTreeNode(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo)
            : base(k2SpyContext, "Properties", smartPropertyInfo.Name, async key => smartPropertyInfo, false)
        {
        }

        public static async Task<SmartPropertyTreeNode> CreateAsync(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo)
        {
            SmartPropertyTreeNode result = new SmartPropertyTreeNode(k2SpyContext, smartPropertyInfo);
            await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo = await base.GetSourceAsync();
            string smartObjectXml = await base.GetK2SpyContext().Cache.SmartObjectRawXmlDefinitionCache.GetAsync(smartPropertyInfo.SmartObjectInfo.Guid);
            return await Definition.CreateFromXmlAsync(() =>
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.LoadXml(smartObjectXml);
                string smartMethodXml = document.SelectSingleNode($"/smartobjectroot/properties/property[@name='{smartPropertyInfo.Name}']").OuterXml;
                document.LoadXml(smartMethodXml);
                document.PrependChild(document.CreateComment(" this is a subset of the parent SmartObject definition "));

                string formattedXml = Xml.PrettyPrint(document);
                return formattedXml;
            });
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo = await base.GetSourceAsync();
            base.Text = smartPropertyInfo.Metadata.DisplayName + " : " + smartPropertyInfo.Type;
        }
    }

    public class SmartMethodTreeNode : K2SpyCacheBackedTreeNode<string, SourceCode.SmartObjects.Management.SmartMethodInfo>
    {
        private SmartMethodTreeNode(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo)
            : base(k2SpyContext, "Method", smartMethodInfo.Name, async key => smartMethodInfo, false)
        {
        }

        public static async Task<SmartMethodTreeNode> CreateAsync(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo)
        {
            SmartMethodTreeNode result = new SmartMethodTreeNode(k2SpyContext, smartMethodInfo);
            await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo = await base.GetSourceAsync();
            string smartObjectXml = await base.GetK2SpyContext().Cache.SmartObjectRawXmlDefinitionCache.GetAsync(smartMethodInfo.SmartObjectInfo.Guid);
            return await Definition.CreateFromXmlAsync(() =>
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.LoadXml(smartObjectXml);
                string smartMethodXml = document.SelectSingleNode($"/smartobjectroot/methods/method[@name='{smartMethodInfo.Name}']").OuterXml;
                document.LoadXml(smartMethodXml);
                document.PrependChild(document.CreateComment(" this is a subset of the parent SmartObject definition "));

                string formattedXml = Xml.PrettyPrint(document);
                return formattedXml;
            });
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo = await base.GetSourceAsync();
            base.Text = smartMethodInfo.Metadata.DisplayName + " : " + smartMethodInfo.Type.ToString().UpperFirstCharacter();
        }
    }

    public class SmartObjectInfoTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.SmartObjects.Management.SmartObjectInfo>
    {
        private SmartObjectInfoTreeNode(K2SpyContext k2SpyContext, Guid smartObjectGuid)
            : base(k2SpyContext, "SmartObject", smartObjectGuid, k2SpyContext.Cache.SmartObjectInfoCache.GetAsync, true)
        {
            base.AddSearchableValue(smartObjectGuid);

            //base.HandleCacheEvent<Guid>("SmartObjectEvicted",
            //    async (sender, e) =>
            //    {
            //        if (e.CacheKey == this.SmartObjectGuid)
            //        {
            //            using (new TreeViewUpdateContext(base.TreeView))
            //            {
            //                base.Nodes.Clear();
            //                base.Nodes.Add(new LoadingTreeNode());
            //                //base.Expand();
            //            }
            //        }
            //    });
            base.HandleCacheEventForCacheKey(
                "SmartObjectRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "SmartObjectEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                    // await this.InitializeAsync();
                });
        }

        public Guid SmartObjectGuid
        {
            get { return base.CacheKey; }
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictSmartObjectAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshSmartObjectAsync(this.SmartObjectGuid);
            await base.RefreshAsync();
        }

        public static async Task<SmartObjectInfoTreeNode> CreateAsync(K2SpyContext k2SpyContext, Guid guid)
        {
            SmartObjectInfoTreeNode result = new SmartObjectInfoTreeNode(k2SpyContext, guid);
            await result.InitializeAsync();
            return result;
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.SmartObjectFormattedXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override Task<Definition> GetRawDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.SmartObjectRawXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        protected async Task InitializeAsync()
        {
            //using (TreeViewUpdateContext.CreateIfNecessary(this))
            //    base.Nodes.Clear();

            List<TreeNode> nodes = new List<TreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await base.GetSourceAsync();
                base.Text = smartObjectInfo.Metadata.DisplayName;
                if (Properties.Settings.Default.PopulateSmartMethodsAndPropertiesInTreeView)
                {
                    foreach (SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo in smartObjectInfo.Properties.OfType<SourceCode.SmartObjects.Management.SmartPropertyInfo>().OrderBy(key => key.Metadata.DisplayName))
                    {
                        nodes.Add(await SmartPropertyTreeNode.CreateAsync(base.GetK2SpyContext(), smartPropertyInfo));
                    }

                    foreach (SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo in smartObjectInfo.Methods.OfType<SourceCode.SmartObjects.Management.SmartMethodInfo>().OrderBy(key => key.Metadata.DisplayName))
                    {
                        nodes.Add(await SmartMethodTreeNode.CreateAsync(base.GetK2SpyContext(), smartMethodInfo));
                    }
                }
            }
            // base.Nodes.Add(new MyK2SpyTreeNode(base.GetK2SpyContext()));
            using (TreeViewUpdateContext.CreateIfNecessary(this))
                base.Nodes.AddRange(nodes.ToArray());
        }

        private class MyK2SpyTreeNode : K2SpyTreeNode
        {
            public MyK2SpyTreeNode(K2SpyContext k2SpyContext)
                : base(k2SpyContext, "", DateTime.Now.ToLongTimeString())
            {
            }
        }

        protected async Task<string> GetDefinitionXmlAsync()
        {
            Guid guid = this.CacheKey;
            return await base.GetK2SpyContext().Cache.SmartObjectFormattedXmlDefinitionCache.GetAsync(guid);
        }
    }

    public class ControlsRootTreeNode : K2SpyTreeNode
    {
        private ControlsRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "Controls", "Controls", true)
        {
        }

        protected override bool RaiseRefreshedEventOnRefresh => true;

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearControlsAsync();
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        public static async Task<ControlsRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            ControlsRootTreeNode result = new ControlsRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                int count = base.Nodes.Count;
                return $@"Control count: {count}
" + string.Join("\r\n", base.Nodes.OfType<TreeNode>().Select(key => "- " + key.Text));
            });
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            base.Nodes.Add(new LoadingTreeNode());
            List<ControlTypeInfoTreeNode> list = new List<ControlTypeInfoTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.Forms.Management.ControlTypeInfo[] controls = await base.GetK2SpyContext().Cache.ControlTypeInfoCache.GetAllValuesAsync();
                foreach (SourceCode.Forms.Management.ControlTypeInfo control in controls.OrderBy(key => key.DisplayName))
                {
                    list.Add(await ControlTypeInfoTreeNode.CreateAsync(base.GetK2SpyContext(), control.FullName));
                }
            }

            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.Clear();
                base.Nodes.AddRange(list.ToArray());
            }
        }
    }

    public class ControlTypeInfoTreeNode : K2SpyCacheBackedTreeNode<string, SourceCode.Forms.Management.ControlTypeInfo>
    {
        private ControlTypeInfoTreeNode(K2SpyContext k2SpyContext, string controlFullName)
            : base(k2SpyContext, "Control", controlFullName, k2SpyContext.Cache.ControlTypeInfoCache.GetAsync, true)
        {
            base.AddSearchableValue(controlFullName);

            base.HandleCacheEventForCacheKey(
                "ControlRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "ControlEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                });
        }

        public string ControlFullName
        {
            get { return base.CacheKey; }
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictControlAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }
        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshControlAsync(this.ControlFullName);
            await base.RefreshAsync();
        }

        public static async Task<ControlTypeInfoTreeNode> CreateAsync(K2SpyContext k2SpyContext, string controlFullName)
        {
            ControlTypeInfoTreeNode viewInfoTreeNode = new ControlTypeInfoTreeNode(k2SpyContext, controlFullName);
            await viewInfoTreeNode.InitializeAsync();
            return viewInfoTreeNode;
        }

        public override async Task<Definition> GetFormattedDefinitionAsync()
        {
            Definition result = await Definition.CreateFromXmlAsync(this.GetDefinitionXmlAsync());
            return result;
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            SourceCode.Forms.Management.ControlTypeInfo controlTypeInfo = await base.GetSourceAsync();
            base.Text = controlTypeInfo.DisplayName;
        }

        protected async Task<string> GetDefinitionXmlAsync()
        {
            string controlFullName = this.ControlFullName;
            return await base.GetK2SpyContext().Cache.ControlTypeDefinitionCache.GetAsync(controlFullName);
        }
    }

    public class ViewsRootTreeNode : K2SpyTreeNode
    {
        private ViewsRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "Views", "Views", true)
        {
        }

        protected override bool RaiseRefreshedEventOnRefresh => true;

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearViewsAsync();
            await this.InitializeAsync();

            using (TreeViewUpdateContext.CreateIfNecessary(base.TreeView))
            {
                foreach (Guid formGuid in await base.GetK2SpyContext().Cache.ViewInfoCache.GetAllKeysAsync())
                    await base.GetK2SpyContext().Cache.RefreshViewAsync(formGuid, false);
            }

            await base.RefreshAsync();
        }

        public static async Task<ViewsRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            ViewsRootTreeNode result = new ViewsRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                int count = base.Nodes.Count;
                return $@"View count: {count}
" + string.Join("\r\n", base.Nodes.OfType<TreeNode>().Select(key => "- " + key.Text));
            });
        }

        protected async Task InitializeAsync()
        {
            List<ViewInfoTreeNode> list = new List<ViewInfoTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.Forms.Management.ViewInfo[] views = await base.GetK2SpyContext().Cache.ViewInfoCache.GetAllValuesAsync();
                foreach (SourceCode.Forms.Management.ViewInfo viewInfo in views.OrderBy(key => key.DisplayName))
                {
                    list.Add(await ViewInfoTreeNode.CreateAsync(base.GetK2SpyContext(), viewInfo.Guid));
                }
            }
            base.Nodes.AddRange(list.ToArray());
        }
    }

    public class ViewInfoTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.Forms.Management.ViewInfo>
    {
        private ViewInfoTreeNode(K2SpyContext k2SpyContext, Guid viewGuid)
            : base(k2SpyContext, "View", viewGuid, k2SpyContext.Cache.ViewInfoCache.GetAsync, true)
        {
            base.AddSearchableValue(viewGuid);

            base.HandleCacheEventForCacheKey(
                "ViewRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "ViewEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                });
        }

        public Guid ViewGuid
        {
            get { return base.CacheKey; }
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictViewAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshViewAsync(this.ViewGuid);
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        public static async Task<ViewInfoTreeNode> CreateAsync(K2SpyContext k2SpyContext, Guid viewGuid)
        {
            ViewInfoTreeNode viewInfoTreeNode = new ViewInfoTreeNode(k2SpyContext, viewGuid);
            await viewInfoTreeNode.InitializeAsync();
            return viewInfoTreeNode;
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.ViewFormattedXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override Task<Definition> GetRawDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.ViewRawXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            SourceCode.Forms.Management.ViewInfo viewInfo = await base.GetSourceAsync();
            base.Text = viewInfo.DisplayName;

            if (viewInfo.IsCheckedOut)
                this.SetImageKey("ViewCheckedOut");
            else
                this.SetImageKey("View");
        }
    }

    public class FormsRootTreeNode : K2SpyTreeNode
    {
        private FormsRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "Forms", "Forms", true)
        {
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.ClearFormsAsync();
            await this.InitializeAsync();

            using (TreeViewUpdateContext.CreateIfNecessary(base.TreeView))
            {
                foreach (Guid formGuid in await base.GetK2SpyContext().Cache.FormInfoCache.GetAllKeysAsync())
                    await base.GetK2SpyContext().Cache.RefreshFormAsync(formGuid, false);
            }

            await base.RefreshAsync();
            await base.OnRefreshedAsync();
        }

        public static async Task<FormsRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            FormsRootTreeNode result = new FormsRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync();
            return result;
        }

        public async override Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromStringAsync(() =>
            {
                int count = base.Nodes.Count;
                return $@"Form count: {count}
" + string.Join("\r\n", base.Nodes.OfType<TreeNode>().Select(key => "- " + key.Text));
            });
        }

        protected async Task InitializeAsync()
        {
            base.Nodes.Clear();
            base.Nodes.Add(new LoadingTreeNode());
            List<K2SpyTreeNode> list = new List<K2SpyTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.Forms.Management.FormInfo[] forms = await base.GetK2SpyContext().Cache.FormInfoCache.GetAllValuesAsync();

                foreach (SourceCode.Forms.Management.FormInfo formInfo in forms.OrderBy(key => key.DisplayName))
                {
                    list.Add(await FormInfoTreeNode.CreateAsync(base.GetK2SpyContext(), formInfo.Guid));
                }
            }

            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.Clear();
                base.Nodes.AddRange(list.ToArray());
            }
        }
    }

    public class FormInfoTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.Forms.Management.FormInfo>
    {
        #region Constructors

        private FormInfoTreeNode(K2SpyContext k2SpyContext, Guid formGuid)
            : base(k2SpyContext, "Form", formGuid, k2SpyContext.Cache.FormInfoCache.GetAsync, true)
        {
            base.AddSearchableValue(formGuid);

            base.HandleCacheEventForCacheKey(
                "FormRefreshed",
                async (sender, e) =>
                {
                    // await Task.Delay(01);
                    // await base.GetSourceAsync();
                    // await Task.CompletedTask;
                    await this.InitializeAsync();
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "FormEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                });
        }

        #endregion

        #region Public Properties

        public Guid FormGuid
        {
            get { return base.CacheKey; }
        }

        #endregion

        #region Public Methods

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictFormAsync(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            await base.GetK2SpyContext().Cache.RefreshFormAsync(this.FormGuid);
            await base.RefreshAsync();
        }

        public static async Task<FormInfoTreeNode> CreateAsync(K2SpyContext k2SpyContext, Guid formGuid)
        {
            FormInfoTreeNode formInfoTreeNode = new FormInfoTreeNode(k2SpyContext, formGuid);
            await formInfoTreeNode.InitializeAsync();
            return formInfoTreeNode;
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.FormFormattedXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override Task<Definition> GetRawDefinitionAsync()
        {
            return Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.FormRawXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        #endregion

        #region Protected Methods

        private SourceCode.Forms.Management.FormInfo m_Source;
        protected async Task InitializeAsync()
        {
            SourceCode.Forms.Management.FormInfo source = await base.GetSourceAsync();
            base.Text = source.DisplayName;
            if (source.IsCheckedOut)
                this.SetImageKey("FormCheckedOut");
            else
                this.SetImageKey("Form");
        }

        #endregion
    }

    internal interface IInitializeChildren
    {
        Task InitializeChildrenAsync();
    }

    public class CategoryRootTreeNode : CategoryTreeNode
    {
        private CategoryRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, 1)
        {
            base.Text = "All Items";

            this.ImageKey = "RootItem";
        }

        public static async Task<CategoryRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            CategoryRootTreeNode result = new CategoryRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await result.InitializeAsync(false);
            return result;
        }
    }

    public class Definition
    {
        private Definition()
        {

        }
        public Definition(string definition, DefinitionType definitionType)
        {
            this.Text = definition;
            this.Type = definitionType;
        }

        public DefinitionType Type { get; private set; }

        public string Text { get; private set; }

        //[Obsolete("",true)]
        //public static Task<Definition> CreateFromXmlAsync(string xml)
        //{
        //    return Definition.CreateFromXmlAsync(() => xml);
        //}

        public static async Task<Definition> CreateFromExceptionAsync(Exception error)
        {
            Definition result = new Definition(error.ToString(), DefinitionType.Text);
            return result;
        }

        public static async Task<Definition> CreateFromStringAsync(Func<string> callback)
        {
            try
            {
                string data = await Task.Run(() => callback());
                return new Definition(data, DefinitionType.Text);
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Failed to create definition from string: {ex}");
                Definition result = await Definition.CreateFromExceptionAsync(ex);
                return result;
            }
        }

        public static async Task<Definition> CreateFromXmlAsync(Task<string> task, bool prettyPrint = false)
        {
            try
            {
                string xml = await task;
                if (!string.IsNullOrEmpty(xml))
                {
                    if (prettyPrint)
                        xml = Xml.PrettyPrint(xml);

                    return new Definition(xml, DefinitionType.XML);
                }
                return null;
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Failed to create definition from XML-string: {ex}");
                Definition result = await Definition.CreateFromExceptionAsync(ex);
                return result;
            }
        }

        public static async Task<Definition> CreateFromJsonAsync(Task<string> task)
        {
            try
            {
                string json = await task;
                if (!string.IsNullOrEmpty(json))
                {
                    return new Definition(json, DefinitionType.JSON);
                }
                return null;
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning($"Failed to create definition from JSON-string: {ex}");
                Definition result = await Definition.CreateFromExceptionAsync(ex);
                return result;
            }
        }

        public static async Task<Definition> CreateFromXmlAsync(Func<string> callback, bool prettyPrint = false)
        {
            return await Definition.CreateFromXmlAsync(Task.Run(callback), prettyPrint);
        }
    }

    public enum DefinitionType
    {
        Unknown,
        XML,
        JSON,
        Text
    }

    public class CategoryTreeNode : K2SpyCacheBackedTreeNode<int, SourceCode.Categories.Client.Category>, IInitializeChildren
    {
        #region Private Fields

        private System.Threading.CancellationTokenSource m_InitializeAsyncCancellationTokenSource;

        #endregion

        #region Constructors

        protected CategoryTreeNode(K2SpyContext k2SpyContext, int categoryId)
            : base(k2SpyContext, "Category", categoryId, k2SpyContext.Cache.CategoryCache.GetAsync, true)
        {
            base.HandleCacheEventForCacheKey(
                "CategoryRefreshed",
                async (sender, e) =>
                {
                    await this.InitializeAsync(true);
                    await this.InitializeChildrenAsync(true);
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "CategoryEvicted",
                async (sender, e) =>
                {
                    //await this.InitializeAsync(true);
                    await base.OnEvictedAsync();
                });
        }

        #endregion

        #region Public Properties

        public int CategoryId
        {
            get { return base.CacheKey; }
        }

        #endregion

        #region Public Methods

        public static async Task<CategoryTreeNode> CreateAsync(K2SpyContext k2SpyContext, int categoryId)
        {
            CategoryTreeNode categoryTreeNode = new CategoryTreeNode(k2SpyContext, categoryId);
            await categoryTreeNode.InitializeAsync(false);
            return categoryTreeNode;
        }

        public void SortChildren()
        {
            TreeNode[] allChildren = base.Nodes.OfType<TreeNode>().ToArray();
            CategoryDataTreeNode[] orderedNodes = base.Nodes.OfType<CategoryDataTreeNode>().OrderBy(key => key.Text).ToArray();
            int offset = allChildren.Length - orderedNodes.Length;
            for (int i = 0; i < orderedNodes.Length; i++)
            {
                int position = i + offset;
                TreeNode node = orderedNodes[i];
                if (node.Index != position)
                {
                    // ok, the order is different, let's do some sorting!
                    node.Remove();
                    base.Nodes.Insert(position, node);
                }
            }
        }

        public override async Task EvictAsync(bool recursive)
        {
            await base.GetK2SpyContext().Cache.EvictCategoryAsync(base.CacheKey, false, false);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
#if true
            //await base.ClearDisposeAndEvictChildrenAsync();
            K2SpyTreeNode[] children = base.Nodes.OfType<K2SpyTreeNode>().ToArray();
            base.Nodes.Clear();
            base.Nodes.Add(new LoadingTreeNode());
            foreach (K2SpyTreeNode node in children)
            {
                await node.EvictAsync(true);
            }
            children.ToList().ForEach(key => key.Dispose());
#else
            // OK, we need to evict all descendants from their caches and repopulate everything
            K2SpyTreeNode[] children = this.Nodes.OfType<K2SpyTreeNode>().ToArray();

            base.Nodes.Clear();

            // dispose nodes to prevent them from listening to irrelevant events
            children.ToList().ForEach(key => key.Dispose());

            // trigger a cache refresh on all the nodes
            foreach (K2SpyTreeNode child in children)
                await child.EvictAsync();
#endif
            await this.EvictAsync(false);
            await base.GetK2SpyContext().Cache.RefreshCategoryAsync(this.CacheKey, false);
            await base.RefreshAsync();
        }

        public override Task<Definition> GetFormattedDefinitionAsync()
        {
#if true
            return Definition.CreateFromStringAsync(() =>
            {
                Func<TreeNode[], string> toString = nodes =>
                {
                    string delimiter = "\r\n";
                    string value = string.Join(delimiter, nodes.Select(key => "- " + key.Text));
                    if (!string.IsNullOrEmpty(value))
                        return value + delimiter;
                    return "";
                };
                CategoryTreeNode[] categories = base.Nodes.OfType<CategoryTreeNode>().ToArray();
                string result = $@"Category count: {categories.Length}
";
                result += toString(categories);

                CategoryDataTreeNode[] data = base.Nodes.OfType<CategoryDataTreeNode>().ToArray();

                CategoryDataTreeNode[] forms = data.Where(key => key.GetSource().DataType == SourceCode.Categories.Client.CategoryServer.dataType.Form).ToArray();
                result += $@"
Form count: {forms.Length}
";
                result += toString(forms);

                CategoryDataTreeNode[] views = data.Where(key => key.GetSource().DataType == SourceCode.Categories.Client.CategoryServer.dataType.View).ToArray();
                result += $@"
View count: {views.Length}
";
                result += toString(views);

                CategoryDataTreeNode[] smartObjects = data.Where(key => key.GetSource().DataType == SourceCode.Categories.Client.CategoryServer.dataType.SmartObject).ToArray();
                result += $@"
SmartObjects count: {smartObjects.Length}
";
                result += toString(smartObjects);

                CategoryDataTreeNode[] workflows = data.Where(key => key.GetSource().DataType == SourceCode.Categories.Client.CategoryServer.dataType.Workflow).ToArray();
                result += $@"
Workflow count: {workflows.Length}
";
                result += toString(workflows);

                return result;
            });
#endif
            return Definition.CreateFromXmlAsync(this.GetXmlDefinitionAsync(), true);
        }


        private bool m_ChildrenInitialized = false;
        public async Task InitializeChildrenAsync()
        {
            await this.InitializeChildrenAsync(false);
        }

        public async Task InitializeChildrenAsync(bool force)
        {
            if (force || !this.m_ChildrenInitialized)
            {
                this.m_ChildrenInitialized = true;
                using (TreeViewUpdateContext.CreateIfNecessary(this, true))
                {
                    foreach (CategoryDataTreeNode categoryDataTreeNode in base.Nodes.OfType<CategoryDataTreeNode>())
                    {
                        await categoryDataTreeNode.UpdateAfterTreeHasBeenInitializedAsync();
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        protected async Task<string> GetXmlDefinitionAsync()
        {
            SourceCode.Categories.Client.Category category = await this.GetSourceAsync();
            return category.ToXml(false);
        }

        protected async Task InitializeAsync(bool reload)
        {
            this.m_InitializeAsyncCancellationTokenSource?.Cancel();
            System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
            this.m_InitializeAsyncCancellationTokenSource = cancellationTokenSource;

            await this.InitializeAsync(reload, cancellationTokenSource.Token);
        }

        protected async Task InitializeAsync(bool reload, System.Threading.CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                //if (reload)
                //{
                //    using (new TreeViewUpdateContext(base.TreeView))
                //    {
                //        base.Nodes.Clear();
                //        base.Nodes.Add(new LoadingTreeNode());
                //        base.Expand();
                //    }
                //}

                List<TreeNode> categoryItems = new List<TreeNode>();
                List<TreeNode> dataItems = new List<TreeNode>();

                using (new TreeNodeLoadingContext(this))
                {
                    SourceCode.Categories.Client.Category category = await base.GetSourceAsync();
                    base.Text = category.Metadata.DisplayName;
                    //using (new TreeNodeLoadingContext(this))
                    {
                        foreach (CategoryId categoryId in category.ChildCategoryIds)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                return;

                            categoryItems.Add(await CategoryTreeNode.CreateAsync(base.GetK2SpyContext(), categoryId.Value));
                        }

                        foreach (SourceCode.Categories.Client.CategoryData categoryData in category.DataList.OrderBy(key => key.Name))
                        {
                            if (cancellationToken.IsCancellationRequested)
                                return;
                            if (CategoryDataTreeNode.IsSupported(K2Spy.Compatibility.CategoryDataTypes.Convert(categoryData.DataType)))
                            {
                                CategoryDataCacheKey categoryDataCacheKey = new CategoryDataCacheKey(category.Id, categoryData.Data);
                                //base.GetK2SpyContext().Cache.CategoryDataCache.Cache(categoryDataCacheKey, categoryData);

                                CategoryDataTreeNode categoryDataTreeNode = await CategoryDataTreeNode.CreateAsync(base.GetK2SpyContext(), categoryDataCacheKey);
                                dataItems.Add(categoryDataTreeNode);
                            }
                        }
                    }
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    using (TreeViewUpdateContext.CreateIfNecessary(this))
                    {
                        //base.Nodes.Clear();
                        //base.TreeView?.SuspendLayout();

                        base.Nodes.AddRange(categoryItems.OrderBy(key => key.Text).ToArray());
                        base.Nodes.AddRange(dataItems.Where(key => key != null).OrderBy(key => key.Text).ToArray());
                        if (reload)
                        {
                            // TODO, do something clever here
                        }

                        //base.TreeView?.ResumeLayout();
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        #endregion
    }

    public class LoadingTreeNode : TreeNode
    {
        public LoadingTreeNode()
        {
            base.Text = "Loading...";
            this.SetImageKey("Loading");
        }
    }

    public class StyleProfilesRootTreeNode : K2SpyTreeNode
    {
        private StyleProfilesRootTreeNode(K2SpyContext k2SpyContext)
            : base(k2SpyContext, "StyleProfiles", "Style profiles", false)
        {
        }

        public static async Task<StyleProfilesRootTreeNode> CreateAsync(K2SpyContext k2SpyContext)
        {
            StyleProfilesRootTreeNode styleProfilesRootTreeNode = new StyleProfilesRootTreeNode(k2SpyContext);
            if (!Properties.Settings.Default.ExperimentalDelayedInitializationOfTreeView)
                await styleProfilesRootTreeNode.InitializeAsync();
            return styleProfilesRootTreeNode;
        }

        public override Task RefreshAsync()
        {
            throw new NotImplementedException();
            return base.RefreshAsync();
        }

        private async Task InitializeAsync()
        {
            List<StyleProfileInfoTreeNode> list = new List<StyleProfileInfoTreeNode>();
            using (new TreeNodeLoadingContext(this))
            {
                SourceCode.Forms.Management.StyleProfileInfo[] profiles = await base.GetK2SpyContext().Cache.StyleProfileInfoCache.GetAllValuesAsync();
                foreach (SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo in profiles)
                {
                    StyleProfileInfoTreeNode styleProfileInfoTreeNode = await StyleProfileInfoTreeNode.CreateAsync(base.GetK2SpyContext(), styleProfileInfo);
                    list.Add(styleProfileInfoTreeNode);
                }
            }
            using (TreeViewUpdateContext.CreateIfNecessary(this))
            {
                base.Nodes.AddRange(list.OrderBy(key => key.Text).ToArray());
            }
        }
    }

    public class StyleProfileInfoTreeNode : K2SpyCacheBackedTreeNode<Guid, SourceCode.Forms.Management.StyleProfileInfo>
    {
        private StyleProfileInfoTreeNode(K2SpyContext k2SpyContext, SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo)
            : base(k2SpyContext, "StyleProfile", styleProfileInfo.Guid, k2SpyContext.Cache.StyleProfileInfoCache.GetAsync, true)
        {
            base.Text = styleProfileInfo.DisplayName;

            base.HandleCacheEventForCacheKey(
                "StyleProfileRefreshed",
                async (sender, e) =>
                {
                    await base.OnRefreshedAsync();
                });
            base.HandleCacheEventForCacheKey(
                "StyleProfileEvicted",
                async (sender, e) =>
                {
                    await base.OnEvictedAsync();
                });
        }

        public static async Task<StyleProfileInfoTreeNode> CreateAsync(K2SpyContext k2SpyContext, SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo)
        {
            StyleProfileInfoTreeNode styleProfileInfoTreeNode = new StyleProfileInfoTreeNode(k2SpyContext, styleProfileInfo);
            await styleProfileInfoTreeNode.InitializeAsync();
            return styleProfileInfoTreeNode;
        }

        public override async Task<Definition> GetFormattedDefinitionAsync()
        {
            return await Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.StyleProfileFormattedXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override async Task<Definition> GetRawDefinitionAsync()
        {
            return await Definition.CreateFromXmlAsync(base.GetK2SpyContext().Cache.StyleProfileRawXmlDefinitionCache.GetAsync(base.CacheKey));
        }

        public override async Task RefreshAsync()
        {
            await this.InitializeAsync();
            await base.RefreshAsync();
        }

        private async Task InitializeAsync()
        {
            SourceCode.Forms.Management.StyleProfileInfo styleProfileInfo = await base.GetSourceAsync();
            if (styleProfileInfo.IsCheckedOut)
                this.SetImageKey("StyleProfileCheckedOut");
            else
                this.SetImageKey("StyleProfile");
        }
    }

    public class CategoryDataTreeNode : K2SpyCacheBackedTreeNode<CategoryDataCacheKey, SourceCode.Categories.Client.CategoryData>, IActAsTreeNode, IInitializeChildren
    {
        private CategoryDataTreeNode(K2SpyContext k2SpyContext, CategoryDataCacheKey key)
            : base(k2SpyContext, null, key, k2SpyContext.Cache.CategoryDataCache.GetAsync, true)
        {
            if (Guid.TryParse(base.CacheKey.Data, out Guid guid))
            {
                this.DataAsGuid = guid;
                base.AddSearchableValue(guid);
            }
            else
            {
                base.AddSearchableValue(base.CacheKey.Data);
            }
        }

        public Guid DataAsGuid { get; private set; }

        public string Data
        {
            get { return base.CacheKey.Data; }
        }

        internal async Task UpdateAfterTreeHasBeenInitializedAsync()
        {
            K2SpyTreeNode actingAs = await this.GetActAsAsync();
            if (actingAs == null)
            {
                if (Properties.Settings.Default.PreserveInvalidCategoryEntriesInTreeView)
                {
                    this.ImageKey = "Error";
                }
                else
                {
                    using (TreeViewUpdateContext.CreateIfNecessary(this))
                    {
                        base.Remove();
                        base.Dispose();
                    }
                }
            }
            else if (true)
            {
                this.SetImageKey(actingAs.ImageKey);
                if (base.Text != actingAs.Text)
                {
                    base.Text = actingAs.Text;
                    ((CategoryTreeNode)base.Parent).SortChildren();
                }
            }
            else
            {
                //using (TreeViewUpdateContext.CreateIfNecessary(base.Parent))
                {
                    TreeNode parent = base.Parent;
                    int index = base.Index;
                    if (!base.IsVisible)
                    {
                        base.Remove();
                    }
                    bool sortPending = false;
                    if (base.Text != actingAs.Text)
                    {
                        base.Text = actingAs.Text;
                        sortPending = true;
                    }
                    if (base.Parent == null)
                        parent.Nodes.Insert(index, this);
                    if (sortPending)
                    {
                        ((CategoryTreeNode)base.Parent).SortChildren();
                    }
                }
            }
            await this.UpdateIconAsync();
        }

        public static bool IsSupported(K2Spy.Compatibility.CategoryDataType dataType)
        {
            if (dataType == K2Spy.Compatibility.CategoryDataType.StyleProfile)
                return true;
            if (dataType == K2Spy.Compatibility.CategoryDataType.Form ||
                dataType == K2Spy.Compatibility.CategoryDataType.View ||
                dataType == K2Spy.Compatibility.CategoryDataType.SmartObject ||
                dataType == K2Spy.Compatibility.CategoryDataType.Workflow)
                return true;
            return false;
        }

        public static async Task<CategoryDataTreeNode> CreateAsync(K2SpyContext k2SpyContext, CategoryDataCacheKey categoryDataCacheKey)
        {
            CategoryDataTreeNode result = new CategoryDataTreeNode(k2SpyContext, categoryDataCacheKey);
            await result.InitializeAsync();
            return result;
        }

        public override async Task EvictAsync(bool recursive)
        {
            K2SpyTreeNode actingAs = await this.GetActAsAsync();
            if (actingAs != null)
                await (actingAs.EvictAsync(recursive) ?? Task.CompletedTask);
            this.GetK2SpyContext().Cache.CategoryDataCache.Evict(base.CacheKey);
            await base.EvictAsync(recursive);
        }

        public override async Task RefreshAsync()
        {
            K2SpyTreeNode actAs = await this.GetActAsAsync();
            if (actAs != null)
            {
                await (actAs.RefreshAsync() ?? Task.CompletedTask);
            }
        }

        public async Task<K2SpyTreeNode> GetActAsAsync()
        {
            TreeView treeView = base.TreeView ?? base.GetK2SpyContext().TreeView;
            // treeView = base.GetK2SpyContext().TreeView;
            K2Spy.Compatibility.CategoryDataType dataType = K2Spy.Compatibility.CategoryDataTypes.Convert((await base.GetSourceAsync()).DataType);
            if (dataType == K2Spy.Compatibility.CategoryDataType.Form)
            {
                // ok, we need to find the FormInfoTreeNode
                return await treeView.GetNodeBelowRootNodeAsync<FormsRootTreeNode, FormInfoTreeNode>(key => key.FormGuid == this.DataAsGuid);
            }
            else if (dataType == K2Spy.Compatibility.CategoryDataType.View)
            {
                // ok, we need to find the ViewInfoTreeNode
                return await treeView.GetNodeBelowRootNodeAsync<ViewsRootTreeNode, ViewInfoTreeNode>(key => key.ViewGuid == this.DataAsGuid);
            }
            else if (dataType == K2Spy.Compatibility.CategoryDataType.SmartObject)
            {
                // ok, we need to find the SmartObjectInfoTreeNode
                return await treeView.GetNodeBelowRootNodeAsync<SmartObjectsRootTreeNode, SmartObjectInfoTreeNode>(key => key.SmartObjectGuid == this.DataAsGuid);
            }
            else if (dataType == K2Spy.Compatibility.CategoryDataType.Workflow)
            {
                string processFullName = this.Data;
                return await treeView.GetNodeBelowRootNodeAsync<WorkflowsRootTreeNode, WorkflowTreeNode>(key => key?.ProcessFullName?.Equals(processFullName, StringComparison.OrdinalIgnoreCase) == true);
            }
            else if (dataType == K2Spy.Compatibility.CategoryDataType.StyleProfile)
            {
                // ok, we need to find the StyleProfileInfoTreeNode
                return await treeView.GetNodeBelowRootNodeAsync<StyleProfilesRootTreeNode, StyleProfileInfoTreeNode>(key => key.CacheKey == this.DataAsGuid);
            }
            return null;
        }

        public override async Task<Definition> GetFormattedDefinitionAsync()
        {
            SourceCode.Categories.Client.CategoryData source = await base.GetSourceAsync();
            return await Definition.CreateFromXmlAsync(source.ToXml, true);
        }

        private bool m_ChildrenInitialized = false;
        public async Task InitializeChildrenAsync()
        {
            if (!this.m_ChildrenInitialized)
            {
                this.m_ChildrenInitialized = true;
                K2SpyTreeNode actingAs = await this.GetActAsAsync();
                if (actingAs != null)
                {
                    TreeNode parent = base.Parent;
                    SourceCode.Categories.Client.CategoryData source = await base.GetSourceAsync();
                    if (source.DataType == SourceCode.Categories.Client.CategoryServer.dataType.SmartObject)
                    {
                        using (TreeViewUpdateContext.CreateIfNecessary(this))
                        {
                            K2SpyTreeNodeClone[] clones = await K2SpyTreeNodeClone.CloneChildrenAsync(base.GetK2SpyContext(), actingAs);
                            base.Nodes.Clear();
                            base.Nodes.AddRange(clones);
                        }
                    }
                }
            }
        }

        protected async override Task OnEvictedAsync()
        {
            await base.OnEvictedAsync();
        }

        private Action m_DisposePreviousUpdateIconAsync;
        protected async Task UpdateIconAsync()
        {
            this.m_DisposePreviousUpdateIconAsync?.Invoke();
            K2SpyTreeNode actAs = await this.GetActAsAsync();
            if (actAs != null)
            {
                AsyncEventHandler handler = async (sender, e) =>
                {
                    await this.UpdateIconAsync();
                };
                this.m_DisposePreviousUpdateIconAsync = () => actAs.Refreshed -= handler;
                actAs.Refreshed += handler;
                this.SetImageKey(actAs.ImageKey);
            }
        }

        protected async Task InitializeAsync()
        {
            SourceCode.Categories.Client.CategoryData categoryData = await base.GetSourceAsync();
            base.Text = categoryData.Name;
            K2Spy.Compatibility.CategoryDataType categoryDataType = K2Spy.Compatibility.CategoryDataTypes.Convert(categoryData.DataType);
            if (categoryDataType == K2Spy.Compatibility.CategoryDataType.Form)
            {
                this.ImageKey = "Form";
                base.HandleCacheEventForCacheKey<Guid>(
                    "FormRefreshed",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        if (this.DataAsGuid == new Guid("f76e3e25-dcc5-4480-a07b-9990ab1db621"))
                        {

                        }
                        //await this.UpdateIconAsync();
                        await this.UpdateAfterTreeHasBeenInitializedAsync();
                        await base.OnRefreshedAsync();
                    });
                base.HandleCacheEventForCacheKey<Guid>(
                    "FormEvicted",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        await this.OnEvictedAsync();
                    });
            }
            else if (categoryDataType == K2Spy.Compatibility.CategoryDataType.View)
            {
                this.ImageKey = "View";
                base.HandleCacheEventForCacheKey<Guid>(
                    "ViewRefreshed",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        //await this.UpdateIconAsync();
                        await base.OnRefreshedAsync();
                    });
                base.HandleCacheEventForCacheKey<Guid>(
                    "ViewEvicted",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        await this.OnEvictedAsync();
                    });
            }
            else if (categoryDataType == K2Spy.Compatibility.CategoryDataType.SmartObject)
            {
                this.ImageKey = "SmartObject";
                base.HandleCacheEventForCacheKey<Guid>(
                    "SmartObjectEvicted",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        await this.OnEvictedAsync();
                    });
                base.HandleCacheEventForCacheKey<Guid>(
                    "SmartObjectRefreshed",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        await this.OnRefreshedAsync();
                    });

                base.Nodes.Add(new LoadingTreeNode());
            }
            else if (categoryDataType == K2Spy.Compatibility.CategoryDataType.Workflow)
            {
                this.ImageKey = "Workflow";
                base.HandleCacheEventForCacheKey<string>(
                    "WorkflowRefreshed",
                    this.Data,
                    async (sender, e) =>
                    {
                        //await this.UpdateIconAsync();
                        await base.OnRefreshedAsync();
                    });
                base.HandleCacheEventForCacheKey<string>(
                    "WorkflowEvicted",
                    this.Data,
                    async (sender, e) =>
                    {
                        await this.OnEvictedAsync();
                    });
            }
            else if (categoryDataType == K2Spy.Compatibility.CategoryDataType.StyleProfile)
            {
                this.ImageKey = "StyleProfile";
                base.HandleCacheEventForCacheKey<Guid>(
                    "StyleProfileRefreshed",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        //await this.UpdateIconAsync();
                        await base.OnRefreshedAsync();
                    });
                base.HandleCacheEventForCacheKey<Guid>(
                    "StyleProfileEvicted",
                    this.DataAsGuid,
                    async (sender, e) =>
                    {
                        await this.OnEvictedAsync();
                    });
            }
            await this.UpdateIconAsync();
        }
    }

    public abstract class K2SpyCacheBackedTreeNode<TCacheKey, TValue> : K2SpyTreeNode
    {
        private Func<TValue> m_GetSource;
        private Func<TCacheKey, Task<TValue>> m_GetSourceAsync;

        protected K2SpyCacheBackedTreeNode(K2SpyContext k2SpyContext, string imageKey, TCacheKey cacheKey, Func<TCacheKey, Task<TValue>> getSourceAsync, bool canRefresh)
            : this(k2SpyContext, imageKey, cacheKey, "", getSourceAsync, canRefresh)
        {
        }

        protected K2SpyCacheBackedTreeNode(K2SpyContext k2SpyContext, string imageKey, TCacheKey cacheKey, string displayName, Func<TCacheKey, Task<TValue>> getSourceAsync, bool canRefresh)
            : base(k2SpyContext, imageKey, displayName, canRefresh)
        {
            this.CacheKey = cacheKey;
            this.m_GetSourceAsync = getSourceAsync;
        }

        public TCacheKey CacheKey { get; private set; }

        public async Task<TValue> GetSourceAsync()
        {
            if (this.m_GetSourceAsync != null)
                return await this.m_GetSourceAsync(this.CacheKey);
            return this.m_GetSource();
        }

        public TValue GetSource()
        {
            if (this.m_GetSource != null)
                return this.m_GetSource();
            return this.GetSourceAsync().GetAwaiter().GetResult();
        }

        public override bool Equals(K2SpyTreeNode other)
        {
            if (base.GetType() == other.GetType())
            {
                if (this.CacheKey.Equals(((K2SpyCacheBackedTreeNode<TCacheKey, TValue>)other).CacheKey))
                {
                    return true;
                }
            }
            return false;
        }

        protected void HandleCacheEventForCacheKey(string eventName, AsyncEventHandler<CacheEventArgs<TCacheKey>> handler)
        {
            if (this.CacheKey.Equals(new Guid("cd3804d6-973d-4de4-bf78-8427f6761011")))
            {

            }
            string name = null;
            AsyncEventHandler<CacheEventArgs<TCacheKey>> innerHandler = handler;
            AsyncEventHandler<CacheEventArgs<TCacheKey>> outerHandler = async (sender, e) =>
            {
                name = eventName;
                if (e.CacheKey.Equals(new Guid("cd3804d6-973d-4de4-bf78-8427f6761011")))
                {

                }
                if (object.Equals(e.CacheKey, this.CacheKey))
                {
                    await innerHandler(sender, e);
                }
            };
            this.HandleEvent<Cache, AsyncEventHandler<CacheEventArgs<TCacheKey>>>(this.GetK2SpyContext().Cache, eventName, outerHandler);
        }
    }

    public class K2SpyTreeNodeClone : K2SpyTreeNode, IActAsTreeNode
    {
        private K2SpyTreeNode m_Source;

        private K2SpyTreeNodeClone(K2SpyContext k2SpyContext, K2SpyTreeNode source)
            : base(k2SpyContext, source.ImageKey, source.Text)
        {
            this.m_Source = source;
        }

        public static async Task<K2SpyTreeNodeClone[]> CloneChildrenAsync(K2SpyContext k2SpyContext, K2SpyTreeNode node)
        {
            await node.InitializeChildrenAsync();
            K2SpyTreeNodeClone[] nodes = node.Nodes.OfType<K2SpyTreeNode>().Select(key => new K2SpyTreeNodeClone(k2SpyContext, key)).ToArray();
            await Task.WhenAll(nodes.Select(key => key.InitializeAsync()));
            return nodes;
        }

        private async Task InitializeAsync()
        {
            K2SpyTreeNodeClone[] nodes = await K2SpyTreeNodeClone.CloneChildrenAsync(base.GetK2SpyContext(), this.m_Source);
            base.Nodes.Clear();
            base.Nodes.AddRange(nodes);
        }

        public async Task<K2SpyTreeNode> GetActAsAsync()
        {
            return this.m_Source;
        }
    }

    public abstract class K2SpyTreeNode : TreeNode, IEquatable<K2SpyTreeNode>, IDisposable
    {
        #region Private Fields

        private List<string> m_SearchableValues = new List<string>();
        private List<Action> m_DisposeActions = new List<Action>();
        private K2SpyContext m_K2SpyContext;
        private static System.Collections.Concurrent.ConcurrentDictionary<string, System.Reflection.EventInfo> m_EventDictionary = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Reflection.EventInfo>();

        #endregion

        #region Constructors

        protected K2SpyTreeNode(K2SpyContext k2SpyContext, string imageKey = null, string displayName = null, bool canRefresh = false)
        {
            this.m_K2SpyContext = k2SpyContext;
            this.ImageKey = imageKey;
            base.Text = displayName;
            this.CanRefresh = canRefresh;
        }

        #endregion

        #region Public Events

        public event EventHandler ImageKeyChanged;

        public event AsyncEventHandler Evicted;

        public event AsyncEventHandler Refreshed;

        public event EventHandler Disposed;

        #endregion

        #region Public Properties

        public new string ImageKey
        {
            get { return base.ImageKey; }
            set
            {
                if (base.ImageKey != value)
                {
                    base.ImageKey = base.SelectedImageKey = value;
                    this.OnImageKeyChanged();
                }
            }
        }

        public virtual bool CanRefresh { get; protected set; }

        #endregion

        #region Protected Properties

        protected virtual bool RaiseRefreshedEventOnRefresh => false;

        #endregion

        #region Public Methods

        public virtual async Task EvictAsync(bool recusirve)
        {
            //Console.WriteLine("Evicting " + base.Text);
            if (recusirve)
            {
                foreach (K2SpyTreeNode child in base.Nodes.OfType<K2SpyTreeNode>())
                {
                    await child.EvictAsync(true);
                }
            }
            await this.Evicted.InvokeAsync(this, EventArgs.Empty);
        }

        public virtual async Task RefreshAsync()
        {
            if (this.RaiseRefreshedEventOnRefresh)
                await this.OnRefreshedAsync();
        }

        public virtual string[] GetSearchableValues()
        {
            return this.m_SearchableValues.Concat(new string[] { base.Text }).ToArray();
        }

        public virtual Task<Definition> GetFormattedDefinitionAsync()
        {
            return null;
        }

        public virtual Task<Definition> GetRawDefinitionAsync()
        {
            return null;
        }

        public virtual bool Equals(K2SpyTreeNode other)
        {
            return base.Equals(other);
        }

        public void Dispose()
        {
            this.OnDispose(true);
        }

        public void AddDisposeAction(Action action)
        {
            this.m_DisposeActions.Add(action);
        }

        ~K2SpyTreeNode()
        {
            this.OnDispose(false);
        }

        #endregion

        #region Protected Methods

        internal protected void ClearAndDisposeChildren()
        {
            K2SpyTreeNode[] nodes = base.Nodes.OfType<K2SpyTreeNode>().ToArray();
            base.Nodes.Clear();
            nodes.ToList().ForEach(key => key.Dispose());
        }

        /// <summary>
        /// Raises the Refreshed-event
        /// </summary>
        /// <returns></returns>
        internal protected virtual async Task OnRefreshedAsync()
        {
            //Console.WriteLine(this.GetType().FullName + ": " + base.Text);
            await this.Refreshed.InvokeAsync(this, EventArgs.Empty);
        }


        protected void HandleEvent<TEventSource, TEventHandler>(TEventSource eventSource, string eventName, TEventHandler eventHandler)
            where TEventHandler : Delegate
        {
            System.Reflection.EventInfo eventInfo = typeof(TEventSource).GetEvent(eventName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (eventInfo == null)
                throw new Exception($"The event {eventName} was not found on the type {typeof(TEventSource).FullName}");
            this.HandleEvent(
                eventSource,
                (eventSource2, eventHandler2) =>
                {
                    eventInfo.AddEventHandler(eventSource2, eventHandler2);
                },
                (eventSource2, eventHandler2) =>
                {
                    eventInfo.RemoveEventHandler(eventSource2, eventHandler2);
                },
                eventHandler);
        }

        protected void HandleEvent<TEventSource, TEventHandler>(TEventSource eventSource, Action<TEventSource, TEventHandler> wire, Action<TEventSource, TEventHandler> unwire, TEventHandler eventHandler)
        {
            wire(eventSource, eventHandler);
            this.AddDisposeAction(() => unwire(eventSource, eventHandler));
        }

        // TODO consider making this obsolete and use HandleCacheEventForCacheKey instead
        [Obsolete("Use HandleCacheEventForCacheKey instead", true)]
        protected void HandleCacheEvent<TKey>(string eventName, AsyncEventHandler<CacheEventArgs<TKey>> handler)
        {
            this.HandleEvent<Cache, AsyncEventHandler<CacheEventArgs<TKey>>>(this.GetK2SpyContext().Cache, eventName, handler);
        }

        protected void HandleCacheEventForCacheKey<TKey>(string eventName, TKey cacheKey, AsyncEventHandler<CacheEventArgs<TKey>> handler)
        {
            AsyncEventHandler<CacheEventArgs<TKey>> innerHandler = handler;
            AsyncEventHandler<CacheEventArgs<TKey>> outerHandler = async (sender, e) =>
            {
                if (object.Equals(e.CacheKey, cacheKey))
                {
                    await innerHandler(sender, e);
                }
            };
            this.HandleEvent<Cache, AsyncEventHandler<CacheEventArgs<TKey>>>(this.GetK2SpyContext().Cache, eventName, outerHandler);
        }

        protected void OnImageKeyChanged()
        {
            this.ImageKeyChanged?.Invoke(this, EventArgs.Empty);
        }

        internal protected K2SpyContext GetK2SpyContext()
        {
            return this.m_K2SpyContext;
        }

        protected virtual async Task OnEvictedAsync()
        {
            await this.Evicted.InvokeAsync(this, EventArgs.Empty);
        }

        protected virtual void OnDispose(bool disposing)
        {
            this.m_DisposeActions.ForEach(key => key());

            foreach (K2SpyTreeNode node in base.Nodes.OfType<K2SpyTreeNode>())
                node.Dispose();

            this.Disposed?.Invoke(this, EventArgs.Empty);

            this.Disposed.ForEach(handler => this.Disposed -= handler);
            this.ImageKeyChanged.ForEach(handler => this.ImageKeyChanged -= handler);
            this.Evicted.ForEach<AsyncEventHandler>(handler => this.Evicted -= handler);

            base.Nodes.Clear();
        }

        protected void AddSearchableValue(Guid guid)
        {
            this.AddSearchableValue(guid.ToString());
            this.AddSearchableValue(guid.ToString("N"));
        }

        protected void AddSearchableValue(string value)
        {
            this.m_SearchableValues.Add(value);
        }

        #endregion
    }
}