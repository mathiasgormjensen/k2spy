using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using K2Spy.ExtensionMethods;

namespace K2Spy.Extensions.Analyzer
{
    public static class UsesInformationCache
    {
        private static Dictionary<string, UsesInformation> m_InmemoryCache = new Dictionary<string, UsesInformation>();
       
        public static async Task<UsesInformation> GetUsesInformationAsync(K2SpyContext k2SpyContext, K2SpyTreeNode node)
        {
            UsesInformation usesInformation = null;
            try
            {
                node = await node.GetActAsOrSelfAsync();
                if (node is SmartObjectInfoTreeNode smartObjectInfoTreeNode)
                {
                    SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo = await smartObjectInfoTreeNode.GetSourceAsync();
                    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, smartObjectInfo);
                }
                else if (node is FormInfoTreeNode formInfoTreeNode)
                {
                    SourceCode.Forms.Management.FormInfo formInfo = await formInfoTreeNode.GetSourceAsync();
                    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, formInfo);
                }
                else if (node is ViewInfoTreeNode viewInfoTreeNode)
                {
                    SourceCode.Forms.Management.ViewInfo viewInfo = await viewInfoTreeNode.GetSourceAsync();
                    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, viewInfo);
                }
                else if (node is WorkflowTreeNode processSetTreeNode)
                {
                    SourceCode.Workflow.Management.ProcessSet processSet = await processSetTreeNode.GetSourceAsync();
                    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, processSet);
                }
                else if (node is SmartMethodTreeNode smartMethodTreeNode)
                {
                    SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo = await smartMethodTreeNode.GetSourceAsync();
                    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, smartMethodInfo);
                }
                //else if (node is SmartPropertyTreeNode smartPropertyTreeNode)
                //{
                //    SourceCode.SmartObjects.Management.SmartPropertyInfo smartPropertyInfo = await smartPropertyTreeNode.GetSourceAsync();
                //    usesInformation = await UsesInformationCache.GetOrCreateAsync(k2SpyContext, smartPropertyInfo);
                //}
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Failed to get or create UsesInformation for {node.FullPath}: {ex}");
                if (!Properties.Settings.Default.IgnoreExceptionsInGetUsesInformationAsync)
                    throw;
            }
            return usesInformation;
        }

        public static async Task<UsesInformation> GetOrCreateAsync(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartMethodInfo smartMethodInfo)
        {
            string fileName = smartMethodInfo.SmartObjectInfo.Guid.ToString("N") + "_" + smartMethodInfo.Name.ReplaceInvalidFileNameChars();
            string path = UsesInformationCache.GetCachePath("SmartMethods", fileName);
            UsesInformation usesInformation = null;
            if (!UsesInformationCache.TryLoadFromCacheOrDisk(path, smartMethodInfo.SmartObjectInfo.ModifiedDate, out usesInformation))
            {
                usesInformation = new UsesInformation();
                SourceCode.SmartObjects.Authoring.SmartObjectDefinition smartObjectDefinition = await k2SpyContext.Cache.SmartObjectDefinitionCache.GetAsync(smartMethodInfo.SmartObjectInfo.Guid);
                Guid[] serviceInstanceGuids = smartObjectDefinition.Methods[smartMethodInfo.Name].ExecutionBlocks.OfType<SourceCode.SmartObjects.Authoring.ExecutionBlock>().Select(key => key.ServiceInstance.Guid).ToArray();
                usesInformation.Guids = serviceInstanceGuids;

                UsesInformationCache.Save(path, usesInformation);
            }
            return usesInformation;
        }

        public static async Task<UsesInformation> GetOrCreateAsync(K2SpyContext k2SpyContext, SourceCode.Workflow.Management.ProcessSet processSet)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync("Workflows", processSet.FullName, processSet.ModifiedDate, k2SpyContext.Cache.WorkflowRawKprxXmlDefinitionCache.GetAsync);
        }

        public static async Task<UsesInformation> GetOrCreateAsync(K2SpyContext k2SpyContext, SourceCode.Forms.Management.FormInfo formInfo)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync("Forms", formInfo.Guid, formInfo.ModifiedDate, k2SpyContext.Cache.FormFormattedXmlDefinitionCache.GetAsync);
        }

        public static async Task<UsesInformation> GetOrCreateAsync(K2SpyContext k2SpyContext, SourceCode.Forms.Management.ViewInfo viewInfo)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync("Views", viewInfo.Guid, viewInfo.ModifiedDate, k2SpyContext.Cache.ViewFormattedXmlDefinitionCache.GetAsync);
        }

        public static async Task<UsesInformation> GetOrCreateAsync(K2SpyContext k2SpyContext, SourceCode.SmartObjects.Management.SmartObjectInfo smartObjectInfo)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync("SmartObjects", smartObjectInfo.Guid, smartObjectInfo.ModifiedDate, k2SpyContext.Cache.SmartObjectFormattedXmlDefinitionCache.GetAsync);
        }

        public static void ClearDiskCache()
        {
            new System.IO.DirectoryInfo(System.IO.Path.Combine(Configuration.Directory, "UsesInformationCache")).DeleteContents();
        }

        private static async Task<UsesInformation> GetOrCreateInternalAsync(string directory, string key, DateTime timestamp, Func<string, Task<string>> getXmlDefinition)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync<string>(directory, key, key.ReplaceInvalidFileNameChars(), timestamp, getXmlDefinition.Invoke);
        }

        private static async Task<UsesInformation> GetOrCreateInternalAsync(string directory, Guid guid, DateTime timestamp, Func<Guid, Task<string>> getXmlDefinition)
        {
            return await UsesInformationCache.GetOrCreateInternalAsync<Guid>(directory, guid, guid.ToString("N"), timestamp, getXmlDefinition.Invoke);
        }

        private static string GetCachePath(string subDirectory, string fileName)
        {
            string path = System.IO.Path.Combine(Configuration.Directory, "UsesInformationCache", subDirectory);
            System.IO.Directory.CreateDirectory(path);
            path = System.IO.Path.Combine(path, fileName);
            return path;
        }

        private static bool TryLoadFromCacheOrDisk(string path, DateTime timestamp,out UsesInformation usesInformation)
        {
            if (UsesInformationCache.m_InmemoryCache.TryGetValue(path, out usesInformation) && UsesInformationCache.IsValid(usesInformation, timestamp))
                return true;

            if (UsesInformationCache.TryLoadFromDisk(path, out usesInformation) && UsesInformationCache.IsValid(usesInformation, timestamp))
                return true;

            return false;
        }

        private static async Task<UsesInformation> GetOrCreateInternalAsync<TKey>(string subDirectory, TKey key, string keyAsFileName, DateTime timestamp, Func<TKey, Task<string>> getXmlDefinition)
        {
            string path = UsesInformationCache.GetCachePath(subDirectory, keyAsFileName);
            UsesInformation usesInformation = null;
            if (!UsesInformationCache.TryLoadFromCacheOrDisk(path, timestamp, out usesInformation))
            {
                usesInformation = new UsesInformation();
                usesInformation.Timestamp = timestamp;

                string xml = await getXmlDefinition(key);
                if (!string.IsNullOrWhiteSpace(xml))
                {
                    System.Xml.XPath.XPathDocument document = xml.AsXPathDocument();
                    if (document != null)
                    {
                        List<Guid> guids = new List<Guid>();
                        string xpath = @"/SourceCode.Forms//Property[Name='ViewID']/Value|
/SourceCode.Forms//Panels/Panel/Areas/Area/Items/Item/@ViewID|
/SourceCode.Forms//Property[Name='FormID']/Value|
/SourceCode.Forms//Property[Name='ObjectID']/Value|
/SourceCode.Forms//Sources/Source/@SourceID|
/Process//FormPart/ID|
/Process//PersistableObjectDictionaryEntry[Key='Guid']/Value/Value/Parts/ValueTypePart/Value|
/smartobjectroot//serviceinstance/@guid";
                        System.Xml.XPath.XPathNavigator navigator = document.CreateNavigator();
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            if (Guid.TryParse(item.Value, out Guid guid))
                                if (!guids.Contains(guid))
                                    guids.Add(guid);
                        }

                        guids.Remove(Guid.Empty);

                        List<string> processFullNames = new List<string>();
                        xpath = $@"/SourceCode.Forms//Property[Name='ProcessName']/Value|
/Process//ProcessName[@Type='SourceCode.Workflow.Authoring.K2Field']/Parts/ValueTypePart/Value";
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            string value = item.Value;
                            if (!processFullNames.Contains(value))
                                processFullNames.Add(value);
                        }

                        List<string> controlNames = new List<string>();
                        xpath = $@"/SourceCode.Forms//Control/@Type";
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            string value = item.Value;
                            if (!controlNames.Contains(value))
                                controlNames.Add(value);
                        }

                        // determine style profile
                        Guid styleProfile;
                        if (Guid.TryParse(navigator.SelectSingleNode("/SourceCode.Forms/Forms/Form/Controls/Control[1]/Properties/Property[Name='StyleProfile']/Value")?.InnerXml, out styleProfile))
                            guids.Add(styleProfile);

                        List<UsedSmartMethodInformation> usedSmartMethods = new List<UsedSmartMethodInformation>();
                        xpath = $"/SourceCode.Forms//Properties[Property[Name='ObjectID']]/Property[Name='Method']/Value";
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            string objectId = item.CreateNavigator().SelectSingleNode("parent::Property/parent::Properties/Property[Name='ObjectID']/Value").Value;
                            string methodName = item.Value;
                            usedSmartMethods.Add(new UsedSmartMethodInformation(new Guid(objectId), methodName));
                        }

                        List<UsedSmartPropertyInformation> usedSmartProperties = new List<UsedSmartPropertyInformation>();
                        xpath = "/SourceCode.Forms//Sources/Source/Fields/Field[@ID=/SourceCode.Forms//Control/@FieldID]/FieldName";
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            string objectId = item.CreateNavigator().SelectSingleNode("parent::Field/parent::Fields/parent::Source/@SourceID").Value;
                            string propertyName = item.Value;
                            usedSmartProperties.Add(new UsedSmartPropertyInformation(new Guid(objectId), propertyName));
                        }

                        // extract list view methods
                        xpath = "/SourceCode.Forms/Views/View[@Type='List']/Sources/Source[@SourceType='Object']/@SourceID";
                        string sourceId = navigator.SelectSingleNode(xpath)?.Value;
                        if (!string.IsNullOrEmpty(sourceId))
                        {
                            Guid guid = Guid.Parse(sourceId);
                            guids.Add(guid);
                            xpath = $@"/SourceCode.Forms/Views/View[@Type='List']//Properties[Property[Name='ViewID'] and Property[Value=/SourceCode.Forms/Views/View/@ID]]/Property[Name='Method']/NameValue";
                            foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                            {
                                usedSmartMethods.Add(new UsedSmartMethodInformation(guid, item.Value));
                            }
                        }

                        // ///Process//Properties/Locals[PersistableObjectDictionaryEntry[Key='Guid' and Value/Value/Parts/ValueTypePart/Value='{guid}']]/PersistableObjectDictionaryEntry[Key='MethodName' and Value/Value/Parts/ValueTypePart/Value='{methodName}']";
                        xpath = $"/Process//Properties/Locals[PersistableObjectDictionaryEntry[Key='Guid' and Value/Value/Parts/ValueTypePart/Value]]/PersistableObjectDictionaryEntry[Key='MethodName']/Value/Value/Parts/ValueTypePart/Value";
                        foreach (System.Xml.XPath.XPathNavigator item in navigator.Select(xpath))
                        {
                            string objectId = item.CreateNavigator().SelectSingleNode("ancestor::Locals/PersistableObjectDictionaryEntry[Key='Guid']/Value/Value/Parts/ValueTypePart/Value").Value;
                            string methodName = item.Value;
                            usedSmartMethods.Add(new UsedSmartMethodInformation(new Guid(objectId), methodName));
                        }

                        usesInformation.SmartProperties = usedSmartProperties.Distinct().ToArray();
                        usesInformation.SmartMethods = usedSmartMethods.Distinct().ToArray();
                        usesInformation.Guids = guids.Distinct().ToArray();
                        usesInformation.ProcessFullNames = processFullNames.Distinct().ToArray();
                        usesInformation.ControlNames = controlNames.Distinct().ToArray();
                    }
                }
                UsesInformationCache.Save(path, usesInformation);
            }
            return usesInformation;
        }


        private static void Save(string path, UsesInformation usesInformation)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(ignore => Xml.Serialize(usesInformation, path));
            UsesInformationCache.m_InmemoryCache[path] = usesInformation;
        }

        private static bool IsValid(UsesInformation usesInformation, DateTime timestamp)
        {
            return usesInformation?.Timestamp == timestamp && usesInformation.Version == UsesInformation.CurrentVersion;
        }

        private static bool TryLoadFromDisk(string path, out UsesInformation usesInformation)
        {
            usesInformation = null;
            try
            {
                usesInformation = Xml.Deserialize<UsesInformation>(path);
                return true;
            }
            catch { }
            return false;
        }
    }
}